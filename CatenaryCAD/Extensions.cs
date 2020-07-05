using CatenaryCAD.Objects;

using Multicad;
using Multicad.AplicationServices;
using Multicad.DatabaseServices;
using Multicad.Geometry;
using Multicad.Runtime;

using System;
using System.Collections.Generic;

namespace CatenaryCAD
{
    public static class Extensions
    {
        [Serializable]
        public enum NatureType
        {
            Line,
            Polygon,
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

            McDocument.ActiveDocument.CustomProperties["viewtype"] = NatureType.Polygon;

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

            McDocument.ActiveDocument.CustomProperties["viewtype"] = NatureType.Line; ;

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
