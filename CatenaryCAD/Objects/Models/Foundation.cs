using CatenaryCAD.Models.Attributes;

using System;
using System.Linq;

namespace CatenaryCAD.Models
{
    /// <summary>
    /// Класс, описывающий модель фундамента опоры контактной сети.
    /// </summary>
    [Serializable]
    public abstract class Foundation : Model, IFoundation
    {

        [NonSerialized]
        internal static readonly Type[] DefaultAvailableFoundations;
        static Foundation()
        {
            DefaultAvailableFoundations = Main.GetCatenaryObjects(typeof(IFoundation))
                .Where((t) => !t.IsAbstract)
                .Where((t) => Attribute.GetCustomAttribute(t, typeof(ModelNonBrowsableAttribute), false) is null).ToArray();
        }

        internal event Action AvailableFoundationsUpdated;

        private Type[] avaliblefoundations = DefaultAvailableFoundations;

        /// <inheritdoc/>
        /// <value>По умолчанию возвращает все модели, наследуемые от <seealso cref="IFoundation"/>.</value>
        public Type[] AvailableFoundations
        { 
            get => avaliblefoundations;
            set
            {
                avaliblefoundations = value;
                AvailableFoundationsUpdated?.Invoke();
            }
        }
    }
}
