using System;
using System.Diagnostics;
using System.Linq;

namespace CatenaryCAD.Models
{
    public abstract partial class Model : IModel
    {
        /// <summary>
        /// Коллекция идентификаторов <see cref="IIdentifier"/> дочерних моделей
        /// </summary>
        protected ConcurrentHashSet<IIdentifier> ChildrensSet = new ConcurrentHashSet<IIdentifier>();

        /// <summary>
        /// Коллекция идентификаторов <see cref="IIdentifier"/> зависимых моделей
        /// </summary>
        protected ConcurrentHashSet<IIdentifier> DependenciesSet = new ConcurrentHashSet<IIdentifier>();

        private IIdentifier parent;
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
                        var parent_childrens = (value as Model).ChildrensSet;
                        if (parent_childrens.Add(Identifier) || parent_childrens.Contains(Identifier))
                            parent = value.Identifier;
                    }
                    else
                    {
                        var model = parent?.GetModel() as Model;

                        if (model != null)
                        {
                            if (model.ChildrensSet.TryRemove(identifier))
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
                ConcurrentHashSet<IIdentifier>.ClearNull(ChildrensSet);
                return ChildrensSet.Select(model => model.GetModel()).ToArray();
            }
        }

        public IModel[] Dependencies
        {
            get
            {
                ConcurrentHashSet<IIdentifier>.ClearNull(DependenciesSet);
                return DependenciesSet.Select(model => model.GetModel()).ToArray();
            }
        }
    }
}