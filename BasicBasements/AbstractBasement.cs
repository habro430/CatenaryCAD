using CatenaryCAD;
using CatenaryCAD.Geometry;
using CatenaryCAD.Objects;
using CatenaryCAD.Parts;
using CatenaryCAD.Properties;
using System;
using System.Linq;

namespace BasicBasements
{
    [Serializable]
    [CatenaryObject("TestBasement", "")]
    public class AbstractBasement : IBasement
    {
        public event Action Updated;

        AbstractGeometry<XY>[] geom = new AbstractGeometry<XY>[] { new Rectangle(new Point<XY>(), 100, 100) };

        protected ConcurrentHashSet<IProperty> Properties = new ConcurrentHashSet<IProperty>();

        public AbstractBasement()
        {
            var prop = new Property<int>("01_basement_size", "basement_size", "Фундамент", ConfigFlags.None);

            prop.Value = 100;
            prop.Updated += (val) =>
            {
                geom[0] = new Rectangle(new Point<XY>(), val, val);

                Updated?.Invoke();
            };

            Properties.Add(prop);
        }
        public void GetGeometry(out AbstractGeometry<XY>[] xy, out AbstractGeometry<XYZ>[] xyz)
        {
            xy = geom;
            xyz = null;
        }

        public IPart[] GetParts() => throw new NotImplementedException();

        public IProperty[] GetProperties() => Properties.ToArray();


    }
}
