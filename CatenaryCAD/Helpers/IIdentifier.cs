using CatenaryCAD.Models;
using System;

namespace CatenaryCAD.Helpers
{
    public interface IIdentifier
    {
        public Guid GetGuid();
        public IModel GetModel();
    }
}
