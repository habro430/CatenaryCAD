using BasicFoundations;
using CatenaryCAD.Attributes;
using CatenaryCAD.Components;
using CatenaryCAD.Geometry;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Geometry.Shapes;
using CatenaryCAD.Models;
using CatenaryCAD.Models.Events;
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
        //при первом вызове класса кэшируем в словарь производные от него опоры в статику
        [NonSerialized]
        private static readonly Dictionary<string, Type> InheritedMasts = AbstractMast.GetInheritedMastsFor(typeof(Metall));
        private static readonly Type[] allowablefoundations = new Type[] { typeof(FoundationDirect) };
        //генерируем геометрию для 2D режима
        private static readonly IShape[] geometry2d = new IShape[] { new Rectangle(new Point2D(), 600, 300) };
        //генерируем стандартные значения длинны стойки
        private static readonly Dictionary<string, int> defaultlenghts = new Dictionary<string, int> { ["10.0 м"] = 10000, ["12.0 м"] = 12000, ["15.0 м"] = 15000 };

        public override Type[] AllowableFoundations => allowablefoundations;
        public override IShape[] GetGeometry() => geometry2d;

        public Metall()
        {
            PropertiesDictionary.TryAdd("mast_type", InheritedMasts.Count > 0 ? new Property<Type>("Марка стойки", "Стойка", InheritedMasts, attr: Attributes.RefreshAfterChange) as IProperty : 
                                                                                new Property<string>("Марка стойки", "Стойка", string.Empty) as IProperty);

            PropertiesDictionary.TryAdd("mast_lenght", new Property<int>("Длинна", "Стойка", defaultlenghts, UpdateLenght, Attributes.RefreshAfterChange));
        }

        private void UpdateLenght(int lenght)
        {
            if (!(bool?)EventInvoke(this, new TryModify())?.Value ?? false) return;

            switch (lenght)
            {
                case 10000:
                    Component m10 = new Component(new IMesh[] { GetOrCreateFromCache("m_10") });
                    ComponentsDictionary.AddOrUpdate("mast", m10, (name, component) => m10);
                    break;
                case 12000:
                    Component m12 = new Component(new IMesh[] { GetOrCreateFromCache("m_12") });
                    ComponentsDictionary.AddOrUpdate("mast", m12, (name, component) => m12);
                    break;
                case 15000:
                    Component m15 = new Component(new IMesh[] { GetOrCreateFromCache("m_15") });
                    ComponentsDictionary.AddOrUpdate("mast", m15, (name, component) => m15);
                    break;
            }

        }
        public override bool CheckAvailableDocking(IModel from)
        {
            var mast_position = new Point2D(Position.X, Position.Y);
            var mast_direction = new Vector2D(Direction.X, Direction.Y);

            switch (from)
            {
                case IAnchor anchor:
                    var anchor_position = new Point2D(anchor.Position.X, anchor.Position.Y);
                    Matrix2D anchor_matrix = Matrix2D.CreateTranslation(mast_position.GetVectorTo(Point2D.Origin)) *
                                             Matrix2D.CreateRotation(mast_direction.GetAngleTo(Vector2D.AxisX), mast_position);

                    var anchor_intersection = GetDockingPoint(from, new Ray2D(anchor_position, anchor_position.GetVectorTo(mast_position))).Value.TransformBy(anchor_matrix);
                    return Math.Abs(anchor_intersection.X) > Math.Abs(anchor_intersection.Y);

                case IBracket bracket:
                    var bracket_position = new Point2D(bracket.Position.X, bracket.Position.Y);
                    Matrix2D bracket_matrix = Matrix2D.CreateTranslation(mast_position.GetVectorTo(Point2D.Origin)) *
                                              Matrix2D.CreateRotation(mast_direction.GetAngleTo(Vector2D.AxisX), mast_position);

                    var bracket_intersection = GetDockingPoint(from, new Ray2D(bracket_position, bracket_position.GetVectorTo(mast_position))).Value.TransformBy(bracket_matrix);
                    return Math.Abs(bracket_intersection.X) < Math.Abs(bracket_intersection.Y);

                default:
                    return false;
            }
        }

        public override Point2D? GetDockingPoint(IModel from, Ray2D ray)
        {
            var mast_position = new Point2D(Position.X, Position.Y);
            var mast_direction = new Vector2D(Direction.X, Direction.Y);
            var control_point = ray.Origin + ray.Direction;

            Matrix2D matrix = Matrix2D.CreateRotation(-mast_direction.GetAngleTo(Vector2D.AxisX), mast_position) *
                              Matrix2D.CreateTranslation(Point2D.Origin.GetVectorTo(mast_position));

            var intersection = GetGeometry().SelectMany(shape => ray.GetIntersections(shape.TransformBy(matrix)))
                                            .Aggregate((first, second) => first.GetDistanceTo(control_point) < second.GetDistanceTo(control_point) ? first : second);

            switch (from)
            {
                case IAnchor anchor:
                case IBracket bracket:
                    var edges = GetGeometry().SelectMany(shape => shape.Indices.Select(index => new Line(shape.Vertices[index[0]], shape.Vertices[index[1]]).TransformBy(matrix)));                    
                    return (edges.Where(line => line.IsInside(intersection)).Single() as Line).GetMiddlePoint();

                default:
                    return null;
            }
        }

        public override Point3D? GetDockingPoint(IModel from, Ray3D ray)
        {
            throw new NotImplementedException();
        }


    }
}
