using CatenaryCAD.Geometry;
using CatenaryCAD.Attributes;
using CatenaryCAD.Models.Events;
using CatenaryCAD.Properties;
using Multicad;
using Multicad.DatabaseServices;
using Multicad.Geometry;
using Multicad.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CatenaryCAD.Models.Handlers
{
    [Serializable]
    [CustomEntity("{2B1A1855-B183-4864-88F9-330AB06801EF}", "ANCHOR", "Анкер опоры контактной сети")]
    internal sealed class AnchorHandler : Handler
    {
        [NonSerialized]
        private static readonly Dictionary<string, Type> Anchors;

        static AnchorHandler()
        {
            Anchors = Main.GetCatenaryObjects(typeof(IAnchor))
                .Where((t) => !t.IsAbstract)
                .Where((t) => Attribute.GetCustomAttribute(t, typeof(ModelNonBrowsableAttribute), false) is null)
                .ToDictionary(p => Attribute.GetCustomAttribute(p, typeof(ModelNameAttribute), false)?.ToString() ?? p.Name, p => p);
        }

        public AnchorHandler()
        {
            Property<Type> anchor_type = new Property<Type>("Тип анкера", "Анкер", attr: CatenaryCAD.Properties.Attributes.RefreshAfterChange);
            anchor_type.Updated += (type) => Model = Activator.CreateInstance(type) as Anchor;

            anchor_type.DropDownValues = Anchors;

            PropertiesDictionary.TryAdd("anchor_type", anchor_type);
        }

        public override void OnTransform(Matrix3d tfm) { return; }

        [CommandMethod("insert_anchor", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
        public static void insert_anchor()
        {
            Handler selected_obj;
            do
            {
                selected_obj = (McObjectManager.SelectObject("Выберите ОПОРУ для размещения АНКЕРОВКИ:").GetObject() as Handler);

                if (selected_obj is MastHandler mhandler)
                {

                    AnchorHandler ahandler = new AnchorHandler();
                    ahandler.PlaceObject(mhandler.Model.Position.ToMultiCAD(), Vector3d.XAxis);

                    Mast mast = mhandler.Model as Mast;
                    Anchor anchor = ahandler.Model as Anchor;

                    anchor.Parent = mast;

                    using (InputJig input = new InputJig())
                    {
                        input.AutoSnap = AutoSnapMode.None;
                        input.SnapMode = OsnapModeMask.Center;
                        input.SnapOverrideMode = InputJig.OsnapOverrideMode.Override;

                        input.DashLine = true;
                        input.AutoHighlight = false;

                        input.ExcludeObjects(new McObjectId[]{ ahandler.ID, mhandler.ID }); 

                        input.MouseMove = (s, a) =>
                        {
                            const int distancetomast = 5000;

                            Point3D mouse = a.Point.ToCatenaryCAD_3D();

                            anchor.Direction = mast.Position.GetVectorTo(mouse);
                            double angle = mast.Position.GetVectorTo(mouse).GetAngleTo(Vector3D.AxisX, Vector3D.AxisZ);

                            anchor.Position = mast.Position
                                            .TransformBy(Matrix3D.CreateTranslation(Vector3D.AxisX * distancetomast))
                                            .TransformBy(Matrix3D.CreateRotation(-angle, mast.Position, Vector3D.AxisZ));

                            anchor.EventInvoke(ahandler, new Update());
                        };

                        InputResult result = input.GetPoint("Выберите направление для размещения обьекта:");
                        if (result.Result != InputResult.ResultCode.Normal)
                        {
                            anchor.EventInvoke(ahandler, new Remove());
                            return;
                        }

                    }
                }

            } while (selected_obj != null);
        }
    }
}
