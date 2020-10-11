using System;
using System.Diagnostics;
using System.Linq;

namespace CatenaryCAD.Models
{
    public abstract partial class Model : IModel
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IIdentifier parent;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ConcurrentHashSet<IIdentifier> childrens = new ConcurrentHashSet<IIdentifier>();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ConcurrentHashSet<IIdentifier> dependencies = new ConcurrentHashSet<IIdentifier>();

        public IModel Parent
        {
            get => parent?.GetModel() ?? null;//зачем "?? null" если он и так возвращает null 
            set
            {
                if (!SendMessageToHandler(HandlerMessages.TryModify) ?? false) return;

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
                        if (parent != null)
                        {
                            var model = parent.GetModel() as Model;

                            if (model != null)
                            {
                                if (model.childrens.TryRemove(identifier))
                                    parent = null;
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
    }
}