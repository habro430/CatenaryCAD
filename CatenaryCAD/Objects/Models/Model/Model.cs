using CatenaryCAD.Geometry;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Geometry.Shapes;
using CatenaryCAD.Models;
using CatenaryCAD.Parts;
using CatenaryCAD.Properties;
using System;
using System.Diagnostics;
using System.Linq;

namespace CatenaryCAD.Models
{
    [Serializable]
    public abstract partial class Model : IModel
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IIdentifier identifier;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Point3D position;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Vector3D direction;

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
                direction = value.Normalize();
            }
        }

        public virtual void TransformBy(Matrix3D m)
        {
            Position = position.TransformBy(m);
            Direction = direction.TransformBy(m);

            foreach (var child in Childrens)
                child.TransformBy(m);
        }

    }
}
