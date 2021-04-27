using BasicFoundations;

using CatenaryCAD.Attributes;
using CatenaryCAD.Components;
using CatenaryCAD.Geometry;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Geometry.Shapes;
using CatenaryCAD.Helpers;
using CatenaryCAD.Models;
using CatenaryCAD.Models.Events;
using CatenaryCAD.Properties;

using System;
using System.Linq;
using System.Collections.Generic;

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
            switch (from)
            {
                case IAnchor anchor:
                    var mast_position = new Point2D(Position.X, Position.Y);
                    var mast_direction = new Vector2D(Direction.X, Direction.Y);

                    var anchor_position = new Point2D(anchor.Position.X, anchor.Position.Y);

                    List<Point2D> intersections = new List<Point2D>();


                    foreach (var shape in GetGeometry().DeepClone())
                    {
                        //трансформируем геометрию из нулевых координат

                        Matrix2D translation_before = Matrix2D.CreateTranslation(Point2D.Origin.GetVectorTo(mast_position));
                        Matrix2D rotation_before = Matrix2D.CreateRotation(-mast_direction.GetAngleTo(Vector2D.AxisX), mast_position);
                        shape.TransformBy(rotation_before * translation_before);

                        //получаем пересечения
                        intersections.AddRange(new Ray2D(anchor_position, anchor_position.GetVectorTo(mast_position)).GetIntersections(shape));

                        var center = new Point2D((shape.Vertices[0].X + shape.Vertices[1].X + shape.Vertices[2].X + shape.Vertices[3].X) / 4,
                                                 (shape.Vertices[0].Y + shape.Vertices[1].Y + shape.Vertices[2].Y + shape.Vertices[3].Y) / 4);

                        shape.TransformBy(Matrix2D.CreateScale(new Vector2D(1.0001d, 1.0001d), center));

                        Triangle[] triangles = new Triangle[]{ new Triangle(shape.Vertices[1], shape.Vertices[2], center),
                                                               new Triangle(shape.Vertices[3], shape.Vertices[0], center) };

                        var inside = triangles.Where((t) => t.IsInside(intersections[0]))
                            .Select((t) => new Point2D((t.Vertices[0].X + t.Vertices[1].X) / 2, (t.Vertices[0].Y + t.Vertices[1].Y) / 2)).ToArray();

                        intersections = new List<Point2D>();
                        intersections.AddRange(inside);

                    }


                    if (intersections.Count > 0)
                        return true;
                    else
                        return false;

                default:
                    return false;
            }
        }


        public override Point2D? GetDockingPoint(IModel from, Ray2D ray)
        {
            switch (from)
            {
                case IAnchor anchor:
                    var position = new Point2D(Position.X, Position.Y);
                    var direction = new Vector2D(Direction.X, Direction.Y);

                    List<Point2D> intersections = new List<Point2D>();


                    foreach (var shape in GetGeometry().DeepClone())
                    {
                        //трансформируем геометрию из нулевых координат

                        Matrix2D translation_before = Matrix2D.CreateTranslation(Point2D.Origin.GetVectorTo(position));
                        Matrix2D rotation_before = Matrix2D.CreateRotation(-direction.GetAngleTo(Vector2D.AxisX), position);
                        shape.TransformBy(rotation_before * translation_before);

                        //получаем пересечения
                        intersections.AddRange(ray.GetIntersections(shape));

                        var center = new Point2D((shape.Vertices[0].X + shape.Vertices[1].X + shape.Vertices[2].X + shape.Vertices[3].X) / 4,
                                                 (shape.Vertices[0].Y + shape.Vertices[1].Y + shape.Vertices[2].Y + shape.Vertices[3].Y) / 4);

                        shape.TransformBy(Matrix2D.CreateScale(new Vector2D(1.0001d, 1.0001d), center));

                        Triangle[] triangles = new Triangle[]{ new Triangle(shape.Vertices[0], shape.Vertices[1], center),
                                                               new Triangle(shape.Vertices[1], shape.Vertices[2], center),
                                                               new Triangle(shape.Vertices[2], shape.Vertices[3], center),
                                                               new Triangle(shape.Vertices[3], shape.Vertices[0], center) };

                        var inside = triangles.Where((t) => t.IsInside(intersections[0]))
                            .Select((t) => new Point2D((t.Vertices[0].X + t.Vertices[1].X)/2, (t.Vertices[0].Y + t.Vertices[1].Y) / 2)).ToArray();

                        intersections = new List<Point2D>();
                        intersections.AddRange(inside);

                    }

                    
                    if (intersections.Count > 0)
                    {
                        //выбираем близжайнее пересечение
                        Point2D intersection = intersections[0];
                        Point2D control_point = ray.Origin + ray.Direction;

                        foreach (var point in intersections)
                            if (point.GetDistanceTo(control_point) < intersection.GetDistanceTo(control_point))
                                intersection = point;
                        return intersection;
                    }
                    else
                        return null;

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
