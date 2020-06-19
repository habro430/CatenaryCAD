﻿using Multicad.Geometry;
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

namespace CatenaryCAD
{
    public static class Extensions
    {
        public static List<Point3d> GetPoint3d(this List<Vertex> vrtx)
        {
            var points = new List<Point3d>(vrtx.Count);

            foreach (var vert in vrtx)
                points.Add(new Point3d(vert.X, vert.Y, vert.Z));

            return points;
        }

        public static Vector3d Normalize(this Vector3d vector)
        {
            return vector / vector.Length;
        }

        [CommandMethod("switch_3d", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
        public static void switch_3d()
        {
            //if (Convert.ToBoolean(McDocument.ActiveDocument.CustomProperties["view3d"])) return;

            ObjectFilter filter = ObjectFilter.Create(true);
            filter.AddType(typeof(AbstractHandler));

            List<McObjectId> ids = filter.GetObjects();

            McsProgress progress = McContext.GetProgress();
            progress.SetRange(0, ids.Count);

            McDocument.ActiveDocument.CustomProperties["view3d"] = true;

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
            //if (!Convert.ToBoolean(McDocument.ActiveDocument.CustomProperties["view3d"])) return;
            ObjectFilter filter = ObjectFilter.Create(true);
            filter.AddType(typeof(AbstractHandler));

            List<McObjectId> ids = filter.GetObjects();

            McsProgress progress = McContext.GetProgress();
            progress.SetRange(0, ids.Count);

            McDocument.ActiveDocument.CustomProperties["view3d"] = false;

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
