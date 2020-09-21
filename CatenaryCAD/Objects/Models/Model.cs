using CatenaryCAD.Geometry;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Geometry.Shapes;
using CatenaryCAD.Models;
using CatenaryCAD.Parts;
using CatenaryCAD.Properties;
using System;

namespace CatenaryCAD.Models
{
    [Serializable]
    public abstract class Model : IModel
    {
        public virtual event Action Update;
        public virtual event Func<bool> TryModify;

        private Point3D position;
        private Vector3D direction;

        public virtual Point3D Position
        {
            get => position;
            set
            {
                if (TryModify?.Invoke() ?? true)
                {
                    position = value;
                }
#if DEBUG
                else throw new ArgumentException("TryModify вернул false");
#endif
            }
        }
        public virtual Vector3D Direction
        {
            get => direction;
            set
            {
                if (TryModify?.Invoke() ?? true)
                {
                    direction = value.Normalize();
                }
#if DEBUG
                else throw new ArgumentException("TryModify вернул false");
#endif

            }
        }

        public virtual void TransformBy(Matrix3D m)
        {
            if (TryModify?.Invoke() ?? true)
            {
                position = position.TransformBy(m);
                direction = direction.TransformBy(m).Normalize();
            }
#if DEBUG
            else throw new ArgumentException("TryModify вернул false");
#endif
        }

        public abstract IMesh[] GetGeometryForLayout();
        public abstract IShape[] GetGeometryForScheme();
        public abstract IPart[] GetParts();
        public abstract IProperty[] GetProperties();

    }
}
