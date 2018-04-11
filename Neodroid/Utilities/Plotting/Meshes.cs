using UnityEngine;

namespace Neodroid.Utilities.Plotting {
  public class Meshes {
    public static Mesh ConeMesh() {
      Mesh mesh = new Mesh();
      mesh.Clear();

      float height = 1f;
      float bottom_radius = .25f;
      float top_radius = .05f;
      int nb_sides = 18;
      int nb_height_seg = 1; // Not implemented yet

      int nb_vertices_cap = nb_sides + 1;

      #region Vertices

// bottom + top + sides
      Vector3[] vertices = new Vector3[nb_vertices_cap + nb_vertices_cap + nb_sides * nb_height_seg * 2 + 2];
      int vert = 0;
      float _2_pi = Mathf.PI * 2f;

// Bottom cap
      vertices[vert++] = new Vector3(0f, 0f, 0f);
      while (vert <= nb_sides) {
        float rad = (float)vert / nb_sides * _2_pi;
        vertices[vert] = new Vector3(Mathf.Cos(rad) * bottom_radius, 0f, Mathf.Sin(rad) * bottom_radius);
        vert++;
      }

// Top cap
      vertices[vert++] = new Vector3(0f, height, 0f);
      while (vert <= nb_sides * 2 + 1) {
        float rad = (float)(vert - nb_sides - 1) / nb_sides * _2_pi;
        vertices[vert] = new Vector3(Mathf.Cos(rad) * top_radius, height, Mathf.Sin(rad) * top_radius);
        vert++;
      }

// Sides
      int v = 0;
      while (vert <= vertices.Length - 4) {
        float rad = (float)v / nb_sides * _2_pi;
        vertices[vert] = new Vector3(Mathf.Cos(rad) * top_radius, height, Mathf.Sin(rad) * top_radius);
        vertices[vert + 1] = new Vector3(Mathf.Cos(rad) * bottom_radius, 0, Mathf.Sin(rad) * bottom_radius);
        vert += 2;
        v++;
      }

      vertices[vert] = vertices[nb_sides * 2 + 2];
      vertices[vert + 1] = vertices[nb_sides * 2 + 3];

      #endregion

      #region Normales

// bottom + top + sides
      Vector3[] normales = new Vector3[vertices.Length];
      vert = 0;

// Bottom cap
      while (vert <= nb_sides) {
        normales[vert++] = Vector3.down;
      }

// Top cap
      while (vert <= nb_sides * 2 + 1) {
        normales[vert++] = Vector3.up;
      }

// Sides
      v = 0;
      while (vert <= vertices.Length - 4) {
        float rad = (float)v / nb_sides * _2_pi;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        normales[vert] = new Vector3(cos, 0f, sin);
        normales[vert + 1] = normales[vert];

        vert += 2;
        v++;
      }

      normales[vert] = normales[nb_sides * 2 + 2];
      normales[vert + 1] = normales[nb_sides * 2 + 3];

      #endregion

      #region UVs

      Vector2[] uvs = new Vector2[vertices.Length];

// Bottom cap
      int u = 0;
      uvs[u++] = new Vector2(0.5f, 0.5f);
      while (u <= nb_sides) {
        float rad = (float)u / nb_sides * _2_pi;
        uvs[u] = new Vector2(Mathf.Cos(rad) * .5f + .5f, Mathf.Sin(rad) * .5f + .5f);
        u++;
      }

// Top cap
      uvs[u++] = new Vector2(0.5f, 0.5f);
      while (u <= nb_sides * 2 + 1) {
        float rad = (float)u / nb_sides * _2_pi;
        uvs[u] = new Vector2(Mathf.Cos(rad) * .5f + .5f, Mathf.Sin(rad) * .5f + .5f);
        u++;
      }

// Sides
      int u_sides = 0;
      while (u <= uvs.Length - 4) {
        float t = (float)u_sides / nb_sides;
        uvs[u] = new Vector3(t, 1f);
        uvs[u + 1] = new Vector3(t, 0f);
        u += 2;
        u_sides++;
      }

      uvs[u] = new Vector2(1f, 1f);
      uvs[u + 1] = new Vector2(1f, 0f);

      #endregion

      #region Triangles

      int nb_triangles = nb_sides + nb_sides + nb_sides * 2;
      int[] triangles = new int[nb_triangles * 3 + 3];

// Bottom cap
      int tri = 0;
      int i = 0;
      while (tri < nb_sides - 1) {
        triangles[i] = 0;
        triangles[i + 1] = tri + 1;
        triangles[i + 2] = tri + 2;
        tri++;
        i += 3;
      }

      triangles[i] = 0;
      triangles[i + 1] = tri + 1;
      triangles[i + 2] = 1;
      tri++;
      i += 3;

// Top cap
//tri++;
      while (tri < nb_sides * 2) {
        triangles[i] = tri + 2;
        triangles[i + 1] = tri + 1;
        triangles[i + 2] = nb_vertices_cap;
        tri++;
        i += 3;
      }

      triangles[i] = nb_vertices_cap + 1;
      triangles[i + 1] = tri + 1;
      triangles[i + 2] = nb_vertices_cap;
      tri++;
      i += 3;
      tri++;

// Sides
      while (tri <= nb_triangles) {
        triangles[i] = tri + 2;
        triangles[i + 1] = tri + 1;
        triangles[i + 2] = tri + 0;
        tri++;
        i += 3;

        triangles[i] = tri + 1;
        triangles[i + 1] = tri + 2;
        triangles[i + 2] = tri + 0;
        tri++;
        i += 3;
      }

      #endregion

      mesh.vertices = vertices;
      mesh.normals = normales;
      mesh.uv = uvs;
      mesh.triangles = triangles;

      mesh.RecalculateBounds();
//mesh.Optimize();

      return mesh;
    }

