using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Multicad;
using Multicad.CustomObjectBase;
using Multicad.DatabaseServices;
using Multicad.Geometry;
using Multicad.Projects;

namespace CatenaryCAD.Handlers
{
    [Serializable]
    public abstract class AbstractHandler : McCustomBase, IAbstractHandler
    {
        public McObjectId ParentID;

        private List<McObjectId> childids = new List<McObjectId>(5);
        public McObjectId[] ChildIDs => childids.ToArray();

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

                direction = value.Normalize();
            }
        }

        public Parameters Parameters = new Parameters();
        //public void AddChildren(AbstractHandler child)
        //{
        //    throw new NotImplementedException();
        //}

        //public AbstractHandler[] GetChildrens()
        //{
        //    throw new NotImplementedException();
        //}

        //public AbstractHandler GetParent()
        //{
        //    throw new NotImplementedException();
        //}

        //public void RemoveChildren(AbstractHandler child)
        //{
        //    throw new NotImplementedException();
        //}
        public override hresult PlaceObject()
        {
            hresult result = base.PlaceObject();

            DbEntity.AddToCurrentDocument();

            return result;
        }
        public hresult PlaceObject(Point3d position, Vector3d direction)
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
            if (!TryModify()) return;

            direction = direction.TransformBy(m * new Vector3d( -m.Translation.X, 
                                                                -m.Translation.Y, 
                                                                -m.Translation.Z)).Normalize();
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
            tfm = Matrix3d.Displacement(Point3d.Origin.GetVectorTo(Position)) *
                  Matrix3d.Rotation(-Direction.GetAngleTo(Vector3d.XAxis), Vector3d.ZAxis, Point3d.Origin);

            return true;
        }

        public void OnParentNotification(NotificationFlags notification)
        {

        }
        public void OnChildNotification()
        {

        }
    }
}
