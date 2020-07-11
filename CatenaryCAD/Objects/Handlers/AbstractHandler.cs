using CatenaryCAD.Geometry;
using CatenaryCAD.Geometry.Core;
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
        public IObject CatenaryObject;

        public McObjectId ParentID = McObjectId.Null;

        private List<McObjectId> childids = new List<McObjectId>(5);
        public McObjectId[] ChildIDs => childids.ToArray();

        public override List<McObjectId> GetDependent() => childids;

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

        public List<IProperty> Properties = new List<IProperty>();

        public override hresult PlaceObject()
        {
            hresult result = base.PlaceObject();

            DbEntity.AddToCurrentDocument();

            return result;
        }
        public virtual hresult PlaceObject(Point3d position, Vector3d direction)
        {
            hresult result = base.PlaceObject();

            Position = position;
            Direction = direction;

            DbEntity.AddToCurrentDocument();

            return result;
        }

        public override void OnTransform(Matrix3d tfm) =>Transform(tfm);
        public virtual void Transform(Matrix3d m)
        {
            if (!TryModify())
                return;

            
            direction = direction.TransformBy(m);
            position = position.TransformBy(m);

            //DbEntity.Update();

            if (ID != null)
            {
                foreach (var child in childids)
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
                                    //dc.DrawLine(geometry.Vertices[geometry.Edges[iedge].Item1].ToMCAD(),
                                    //            geometry.Vertices[geometry.Edges[iedge].Item2].ToMCAD());
                                    dc.DrawLine(geometry.Edges[iedge].Item1.ToMCAD(),
                                                geometry.Edges[iedge].Item2.ToMCAD());
                                }
                            }
                        }
                        else
                            dc.DrawText(new TextGeom("ERR XY", Point3d.Origin, Vector3d.XAxis, HorizTextAlign.Center, VertTextAlign.Center, "normal", 1000, 1000));

                        break;

                    case NatureType.Polygon:

                        if (geometry_xyz != null)
                        {
                            for (int igeom = 0; igeom < geometry_xyz.Length; igeom++)
                            {
                                var geometry = geometry_xyz[igeom];

                                for (int iedge = 0; iedge < geometry.Edges.Length; iedge++)
                                {
                                    //dc.DrawLine(geometry.Vertices[geometry.Edges[iedge].Item1].ToMCAD(),
                                    //            geometry.Vertices[geometry.Edges[iedge].Item2].ToMCAD());
                                    dc.DrawLine(geometry.Edges[iedge].Item1.ToMCAD(),
                                                geometry.Edges[iedge].Item2.ToMCAD());

                                }
                            }
                        }
                        else
                            dc.DrawText(new TextGeom("ERR XYZ", Point3d.Origin, Vector3d.XAxis, HorizTextAlign.Center, VertTextAlign.Center, "normal", 1000, 1000));

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
                return Properties
                    .Concat(CatenaryObject.GetProperties())
                    .OrderBy(n => n.ID).ToArray().ToAdapterProperty();
            }
            else
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