    public static Mesh SphereMesh() {
      Mesh mesh = new Mesh();
      mesh.Clear();

      float radius = 1f;
// Longitude |||
      int nb_long = 24;
// Latitude ---
      int nb_lat = 16;

      #region Vertices

      Vector3[] vertices = new Vector3[(nb_long + 1) * nb_lat + 2];
      float pi = Mathf.PI;
      float _2_pi = pi * 2f;

      vertices[0] = Vector3.up * radius;
      for (int lat = 0; lat < nb_lat; lat++) {
        float a1 = pi * (float)(lat + 1) / (nb_lat + 1);
        float sin1 = Mathf.Sin(a1);
        float cos1 = Mathf.Cos(a1);

        for (int lon = 0; lon <= nb_long; lon++) {
          float a2 = _2_pi * (float)(lon == nb_long ? 0 : lon) / nb_long;
          float sin2 = Mathf.Sin(a2);
          float cos2 = Mathf.Cos(a2);

          vertices[lon + lat * (nb_long + 1) + 1] = new Vector3(sin1 * cos2, cos1, sin1 * sin2) * radius;
        }
      }

      vertices[vertices.Length - 1] = Vector3.up * -radius;

      #endregion

      #region Normales

      Vector3[] normales = new Vector3[vertices.Length];
      for (int n = 0; n < vertices.Length; n++)
        normales[n] = vertices[n].normalized;

      #endregion

      #region UVs

      Vector2[] uvs = new Vector2[vertices.Length];
      uvs[0] = Vector2.up;
      uvs[uvs.Length - 1] = Vector2.zero;
      for (int lat = 0; lat < nb_lat; lat++)
        for (int lon = 0; lon <= nb_long; lon++)
          uvs[lon + lat * (nb_long + 1) + 1] = new Vector2(
              (float)lon / nb_long,
              1f - (float)(lat + 1) / (nb_lat + 1));

      #endregion

      #region Triangles

      int nb_faces = vertices.Length;
      int nb_triangles = nb_faces * 2;
      int nb_indexes = nb_triangles * 3;
      int[] triangles = new int[nb_indexes];

//Top Cap
      int i = 0;
      for (int lon = 0; lon < nb_long; lon++) {
        triangles[i++] = lon + 2;
        triangles[i++] = lon + 1;
        triangles[i++] = 0;
      }

//Middle
      for (int lat = 0; lat < nb_lat - 1; lat++) {
        for (int lon = 0; lon < nb_long; lon++) {
          int current = lon + lat * (nb_long + 1) + 1;
          int next = current + nb_long + 1;

          triangles[i++] = current;
          triangles[i++] = current + 1;
          triangles[i++] = next + 1;

          triangles[i++] = current;
          triangles[i++] = next + 1;
          triangles[i++] = next;
        }
      }

//Bottom Cap
      for (int lon = 0; lon < nb_long; lon++) {
        triangles[i++] = vertices.Length - 1;
        triangles[i++] = vertices.Length - (lon + 2) - 1;
        triangles[i++] = vertices.Length - (lon + 1) - 1;
      }

      #endregion

      mesh.vertices = vertices;
      mesh.normals = normales;
      mesh.uv = uvs;
      mesh.triangles = triangles;

      mesh.RecalculateBounds();
      return mesh;
    }
  }
}
