﻿using CatenaryCAD.Properties;
using Multicad;
using Multicad.DatabaseServices;
using Multicad.Geometry;
using Multicad.Reinforcements;
using Multicad.Runtime;

using System;
using System.Collections.Generic;
using System.Linq;

namespace CatenaryCAD.Objects
{
    [Serializable]
    [CustomEntity("{742ECCF0-0CEC-4791-B4BE-4E3568E2C43E}", "MAST", "Опора контактной сети")]
    internal sealed class MastHandler : AbstractHandler
    {
        [NonSerialized]
        private static readonly Dictionary<string, Type> Masts;

        static MastHandler()
        {
            //при первом вызове класса кэшируем в словарь производные от IMast опоры в статику
            //получаем все не абстрактные обьекты производные от IMast и имеющие атрибут CatenaryObjectAttribute
            var masts = Main.CachedCatenaryObjects
                .Where(abstr => !abstr.IsAbstract)
                .Where(interf => interf.GetInterface(typeof(IMast).FullName) != null)
                .Where(attr => Attribute.IsDefined(attr, typeof(CatenaryObjectAttribute), false));

            //получем словарь из CatenaryObjectAttribute и типа производного от IMast класса
            Masts = masts.Select((type) => new
            {
                type,
                atrr = type.GetCustomAttributes(typeof(CatenaryObjectAttribute), false)
                            .FirstOrDefault() as CatenaryObjectAttribute
            }).ToDictionary(p => p.atrr.Name, p => p.type);

        }        
        public MastHandler()
        {
            Property<Type> mast_type = new Property<Type>("01_mast_type", "Тип стойки", "Стойка", PropertyFlags.RefreshAfterChange);

            mast_type.DictionaryValues = Masts;

            mast_type.Updated += (type) =>
            {
                if (!TryModify()) return;
                CatenaryObject = (IMast)Activator.CreateInstance(type);
                CatenaryObject.Updated += () => { if (!TryModify()) return; };
            };

            mast_type.Value = mast_type.DictionaryValues.Values.FirstOrDefault();

            Properties.Add(mast_type);
        }        
        
        [CommandMethod("insert_mast", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
        public static void insert_mast()
        {
            using (InputJig input = new InputJig())
            {
                input.AutoSnap = AutoSnapMode.Magnet;
                input.SnapMode = OsnapModeMask.Center;
                input.SnapOverrideMode = InputJig.OsnapOverrideMode.Override;

                input.DashLine = true;
                input.AutoHighlight = false;

                List<MastHandler> placed_masts = new List<MastHandler>();
                List<BasementHandler> placed_basements = new List<BasementHandler>();

                while (true)
                {
                    MastHandler mast = new MastHandler();
                    BasementHandler basement = new BasementHandler();

                    mast.PlaceObject(Point3d.Origin, Vector3d.XAxis);
                    basement.PlaceObject(Point3d.Origin, Vector3d.XAxis, mast);

                    input.ExcludeObjects(new McObjectId[] { mast.ID, basement.ID });

                    input.MouseMove = (s, a) =>
                    {
                        mast.Position = a.Point;
                        basement.Position = a.Point;

                        if (placed_masts.Count != 0)
                        {
                            mast.Direction = placed_masts[placed_masts.Count - 1].Position.GetVectorTo(a.Point);
                            basement.Direction = placed_basements[placed_basements.Count - 1].Position.GetVectorTo(a.Point);

                            if (placed_masts.Count > 1)
                            {
                                placed_masts[placed_masts.Count - 1].Direction = (placed_masts[placed_masts.Count - 2].Direction + mast.Direction) / 2;
                                placed_basements[placed_basements.Count - 1].Direction = (placed_basements[placed_basements.Count - 2].Direction + basement.Direction) / 2;

                            }
                            else
                            {
                                placed_masts[placed_masts.Count - 1].Direction = mast.Direction;
                                placed_basements[placed_basements.Count - 1].Direction = basement.Direction;

                            }

                            placed_masts[placed_masts.Count - 1].DbEntity.Update();
                            placed_basements[placed_basements.Count - 1].DbEntity.Update();
                        }

                        mast.DbEntity.Update();
                        basement.DbEntity.Update();
                    };

                    InputResult result = null;

                    if (placed_masts.Count == 0)
                        result = input.GetPoint("Укажите точку для размещения обьекта:");
                    else
                        result = input.GetDistance("Укажите точку для размещения обьекта:", placed_masts.Last().Position);

                    if (result.Result != InputResult.ResultCode.Normal)
                    {
                        McObjectManager.Erase(mast.ID);

                        return;
                    }

                    placed_masts.Add(mast);
                    placed_basements.Add(basement);
                }
            }
        }
    }
}
