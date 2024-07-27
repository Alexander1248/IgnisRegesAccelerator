using UnityEngine;
using UnityEngine.UI;

namespace Render
{
    [ExecuteInEditMode]
    public class CanvasMesh : Graphic
    {
        // Inspector properties
        public Mesh mesh = null;
        public Quaternion rotation;
        public Vector3 scale;
        public Vector3 move;


        /// <summary>
        /// Callback function when a UI element needs to generate vertices.
        /// </summary>
        /// <param name="vh">VertexHelper utility.</param>
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            if (mesh == null) return;
            // Get data from mesh
            var verts = mesh.vertices;
            var uvs = mesh.uv;
            if (uvs.Length < verts.Length)
                uvs = new Vector2[verts.Length];
            // Get mesh bounds parameters
            var meshMin = mesh.bounds.min;
            var meshSize = mesh.bounds.size;
            // Add scaled vertices
            for (var ii = 0; ii < verts.Length; ii++)
            {
                var v = Vector3.Scale(rotation * verts[ii] + move, scale);
                v.x = (v.x - meshMin.x) / meshSize.x;
                v.y = (v.y - meshMin.y) / meshSize.y;
                v.z = (v.z - meshMin.z) / meshSize.z;
                v = Vector3.Scale(v - (Vector3)rectTransform.pivot, rectTransform.rect.size);
                vh.AddVert(v, color, uvs[ii]);
            }
            var tris = mesh.triangles;
            for (var ii = 0; ii < tris.Length; ii += 3)
                vh.AddTriangle(tris[ii], tris[ii + 1], tris[ii + 2]);
        }

        /// <summary>
        /// Converts a vertex in mesh coordinates to a point in world coordinates.
        /// </summary>
        /// <param name="vertex">The input vertex.</param>
        /// <returns>A point in world coordinates.</returns>
        public Vector3 TransformVertex(Vector3 vertex)
        {
            // Convert vertex into local coordinates
            Vector2 v;
            v.x = (vertex.x - mesh.bounds.min.x) / mesh.bounds.size.x;
            v.y = (vertex.y - mesh.bounds.min.y) / mesh.bounds.size.y;
            v = Vector2.Scale(v - rectTransform.pivot, rectTransform.rect.size);
            // Convert from local into world
            return transform.TransformPoint(v);
        }

        /// <summary>
        /// Converts a vertex in world coordinates into a vertex in mesh coordinates.
        /// </summary>
        /// <param name="vertex">The input vertex.</param>
        /// <returns>A point in mesh coordinates.</returns>
        public Vector3 InverseTransformVertex(Vector3 vertex)
        {
            // Convert from world into local coordinates
            Vector2 v = transform.InverseTransformPoint(vertex);
            // Convert into mesh coordinates
            v.x /= rectTransform.rect.size.x;
            v.y /= rectTransform.rect.size.y;
            v += rectTransform.pivot;
            v = Vector2.Scale(v, mesh.bounds.size);
            v.x += mesh.bounds.min.x;
            v.y += mesh.bounds.min.y;
            return vertex;
        }
    }
}
