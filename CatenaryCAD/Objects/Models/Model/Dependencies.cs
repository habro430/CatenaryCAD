using System;
using System.Diagnostics;
using System.Linq;

namespace CatenaryCAD.Models
{
    public abstract partial class Model : IModel
    {
        /// <summary>
        /// Идентификатор <see cref="IIdentifier"/> родительской модели
        /// </summary>
        protected IIdentifier ParentIdentifier;

        /// <summary>
        /// Коллекция идентификаторов <see cref="IIdentifier"/> дочерних моделей
        /// </summary>
        protected ConcurrentHashSet<IIdentifier> ChildrensSet = new ConcurrentHashSet<IIdentifier>();

        /// <summary>
        /// Коллекция идентификаторов <see cref="IIdentifier"/> зависимых моделей
        /// </summary>
        protected ConcurrentHashSet<IIdentifier> DependenciesSet = new ConcurrentHashSet<IIdentifier>();

        public IModel Parent
        {
            get => ParentIdentifier?.GetModel() ?? null;//зачем "?? null" если он и так возвращает null 
            set
            {
                if (!SendMessageToHandler(HandlerMessages.TryModify) ?? false) return;

                if (value?.Identifier.GetGuid() != Guid.Empty)
                {
                    if (value != null)
                    {
                        var childrens = (value as Model).ChildrensSet;
                        if (childrens.Add(identifier) || childrens.Contains(identifier))
                            ParentIdentifier = value.Identifier;
                    }
                    else
                    {
                        if (ParentIdentifier != null)
                        {
                            var model = ParentIdentifier.GetModel() as Model;

                            if (model != null)
                            {
                                if (model.ChildrensSet.TryRemove(identifier))
                                    ParentIdentifier = null;
                            }

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