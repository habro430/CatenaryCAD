using CatenaryCAD.Geometry;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Geometry.Shapes;
using CatenaryCAD.Models;
using CatenaryCAD.Parts;
using CatenaryCAD.Properties;
using System;
using System.Linq;

namespace CatenaryCAD.Models
{
    [Serializable]
    public abstract partial class Model : IModel
    {
        private IIdentifier identifier;

        private Point3D position;
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
                if (identifier != null)
                    if (!SendMessageToHandler(HandlerMessages.TryModify)) return;

                position = value; 
            }
        }

        public virtual Vector3D Direction
        {
            get => direction;
            set 
            {
                if (identifier != null)
                    if (!SendMessageToHandler(HandlerMessages.TryModify)) return;
                
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
