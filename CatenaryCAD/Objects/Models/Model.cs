using CatenaryCAD.Components;
using CatenaryCAD.Geometry;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Geometry.Shapes;
using CatenaryCAD.Helpers;
using CatenaryCAD.Models.Events;
using CatenaryCAD.Properties;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;

namespace CatenaryCAD.Models
{
    [Serializable]
    public abstract class Model : IModel
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IObjectID identifier = null;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Point3D position = Point3D.Origin;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Vector3D direction = Vector3D.AxisX;

        public IObjectID Identifier
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
                if (!(bool?)EventInvoke(this, new TryModify())?.Value ?? false) return;
                position = value;
            }
        }

        /// <inheritdoc/>
        public virtual Vector3D Direction
        {
            get => direction;
            set
            {
                if (!(bool?)EventInvoke(this, new TryModify())?.Value ?? false) return;
                direction = value.GetNormalize();
            }
        }

        /// <inheritdoc/>
        /// <param name="matrix">Матрица для преобразования объекта.</param>
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


        internal delegate EventOutput EventHandler(object s, EventInput i);
        internal event EventHandler Event;

        public EventOutput EventInvoke(object sender, EventInput input) => Event?.Invoke(sender, input);

        /// <summary>
        /// Коллекция идентификаторов <see cref="IObjectID"/> дочерних моделей
        /// </summary>
        protected ConcurrentHashSet<IObjectID> ChildrensSet = new ConcurrentHashSet<IObjectID>();

        /// <summary>
        /// Коллекция идентификаторов <see cref="IObjectID"/> зависимых моделей
        /// </summary>
        protected ConcurrentHashSet<IObjectID> DependenciesSet = new ConcurrentHashSet<IObjectID>();

        private IObjectID parent;
        public IModel Parent
        {
            get => parent?.GetModel();
            set
            {
                if (!(bool?)EventInvoke(this, new TryModify())?.Value ?? false) return;

                if (value?.Identifier.GetGuid() != Guid.Empty)
                {

                    if (value != null)
                    {
                        var value_model = value as Model;
                        if (value_model.ChildrensSet.Add(Identifier) || value_model.ChildrensSet.Contains(Identifier))
                            parent = value.Identifier;
                    }
                    else
                    {
                        var parent_model = parent?.GetModel() as Model;
                        if (parent_model != null)
                        {
                            if (parent_model.ChildrensSet.TryRemove(identifier))
                                parent = null;
                        }
                    }
                }
            }
        }

        public IModel[] Childrens
        {
            get
            {
                ConcurrentHashSet<IObjectID>.ClearNull(ChildrensSet);
                return ChildrensSet.Select(model => model.GetModel()).ToArray();
            }
        }

        public IModel[] Dependencies
        {
            get
            {
                ConcurrentHashSet<IObjectID>.ClearNull(DependenciesSet);
                return DependenciesSet.Select(model => model.GetModel()).ToArray();
            }
        }

        /// <summary>
        /// Коллекция деталей <see cref="IComponent"/>
        /// </summary>
        protected ConcurrentDictionary<string, IComponent> ComponentsDictionary = new ConcurrentDictionary<string, IComponent>();
        /// <inheritdoc/>
        public virtual IComponent[] Components { get => ComponentsDictionary.Values.ToArray(); }

        /// <summary>
        /// Коллекция параметров <see cref="IProperty"/>
        /// </summary>
        protected ConcurrentDictionary<string, IProperty> PropertiesDictionary = new ConcurrentDictionary<string, IProperty>();
        
        /// <inheritdoc/>
        public virtual IProperty[] Properties { get => PropertiesDictionary.Values.ToArray(); }

        /// <inheritdoc/>
        public abstract IShape[] GetSchemeGeometry();

        /// <inheritdoc/>
        public abstract IMesh[] GetLayoutGeometry();

        /// <inheritdoc/>
        public abstract Point2D? GetDockingPoint(IModel from, Ray2D ray);

        /// <inheritdoc/>
        public abstract Point3D? GetDockingPoint(IModel from, Ray3D ray);

        public abstract bool CheckAvailableDocking(IModel from);
    }
}
