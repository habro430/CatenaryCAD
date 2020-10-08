using CatenaryCAD.Models;
using System;

namespace CatenaryCAD
{
    public interface IIdentifier
    {
        public Guid GetGuid();
        public IModel GetModel();
    }
}
