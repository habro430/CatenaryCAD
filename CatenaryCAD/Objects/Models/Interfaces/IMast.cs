using CatenaryCAD.Geometry;

namespace CatenaryCAD.Models
{
    public interface IMast : IModel
    {
        Point2D? GetPointForDockingJoint(Ray2D ray);
        Point3D? GetPointForDockingJoint(Ray3D ray);
    }
}
