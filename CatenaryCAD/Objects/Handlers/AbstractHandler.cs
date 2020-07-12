using CatenaryCAD.Geometry;
using CatenaryCAD.Properties;

using Multicad;
using Multicad.CustomObjectBase;
using Multicad.DatabaseServices;
using Multicad.Geometry;

using System;
using System.Collections.Generic;
using System.Linq;
using static CatenaryCAD.Extensions;

namespace CatenaryCAD.Objects
{
    [Serializable]
    internal abstract class AbstractHandler : McCustomBase, IMcDynamicProperties
    {
        /// <summary>
        /// Обьект CatenaryCAD
        /// </summary>
        public IObject CatenaryObject;

        private McObjectId parent = McObjectId.Null;
        private ConcurrentHashSet<McObjectId> childrens = new ConcurrentHashSet<McObjectId>();

        /// <summary>
        /// Родительскисй объект для текущего
        /// </summary>
        public McObjectId ParentID { get => parent; }
        /// <summary>
        /// Дочерние объекты для текущего
        /// </summary>
        public McObjectId[] ChildrensID { get => childrens.ToArray(); }


        public bool AddChild(AbstractHandler handler)
        {
            var answer = childrens.Add(handler.ID);
            if(answer) handler.parent = ID;

            return answer;
        }
        public bool RemoveChild(AbstractHandler handler)
        {
            var answer = childrens.TryRemove(handler.ID);
            if (answer) handler.parent = McObjectId.Null;

            return answer;
        }

        public override List<McObjectId> GetDependent() => childrens.ToList();

        private Point3d position = Point3d.Origin;
        private Vector3d direction = Vector3d.XAxis;

        /// <summary>
        /// Позиция текущего объекта
        /// </summary>
        public Point3d Position
        {
            get => position;
            set
            {
                if (!TryModify()) return;

                position = value;
            }
        }

        /// <summary>
        /// Направление текущего объекта
        /// </summary>
        public Vector3d Direction
        {
            get => direction;
            set
            {
                if (!TryModify()) return;

                direction = value.GetNormal();
            }
        }

        /// <summary>
        /// Список параметров объекта
        /// </summary>
        public List<IProperty> Properties = new List<IProperty>();

        public override hresult PlaceObject()
        {
            hresult result = base.PlaceObject();
            DbEntity.AddToCurrentDocument();

            return result;
        }
        public virtual hresult PlaceObject(Point3d position, Vector3d direction)
        {
            Position = position;
            Direction = direction;

            return PlaceObject();
        }
        public virtual hresult PlaceObject(Point3d position, Vector3d direction, AbstractHandler parent)
        {
            parent.AddChild(this);

            return PlaceObject(position, direction);
        }
        
        public override void OnErase()
        {
            if (!ParentID.IsNull)
                ParentID.GetObjectOfType<AbstractHandler>().RemoveChild(this);

            foreach (var child in childrens)
                McObjectManager.Erase(child);
        }

        /// <summary>
        /// Функция вызываемая событием при пользовательской трансформации обьекта
        /// </summary>
        /// <param name="tfm"></param>
        public override void OnTransform(Matrix3d tfm) =>Transform(tfm);

        /// <summary>
        /// Функция транформации объекта
        /// </summary>
        /// <param name="m"></param>
        public virtual void Transform(Matrix3d m)
        {
            if (!TryModify())
                return;

            direction = direction.TransformBy(m);
            position = position.TransformBy(m);

            if (!ID.IsNull)
            {
                foreach (var child in childrens)
                    (McObjectManager.GetObject(child) as AbstractHandler).Transform(m);
            }
        }

        public override bool GetECS(out Matrix3d tfm)
        {
            double angle = Direction.GetAngleTo(Vector3d.XAxis,Vector3d.ZAxis);
            tfm = Matrix3d.Displacement(Position.GetAsVector()).PostMultiplyBy(
                 Matrix3d.Rotation(-angle, Vector3d.ZAxis, Point3d.Origin));

            return true;
        }

        public override bool OnGetOsnapPoints(OsnapMode osnapMode, Point3d pickPoint, Point3d lastPoint, List<Point3d> osnapPoints)
        {
            switch (osnapMode)
            {
                case OsnapMode.Center:
                    osnapPoints.Add(Position);
                    break;
            }
            return true;
        }

        public override void OnDraw(GeometryBuilder dc)
        {
            dc.Clear();

            dc.Color = Multicad.Constants.Colors.ByObject;
            dc.LineType = Multicad.Constants.LineTypes.ByObject;

            if (CatenaryObject != null)
            {
                NatureType viewtype = (NatureType)(McDocument.ActiveDocument.CustomProperties["viewtype"] ?? 
                                         NatureType.Line);

                AbstractGeometry<XY>[] geometry_xy;
                AbstractGeometry<XYZ>[] geometry_xyz;

                CatenaryObject.GetGeometry(out geometry_xy, out geometry_xyz);

                switch (viewtype)
                {
                    case NatureType.Line:

                        if (geometry_xy != null)
                        {
                            for (int igeom = 0; igeom < geometry_xy.Length; igeom++)
                            {
                                var geometry = geometry_xy[igeom];

                                for (int iedge = 0; iedge < geometry.Edges.Length; iedge++)
                                {
                                    dc.DrawLine(geometry.Edges[iedge].Item1.ToMCAD(),
                                                geometry.Edges[iedge].Item2.ToMCAD());
                                }
                            }
                        }

                        break;

                    case NatureType.Polygon:

                        if (geometry_xyz != null)
                        {
                            for (int igeom = 0; igeom < geometry_xyz.Length; igeom++)
                            {
                                var geometry = geometry_xyz[igeom];

                                for (int iedge = 0; iedge < geometry.Edges.Length; iedge++)
                                {
                                    dc.DrawLine(geometry.Edges[iedge].Item1.ToMCAD(),
                                                geometry.Edges[iedge].Item2.ToMCAD());

                                }
                            }
                        }

                        break;
                }
            }
            else
            {
                dc.DrawText(new TextGeom("ERR OBJ", Point3d.Origin, Vector3d.XAxis, HorizTextAlign.Center, VertTextAlign.Center, "normal", 1000, 1000));
            }

        }

        public ICollection<McDynamicProperty> GetProperties(out bool exclusive)
        {
            exclusive = true;

            if (CatenaryObject != null)
            {
                var props = CatenaryObject.GetProperties();

                if (props != null)
                {
                    return Properties.Concat(props).OrderBy(n => n.ID).ToArray().ToAdapterProperty();
                }
            }
                
            return Properties.OrderBy(n => n.ID).ToArray().ToAdapterProperty();
        }
        public McDynamicProperty GetProperty(string id)
        {

            //for (int i = 0; i < Properties.Count; i++)
            //{
            //    if (Properties[i].ID == id)
            //        return Properties[i].ToAdapterProperty();
            //}

            return null;
        }
    }
}
