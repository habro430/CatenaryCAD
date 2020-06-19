using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WaveformParser.Types;
using Multicad.Geometry;
namespace WaveformParser
{
	public class Waveform
	{
		public List<Vertex> Vertices { get; private set; }
		public List<Face> Faces { get; private set; }

		public Extent Size { get; private set; }

        /// <summary>
        /// Constructor. Initializes VertexList, FaceList and TextureList.
        /// </summary>
	    public Waveform()
	    {
            Vertices = new List<Vertex>();
            Faces = new List<Face>();
            //TextureList = new List<TextureVertex>();
        }

        /// <summary>
        /// Load .obj from a filepath.
        /// </summary>
        /// <param name="file"></param>
        public void Load(string path)
        {
            Load(File.ReadAllLines(path));
        }

        /// <summary>
        /// Load .obj from a stream.
        /// </summary>
        /// <param name="file"></param>
	    public void Load(Stream data)
	    {
            using (var reader = new StreamReader(data))
            {
                Load(reader.ReadToEnd().Split(Environment.NewLine.ToCharArray()));
            }
	    }

        /// <summary>
        /// Load .obj from a list of strings.
        /// </summary>
        /// <param name="data"></param>
	    public void Load(IEnumerable<string> data)
	    {
            foreach (var line in data)
            {
                processLine(line);
            }

            updateSize();
        }

	    /// <summary>
		/// Sets our global object size with an extent object
		/// </summary>
		private void updateSize()
		{
            // If there are no vertices then size should be 0.
	        if (Vertices.Count == 0)
	        {
	            Size = new Extent
	            {
                    XMax = 0,
                    XMin = 0,
                    YMax = 0,
                    YMin = 0,
                    ZMax = 0,
                    ZMin = 0
	            };

	            // Avoid an exception below if VertexList was empty.
	            return;
	        }

			Size = new Extent
			{
				XMax = Vertices.Max(v => v.X),
				XMin = Vertices.Min(v => v.X),
				YMax = Vertices.Max(v => v.Y),
				YMin = Vertices.Min(v => v.Y),
				ZMax = Vertices.Max(v => v.Z),
				ZMin = Vertices.Min(v => v.Z)
			};		
		}

		/// <summary>
		/// Parses and loads a line from an OBJ file.
		/// Currently only supports V, VT, F and MTLLIB prefixes
		/// </summary>		
		private void processLine(string line)
		{
			string[] parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

			if (parts.Length > 0)
			{
				switch (parts[0])
				{
					//case "usemtl":
					//	UseMtl = parts[1];
					//	break;
					//case "mtllib":
					//	Mtl = parts[1];
					//	break;
					case "v":
						Vertex v = new Vertex();
						v.LoadFromStringArray(parts);
						Vertices.Add(v);
						v.Index = Vertices.Count();
						break;
					case "f":
						Face f = new Face();
						f.LoadFromStringArray(parts);
						//f.UseMtl = UseMtl;
						Faces.Add(f);
						break;
					//case "vt":
					//	TextureVertex vt = new TextureVertex();
					//	vt.LoadFromStringArray(parts);
					//	TextureList.Add(vt);
					//	vt.Index = TextureList.Count();
					//	break;

				}
			}
		}



    }
}
