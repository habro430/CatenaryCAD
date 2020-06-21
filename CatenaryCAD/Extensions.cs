using Multicad.Geometry;
using Multicad.Runtime;
using Multicad.AplicationServices;
using Multicad.DatabaseServices;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CatenaryCAD.Objects;
using Multicad;
using WaveformParser.Types;
using CatenaryCAD.Objects.Masts;

namespace CatenaryCAD
{
    public static class Extensions
    {
        [Serializable]
        public enum ViewType
        {
            Geometry2D,
            Geometry3D,
        }

        internal static List<Point3d> GetPoint3d(this List<Vertex> vrtx)
        {
            var points = new List<Point3d>(vrtx.Count);

            foreach (var vert in vrtx)
                points.Add(new Point3d(vert.X, vert.Y, vert.Z));

            return points;
        }

        internal static Vector3d Normalize(this Vector3d vector)
        {
            return vector / vector.Length;
        }

        [CommandMethod("switch_3d", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
        public static void switch_3d()
        {
            ObjectFilter filter = ObjectFilter.Create(true);
            filter.AddType(typeof(AbstractHandler));

            List<McObjectId> ids = filter.GetObjects();

            McsProgress progress = McContext.GetProgress();
            progress.SetRange(0, ids.Count);

            McDocument.ActiveDocument.CustomProperties["viewtype"] = ViewType.Geometry3D;

            for (int i =0; i< ids.Count; i++)
            {
                AbstractHandler handler = ids[i].GetObjectOfType<AbstractHandler>();
                if (handler != null)
                {
                    handler.TryModify();
                    handler.DbEntity.Update();
                }
                progress.SetPosition(i);
            }

            progress.Dispose();
        }
        [CommandMethod("switch_2d", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
        public static void switch_2d()
        {
            ObjectFilter filter = ObjectFilter.Create(true);
            filter.AddType(typeof(AbstractHandler));

            List<McObjectId> ids = filter.GetObjects();

            McsProgress progress = McContext.GetProgress();
            progress.SetRange(0, ids.Count);

            McDocument.ActiveDocument.CustomProperties["viewtype"] = ViewType.Geometry2D; ;

            for (int i = 0; i < ids.Count; i++)
            {
                AbstractHandler handler = ids[i].GetObjectOfType<AbstractHandler>();

                if (handler != null)
                {
                    handler.TryModify();
                    handler.DbEntity.Update();
                }
                progress.SetPosition(i);
            }

            progress.Dispose();
        }
    }
}
