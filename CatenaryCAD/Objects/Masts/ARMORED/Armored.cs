using CatenaryCAD.Geometry;
using CatenaryCAD.Properties;
using Multicad.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Runtime.Caching;
using System.Text;
using WaveformParser;

namespace CatenaryCAD.Objects.Masts
{
    [Serializable]
    [Mast("Железобетонная")]
    public class Armored : AbstractMast
    {
        public override event Action Updated;

        //кэш 3d моделей загружаемых из ресурсов при первом доступе к ним
        [NonSerialized]
        private static ObjectCache GeometryCache = new MemoryCache(typeof(Metall).Name);

        [NonSerialized]
        private static readonly Dictionary<string, Type> InheritedMasts;
        //при первом вызове класса кэшируем в словарь производные от него опоры в статику
        static Armored() => InheritedMasts = AbstractMast.GetInheritedMastsFor(typeof(Armored));

        private static Mesh[] GetOrCreateFromCache(string key)
        {
            //есть ли 3d модель в кэше
            if (!GeometryCache.Contains(key))
            {
                //читаем 3d модель из ресурсов и кэшируем 

                ResourceManager rm = new ResourceManager(typeof(Properties.Resources));
                GeometryCache.Set(key, GenarateMeshFrom(rm.GetString(key)), new CacheItemPolicy());
            }

            //читаем модель из кэша и возвращаем
            return GeometryCache.Get(key) as Mesh[];
        }

        private static Mesh[] GenarateMeshFrom(string base64)
        {
            string[] model = Encoding.Default.GetString(Convert.FromBase64String(base64)).
                                    Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            var obj = new Waveform();
            obj.Load(model);

            List<Mesh> meshes = new List<Mesh>(obj.Faces.Count);

            foreach (var face in obj.Faces)
            {
                if (face.Vertices.Count() != 4) throw new ArgumentException();

                Mesh mesh = new Mesh(2, 2);
                mesh.Vertices[0, 1].Coordinate = (Point3d)obj.Vertices[face.Vertices[0] - 1];
                mesh.Vertices[1, 1].Coordinate = (Point3d)obj.Vertices[face.Vertices[1] - 1];
                mesh.Vertices[1, 0].Coordinate = (Point3d)obj.Vertices[face.Vertices[2] - 1];
                mesh.Vertices[0, 0].Coordinate = (Point3d)obj.Vertices[face.Vertices[3] - 1];

                meshes.Add(mesh);
            }

            return meshes.ToArray();
        }

        private Mesh[] Geometry = null;

        public Armored()
        {
            //генериуем геометрию для 2D режима
            Geometry2D = new AbstractGeometry[] { new Circle(new Geometry.Core.Point(), 600, 20) };


            if (InheritedMasts.Count > 0)
            {
                Property<Type> mast_subtype = new Property<Type>("02_mast_armored_type", "Марка стойки", "Стойка",  PropertyFlags.RefreshAfterChange);
                
                mast_subtype.DictionaryValues = InheritedMasts;
                mast_subtype.Value = mast_subtype.DictionaryValues.Values.FirstOrDefault();

                properties.Add(mast_subtype);
            }
            else
            {
                properties.Add(new Property<string>("02_mast_armored_type", "Тип", "Стойка"));

            }

            Property<int> m_len = new Property<int>("03_mast_len", "Длинна", "Стойка", PropertyFlags.RefreshAfterChange);

            m_len.DictionaryValues = new Dictionary<string, int>
            {
                ["10.0 м"] = 10000,
                ["12.0 м"] = 12000,
                ["15.0 м"] = 15000,

            };

            m_len.Updated += len_updated;
            m_len.Value = m_len.DictionaryValues.Values.FirstOrDefault();

            properties.Add(m_len);
        }
        private void len_updated(int value)
        {
            switch (value)
            {
                case 10000:
                    Geometry = GetOrCreateFromCache("a_10");
                    break;

                case 12000:
                    Geometry = GetOrCreateFromCache("a_12");
                    break;

                case 15000:
                    Geometry = GetOrCreateFromCache("a_15");
                    break;
            }

            Updated?.Invoke();
        }

        private List<AbstractProperty> properties = new List<AbstractProperty>();
        public override AbstractProperty[] GetProperties() => properties.ToArray();

        //public override void GetGeometry2D(GeometryBuilder geometry, Color color, double scale)
        //{
        //    geometry.Color = Multicad.Constants.Colors.ByObject;
        //    geometry.LineType = Multicad.Constants.LineTypes.ByObject;

           
        //    //foreach (var edge in circle.Edges)
        //    //{
        //    //    geometry.DrawLine(circle.Points[edge.Item1].ToNanoCAD(), circle.Points[edge.Item2].ToNanoCAD());
        //    //}

        //}
        //public override void GetGeometry3D(GeometryBuilder geometry, Color color, double scale)
        //{
        //    foreach (var mesh in Geometry)
        //    {
        //        foreach (var edge in mesh.Edges.Edges)
        //            edge.Color = color;
        //        geometry.DrawMesh(mesh, -1);
        //    }
        //}
    }
}
