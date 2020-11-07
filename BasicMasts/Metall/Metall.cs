using CatenaryCAD.Geometry;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Geometry.Shapes;
using CatenaryCAD.Models;
using CatenaryCAD.Models.Attributes;
using CatenaryCAD.Properties;

using System;
using System.Collections.Generic;
using System.Linq;

namespace BasicMasts
{
    [Serializable]
    [ModelName("Металлическая")]
    [ModelDescription("Представляет базовую модель металлической стойки")]

    public class Metall : AbstractMast
    {
        [NonSerialized]
        private static readonly Dictionary<string, Type> InheritedMasts;
        //при первом вызове класса кэшируем в словарь производные от него опоры в статику
        static Metall() => InheritedMasts = AbstractMast.GetInheritedMastsFor(typeof(Metall));

        public Metall()
        {
            //генериуем геометрию для 2D режима
            Geometry2D = new IShape[] { new Rectangle(new Point2D(), 600, 600) };

            if (InheritedMasts.Count > 0)
            {
                Property<Type> mast_subtype = new Property<Type>("02_mast_metall_type", "Марка стойки", "Стойка", ConfigFlags.RefreshAfterChange);

                mast_subtype.DictionaryValues = InheritedMasts;
                mast_subtype.Value = mast_subtype.DictionaryValues.Values.FirstOrDefault();

                PropertiesSet.Add(mast_subtype);
            }
            else
            {
                Property<string> mast_subtype = new Property<string>("02_mast_metall_type", "Марка стойки", "Стойка");
                mast_subtype.Value = string.Empty;

                PropertiesSet.Add(mast_subtype);
            }

            Property<int> m_len = new Property<int>("03_mast_len", "Длинна", "Стойка", ConfigFlags.RefreshAfterChange);

            m_len.DictionaryValues = new Dictionary<string, int>
            {
                ["10.0 м"] = 10000,
                ["12.0 м"] = 12000,
                ["15.0 м"] = 15000,

            };

            m_len.Updated += (val) =>
            {

                if (!SendMessageToHandler(HandlerMessages.TryModify) ?? false)
                    return;

                //читаем и генериуем геометрию для 3D режима в зависимости от длинны
                switch (val)
                {
                    case 10000:
                        Geometry3D = new IMesh[] { GetOrCreateFromCache("m_10") };
                        break;
                    case 12000:
                        Geometry3D = new IMesh[] { GetOrCreateFromCache("m_12") };
                        break;
                    case 15000:
                        Geometry3D = new IMesh[] { GetOrCreateFromCache("m_15") };
                        break;
                }
            };

            m_len.Value = m_len.DictionaryValues.Values.FirstOrDefault();

            PropertiesSet.Add(m_len);
        }
        //public override Point2D? GetPointForDockingJoint(Ray2D ray)
        //{
        //    var position = new Point2D(Position.X, Position.Y);
        //    var direction = new Vector2D(Direction.X, Direction.Y);

        //    Circle circle = new Circle(position, 200d, 15);

        //    IShape rect = Geometry2D[0].TransformBy(Matrix2D.CreateTranslation(Point2D.Origin.VectorTo(position)))
        //                               .TransformBy(Matrix2D.CreateRotation(-direction.AngleTo(Vector2D.AxisX), position));


        //    Triangle tr0 = new Triangle(position, rect.Edges[0].First, rect.Edges[0].Second);
        //    Triangle tr1 = new Triangle(position, rect.Edges[1].First, rect.Edges[1].Second);
        //    Triangle tr2 = new Triangle(position, rect.Edges[2].First, rect.Edges[2].Second);
        //    Triangle tr3 = new Triangle(position, rect.Edges[3].First, rect.Edges[3].Second);

        //    Point2D[] intersections = ray.GetIntersections(circle);

        //    Point2D intersection = intersections[0];
        //    Point2D control_point = ray.Origin + ray.Direction;

        //    foreach (var point in intersections)
        //        if (point.DistanceTo(control_point) < intersection.DistanceTo(control_point))
        //            intersection = point;

        //    if (tr0.IsInside(intersection)) return new Point2D((tr0.Edges[1].First.X + tr0.Edges[1].Second.X) / 2,
        //                                                       (tr0.Edges[1].First.Y + tr0.Edges[1].Second.Y) / 2);

        //    if (tr1.IsInside(intersection)) return new Point2D((tr1.Edges[1].First.X + tr1.Edges[1].Second.X) / 2,
        //                                                       (tr1.Edges[1].First.Y + tr1.Edges[1].Second.Y) / 2);

        //    if (tr2.IsInside(intersection)) return new Point2D((tr2.Edges[1].First.X + tr2.Edges[1].Second.X) / 2,
        //                                                       (tr2.Edges[1].First.Y + tr2.Edges[1].Second.Y) / 2);

        //    if (tr3.IsInside(intersection)) return new Point2D((tr3.Edges[1].First.X + tr3.Edges[1].Second.X) / 2,
        //                                                       (tr3.Edges[1].First.Y + tr3.Edges[1].Second.Y) / 2);

        //    return intersection;
        //}
        //public override Point3D? GetPointForDockingJoint(Ray3D ray)
        //{
        //    throw new NotImplementedException();
        //}

    }
}
