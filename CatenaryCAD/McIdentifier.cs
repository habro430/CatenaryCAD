﻿using CatenaryCAD.Models;
using CatenaryCAD.Models.Handlers;
using Multicad;
using System;

namespace CatenaryCAD
{
    [Serializable]
    internal class McIdentifier : IIdentifier
    {

        private readonly McObjectId ObjectId;
        internal McIdentifier(McObjectId id) => ObjectId = id;

        public Guid GetGuid() => ObjectId.ToGuid();

        public IModel GetModel() => ObjectId.GetObjectOfType<Handler>()?.Model;

        internal IHandler GetHandler() => ObjectId.GetObjectOfType<Handler>();
        internal McObjectId ToMcObjectId() => ObjectId;
    }

}
