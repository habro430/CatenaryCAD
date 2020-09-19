namespace CatenaryCAD.Geometry.Shapes
{
    public interface IShape
    {
        Edge2D[] Edges { get; }

        IShape TransformBy(in Matrix2D m);

    }
}
