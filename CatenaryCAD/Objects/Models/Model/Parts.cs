using CatenaryCAD.Parts;

namespace CatenaryCAD.Models
{
    public abstract partial class Model : IModel
    {
        public abstract IPart[] GetParts();
    }
}