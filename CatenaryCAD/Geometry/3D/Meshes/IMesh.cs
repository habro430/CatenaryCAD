namespace CatenaryCAD.Geometry.Meshes
{
    public interface IMesh
    {
        Face3D[] Faces { get; }

        IMesh TransformBy(in Matrix3D m);

    }
}
