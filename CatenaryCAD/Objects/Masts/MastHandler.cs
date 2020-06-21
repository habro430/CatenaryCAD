using CatenaryCAD.Geometry;
using CatenaryCAD.Geometry.Core;
using CatenaryCAD.Objects.Masts;
using CatenaryCAD.Properties;
using Multicad;
using Multicad.CustomObjectBase;
using Multicad.DatabaseServices;
using Multicad.Geometry;
using Multicad.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using static CatenaryCAD.Extensions;

namespace CatenaryCAD.Objects
{
    [CustomEntity("{742ECCF0-0CEC-4791-B4BE-4E3568E2C43E}", "MAST", "Опора контактной сети")]
    [Serializable]

    internal sealed class MastHandler : AbstractHandler, IMcDynamicProperties
    {
        AbstractMast Mast;

        [NonSerialized]
        private static readonly Dictionary<string, Type> InheritedMasts;
        //при первом вызове класса кэшируем в словарь производные от AbstarctMast опоры в статику
        static MastHandler() => InheritedMasts =  AbstractMast.GetInheritedMastsFor(typeof(AbstractMast));

        
        public MastHandler()
        {
            Property<Type> mast_type = new Property<Type>("01_mast_type", "Тип стойки", "Стойка", PropertyFlags.RefreshAfterChange);

            mast_type.DictionaryValues = InheritedMasts;

            mast_type.Updated += mast_type_updated;
            mast_type.Value = mast_type.DictionaryValues.Values.FirstOrDefault();

            Properties.Add(mast_type);
        }

        private void mast_type_updated(Type type)
        {
            if (!TryModify()) return;
            Mast = (AbstractMast)Activator.CreateInstance(type);

            Mast.Updated += () => { if (!TryModify()) return; };
        }


        public override void OnDraw(GeometryBuilder dc)
        {
            dc.Clear();

            dc.Color = Multicad.Constants.Colors.ByObject;
            dc.LineType = Multicad.Constants.LineTypes.ByObject;

            if (Mast != null)
            {
                ViewType viewtype = (ViewType)(McDocument.ActiveDocument.CustomProperties["viewtype"] ?? ViewType.Geometry2D);
                AbstractGeometry[] geometryarr = Mast.GetGeometry(viewtype);
                if (geometryarr != null)
                {
                    foreach (var geometry in geometryarr)
                    {
                        foreach (var edge in geometry.Edges)
                        {
                            dc.DrawLine(geometry.Vertices[edge.Item1].ToMultiCAD(), geometry.Vertices[edge.Item2].ToMultiCAD());
                        }
                    }
                }
                else
                    dc.DrawText(new TextGeom("ERROR", Point3d.Origin, Vector3d.XAxis, HorizTextAlign.Center, VertTextAlign.Center, "normal", 1000, 1000));

            }
            else
            {
                dc.DrawText(new TextGeom("ERROR", Point3d.Origin, Vector3d.XAxis, HorizTextAlign.Center, VertTextAlign.Center, "normal", 1000, 1000));
            }

        }
        public override bool OnGetOsnapPoints(OsnapMode osnapMode, Point3d pickPoint, Point3d lastPoint, List<Point3d> osnapPoints)
        {
           switch(osnapMode)
            {
                case OsnapMode.Center:
                    osnapPoints.Add(Position);
                    break;
            }
            return true;
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

                List<MastHandler> placed_masts = new List<MastHandler>();
                while (true)
                {
                    MastHandler mast = new MastHandler();
                    mast.PlaceObject(Point3d.Origin, Vector3d.XAxis);

                    input.ExcludeObject(mast.ID);

                    input.MouseMove = (s, a) =>
                    {
                        mast.Position = a.Point;

                        if (placed_masts.Count != 0)
                        {
                            mast.Direction = placed_masts[placed_masts.Count - 1].Position.GetVectorTo(a.Point);

                            if (placed_masts.Count >1)
                                placed_masts[placed_masts.Count - 1].Direction = (placed_masts[placed_masts.Count - 2].Direction + mast.Direction) / 2;
                            else
                                placed_masts[placed_masts.Count - 1].Direction = mast.Direction;

                            placed_masts[placed_masts.Count - 1].DbEntity.Update();

                        }

                        mast.DbEntity.Update();
                    };

                    InputResult result = null;

                    if (placed_masts.Count == 0)
                        result = input.GetPoint("Укажите точку для размещения обьекта:");
                    else
                        result = input.GetDistance("Укажите точку для размещения обьекта:", placed_masts.Last().Position);

                    if (result.Result != InputResult.ResultCode.Normal)
                    {
                        McObjectManager.Erase(mast.ID);
                        return;
                    }

                    placed_masts.Add(mast);
                }
            }
        }

        public ICollection<McDynamicProperty> GetProperties(out bool exclusive)
        {
            exclusive = true;
            
            if (Mast != null)
            {
                return Properties
                    .Concat(Mast.GetProperties())                    
                    .OrderBy(n => n.ID).ToArray().ToAdapterProperty();
            }
            else
                return Properties.OrderBy(n => n.ID).ToArray().ToAdapterProperty();
        }

        public McDynamicProperty GetProperty(string id)
        {
            foreach (var prop in Properties)
            {
                if (prop.ID == id) 
                    return prop.ToAdapterProperty();
            }
            return null;
        }

    }
}
