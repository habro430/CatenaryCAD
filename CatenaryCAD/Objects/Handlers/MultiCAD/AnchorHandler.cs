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

namespace CatenaryCAD.Models.Handlers
{
    [Serializable]
    [CustomEntity("{2B1A1855-B183-4864-88F9-330AB06801EF}", "ANCHOR", "Анкер опоры контактной сети")]
    internal sealed class AnchorHandler : Handler
    {
        [NonSerialized]
        private static readonly Type[] Anchors;
        static AnchorHandler() => Anchors = Main.GetCatenaryObjects(typeof(IAnchor));
        public AnchorHandler()
        {
            Property<Type> anchor_type = new Property<Type>("01_anchor_type", "Тип анкера", "Анкер", ConfigFlags.RefreshAfterChange);

            anchor_type.DictionaryValues = Anchors
                .Where((type) => type.GetCustomAttributes(typeof(ModelNonBrowsableAttribute), false).FirstOrDefault() == null)
                .Select((type) => new
                {
                    type,
                    atrr = (type.GetCustomAttributes(typeof(ModelNameAttribute), false)
                               .FirstOrDefault() as ModelNameAttribute ?? new ModelNameAttribute(type.Name)).Name
                }).ToDictionary(p => p.atrr, p => p.type);

            anchor_type.Updated += (type) =>
            {
                if (!TryModify()) return;

                IIdentifier identifier = Model?.Identifier ?? new McIdentifier(ID);
                Point3D position = Model?.Position ?? Point3D.Origin;
                Vector3D direction = Model?.Direction ?? Vector3D.AxisX;

                IModel parent = Model?.Parent;

                var anchor = Activator.CreateInstance(type) as Model;

                anchor.Identifier = identifier;
                anchor.Position = position;
                anchor.Direction = direction;

                anchor.Parent = parent;

                anchor.TryModifyHandler += () => TryModify();
                anchor.UpdateHandler += () => DbEntity.Update();

                Model = anchor;
            };

            anchor_type.Value = anchor_type.DictionaryValues.Values.FirstOrDefault();

            properties.Add(anchor_type);
        }
        public override void OnTransform(Matrix3d tfm)
        {
            if (Model.Parent == null)
            {
                if (ID.IsNull)
                    return;

                TransformBy(tfm);
            }
        }
        [CommandMethod("insert_anchor", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
        public static void insert_anchor()
        {
            Handler selected_obj;
            do
            {
                selected_obj = (McObjectManager.SelectObject("Выберите ОПОРУ для размещения АНКЕРОВКИ:").GetObject() as Handler);

                if (selected_obj is MastHandler mast)
                {
                    AnchorHandler anchor = new AnchorHandler();
                    anchor.PlaceObject(mast.Position, Vector3d.XAxis);

                    using (InputJig input = new InputJig())
                    {
                        input.AutoSnap = AutoSnapMode.None;
                        input.SnapMode = OsnapModeMask.Center;
                        input.SnapOverrideMode = InputJig.OsnapOverrideMode.Override;

                        input.DashLine = true;
                        input.AutoHighlight = false;

                        input.ExcludeObjects(new McObjectId[]{ anchor.ID, mast.ID }); 

                        input.MouseMove = (s, a) =>
                        {
                            var shapes = mast.Model.GetGeometryForScheme();

                            foreach(var shape in shapes)
                            {
                                Point2D point = a.Point.ToCatenaryCAD_2D();
                                Vector2D vector = a.Point.GetVectorTo(mast.Position).ToCatenaryCAD_2D();

                                Ray2D ray = new Ray2D(point, vector - vector.Normalize()) ;

                                var transofrmed_shape = 
                                    shape.TransformBy(Matrix2D.CreateTranslation(mast.Position.ToCatenaryCAD_2D()));


                                Point2D[] intersections;
                                if(ray.GetIntersections(transofrmed_shape, out intersections))
                                {
                                    Point2D result = intersections[0];
                                    Point2D control_point = ray.Origin + ray.Direction;
                                    foreach (var intersection in intersections)
                                    {
                                        if (intersection.DistanceTo(control_point) < result.DistanceTo(control_point))
                                            result = intersection;
                                    }
                                    anchor.Position = result.ToMultiCAD();

                                    anchor.Direction = anchor.Position.GetVectorTo(a.Point);
                                    anchor.DbEntity.Update();
                                }

                            }
                        };

                        InputResult result = input.GetPoint("Выберите направление для размещения обьекта:");
                        if (result.Result != InputResult.ResultCode.Normal) McObjectManager.Erase(anchor.ID);

                    }
                }

            } while (selected_obj != null);
        }
    }
}
