using CatenaryCAD.Objects.Attributes;
using CatenaryCAD.Properties;
using Multicad.DatabaseServices;
using Multicad.Geometry;
using Multicad.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CatenaryCAD.Objects
{
    [Serializable]
    [CustomEntity("{2B1A1855-B183-4864-88F9-330AB06801EF}", "ANCHOR", "Анкер опоры контактной сети")]
    internal class AnchorHandler : AbstractHandler
    {
        [NonSerialized]
        private static readonly Type[] Anchors;
        static AnchorHandler() => Anchors = Main.GetCatenaryObjects(typeof(IAnchor));
        public AnchorHandler()
        {
            Property<Type> anchor_type = new Property<Type>("01_anchor_type", "Тип анкера", "Анкер", ConfigFlags.RefreshAfterChange);

            anchor_type.DictionaryValues = Anchors
                .Where((type) => type.GetCustomAttributes(typeof(NonBrowsableAttribute), false).FirstOrDefault() == null)
                .Select((type) => new
                {
                    type,
                    atrr = (type.GetCustomAttributes(typeof(NameAttribute), false)
                               .FirstOrDefault() as NameAttribute ?? new NameAttribute(type.Name)).Name
                }).ToDictionary(p => p.atrr, p => p.type);

            anchor_type.Updated += (type) =>
            {
                if (!TryModify()) return;
                CatenaryObject = (IAnchor)Activator.CreateInstance(type);
                CatenaryObject.Updated += () => { if (!TryModify()) return; };
            };

            anchor_type.Value = anchor_type.DictionaryValues.Values.FirstOrDefault();

            Properties.Add(anchor_type);
        }
        public override void OnTransform(Matrix3d tfm)
        {
            if (Parent == null)
            {
                if (ID.IsNull)
                    return;

                TransformBy(tfm);
            }
        }
        [CommandMethod("insert_anchor", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
        public static void insert_anchor()
        {
            AbstractHandler selected_obj;
            do
            {
                selected_obj = (McObjectManager.SelectObject("Выберите ОПОРУ для размещения АНКЕРОВКИ:").GetObject() as AbstractHandler);

                if (selected_obj is MastHandler mast)
                {
                    AnchorHandler anchor = new AnchorHandler();
                    anchor.PlaceObject(mast.Position, Vector3d.XAxis, mast);

                    using (InputJig input = new InputJig())
                    {
                        input.AutoSnap = AutoSnapMode.Magnet;
                        input.SnapMode = OsnapModeMask.Center;
                        input.SnapOverrideMode = InputJig.OsnapOverrideMode.Override;

                        input.DashLine = true;
                        input.AutoHighlight = false;

                        input.MouseMove = (s, a) =>
                        {
                            anchor.Direction = a.Point.GetVectorTo(mast.Position);
                            anchor.DbEntity.Update();

                        };

                        InputResult result = input.GetPoint("Выберите направление для размещения обьекта:");
                        if (result.Result != InputResult.ResultCode.Normal) McObjectManager.Erase(anchor.ID);

                    }


                    //    Anchor anchor = new Anchor(Anchor.AnchorCountEnumeration.SingleAnchor,
                    //        Anchor.JointCountEnumeration.DoubleJoint);

                    //    anchor.PlaceObject();

                    //    selected_obj.AddChildren(anchor);

                    //    using (InputJig input = new InputJig())
                    //    {
                    //        excluded_objs.Add(mast.ObjectHandler.ID);
                    //        excluded_objs.Add(anchor.ObjectHandler.ID);

                    //        input.ExcludeObjects(excluded_objs);

                    //        input.MouseMove += (s, a) =>
                    //        {
                    //            Point position = (mast.ObjectHandler as MastHandler).GetPositionForChild(a.Point.ToCoustomPoint());
                    //            if (!position.IsUnavalible())
                    //            {
                    //                anchor.Position = position;
                    //                anchor.Direction = a.Point.ToCoustomPoint().GetVectorTo(mast.Position);

                    //                anchor.Update();
                    //            }
                    //        };

                    //        InputResult result = input.GetPoint("Выберите направление для размещения обьекта:");
                    //        if (result.Result != InputResult.ResultCode.Normal) anchor.ObjectHandler.DbEntity.Erase();
                    //    }
                }

            } while (selected_obj != null);
        }
    }
}
