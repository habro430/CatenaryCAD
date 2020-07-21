using CatenaryCAD.Properties;
using Multicad;
using Multicad.DatabaseServices;
using Multicad.Geometry;
using Multicad.Runtime;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using static CatenaryCAD.Extensions;

namespace CatenaryCAD.Objects
{
    [Serializable]
    [CustomEntity("{742ECCF0-0CEC-4791-B4BE-4E3568E2C43E}", "MAST", "Опора контактной сети")]
    internal sealed class MastHandler : AbstractHandler
    {
        [NonSerialized]
        private static readonly Dictionary<string, Type> Masts;

        static MastHandler()
        {
            //при первом вызове класса кэшируем в словарь производные от IMast опоры в статику
            //получаем все не абстрактные обьекты производные от IMast и имеющие атрибут CatenaryObjectAttribute
            var masts = Main.CachedCatenaryObjects
                .Where(abstr => !abstr.IsAbstract)
                .Where(interf => interf.GetInterface(typeof(IMast).FullName) != null)
                .Where(attr => Attribute.IsDefined(attr, typeof(CatenaryObjectAttribute), false));

            //получем словарь из CatenaryObjectAttribute и типа производного от IMast класса
            Masts = masts.Select((type) => new
            {
                type,
                atrr = type.GetCustomAttributes(typeof(CatenaryObjectAttribute), false)
                            .FirstOrDefault() as CatenaryObjectAttribute
            }).ToDictionary(p => p.atrr.Name, p => p.type);

        }        
        public MastHandler()
        {
            Property<Type> mast_type = new Property<Type>("01_mast_type", "Тип стойки", "Стойка", ConfigFlags.RefreshAfterChange);

            mast_type.DictionaryValues = Masts;

            mast_type.Updated += (type) =>
            {
                if (!TryModify()) return;
                CatenaryObject = (IMast)Activator.CreateInstance(type);
                CatenaryObject.Updated += () => { if (!TryModify()) return; };
            };

            mast_type.Value = mast_type.DictionaryValues.Values.FirstOrDefault();

            Properties.Add(mast_type);
        }

        public override ICollection<McDynamicProperty> GetProperties(out bool exclusive)
        {
            exclusive = true;

            if (CatenaryObject != null)
            {
                var mast_props = CatenaryObject.GetProperties();
                if (mast_props != null)
                {
                    var basement_props = Childrens.Where(child => child is BasementHandler).FirstOrDefault().Properties;
                    NatureType viewtype = (NatureType)(McDocument.ActiveDocument.CustomProperties["viewtype"] ?? NatureType.Line);

                    if (basement_props != null && viewtype == NatureType.Line)
                        return Properties.Concat(mast_props).Concat(basement_props).OrderBy(n => n.ID).ToArray().ToAdapterProperty();
                    else
                        return Properties.Concat(mast_props).OrderBy(n => n.ID).ToArray().ToAdapterProperty();
                }
            }
            return Properties.OrderBy(n => n.ID).ToArray().ToAdapterProperty();
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
                    BasementHandler basement = new BasementHandler();

                    mast.PlaceObject(Point3d.Origin, Vector3d.XAxis);
                    basement.PlaceObject(Point3d.Origin, Vector3d.XAxis, mast);

                    input.ExcludeObjects(new McObjectId[] { mast.ID, basement.ID });

                    input.MouseMove = (s, a) =>
                    {
                        mast.Transform(Matrix3d.Displacement(mast.Position.GetVectorTo(a.Point)));
                        
                        if (last_mast != null)
                        {
                            double angle = last_mast.Position
                                            .GetVectorTo(a.Point)
                                            .GetAngleTo(mast.Direction, Vector3d.ZAxis);

                            mast.Transform(Matrix3d.Rotation(-angle, Vector3d.ZAxis, mast.Position));
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
