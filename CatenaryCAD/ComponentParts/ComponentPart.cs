using CatenaryCAD.Geometry;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Helpers;
using CatenaryCAD.Maintenances;
using System;
using System.Linq;

namespace CatenaryCAD.ComponentParts
{
    [Serializable]
    public class ComponentPart : IComponentPart
    {

        private IMesh[] geometry;

        /// <inheritdoc/>
        public IMesh[] Geometry
        {
            get
            {
                Matrix3D rotationx = Matrix3D.CreateRotation(-Rotation.X, Point3D.Origin, Vector3D.AxisX);
                Matrix3D rotationy = Matrix3D.CreateRotation(-Rotation.Y, Point3D.Origin, Vector3D.AxisY);
                Matrix3D rotationz = Matrix3D.CreateRotation(-Rotation.Z, Point3D.Origin, Vector3D.AxisZ);

                Matrix3D translation = Matrix3D.CreateTranslation(Point3D.Origin.GetVectorTo(Origin));

                return geometry.DeepClone()
                    .Select(m => m.TransformBy(rotationx * rotationy * rotationz * translation))
                    .ToArray();
            }
        }

        /// <inheritdoc/>
        public IMaintenance[] Maintenances => throw new NotImplementedException();

        /// <inheritdoc/>
        public Vector3D Rotation { get; set ; }

        /// <inheritdoc/>
        public Point3D Origin { get; set; }
        
        public ComponentPart(IMesh[] geometry)
        {
            this.geometry = geometry;
        }
    }
}
