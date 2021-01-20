using CatenaryCAD.Models;
using System;

namespace CatenaryCAD.Helpers
{
    public interface IObjectID
    {
        public Guid GetGuid();
        public IModel GetModel();
    }
}
