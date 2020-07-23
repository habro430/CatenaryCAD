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


        #region Parent & Childrens region

        private McObjectId parentid = McObjectId.Null;
        private ConcurrentHashSet<McObjectId> childrensid = new ConcurrentHashSet<McObjectId>();

        /// <summary>
        /// Родительский объект <see cref="AbstractHandler"/>
        /// </summary>
        public AbstractHandler Parent => parentid.GetObjectOfType<AbstractHandler>();

        /// <summary>
        /// Получить дочерние объекты <see cref="AbstractHandler"/>
        /// </summary>
        /// <returns>Массив из дочерних объектов <see cref="AbstractHandler"/>[]</returns>
        public AbstractHandler[] GetChildrens() => childrensid
                                                    .Select(obj => obj.GetObjectOfType<AbstractHandler>())
                                                    .ToArray();

        /// <summary>
        /// Добавить дочерний объект <see cref="AbstractHandler"/> в <see cref="AbstractHandler"/>
        /// </summary>
        /// <param name="handler">Дочерний объект <see cref="AbstractHandler"/></param>
        /// <returns>Если дочерний объект добавлен то <c>true</c>, иначе <c>false</c></returns>
        public bool AddChild(AbstractHandler handler)
        {
            var answer = childrensid.Add(handler.ID);
            if(answer) handler.parentid = ID;

            return answer;
        }
        
        /// <summary>
        /// Удалить дочерний объект <see cref="AbstractHandler"/> из <see cref="AbstractHandler"/>
        /// </summary>
        /// <param name="handler">Дочерний объект <see cref="AbstractHandler"/></param>
        /// <returns>Если дочерний объект удален то <c>true</c>, иначе <c>false</c></returns>
        public bool RemoveChild(AbstractHandler handler)
        {
            var answer = childrensid.TryRemove(handler.ID);
            if (answer) handler.parentid = McObjectId.Null;

            return answer;
        }
       
        #endregion

        #region Dependents region

        private ConcurrentHashSet<McObjectId> dependentsid = new ConcurrentHashSet<McObjectId>();

        /// <summary>
        /// Получить зависимые обьекты <see cref="AbstractHandler"/>
        /// </summary>
        /// <returns>Массив из зависимых объектов <see cref="AbstractHandler"/>[]</returns>
        public AbstractHandler[] GetDependents() => dependentsid
                                                    .Select(obj => obj.GetObjectOfType<AbstractHandler>())
                                                    .ToArray();

        /// <summary>
        /// Добавить зависимый объект <see cref="AbstractHandler"/> в <see cref="AbstractHandler"/>
        /// </summary>
        /// <param name="handler">Зависимый объект <see cref="AbstractHandler"/></param>
        /// <returns>Если зависимый объект добавлен то <c>true</c>, иначе <c>false</c></returns>
        public bool AddDependent(AbstractHandler handler) => dependentsid.Add(handler.ID);

        /// <summary>
        /// Удалить зависимый обьект <see cref="AbstractHandler"/> из <see cref="AbstractHandler"/>
        /// </summary>
        /// <param name="handler">Зависимый объект <see cref="AbstractHandler"/></param>
        /// <returns>Если зависимый объект удален то <c>true</c>, иначе <c>false</c></returns>
        public bool RemoveDependent(AbstractHandler handler) => dependentsid.TryRemove(handler.ID);
        
        #endregion

        #region Properties region

        private ConcurrentHashSet<IProperty> properties = new ConcurrentHashSet<IProperty>();

        /// <summary>
        /// Получить параметры <see cref="AbstractHandler"/> + <see cref="IObject"/> 
        /// </summary>
        /// <returns>Отсортированный по <see cref="IProperty.ID"/> массив параметров <see cref="IProperty"/>[]</returns>
        public IProperty[] GetProperties()
        {
            if (CatenaryObject != null)
            {
                var props = CatenaryObject.GetProperties();

                if (props != null)
                    return properties.Concat(props).OrderBy(n => n.ID).ToArray();
            }
            return properties.OrderBy(n => n.ID).ToArray();
        }
        
        /// <summary>
        /// Добавить параметр <see cref="IProperty"/> в <see cref="AbstractHandler"/>
        /// </summary>
        /// <param name="property">Зависимый объект <see cref="IProperty"/></param>
        /// <returns>Если зависимый объект добавлен то <c>true</c>, иначе <c>false</c></returns>
        public bool AddProperty(IProperty property) => properties.Add(property);

        /// <summary>
        /// Удалить параметр <see cref="IProperty"/> из <see cref="AbstractHandler"/>
        /// </summary>
        /// <param name="property">Зависимый объект <see cref="AbstractHandler"/></param>
        /// <returns>Если зависимый объект удален то <c>true</c>, иначе <c>false</c></returns>
        public bool RemoveProperty(IProperty property) => properties.TryRemove(property);
        #endregion

        #region Position & Direction region

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

        #endregion

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
        public override List<McObjectId> GetDependent() => childrensid.Concat(dependentsid).ToList();

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
            return GetProperties().ToAdapterProperty();
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
