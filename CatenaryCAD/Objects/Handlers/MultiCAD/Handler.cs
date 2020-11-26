using CatenaryCAD.Geometry;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Properties;
using Multicad;
using Multicad.CustomObjectBase;
using Multicad.DatabaseServices;
using Multicad.Geometry;
using Multicad.Mc3D;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using static CatenaryCAD.Extensions;

using Matrix3d = Multicad.Geometry.Matrix3d;
using Point3d = Multicad.Geometry.Point3d;
using Vector3d = Multicad.Geometry.Vector3d;

namespace CatenaryCAD.Models.Handlers
{
    [Serializable]
    internal abstract class Handler : McCustomBase, IMcDynamicProperties, IHandler
    {
        private IModel model;
        public IModel Model 
        { 
            get => model;
            set
            {
                if (!TryModify()) return;

                var newmodel = value as Model;

                if (model != null)
                {
                    newmodel.Position = model.Position;
                    newmodel.Direction = model.Direction;

                    newmodel.Identifier = model.Identifier;

                    newmodel.Parent = model.Parent;

                    foreach (var chil in model.Childrens)
                        chil.Parent = newmodel;
                }
                else
                {
                    newmodel.Identifier = new McIdentifier(ID);
                }

                newmodel.TryModify += () => TryModify();
                newmodel.Update += () => DbEntity.Update();
                newmodel.Remove += () => McObjectManager.Erase(ID);

                model = newmodel;
            }
        }

        protected ConcurrentHashSet<IProperty> Properties = new ConcurrentHashSet<IProperty>();
        public IProperty[] GetProperties()
        {
            if (Model != null)
            {
                var props = Model.Properties;

                if (props != null)
                    return Properties.Concat(props).ToArray();
            }
            return Properties.ToArray();
        }


        public override hresult PlaceObject()
        {
            hresult result = base.PlaceObject();
            DbEntity.AddToCurrentDocument();

            return result;
        }
        public virtual hresult PlaceObject(Point3d position, Vector3d direction)
        {
            Model.Position = position.ToCatenaryCAD_3D();
            Model.Direction = direction.ToCatenaryCAD_3D();

            return PlaceObject();
        }

        public override void OnErase()
        {
            Model.Parent = null;

            foreach (var child in Model.Childrens)
                McObjectManager.Erase((child.Identifier as McIdentifier).ToMcObjectId());
        }

        public override void OnTransform(Matrix3d tfm) => Model.TransformBy(tfm.ToCatenaryCAD());

        public override bool GetECS(out Matrix3d tfm)
        {
            double angle = Model.Direction.ToMultiCAD().GetAngleTo(Vector3d.XAxis,Vector3d.ZAxis);
            tfm = Matrix3d.Displacement(Model.Position.ToMultiCAD().GetAsVector()).PostMultiplyBy(
                 Matrix3d.Rotation(-angle, Vector3d.ZAxis, Point3d.Origin));

            return true;
        }
        public override List<McObjectId> GetDependent() => Model.Childrens.Concat(Model.Dependencies)
                                                                .Select(m => (m.Identifier as McIdentifier).ToMcObjectId())
                                                                .ToList();

        public override bool OnGetOsnapPoints(OsnapMode osnapMode, Point3d pickPoint, Point3d lastPoint, List<Point3d> osnapPoints)
        {
            switch (osnapMode)
            {
                case OsnapMode.Center:
                    osnapPoints.Add(Model.Position.ToMultiCAD());
                    break;
            }
            return true;
        }

