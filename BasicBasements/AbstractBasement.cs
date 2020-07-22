using CatenaryCAD.Geometry;
using CatenaryCAD.Objects;
using CatenaryCAD.Parts;
using CatenaryCAD.Properties;
using System;

namespace BasicBasements
{
    [Serializable]
    [CatenaryObject("TestBasement", "")]
    public class AbstractBasement : IBasement
    {
        public event Action Updated;

        AbstractGeometry<XY>[] geom = new AbstractGeometry<XY>[] { new Rectangle(new Point<XY>(), 100, 100) };

        IProperty[] props;
        public AbstractBasement()
        {
            var prop = new Property<int>("01_basement_size", "basement_size", "Фундамент", ConfigFlags.None);

            prop.Value = 100;
            prop.Updated += (val) =>
            {
                geom[0] = new Rectangle(new Point<XY>(), val, val);

                Updated?.Invoke();
            };

            props = new IProperty[] { prop };
        }
        public void GetGeometry(out AbstractGeometry<XY>[] xy, out AbstractGeometry<XYZ>[] xyz)
        {
            xy = geom;
            xyz = null;
        }

        public IPart[] GetParts() => throw new NotImplementedException();
        public IProperty[] GetProperties() => props;


    }
}
