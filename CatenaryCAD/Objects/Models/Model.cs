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
    public abstract class Model : IModel
    {
        internal event Func<bool> TryModifyHandler;
        internal event Func<bool> UpdateHandler;

        private IIdentifier identifier;

        private Point3D position;
        private Vector3D direction;

        private IIdentifier parent;
        private ConcurrentHashSet<IIdentifier> childrens = new ConcurrentHashSet<IIdentifier>();
        private ConcurrentHashSet<IIdentifier> dependencies = new ConcurrentHashSet<IIdentifier>();

        public bool TryModifyModel() => TryModifyHandler?.Invoke() ?? true;
        public bool UpdateModel() => UpdateHandler?.Invoke() ?? true;

        public IIdentifier Identifier
        {
            get => identifier;
            internal set => identifier = value;
        }

        public IModel Parent
        {
            get => parent?.GetModel() ?? null;
            set
            {
                if (!TryModifyModel()) return;

                if (value?.Identifier.GetGuid() != Guid.Empty)
                {
                    if (value != null)
                    {
                        var childrens = (value as Model).childrens;
                        if (childrens.Add(identifier) || childrens.Contains(identifier))
                            parent = value.Identifier;
                    }
                    else
                    {
                        if ((parent?.GetModel() as Model)?.childrens.TryRemove(identifier) ?? false)
                            parent = null;
                    }
                }
            }
        }

        public IModel[] Childrens
        {
            get
            {
                ConcurrentHashSet<IIdentifier>.ClearNull(childrens);
                return childrens.Select(model => model.GetModel()).ToArray();
            }
        }

        public IModel[] Dependencies 
        {
            get
            {
                ConcurrentHashSet<IIdentifier>.ClearNull(dependencies);
                return dependencies.Select(model => model.GetModel()).ToArray(); 
            } 
        }


        public virtual Point3D Position
        {
            get => position;
            set { if (TryModifyModel()) position = value; }
        }
        public virtual Vector3D Direction
        {
            get => direction;
            set { if (TryModifyModel()) direction = value.Normalize(); }
        }

        public virtual void TransformBy(Matrix3D m)
        {
            Position = position.TransformBy(m);
            Direction = direction.TransformBy(m);

            foreach (var child in Childrens)
                child.TransformBy(m);
        }


        public abstract IMesh[] GetGeometryForLayout();
        public abstract IShape[] GetGeometryForScheme();
        public abstract IPart[] GetParts();
        public abstract IProperty[] GetProperties();

    }
}
