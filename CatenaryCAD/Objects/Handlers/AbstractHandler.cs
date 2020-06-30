using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CatenaryCAD.Properties;
using Multicad;
using Multicad.CustomObjectBase;
using Multicad.DatabaseServices;
using Multicad.Geometry;
using Multicad.Projects;

namespace CatenaryCAD.Objects
{
    [Serializable]
    internal abstract class AbstractHandler : McCustomBase
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
            foreach (var prop in Properties)
            {
                if (prop.ID == id)
                    return prop.ToAdapterProperty();
            }
            return null;
        }
    }
}
