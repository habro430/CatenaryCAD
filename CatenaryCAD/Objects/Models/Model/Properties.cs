using CatenaryCAD.Properties;

namespace CatenaryCAD.Models
{
    public abstract partial class Model : IModel
    {
        public abstract IProperty[] GetProperties();
    }
}