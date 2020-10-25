using CatenaryCAD.Geometry;
using CatenaryCAD.Models.Attributes;
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
                        .Where((t) => Attribute.GetCustomAttribute(t, typeof(ModelNonBrowsableAttribute), false) is null)
                        .ToDictionary(p => Attribute.GetCustomAttribute(p, typeof(ModelNameAttribute), false)?.ToString() ?? p.Name, p => p);
        }

        public AnchorHandler()
        {
            Property<Type> anchor_type = new Property<Type>("01_anchor_type", "Тип анкера", "Анкер", props: ConfigFlags.RefreshAfterChange);

            anchor_type.DictionaryValues = Anchors;

            anchor_type.Updated += (type) => Model = Activator.CreateInstance(type) as Anchor;
            anchor_type.Value = Anchors.Values.FirstOrDefault();

            Properties.Add(anchor_type);
        }

        public override void OnTransform(Matrix3d tfm)
        {
            if (Model.Parent == null)
            {
                if (ID.IsNull)
                    return;

                Model.TransformBy(tfm.ToCatenaryCAD());
            }
        }
        [CommandMethod("insert_anchor", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
        public static void insert_anchor()
        {
            Handler selected_obj;
            do
            {
                selected_obj = (McObjectManager.SelectObject("Выберите ОПОРУ для размещения АНКЕРОВКИ:").GetObject() as Handler);

                if (selected_obj is MastHandler mast)
                {
                    AnchorHandler anchor = new AnchorHandler();
                    anchor.Model.Parent = mast.Model;

                    anchor.PlaceObject(mast.Model.Position.ToMultiCAD(), Vector3d.XAxis);

                    using (InputJig input = new InputJig())
                    {
                        input.AutoSnap = AutoSnapMode.None;
                        input.SnapMode = OsnapModeMask.Center;
                        input.SnapOverrideMode = InputJig.OsnapOverrideMode.Override;

                        input.DashLine = true;
                        input.AutoHighlight = false;

                        input.ExcludeObjects(new McObjectId[]{ anchor.ID, mast.ID }); 

                        input.MouseMove = (s, a) =>
                        {
                            const int distancetomast = 5000;
                            Point3D mouse = a.Point.ToCatenaryCAD_3D();

                            anchor.Model.Direction = mast.Model.Position.VectorTo(mouse);

                            double angle = mast.Model.Position.VectorTo(mouse).AngleTo(Vector3D.AxisX, Vector3D.AxisZ);

                            anchor.Model.Position = mast.Model.Position
                                            .TransformBy(Matrix3D.CreateTranslation(Vector3D.AxisX * distancetomast))
                                            .TransformBy(Matrix3D.CreateRotation(-angle, mast.Model.Position, Vector3D.AxisZ));

                            anchor.DbEntity.Update();

                        };

                        InputResult result = input.GetPoint("Выберите направление для размещения обьекта:");
                        if (result.Result != InputResult.ResultCode.Normal) McObjectManager.Erase(anchor.ID);

                    }
                }

            } while (selected_obj != null);
        }
    }
}