        public override void OnDraw(GeometryBuilder dc)
        {

            dc.Clear();

            dc.Color = Multicad.Constants.Colors.ByObject;
            dc.LineType = Multicad.Constants.LineTypes.ByObject;


            if (Model != null)
            {

#if DEBUG
                //если скомпилировано в DEBUG то то отрисовываем центр модели
                dc.DrawCircle(Point3d.Origin, 5);
                dc.DrawLine(new Point3d(-15d, -15d, 0d), new Point3d(15d, 15d, 0d));
                dc.DrawLine(new Point3d(-15d, 15d, 0d), new Point3d(15d, -15d, 0d));
#endif

                var mode = (OperationalMode)(McDocument.ActiveDocument
                            .CustomProperties["OperationalMode"] ?? OperationalMode.Scheme);

                switch (mode)
                {
                    case OperationalMode.Scheme:

                        var geometry_scheme = Model.GetGeometryForScheme();

                        if (geometry_scheme != null)
                        {
                            for (int igeom = 0; igeom < geometry_scheme.Length; igeom++)
                            {
                                var geometry = geometry_scheme[igeom];

                                foreach (var edge in geometry.Indices)
                                    dc.DrawLine(geometry.Vertices[edge[0]].ToMultiCAD(),
                                                geometry.Vertices[edge[1]].ToMultiCAD());
                            }
                        }

                        break;

                    case OperationalMode.Layout:

                        var geometry_layout = Model.GetGeometryForLayout(); ;

                        //string model = "v -500.000000 500.000000 500.000000" + Environment.NewLine +
                        //                "v -500.000000 500.000000 -500.000000" + Environment.NewLine +
                        //                "v 500.000000 500.000000 500.000000" + Environment.NewLine +
                        //                "v 500.000000 500.000000 -500.000000" + Environment.NewLine +
                        //                "v -500.000000 -500.000000 500.000000" + Environment.NewLine +
                        //                "v -500.000000 -500.000000 -500.000000" + Environment.NewLine +
                        //                "v 500.000000 -500.000000 500.000000" + Environment.NewLine +
                        //                "v 500.000000 -500.000000 -500.000000" + Environment.NewLine +
                        //                "vn 0.0000 0.0000 1.0000" + Environment.NewLine +
                        //                "vn 1.0000 0.0000 0.0000" + Environment.NewLine +
                        //                "vn 0.0000 -1.0000 0.0000" + Environment.NewLine +
                        //                "vn 0.0000 0.0000 -1.0000" + Environment.NewLine +
                        //                "vn 0.0000 1.0000 0.0000" + Environment.NewLine +
                        //                "vn -1.0000 0.0000 0.0000" + Environment.NewLine +
                        //                "f 5//1 3//1 1//1" + Environment.NewLine +
                        //                "f 3//2 8//2 4//2" + Environment.NewLine +
                        //                "f 7//3 6//3 8//3" + Environment.NewLine +
                        //                "f 2//4 8//4 6//4" + Environment.NewLine +
                        //                "f 1//5 4//5 2//5" + Environment.NewLine +
                        //                "f 5//6 2//6 6//6" + Environment.NewLine +
                        //                "f 5//1 7//1 3//1" + Environment.NewLine +
                        //                "f 3//2 7//2 8//2" + Environment.NewLine +
                        //                "f 7//3 5//3 6//3" + Environment.NewLine +
                        //                "f 2//4 4//4 8//4" + Environment.NewLine +
                        //                "f 1//5 3//5 4//5" + Environment.NewLine +
                        //                "f 5//6 1//6 2//6";

                        

                        //Multicad.Geometry.Mesh mesh = new Multicad.Geometry.Mesh();
                        //mesh.InitAsShell(verticles, new Vector3d[] { }, indices, false);

                        //dc.DrawMesh(mesh, -1);

                        if (geometry_layout != null)
                        {
                            foreach (IMesh geometry in geometry_layout)
                            {
                                Point3d[] verticles = geometry.Vertices.Select(v => v.ToMultiCAD()).ToArray();
                                List<int> indices = new List<int>();

                                foreach (int[] face in geometry.Indices)
                                {
                                    indices.Add(face.Length);
                                    indices.AddRange(face);
                                }

                                Multicad.Geometry.Mesh mesh = new Multicad.Geometry.Mesh();
                                mesh.InitAsShell(verticles, new Vector3d[] { }, indices.ToArray(), false);

                                dc.DrawMesh(mesh, -1);
                            }
                        }

                        break;
                }
            }
            else
            {
                dc.DrawText(new TextGeom("ERR OBJ", Point3d.Origin, Vector3d.XAxis, HorizTextAlign.Center, VertTextAlign.Center, "normal", 1000, 1000));
            }

        }
        public static void FromObj1(string obj, out List<Point3d> vertices, out List<Vector3d> normals, out List<int> indices)
        {
            string[] lines = obj.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            vertices = new List<Point3d>();
            normals = new List<Vector3d>();
            indices = new List<int>();

            var normalssrc = new List<Vector3d>();

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] == null)
                    continue;

                string[] data = lines[i].Split(' ');

                bool success;
                switch (data[0])
                {
                    case "v": //парсер вершин
                        double xv, yv, zv;

                        success = double.TryParse(data[1], NumberStyles.Any, CultureInfo.InvariantCulture, out xv);
                        if (!success) throw new ArgumentException("Невозможно преобразовать параметр X как число double");

                        success = double.TryParse(data[2], NumberStyles.Any, CultureInfo.InvariantCulture, out yv);
                        if (!success) throw new ArgumentException("Невозможно преобразовать параметр Y как число double");

                        success = double.TryParse(data[3], NumberStyles.Any, CultureInfo.InvariantCulture, out zv);
                        if (!success) throw new ArgumentException("Невозможно преобразовать параметр Z как число double");

                        vertices.Add(new Point3d(xv, yv, zv));
                        normals.Add(Vector3d.Identity);

                        break;

                    case "vn": //парсер нормалей
                        double xn, yn, zn;

                        success = double.TryParse(data[1], NumberStyles.Any, CultureInfo.InvariantCulture, out xn);
                        if (!success) throw new ArgumentException("Невозможно преобразовать параметр X как число double");

                        success = double.TryParse(data[2], NumberStyles.Any, CultureInfo.InvariantCulture, out yn);
                        if (!success) throw new ArgumentException("Невозможно преобразовать параметр Y как число double");

                        success = double.TryParse(data[3], NumberStyles.Any, CultureInfo.InvariantCulture, out zn);
                        if (!success) throw new ArgumentException("Невозможно преобразовать параметр Z как число double");

                        normalssrc.Add(new Vector3d(xn, yn, zn));
                        break;

                    case "f": // парсер многоугольника
                        int vcount = data.Length -1;
                        int[] edge = new int[vcount];

                        indices.Add(vcount);

                        for (int ivert = 0; ivert < vcount; ivert++)
                        {
                            string[] parts = data[ivert + 1].Split('/');

                            int vindex, nindex;
                            success = int.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out vindex);
                            if (!success) throw new ArgumentException("Невозможно преобразовать параметр как число int");

                            success = int.TryParse(parts[2], NumberStyles.Any, CultureInfo.InvariantCulture, out nindex);
                            if (!success) throw new ArgumentException("Невозможно преобразовать параметр как число int");

                            edge[ivert] = vindex - 1;
                            normals[vindex - 1] = normalssrc[nindex - 1];
                        }

                        indices.AddRange(edge);
                        break;
                }
            }
            //return new Mesh(faces.ToArray());
        }

        public virtual ICollection<McDynamicProperty> GetProperties(out bool exclusive)
        {
            exclusive = true;
            return GetProperties().OrderBy(n => n.Identifier).ToAdapterProperty();
        }
        public McDynamicProperty GetProperty(string id) => null;
            //GetProperties().Where(n => n.ID == id).FirstOrDefault().ToAdapterProperty() ;

    }
}
