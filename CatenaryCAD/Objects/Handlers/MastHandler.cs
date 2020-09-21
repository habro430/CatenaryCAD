using CatenaryCAD.Properties;
using Multicad;
using Multicad.DatabaseServices;
using Multicad.Geometry;
using Multicad.Runtime;
using CatenaryCAD.Models.Attributes;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using static CatenaryCAD.Extensions;
using CatenaryCAD.Geometry;

namespace CatenaryCAD.Models.Handlers
{
    [Serializable]
    [CustomEntity("{742ECCF0-0CEC-4791-B4BE-4E3568E2C43E}", "MAST", "Стойка опоры контактной сети")]
    internal sealed class MastHandler : Handler
    {
        [NonSerialized]
        private static readonly Type[] Masts;

        static MastHandler() => Masts = Main.GetCatenaryObjects(typeof(IMast));

        public MastHandler()
        {
            Property<Type> mast_type = new Property<Type>("01_mast_type", "Тип стойки", "Стойка", ConfigFlags.RefreshAfterChange);

            mast_type.DictionaryValues = Masts
                .Where((type) => type.GetCustomAttributes(typeof(ModelNonBrowsableAttribute), false).FirstOrDefault() == null)
                .Select((type) => new
                {
                    type,
                    atrr = (type.GetCustomAttributes(typeof(ModelNameAttribute), false)
                               .FirstOrDefault() as ModelNameAttribute ?? new ModelNameAttribute(type.Name)).Name
                }).ToDictionary(p => p.atrr, p => p.type);

            mast_type.Updated += (type) =>
            {
                Point3D position = Model?.Position ?? new Point3D(0, 0, 0);
                Vector3D direction = Model?.Direction ?? new Vector3D(1, 0, 0);

                if (!TryModify()) return;
                Model = (IMast)Activator.CreateInstance(type);

                Model.Position = position;
                Model.Direction = direction;

                Model.TryModify += () => TryModify();
                Model.Update += () => DbEntity.Update();
            };

            mast_type.Value = mast_type.DictionaryValues.Values.FirstOrDefault();

            properties.Add(mast_type);
        }

        public override ICollection<McDynamicProperty> GetProperties(out bool exclusive)
        {
            exclusive = true;

            var basement = GetChildrens().Where(child => child is FoundationHandler).FirstOrDefault();
            var basement_props =  basement?.GetProperties();

            //проверяем режим работы если режим работы не определен то по умолчанию используем OperationalMode.Scheme
            OperationalMode mode = (OperationalMode)(McDocument.ActiveDocument.CustomProperties["OperationalMode"] ?? OperationalMode.Scheme);

            if (basement_props != null && mode == OperationalMode.Scheme)
                //если режим работы OperationalMode.Scheme то выдаем отсортированные по ID параметры basement + mast
                return GetProperties().Concat(basement_props).OrderBy(n => n.ID).ToAdapterProperty();
            else
                //в противном случае только отсортированные по ID параметры mast
                return GetProperties().OrderBy(n => n.ID).ToAdapterProperty();
        }

        [CommandMethod("insert_mast", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
        public static void insert_mast()
        {
            using (InputJig input = new InputJig())
            {
                input.AutoSnap = AutoSnapMode.Magnet;
                input.SnapMode = OsnapModeMask.Center;
                input.SnapOverrideMode = InputJig.OsnapOverrideMode.Override;

                input.DashLine = true;
                input.AutoHighlight = false;

                MastHandler last_mast = null;

                while (true)
                {
                    MastHandler mast = new MastHandler();
                    FoundationHandler basement = new FoundationHandler();

                    mast.PlaceObject(Point3d.Origin, Vector3d.XAxis);
                    basement.PlaceObject(Point3d.Origin, Vector3d.XAxis, mast);

                    input.ExcludeObjects(new McObjectId[] { mast.ID, basement.ID });

                    input.MouseMove = (s, a) =>
                    {
                        mast.TransformBy(Matrix3d.Displacement(mast.Position.GetVectorTo(a.Point)));

                        if (last_mast != null)
                        {
                            double angle = last_mast.Position
                                            .GetVectorTo(a.Point)
                                            .GetAngleTo(mast.Direction, Vector3d.ZAxis);

                            mast.TransformBy(Matrix3d.Rotation(-angle, Vector3d.ZAxis, mast.Position));
                        }

                        mast.DbEntity.Update();
                        basement.DbEntity.Update();
                    };

                    InputResult result = null;

                    if (last_mast == null)
                        result = input.GetPoint("Укажите точку для размещения обьекта:");
                    else
                        result = input.GetDistance("Укажите точку для размещения обьекта:", last_mast.Position);

                    if (result.Result != InputResult.ResultCode.Normal)
                    {
                        McObjectManager.Erase(mast.ID);

                        return;
                    }

                    last_mast = mast;
                }
            }
        }
    }
}
