﻿using Catenary.Geometry;
using Catenary.Geometry.Meshes;
using Catenary.Helpers;
using Catenary.Maintenances;
using System;
using System.Linq;

namespace Catenary.Components
{
    [Serializable]
    public class Component : IComponent
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

                return geometry
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
        
        public Component(IMesh[] geometry)
        {
            this.geometry = geometry;
        }
    }
}
