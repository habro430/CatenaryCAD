using CatenaryCAD.Geometry;
using CatenaryCAD.Models.Attributes;
using CatenaryCAD.Properties;
using Multicad;
using Multicad.DatabaseServices;
using Multicad.Geometry;
using Multicad.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using static CatenaryCAD.Extensions;

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
            Property<Type> mast_type = new Property<Type>("01_mast_type", "Тип стойки", "Стойка", props: ConfigFlags.RefreshAfterChange);

            mast_type.DictionaryValues = Masts
                .Where((type) => type.GetCustomAttributes(typeof(ModelNonBrowsableAttribute), false).FirstOrDefault() == null)
                .Select((type) => new
                {
                    type,
                    atrr = (type.GetCustomAttributes(typeof(ModelNameAttribute), false)
                               .FirstOrDefault() as ModelNameAttribute ?? new ModelNameAttribute(type.Name)).Name
                }).ToDictionary(p => p.atrr, p => p.type);


            mast_type.Updated += (type) => Model = Activator.CreateInstance(type) as Mast;
            mast_type.Value = mast_type.DictionaryValues.Values.FirstOrDefault();

            properties.Add(mast_type);
        }

        public override ICollection<McDynamicProperty> GetProperties(out bool exclusive)
        {
            exclusive = true;

            var basement = Model.Childrens.Select(t => (t.Identifier as McIdentifier).GetHandler())
                                    .Where(child => child is FoundationHandler).FirstOrDefault();

            var basement_props =  basement?.GetProperties();

            //проверяем режим работы если режим работы не определен то по умолчанию используем OperationalMode.Scheme
            OperationalMode mode = (OperationalMode)(McDocument.ActiveDocument.CustomProperties["OperationalMode"] ?? OperationalMode.Scheme);

            if (basement_props != null && mode == OperationalMode.Scheme)
                //если режим работы OperationalMode.Scheme то выдаем отсортированные по ID параметры basement + mast
                return GetProperties().Concat(basement_props).OrderBy(n => n.Identifier).ToAdapterProperty();
            else
                //в противном случае только отсортированные по ID параметры mast
                return GetProperties().OrderBy(n => n.Identifier).ToAdapterProperty();
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

                Mast last_mast = null;

                while (true)
                {
                    var mhandler = new MastHandler();
                    var fhandler = new FoundationHandler();

                    mhandler.PlaceObject(Point3d.Origin, Vector3d.XAxis);
                    fhandler.PlaceObject(Point3d.Origin, Vector3d.XAxis);

                    Mast mast = mhandler.Model as Mast;
                    Foundation foundation = fhandler.Model as Foundation;

                    foundation.Parent = mast;

                    input.ExcludeObjects(new McObjectId[] { mhandler.ID, fhandler.ID });

                    input.MouseMove = (s, a) =>
                    {
                        Point3D mouse = a.Point.ToCatenaryCAD_3D();

                        mast.TransformBy(Matrix3D.CreateTranslation(mast.Position.VectorTo(mouse)));

                        if (last_mast != null)
                        {
                            double angle = last_mast.Position.VectorTo(mouse).AngleTo(mast.Direction, Vector3D.AxisZ);
                            mast.TransformBy(Matrix3D.CreateRotation(-angle, mast.Position, Vector3D.AxisZ));
                        }

                        mast.SendMessageToHandler(HandlerMessages.Update);
                        foundation.SendMessageToHandler(HandlerMessages.Update);
                    };

                    InputResult result = null;

                    if (last_mast == null)
                        result = input.GetPoint("Укажите точку для размещения обьекта:");
                    else
                        result = input.GetDistance("Укажите точку для размещения обьекта:", last_mast.Position.ToMultiCAD());

                    if (result.Result != InputResult.ResultCode.Normal)
                    {
                        mast.SendMessageToHandler(HandlerMessages.Remove);
                        return;
                    }

                    last_mast = mast;
                }
            }
        }
    }
}
