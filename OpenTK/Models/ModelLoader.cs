using JeremyAnsel.Media.WavefrontObj;
using SharpGLTF.Schema2;
using System.Numerics;

namespace OpenTKProject
{
    public enum ModelFormat
    {
        Obj,
        Gltf
    }

    public class VisualModel
    {
        private string path;
        private ModelFormat format;
        public string PathToModel { get { return path; } }
        public ModelFormat Format { get { return format; } }

        private List<float> vertices = new List<float>();
        private List<uint> indices = new List<uint>();

        private List<Vector3> tempPositions = new List<Vector3>();
        private List<Vector2> tempUVs = new List<Vector2>();
        private List<Vector3> tempNormals = new List<Vector3>();
        private List<Vector4> tempTangents = new List<Vector4>();

        public VisualModel(string modelPath, ModelFormat modelFormat = ModelFormat.Obj)
        {
            path = modelPath;
            format = modelFormat;

            switch (format)
            {
                case ModelFormat.Obj:
                    LoadObjModel();
                    break;
                case ModelFormat.Gltf:
                    LoadGltfModel();
                    break;
            }
        }

        private void LoadObjModel()
        {
            var model = ObjFile.FromFile(path);

            tempPositions.Clear();
            tempUVs.Clear();
            tempNormals.Clear();
            tempTangents.Clear();
            vertices.Clear();
            indices.Clear();

            var vertexMap = new Dictionary<(int posIdx, int uvIdx, int normIdx), uint>();

            foreach (var face in model.Faces)
            {
                foreach (var vertex in face.Vertices)
                {
                    var key = (vertex.Vertex - 1, vertex.Texture - 1, vertex.Normal - 1);

                    if (!vertexMap.TryGetValue(key, out uint index))
                    {
                        index = (uint)tempPositions.Count;
                        vertexMap[key] = index;

                        var position = model.Vertices[vertex.Vertex - 1].Position;
                        tempPositions.Add(new Vector3(position.X, position.Y, position.Z));

                        if (vertex.Texture > 0 && model.TextureVertices.Count > 0)
                        {
                            var texCoord = model.TextureVertices[vertex.Texture - 1];
                            tempUVs.Add(new Vector2(texCoord.X, texCoord.Y));
                        }
                        else
                        {
                            tempUVs.Add(Vector2.Zero);
                        }

                        if (vertex.Normal > 0 && model.VertexNormals.Count > 0)
                        {
                            var normal = model.VertexNormals[vertex.Normal - 1];
                            tempNormals.Add(new Vector3(normal.X, normal.Y, normal.Z));
                        }
                        else
                        {
                            tempNormals.Add(Vector3.UnitY);
                        }

                        tempTangents.Add(Vector4.Zero);
                    }

                    indices.Add(index);
                }
            }

            CalculateTangents();

            for (int i = 0; i < tempPositions.Count; i++)
            {
                vertices.Add(tempPositions[i].X);
                vertices.Add(tempPositions[i].Y);
                vertices.Add(tempPositions[i].Z);

                vertices.Add(tempUVs[i].X);
                vertices.Add(tempUVs[i].Y);

                vertices.Add(tempNormals[i].X);
                vertices.Add(tempNormals[i].Y);
                vertices.Add(tempNormals[i].Z);

                vertices.Add(tempTangents[i].X);
                vertices.Add(tempTangents[i].Y);
                vertices.Add(tempTangents[i].Z);
            }
        }

        private void CalculateTangents()
        {
            Vector3[] tan1 = new Vector3[tempPositions.Count];
            Vector3[] tan2 = new Vector3[tempPositions.Count];

            for (int i = 0; i < indices.Count; i += 3)
            {
                int i1 = (int)indices[i];
                int i2 = (int)indices[i + 1];
                int i3 = (int)indices[i + 2];

                Vector3 v1 = tempPositions[i1];
                Vector3 v2 = tempPositions[i2];
                Vector3 v3 = tempPositions[i3];

                Vector2 w1 = tempUVs[i1];
                Vector2 w2 = tempUVs[i2];
                Vector2 w3 = tempUVs[i3];

                Vector3 deltaPos1 = v2 - v1;
                Vector3 deltaPos2 = v3 - v1;

                Vector2 deltaUV1 = w2 - w1;
                Vector2 deltaUV2 = w3 - w1;

                float r = 1.0f / (deltaUV1.X * deltaUV2.Y - deltaUV1.Y * deltaUV2.X);

                Vector3 tangent = (deltaPos1 * deltaUV2.Y - deltaPos2 * deltaUV1.Y) * r;
                Vector3 bitangent = (deltaPos2 * deltaUV1.X - deltaPos1 * deltaUV2.X) * r;

                tan1[i1] += tangent;
                tan1[i2] += tangent;
                tan1[i3] += tangent;

                tan2[i1] += bitangent;
                tan2[i2] += bitangent;
                tan2[i3] += bitangent;
            }

            for (int i = 0; i < tempPositions.Count; i++)
            {
                Vector3 n = tempNormals[i];
                Vector3 t = tan1[i];

                if (t.LengthSquared() < 0.0001f)
                {
                    Vector3 arbitrary = Math.Abs(n.X) < 0.9f ? Vector3.UnitX : Vector3.UnitZ;
                    t = Vector3.Cross(n, arbitrary);
                }

                Vector3 tangent = Vector3.Normalize(t - n * Vector3.Dot(n, t));

                float handedness = (Vector3.Dot(Vector3.Cross(n, t), tan2[i]) < 0.0f) ? -1.0f : 1.0f;

                tempTangents[i] = new Vector4(tangent, handedness);
            }
        }

