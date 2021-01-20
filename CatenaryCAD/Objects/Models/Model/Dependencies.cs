using CatenaryCAD.Helpers;
using System;
using System.Linq;

namespace CatenaryCAD.Models
{
    public abstract partial class Model : IModel
    {
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
                if (!SendMessageToHandler(HandlerMessages.TryModify) ?? false) return;

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
    }
}