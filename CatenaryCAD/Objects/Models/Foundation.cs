using CatenaryCAD.Models.Attributes;

using System;
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
        internal static readonly Type[] DefaultAllowableFoundations;
        static Foundation()
        {
            DefaultAllowableFoundations = Main.GetCatenaryObjects(typeof(IFoundation))
                .Where((t) => !t.IsAbstract)
                .Where((t) => Attribute.GetCustomAttribute(t, typeof(ModelNonBrowsableAttribute), false) is null).ToArray();
        }

        internal event Action AllowableModelsUpdated;

        private Type[] allowablemodels = DefaultAllowableFoundations;

        /// <inheritdoc/>
        public Type[] AllowableModels 
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
