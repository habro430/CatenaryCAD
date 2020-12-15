using CatenaryCAD.Geometry;
using CatenaryCAD.Helpers;
using System;
using System.Diagnostics;

namespace CatenaryCAD.Models
{
    [Serializable]
    public abstract partial class Model : IModel
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IIdentifier identifier = null;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Point3D position = Point3D.Origin;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Vector3D direction = Vector3D.AxisX;

        public IIdentifier Identifier
        {
            get => identifier;
            internal set => identifier = value;
        }

        public virtual Point3D Position
        {
            get => position;
            set
            {
                if (!SendMessageToHandler(HandlerMessages.TryModify) ?? false) return;
                position = value;
            }
        }

        public virtual Vector3D Direction
        {
            get => direction;
            set
            {
                if (!SendMessageToHandler(HandlerMessages.TryModify) ?? false) return;
                direction = value.GetNormalize();
            }
        }

        public virtual void TransformBy(Matrix3D m)
        {
            Position = position.TransformBy(m);
            Direction = direction.TransformBy(m);

            if (Identifier.GetGuid() != Guid.Empty)
            {
                foreach (var child in Childrens)
                    child.TransformBy(m);
            }
        }
    }
}
