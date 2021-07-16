using Catenary.Models;
using System;

namespace Catenary.Helpers
{
    public interface IObjectID
    {
        public Guid GetGuid();
        public IModel GetModel();
    }
}
