using CatenaryCAD.Geometry;

namespace CatenaryCAD.Models
{
    public interface IMast : IModel
    {
        Point2D? GetDockingJointPoint(Ray2D ray);
        Point3D? GetDockingJointPoint(Ray3D ray);
    }
}
