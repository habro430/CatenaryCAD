using CatenaryCAD.Models;
using CatenaryCAD.Models.Handlers;
using Multicad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CatenaryCAD
{
    [Serializable]
    internal class Dependencies : IDependencies
    {
        public readonly McObjectId parent;
        public readonly ConcurrentHashSet<McObjectId> childrens = new ConcurrentHashSet<McObjectId>();
        public readonly ConcurrentHashSet<McObjectId> dependents = new ConcurrentHashSet<McObjectId>();

        public IModel Parent => parent.GetObjectOfType<Handler>()?.Model;

        public IModel[] Childrens => childrens
                                        .Select(obj => obj.GetObjectOfType<Handler>()?.Model)
                                        .ToArray();

        public IModel[] Dependens => dependents
                                        .Select(obj => obj.GetObjectOfType<Handler>()?.Model)
                                        .ToArray();

        public Dependencies(McObjectId par, McObjectId[] childs, McObjectId[] depends)
        {
            parent = par;

            if (childs != null)
                foreach (var child in childs)
                    childrens.Add(child);

            if (depends != null)
                foreach (var depend in depends)
                    dependents.Add(depend);
        }
    }
}
