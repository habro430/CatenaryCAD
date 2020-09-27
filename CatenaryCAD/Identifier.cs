using CatenaryCAD.Models;
using CatenaryCAD.Models.Handlers;
using Multicad;
using System;

namespace CatenaryCAD
{
    [Serializable]
    public class Identifier
    {

        private McObjectId ObjectId;
        internal Identifier(McObjectId id)
        {
            ObjectId = id;
        }

        public Guid Guid => ObjectId.ToGuid();
        public IModel Model => ObjectId.GetObjectOfType<Handler>()?.Model;
        internal IHandler Handler => ObjectId.GetObjectOfType<Handler>();
        internal McObjectId ToMcObjectId() => ObjectId;

    }

}
