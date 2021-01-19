using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Geometry.Shapes;

namespace CatenaryCAD.Models
{
    public abstract partial class Model : IModel
    {
        public abstract IMesh[] GetGeometryForLayout();
        public abstract IShape[] GetGeometry();

    }
}