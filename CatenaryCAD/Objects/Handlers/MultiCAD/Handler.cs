using CatenaryCAD.Geometry;
using CatenaryCAD.Parts;
using CatenaryCAD.Properties;
using Multicad;
using Multicad.CustomObjectBase;
using Multicad.DatabaseServices;
using Multicad.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

using static CatenaryCAD.Extensions;

using Matrix3d = Multicad.Geometry.Matrix3d;
using Point3d = Multicad.Geometry.Point3d;
using Vector3d = Multicad.Geometry.Vector3d;

namespace CatenaryCAD.Models.Handlers
{
    [Serializable]
    internal abstract class Handler : McCustomBase, IMcDynamicProperties, IHandler
    {
        public IModel Model { get; set; }

        protected ConcurrentHashSet<IProperty> properties = new ConcurrentHashSet<IProperty>();
        public IProperty[] GetProperties()
        {
            if (Model != null)
            {
                var props = Model.Properties;

                if (props != null)
                    return properties.Concat(props).ToArray();
            }
            return properties.ToArray();
        }


        public override hresult PlaceObject()
        {
            hresult result = base.PlaceObject();
            DbEntity.AddToCurrentDocument();

            return result;
        }
        public virtual hresult PlaceObject(Point3d position, Vector3d direction)
        {
            Model.Position = position.ToCatenaryCAD_3D();
            Model.Direction = direction.ToCatenaryCAD_3D();

            return PlaceObject();
        }

        public override void OnErase()
        {
            Model.Parent = null;

            foreach (var child in Model.Childrens)
                McObjectManager.Erase((child.Identifier as McIdentifier).ToMcObjectId());
        }

        public override void OnTransform(Matrix3d tfm) => Model.TransformBy(tfm.ToCatenaryCAD());

        public override bool GetECS(out Matrix3d tfm)
        {
            double angle = Model.Direction.ToMultiCAD().GetAngleTo(Vector3d.XAxis,Vector3d.ZAxis);
            tfm = Matrix3d.Displacement(Model.Position.ToMultiCAD().GetAsVector()).PostMultiplyBy(
                 Matrix3d.Rotation(-angle, Vector3d.ZAxis, Point3d.Origin));

            return true;
        }
        public override List<McObjectId> GetDependent() => Model.Childrens.Concat(Model.Dependencies)
                                                                .Select(m => (m.Identifier as McIdentifier).ToMcObjectId())
                                                                .ToList();

        public override bool OnGetOsnapPoints(OsnapMode osnapMode, Point3d pickPoint, Point3d lastPoint, List<Point3d> osnapPoints)
        {
            switch (osnapMode)
            {
                case OsnapMode.Center:
                    osnapPoints.Add(Model.Position.ToMultiCAD());
                    break;
            }
            return true;
        }

        public override void OnDraw(GeometryBuilder dc)
        {
            dc.Clear();

            dc.Color = Multicad.Constants.Colors.ByObject;
            dc.LineType = Multicad.Constants.LineTypes.ByObject;

            if (Model != null)
            {
                var mode = (OperationalMode)(McDocument.ActiveDocument
                            .CustomProperties["OperationalMode"] ?? OperationalMode.Scheme);

                switch (mode)
                {
                    case OperationalMode.Scheme:

                        var geometry_scheme = Model.GetGeometryForScheme();

                        if (geometry_scheme != null)
                        {
                            for (int igeom = 0; igeom < geometry_scheme.Length; igeom++)
                            {
                                var geometry = geometry_scheme[igeom];

                                foreach (var edge in geometry.Edges)
                                    dc.DrawLine(edge.First.ToMultiCAD(),
                                                edge.Second.ToMultiCAD());
                            }
                        }

                        break;

                    case OperationalMode.Layout:

                        var geometry_layout = Model.GetGeometryForLayout(); ;

                        if (geometry_layout != null)
                        {
                            for (int igeom = 0; igeom < geometry_layout.Length; igeom++)
                            {
                                var geometry = geometry_layout[igeom];

                                foreach( var face in geometry.Faces)
                                    foreach(var edge in face.Edges)
                                        dc.DrawLine(edge.First.ToMultiCAD(),
                                                    edge.Second.ToMultiCAD());
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
        public McDynamicProperty GetProperty(string id) => null;
            //GetProperties().Where(n => n.ID == id).FirstOrDefault().ToAdapterProperty() ;

    }
}
