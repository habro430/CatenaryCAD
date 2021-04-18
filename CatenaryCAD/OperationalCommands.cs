using CatenaryCAD.Models.Handlers;
using Multicad;
using Multicad.AplicationServices;
using Multicad.DatabaseServices;
using Multicad.Runtime;

using System;
using System.Collections.Generic;

namespace CatenaryCAD
{
    internal static class OperationalCommands
    {
        [Serializable]
        public enum OperationalMode
        {
            Scheme,
            Layout
        }   

        [CommandMethod("switch_3d", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
        public static void switch_3d()
        {
            ObjectFilter filter = ObjectFilter.Create(true);
            filter.AddType(typeof(Handler));

            List<McObjectId> ids = filter.GetObjects();

            McsProgress progress = McContext.GetProgress();
            progress.SetRange(0, ids.Count);

            McDocument.ActiveDocument.CustomProperties["OperationalMode"] = OperationalMode.Layout;

            for (int i = 0; i < ids.Count; i++)
            {
                Handler handler = ids[i].GetObjectOfType<Handler>();
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
            filter.AddType(typeof(Handler));

            List<McObjectId> ids = filter.GetObjects();

            McsProgress progress = McContext.GetProgress();
            progress.SetRange(0, ids.Count);

            McDocument.ActiveDocument.CustomProperties["OperationalMode"] = OperationalMode.Scheme; ;

            for (int i = 0; i < ids.Count; i++)
            {
                Handler handler = ids[i].GetObjectOfType<Handler>();

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
