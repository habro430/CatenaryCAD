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

        /// <inheritdoc/>
        public virtual Point3D Position
        {
            get => position;
            set
            {
                if (!SendMessageToHandler(HandlerMessages.TryModify) ?? false) return;
                position = value;
            }
        }

        /// <inheritdoc/>
        public virtual Vector3D Direction
        {
            get => direction;
            set
            {
                if (!SendMessageToHandler(HandlerMessages.TryModify) ?? false) return;
                direction = value.GetNormalize();
            }
        }

        /// <inheritdoc/>
        public virtual IModel TransformBy(in Matrix3D matrix)
        {
            Position = position.TransformBy(matrix);
            Direction = direction.TransformBy(matrix);

            if (Identifier.GetGuid() != Guid.Empty)
            {
                foreach (var child in Childrens)
                    child.TransformBy(matrix);
            }

            return this;
        }
    }
}
