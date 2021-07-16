using Catenary.Helpers;
using Catenary.Models;
using Catenary.Models.Handlers;
using Multicad;
using System;
using System.Diagnostics;

namespace Catenary
{
    [Serializable, DebuggerDisplay("{GetGuid()}")]
    internal class McIdentifier : IObjectID
    {

        private readonly McObjectId ObjectId;
        internal McIdentifier(McObjectId id) => ObjectId = id;

        public Guid GetGuid() => ObjectId.ToGuid();

        public IModel GetModel() => ObjectId.GetObjectOfType<Handler>()?.Model;

        internal IHandler GetHandler() => ObjectId.GetObjectOfType<Handler>();
        internal McObjectId ToMcObjectId() => ObjectId;
    }

}