        private void LoadGltfModel()
        {
            var model = ModelRoot.Load(path);

            uint vertexOffset = 0;

            tempPositions.Clear();
            tempUVs.Clear();
            tempNormals.Clear();
            tempTangents.Clear();
            vertices.Clear();
            indices.Clear();

            foreach (var node in model.DefaultScene.VisualChildren)
            {
                if (node.Mesh == null) continue;

                var mesh = node.Mesh;
                var transform = node.WorldMatrix;

                foreach (var primitive in mesh.Primitives)
                {
                    var positionAccessor = primitive.GetVertexAccessor("POSITION");
                    var normalAccessor = primitive.GetVertexAccessor("NORMAL");
                    var texCoordAccessor = primitive.GetVertexAccessor("TEXCOORD_0");
                    var tangentAccessor = primitive.GetVertexAccessor("TANGENT");

                    if (positionAccessor == null) continue;

                    var positions = positionAccessor.AsVector3Array();
                    var normals = normalAccessor?.AsVector3Array();
                    var texCoords = texCoordAccessor?.AsVector2Array();
                    var tangents = tangentAccessor?.AsVector4Array();

                    for (int i = 0; i < positions.Count; i++)
                    {
                        var worldPos = Vector3.Transform(positions[i], transform);
                        tempPositions.Add(worldPos);

                        if (texCoords != null && i < texCoords.Count)
                        {
                            tempUVs.Add(new Vector2(texCoords[i].X, texCoords[i].Y));
                        }
                        else
                        {
                            tempUVs.Add(Vector2.Zero);
                        }

                        if (normals != null && i < normals.Count)
                        {
                            var worldNormal = Vector3.TransformNormal(normals[i], transform);
                            tempNormals.Add(Vector3.Normalize(worldNormal));
                        }
                        else
                        {
                            tempNormals.Add(Vector3.UnitY);
                        }

                        if (tangents != null && i < tangents.Count)
                        {
                            Vector3 tangentVec = new Vector3(tangents[i].X, tangents[i].Y, tangents[i].Z);
                            var worldTangent = Vector3.TransformNormal(tangentVec, transform);
                            float handedness = tangents[i].W;
                            tempTangents.Add(new Vector4(Vector3.Normalize(worldTangent), handedness));
                        }
                        else
                        {
                            tempTangents.Add(new Vector4(Vector3.UnitX, 1.0f));
                        }
                    }

                    var indicesAccessor = primitive.GetIndexAccessor();
                    if (indicesAccessor != null)
                    {
                        var indicesArray = indicesAccessor.AsIndicesArray();
                        foreach (var idx in indicesArray)
                        {
                            indices.Add(vertexOffset + (uint)idx);
                        }
                    }
                    else
                    {
                        for (uint i = 0; i < positions.Count; i++)
                        {
                            indices.Add(vertexOffset + i);
                        }
                    }

                    vertexOffset += (uint)positions.Count;
                }
            }

            bool hasTangents = false;
            foreach (var t in tempTangents)
            {
                if (t.X != 1.0f || t.Y != 0.0f || t.Z != 0.0f)
                {
                    hasTangents = true;
                    break;
                }
            }

            if (!hasTangents)
            {
                CalculateTangents();
            }

            for (int i = 0; i < tempPositions.Count; i++)
            {
                vertices.Add(tempPositions[i].X);
                vertices.Add(tempPositions[i].Y);
                vertices.Add(tempPositions[i].Z);
                vertices.Add(tempUVs[i].X);
                vertices.Add(tempUVs[i].Y);
                vertices.Add(tempNormals[i].X);
                vertices.Add(tempNormals[i].Y);
                vertices.Add(tempNormals[i].Z);
                vertices.Add(tempTangents[i].X);
                vertices.Add(tempTangents[i].Y);
                vertices.Add(tempTangents[i].Z);
            }
        }

        public List<float> Vertices { get { return vertices; } }
        public List<uint> Indices { get { return indices; } }
    }
}