namespace CatenaryCAD.Geometry.Meshes
{
    public interface IMesh
    {
        Point3D[] Vertices { get; }
        Vector3D[] Normals { get; }
        int[][] Indices { get; }

        IMesh TransformBy(in Matrix3D m);

    }
}
