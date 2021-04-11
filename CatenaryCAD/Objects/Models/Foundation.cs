using CatenaryCAD.Models.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;

namespace CatenaryCAD.Models
{
    /// <summary>
    /// Модель, описывающая фундамент опоры контактной сети.
    /// </summary>
    [Serializable]
    public abstract class Foundation : Model, IFoundation
    {

        [NonSerialized]
        internal static readonly Dictionary<string, Type> DefaultAllowableFoundations;
        static Foundation()
        {
            DefaultAllowableFoundations = Main.GetCatenaryObjects(typeof(IFoundation))
                .Where((t) => !t.IsAbstract)
                .Where((t) => Attribute.GetCustomAttribute(t, typeof(ModelNonBrowsableAttribute), false) is null)
                .ToDictionary(dict => Attribute.GetCustomAttribute(dict, typeof(ModelNameAttribute), false)?.ToString() ?? dict.Name, p => p);
        }

        internal event Action AllowableModelsUpdated;

        private Dictionary<string, Type> allowablemodels = DefaultAllowableFoundations;

        /// <inheritdoc/>
        public Dictionary<string, Type> AllowableModels 
        { 
            get => allowablemodels;
            set
            {
                allowablemodels = value;
                AllowableModelsUpdated?.Invoke();
            }
        }
    }
}
