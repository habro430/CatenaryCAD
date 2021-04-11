﻿using CatenaryCAD.Geometry;
using CatenaryCAD.Models.Attributes;
using CatenaryCAD.Properties;

using Multicad;
using Multicad.DatabaseServices;
using Multicad.Geometry;
using Multicad.Runtime;

using System;
using System.Collections.Generic;
using System.Linq;
using static CatenaryCAD.Extensions;

namespace CatenaryCAD.Models.Handlers
{
    [Serializable]
    [CustomEntity("{742ECCF0-0CEC-4791-B4BE-4E3568E2C43E}", "MAST", "Стойка опоры контактной сети")]
    internal sealed class MastHandler : Handler
    {
        [NonSerialized]
        private static readonly Type[] DefaultAllowableMasts;

        static MastHandler()
        {
            DefaultAllowableMasts = Main.GetCatenaryObjects(typeof(IMast))
                .Where((t) => !t.IsAbstract)
                .Where((t) => Attribute.GetCustomAttribute(t, typeof(ModelNonBrowsableAttribute), false) is null).ToArray();
        }

        public MastHandler()
        {
            SetAllowableMasts(DefaultAllowableMasts);
        }

        public void SetAllowableMasts(Type[] masts)
        {
            var allowable = masts.ToDictionary(dict => Attribute.GetCustomAttribute(dict, typeof(ModelNameAttribute), false)?.ToString() ?? dict.Name, p => p); ;

            Property<Type> mast_type = new Property<Type>("Тип стойки", "Стойка", attr: CatenaryCAD.Properties.Attributes.RefreshAfterChange);

            mast_type.DropDownValues = allowable;

            mast_type.Updated += (type) =>
            {
                var mast = Activator.CreateInstance(type) as Mast;
                var mastfoundations = mast.PossibleFoundations;

                var allfoundations = Main.GetCatenaryObjects(typeof(IFoundation));

                var possiblefoundations = mastfoundations
                    .SelectMany((all) => allfoundations
                        .Where((sub) => sub.IsSubclassOf(all))
                        .Union(mastfoundations)
                    .Where((abs) => !abs.IsAbstract))//отсеиваем все абстрактые объекты
                    .Where((non) => Attribute.GetCustomAttribute(non, typeof(ModelNonBrowsableAttribute), false) is null) //отсеиваем объекты которые помечены как недоступные
                    .ToDictionary(dict => Attribute.GetCustomAttribute(dict, typeof(ModelNameAttribute), false)?.ToString() ?? dict.Name, p => p); 

                Model = mast;

                Foundation foundation = Model.Childrens.OfType<Foundation>().FirstOrDefault();
                if (foundation != null) foundation.AllowableModels = possiblefoundations;

            };

            mast_type.Value = allowable.Values.FirstOrDefault();
            PropertiesDictionary.TryAdd("mast_type", mast_type);
        }

        public override ICollection<McDynamicProperty> GetProperties(out bool exclusive)
        {
            exclusive = true;

            var basement = Model.Childrens.Select(t => (t.Identifier as McIdentifier).GetHandler())
                                    .Where(child => child is FoundationHandler).FirstOrDefault();

            var basement_props =  basement?.GetProperties();

            //проверяем режим работы если режим работы не определен то по умолчанию используем OperationalMode.Scheme
            OperationalMode mode = (OperationalMode)(McDocument.ActiveDocument.CustomProperties["OperationalMode"] ?? OperationalMode.Scheme);

            if (basement_props != null && mode == OperationalMode.Scheme)
                //если режим работы OperationalMode.Scheme то выдаем отсортированные по Name параметры basement + mast
                return GetProperties().Concat(basement_props).OrderBy(n => n.Name).ToAdapterProperty();
            else
                //в противном случае только отсортированные по Name параметры mast
                return GetProperties().OrderBy(n => n.Name).ToAdapterProperty();
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

                Mast last_mast = null;

                while (true)
                {
                    var mhandler = new MastHandler();
                    var fhandler = new FoundationHandler();

                    mhandler.PlaceObject(Point3d.Origin, Vector3d.XAxis);
                    fhandler.PlaceObject(Point3d.Origin, Vector3d.XAxis);
                    
                    Mast mast = mhandler.Model as Mast;
                    Foundation foundation = fhandler.Model as Foundation;

                    /////////////////////////////////////////////////////////////////////
                    //var possible_foundations = mast.PossibleFoundations;
                    //var all_foundations = Main.GetCatenaryObjects(typeof(IFoundation));

                    //var allowable_foundations = possible_foundations
                    //    .SelectMany((all) => all_foundations
                    //        .Where((sub) => sub.IsSubclassOf(all))
                    //        .Union(possible_foundations)
                    //    .Where((abs) => !abs.IsAbstract))//отсеиваем все абстрактые объекты
                    //    .Where((non) => Attribute.GetCustomAttribute(non, typeof(ModelNonBrowsableAttribute), false) is null); //отсеиваем объекты которые помечены как недоступные

                    //fhandler.SetAllowableFoundations(allowable_foundations.ToArray());
                    /////////////////////////////////////////////////////////////////////

                    foundation.Parent = mast;

                    input.ExcludeObjects(new McObjectId[] { mhandler.ID, fhandler.ID });

                    input.MouseMove = (s, a) =>
                    {
                        Point3D mouse = a.Point.ToCatenaryCAD_3D();

                        mast.TransformBy(Matrix3D.CreateTranslation(mast.Position.GetVectorTo(mouse)));

                        if (last_mast != null)
                        {
                            double angle = last_mast.Position.GetVectorTo(mouse).GetAngleTo(mast.Direction, Vector3D.AxisZ);
                            mast.TransformBy(Matrix3D.CreateRotation(-angle, mast.Position, Vector3D.AxisZ));
                        }

                        mast.SendMessageToHandler(HandlerMessages.Update);
                        foundation.SendMessageToHandler(HandlerMessages.Update);
                    };

                    InputResult result = null;

                    if (last_mast == null)
                        result = input.GetPoint("Укажите точку для размещения обьекта:");
                    else
                        result = input.GetDistance("Укажите точку для размещения обьекта:", last_mast.Position.ToMultiCAD());

                    if (result.Result != InputResult.ResultCode.Normal)
                    {
                        mast.SendMessageToHandler(HandlerMessages.Remove);
                        return;
                    }

                    last_mast = mast;
                }
            }
        }
    }
}
