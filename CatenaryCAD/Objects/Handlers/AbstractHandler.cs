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

        private McObjectId parentid = McObjectId.Null;
        private ConcurrentHashSet<McObjectId> childrensid = new ConcurrentHashSet<McObjectId>();
        private ConcurrentHashSet<McObjectId> dependentsid = new ConcurrentHashSet<McObjectId>();

        /// <summary>
        /// Родительскисй объект для данного объекта
        /// </summary>
        public AbstractHandler Parent { 
            get => parentid.GetObjectOfType<AbstractHandler>(); 
        
        }
        /// <summary>
        /// Дочерние объекты для данного объекта
        /// </summary>
        public AbstractHandler[] Childrens { 
            get => childrensid
                .Select(obj => obj.GetObjectOfType<AbstractHandler>())
                .ToArray(); 
        }

        /// <summary>
        /// Зависимые объекты от данного объекта
        /// </summary>
        public AbstractHandler[] Dependents { 
            get => dependentsid
                .Select(obj => obj.GetObjectOfType<AbstractHandler>())
                .ToArray();
        }

        public override List<McObjectId> GetDependent() => childrensid.Concat(dependentsid).ToList();

        /// <summary>
        /// Добавить дочерний обьект
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        public bool AddChild(AbstractHandler handler)
        {
            var answer = childrensid.Add(handler.ID);
            if(answer) handler.parentid = ID;

            return answer;
        }
        /// <summary>
        /// Удалить дочерний обьект
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        public bool RemoveChild(AbstractHandler handler)
        {
            var answer = childrensid.TryRemove(handler.ID);
            if (answer) handler.parentid = McObjectId.Null;

            return answer;
        }
        
        /// <summary>
        /// Добавить зависимый обьект
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        public bool AddDependent(AbstractHandler handler) => dependentsid.Add(handler.ID);

        /// <summary>
        /// Удалить зависимый обьект
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        public bool RemoveDependent(AbstractHandler handler) => dependentsid.TryRemove(handler.ID);



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
            //set => Transform(Matrix3d.Displacement(position.GetVectorTo(value)));
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
        /// Список параметров AbstractHandler
        /// </summary>
        protected List<IProperty> Properties = new List<IProperty>();
        /// <summary>
        /// Возвращает сложенные списки параметров IObject и AbstractHandler
        /// </summary>
        /// <returns></returns>
        public IProperty[] GetPropertiesComplex()
        {
            if (CatenaryObject != null)
            {
                var props = CatenaryObject.GetProperties();
                if (props != null)
                    return Properties.Concat(props).OrderBy(n => n.ID).ToArray();
            }
            return Properties.OrderBy(n => n.ID).ToArray();
        }

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
        
        /// <summary>
        /// Функция вызываема событием при удалении обьекта из документа
        /// </summary>
        public override void OnErase()
        {
            if (!parentid.IsNull)
                Parent.RemoveChild(this);

            foreach (var child in childrensid)
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
                foreach (var child in childrensid)
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
        public virtual ICollection<McDynamicProperty> GetProperties(out bool exclusive)
        {
            exclusive = true;
            return GetPropertiesComplex().ToAdapterProperty();
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
