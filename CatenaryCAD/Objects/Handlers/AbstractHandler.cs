using CatenaryCAD.Geometry;
using CatenaryCAD.Objects.Handlers;
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
    internal abstract class AbstractHandler : McCustomBase, IMcDynamicProperties, IHandler
    {
        public IObject CatenaryObject { get; set; }
        public McObjectId GetID() => this.ID;

        #region Parent & Childrens region
        private McObjectId parentid = McObjectId.Null;
        private ConcurrentHashSet<McObjectId> childrensid = new ConcurrentHashSet<McObjectId>();

        public IHandler Parent => parentid.GetObjectOfType<AbstractHandler>();
        public IHandler[] GetChildrens() => childrensid
                                                    .Select(obj => obj.GetObjectOfType<AbstractHandler>())
                                                    .ToArray();
        public bool AddChild(IHandler handler)
        {
            var answer = childrensid.Add(handler.GetID());
            if(answer) (handler as AbstractHandler).parentid = ID;

            return answer;
        }
        public bool RemoveChild(IHandler handler)
        {
            var answer = childrensid.TryRemove(handler.GetID());
            if (answer) (handler as AbstractHandler).parentid = McObjectId.Null;

            return answer;
        }
        #endregion

        #region Dependents region
        private ConcurrentHashSet<McObjectId> dependentsid = new ConcurrentHashSet<McObjectId>();

        public IHandler[] GetDependents() => dependentsid
                                                    .Select(obj => obj.GetObjectOfType<AbstractHandler>())
                                                    .ToArray();
        public bool AddDependent(IHandler handler) => dependentsid.Add(handler.GetID());
        public bool RemoveDependent(IHandler handler) => dependentsid.TryRemove(handler.GetID());
        #endregion

        #region Properties region
        private ConcurrentHashSet<IProperty> properties = new ConcurrentHashSet<IProperty>();

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
        
        public bool AddProperty(IProperty property) => properties.Add(property);
        public bool RemoveProperty(IProperty property) => properties.TryRemove(property);
        #endregion

        #region Position & Direction region

        private Point3d position = Point3d.Origin;
        private Vector3d direction = Vector3d.XAxis;

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
        
        public override void OnErase()
        {
            if (!parentid.IsNull)
                Parent.RemoveChild(this);

            foreach (var child in childrensid)
                McObjectManager.Erase(child);
        }

        public override void OnTransform(Matrix3d tfm) =>Transform(tfm);

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
