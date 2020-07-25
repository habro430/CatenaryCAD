﻿using CatenaryCAD.Geometry;
using CatenaryCAD.Parts;
using CatenaryCAD.Properties;

using Multicad;
using Multicad.CustomObjectBase;
using Multicad.DatabaseServices;
using Multicad.Geometry;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using static CatenaryCAD.Extensions;

namespace CatenaryCAD.Objects
{
    [Serializable]
    internal abstract class AbstractHandler : McCustomBase, IMcDynamicProperties, IHandler
    {
        public IObject CatenaryObject { get; set; }
        public McObjectId Identifier => ID;

        #region Parent & Childrens region

        protected McObjectId parentid = McObjectId.Null;
        protected ConcurrentHashSet<McObjectId> Childrens = new ConcurrentHashSet<McObjectId>();

        public IHandler Parent => parentid.GetObjectOfType<AbstractHandler>();
        public IHandler[] GetChildrens() => Childrens
                    .Select(obj => obj.GetObjectOfType<AbstractHandler>())
                    .ToArray();

        #endregion

        protected ConcurrentHashSet<McObjectId> Dependents = new ConcurrentHashSet<McObjectId>();
        public IHandler[] GetDependents() => Dependents
                                                    .Select(obj => obj.GetObjectOfType<AbstractHandler>())
                                                    .ToArray();

        protected ConcurrentHashSet<IProperty> Properties = new ConcurrentHashSet<IProperty>();
        public IProperty[] GetProperties()
        {
            if (CatenaryObject != null)
            {
                var props = CatenaryObject.GetProperties();

                if (props != null)
                    return Properties.Concat(props).ToArray();
            }
            return Properties.ToArray();
        }

        protected ConcurrentHashSet<IPart> Parts = new ConcurrentHashSet<IPart>();
        public IPart[] GetParts() => throw new NotImplementedException();

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
            var answer = parent.Childrens.Add(Identifier);
            if (answer) this.parentid = parent.Identifier;

            return PlaceObject(position, direction);
        }
        
        public override void OnErase()
        {
            if (Parent != null)
                (Parent as AbstractHandler).Childrens.TryRemove(this.Identifier);
            
            foreach (var child in Childrens)
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
                ConcurrentHashSet<McObjectId>.ClearMcObjectIdNull(Childrens);

                foreach (var child in Childrens)
                {
                    var handler = child.GetObjectOfType<AbstractHandler>();
                    if (handler != null) handler.Transform(m);
                }
            }
        }

        public override bool GetECS(out Matrix3d tfm)
        {
            double angle = Direction.GetAngleTo(Vector3d.XAxis,Vector3d.ZAxis);
            tfm = Matrix3d.Displacement(Position.GetAsVector()).PostMultiplyBy(
                 Matrix3d.Rotation(-angle, Vector3d.ZAxis, Point3d.Origin));

            return true;
        }
        public override List<McObjectId> GetDependent() => Childrens.Concat(Dependents).ToList();

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
                var mode = (OperationalMode)(McDocument.ActiveDocument
                            .CustomProperties["OperationalMode"] ?? OperationalMode.Scheme);

                switch (mode)
                {
                    case OperationalMode.Scheme:

                        var geometry_scheme = CatenaryObject.GetGeometryForScheme();

                        if (geometry_scheme != null)
                        {
                            for (int igeom = 0; igeom < geometry_scheme.Length; igeom++)
                            {
                                var geometry = geometry_scheme[igeom];

                                for (int iedge = 0; iedge < geometry.Edges.Length; iedge++)
                                {
                                    dc.DrawLine(geometry.Edges[iedge].Item1.ToMCAD(),
                                                geometry.Edges[iedge].Item2.ToMCAD());
                                }
                            }
                        }

                        break;

                    case OperationalMode.Layout:

                        var geometry_layout = CatenaryObject.GetGeometryForLayout(); ;

                        if (geometry_layout != null)
                        {
                            for (int igeom = 0; igeom < geometry_layout.Length; igeom++)
                            {
                                var geometry = geometry_layout[igeom];

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
            return GetProperties().OrderBy(n => n.ID).ToAdapterProperty();
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
