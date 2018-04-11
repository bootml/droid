using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
#endif

namespace Neodroid.Utilities.BoundingBoxes {
  [ExecuteInEditMode]
  public class BoundingBox : MonoBehaviour {
    Vector3 _bottom_back_left;
    Vector3 _bottom_back_right;
    Vector3 _bottom_front_left;
    Vector3 _bottom_front_right;

    [HideInInspector] public Bounds _Bounds;

    [HideInInspector] public Vector3 _Bounds_Offset;

    public DrawBoundingBoxOnCamera _Camera;
    Collider[] _children_colliders;

    MeshFilter[] _children_meshes;

    public bool _Collider_Based;

    Vector3[] _corners;
    public bool _Freeze = true;

    public bool _Include_Children = true;

    // Vector3 startingBoundSize;
    // Vector3 startingBoundCenterLocal;
    Vector3 _last_position;
    Quaternion _last_rotation;

    [HideInInspector]
    //public Vector3 startingScale;
    Vector3 _last_scale;

    public Color _Line_Color = new Color(0f, 1f, 0.4f, 0.74f);

    Vector3[,] _lines;

    Quaternion _rotation;

    public bool _Setup_On_Awake = true;
    Vector3 _top_back_left;
    Vector3 _top_back_right;

    Vector3 _top_front_left;
    Vector3 _top_front_right;

    public Vector3[] BoundingBoxCoordinates {
      get {
        return new[] {
            this._top_front_left,
            this._top_front_right,
            this._top_back_left,
            this._top_back_right,
            this._bottom_front_left,
            this._bottom_front_right,
            this._bottom_back_left,
            this._bottom_back_right
        };
      }
    }

    public Bounds Bounds { get { return this._Bounds; } }

    public Vector3 Max { get { return this._Bounds.max; } }

    public Vector3 Min { get { return this._Bounds.min; } }

    public string BoundingBoxCoordinatesAsString {
      get {
        var str_rep = "";
        str_rep += "\"_top_front_left\":" + this.BoundingBoxCoordinates[0] + ", ";
        str_rep += "\"_top_front_right\":" + this.BoundingBoxCoordinates[1] + ", ";
        str_rep += "\"_top_back_left\":" + this.BoundingBoxCoordinates[2] + ", ";
        str_rep += "\"_top_back_right\":" + this.BoundingBoxCoordinates[3] + ", ";
        str_rep += "\"_bottom_front_left\":" + this.BoundingBoxCoordinates[4] + ", ";
        str_rep += "\"_bottom_front_right\":" + this.BoundingBoxCoordinates[5] + ", ";
        str_rep += "\"_bottom_back_left\":" + this.BoundingBoxCoordinates[6] + ", ";
        str_rep += "\"_bottom_back_right\":" + this.BoundingBoxCoordinates[7];
        return str_rep;
      }
    }

    public string BoundingBoxCoordinatesAsJson {
      get {
        var str_rep = "{";
        str_rep += "\"_top_front_left\":" + this.JsoNifyVec3(this.BoundingBoxCoordinates[0]) + ", ";
        str_rep += "\"_bottom_back_right\":" + this.JsoNifyVec3(this.BoundingBoxCoordinates[7]);
        str_rep += "}";
        return str_rep;
      }
    }

    string JsoNifyVec3(Vector3 vec) { return $"[{vec.x},{vec.y},{vec.z}]"; }

    void Reset() { this.Awake(); }

    void Awake() {
      if (this._Setup_On_Awake) {
        this.Setup();
        this.CalculateBounds();
      }

      this._last_position = this.transform.position;
      this._last_rotation = this.transform.rotation;
      this._last_scale = this.transform.localScale;
      this.Initialise();
      this._children_meshes = this.GetComponentsInChildren<MeshFilter>();
      this._children_colliders = this.GetComponentsInChildren<Collider>();
    }

    void Setup() {
      this._Camera = FindObjectOfType<DrawBoundingBoxOnCamera>();
      this._children_meshes = this.GetComponentsInChildren<MeshFilter>();
      this._children_colliders = this.GetComponentsInChildren<Collider>();
    }

    public void Initialise() {
      this.RecalculatePoints();
      this.RecalculateLines();
    }

    void LateUpdate() {
      if (this._Freeze)
        return;
      if (this._children_meshes != this.GetComponentsInChildren<MeshFilter>())
        this.Reset();
      if (this._children_colliders != this.GetComponentsInChildren<Collider>())
        this.Reset();
      if (this.transform.localScale != this._last_scale) {
        this.ScaleBounds();
        this.RecalculatePoints();
      }

      if (this.transform.position != this._last_position
          || this.transform.rotation != this._last_rotation
          || this.transform.localScale != this._last_scale) {
        this.RecalculateLines();
        this._last_rotation = this.transform.rotation;
        this._last_position = this.transform.position;
        this._last_scale = this.transform.localScale;
      }

      if (this._Camera)
        this._Camera.SetOutlines(this._lines, this._Line_Color, new Vector3[0, 0]);
    }

    public void ScaleBounds() {
      //_bounds.size = new Vector3(startingBoundSize.x * transform.localScale.x / startingScale.x, startingBoundSize.y * transform.localScale.y / startingScale.y, startingBoundSize.z * transform.localScale.z / startingScale.z);
      //_bounds.center = transform.TransformPoint(startingBoundCenterLocal);
    }

    void FitBoundingBoxToChildrenColliders() {
      var col = this.GetComponent<BoxCollider>();
      var bounds = new Bounds(this.transform.position, Vector3.zero); // position and size

      if (col)
        bounds.Encapsulate(col.bounds);

      if (this._Include_Children) {
        foreach (var child_col in this._children_colliders) {
          if (child_col != col)
            bounds.Encapsulate(child_col.bounds);
        }
      }

      this._Bounds = bounds;
      this._Bounds_Offset = bounds.center - this.transform.position;
    }

    void FitBoundingBoxToChildrenRenders() {
      var bounds = new Bounds(this.transform.position, Vector3.zero);

      var mesh = this.GetComponent<MeshFilter>();
      if (mesh) {
        var ms = mesh.sharedMesh;
        var vc = ms.vertexCount;
        for (var i = 0; i < vc; i++)
          bounds.Encapsulate(mesh.transform.TransformPoint(ms.vertices[i]));
      }

      for (var i = 0; i < this._children_meshes.Length; i++) {
        var ms = this._children_meshes[i].sharedMesh;
        var vc = ms.vertexCount;
        for (var j = 0; j < vc; j++)
          bounds.Encapsulate(this._children_meshes[i].transform.TransformPoint(ms.vertices[j]));
      }

      this._Bounds = bounds;
      this._Bounds_Offset = this._Bounds.center - this.transform.position;
    }

    void CalculateBounds() {
      this._rotation = this.transform.rotation;
      this.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

      if (this._Collider_Based)
        this.FitBoundingBoxToChildrenColliders();
      else
        this.FitBoundingBoxToChildrenRenders();

      this.transform.rotation = this._rotation;
    }

    void RecalculatePoints() {
      this._Bounds.size = new Vector3(
          this._Bounds.size.x * this.transform.localScale.x / this._last_scale.x,
          this._Bounds.size.y * this.transform.localScale.y / this._last_scale.y,
          this._Bounds.size.z * this.transform.localScale.z / this._last_scale.z);
      this._Bounds_Offset = new Vector3(
          this._Bounds_Offset.x * this.transform.localScale.x / this._last_scale.x,
          this._Bounds_Offset.y * this.transform.localScale.y / this._last_scale.y,
          this._Bounds_Offset.z * this.transform.localScale.z / this._last_scale.z);

      this._top_front_right = this._Bounds_Offset + Vector3.Scale(this._Bounds.extents, new Vector3(1, 1, 1));
      this._top_front_left = this._Bounds_Offset + Vector3.Scale(this._Bounds.extents, new Vector3(-1, 1, 1));
      this._top_back_left = this._Bounds_Offset + Vector3.Scale(this._Bounds.extents, new Vector3(-1, 1, -1));
      this._top_back_right = this._Bounds_Offset + Vector3.Scale(this._Bounds.extents, new Vector3(1, 1, -1));
      this._bottom_front_right =
          this._Bounds_Offset + Vector3.Scale(this._Bounds.extents, new Vector3(1, -1, 1));
      this._bottom_front_left =
          this._Bounds_Offset + Vector3.Scale(this._Bounds.extents, new Vector3(-1, -1, 1));
      this._bottom_back_left =
          this._Bounds_Offset + Vector3.Scale(this._Bounds.extents, new Vector3(-1, -1, -1));
      this._bottom_back_right =
          this._Bounds_Offset + Vector3.Scale(this._Bounds.extents, new Vector3(1, -1, -1));

      this._corners = new[] {
          this._top_front_right,
          this._top_front_left,
          this._top_back_left,
          this._top_back_right,
          this._bottom_front_right,
          this._bottom_front_left,
          this._bottom_back_left,
          this._bottom_back_right
      };
    }

    void RecalculateLines() {
      var rot = this.transform.rotation;
      var pos = this.transform.position;

      var lines = new List<Vector3[]>();
      //int linesCount = 12;

      for (var i = 0; i < 4; i++) {
        //width
        var line = new[] {rot * this._corners[2 * i] + pos, rot * this._corners[2 * i + 1] + pos};
        lines.Add(line);

        //height
        line = new[] {rot * this._corners[i] + pos, rot * this._corners[i + 4] + pos};
        lines.Add(line);

        //depth
        line = new[] {rot * this._corners[2 * i] + pos, rot * this._corners[2 * i + 3 - 4 * (i % 2)] + pos};
        lines.Add(line);
      }

      this._lines = new Vector3[lines.Count, 2];
      for (var j = 0; j < lines.Count; j++) {
        this._lines[j, 0] = lines[j][0];
        this._lines[j, 1] = lines[j][1];
      }
    }

    void OnMouseDown() {
      //if (_permanent)
      //  return;
      this.enabled = !this.enabled;
    }

    #if UNITY_EDITOR
    void OnValidate() {
      if (EditorApplication.isPlaying)
        return;
      this.Initialise();
    }

        #endif

    void OnDrawGizmos() {
      Gizmos.color = this._Line_Color;
      if (this._lines != null) {
        for (var i = 0; i < this._lines.GetLength(0); i++)
          Gizmos.DrawLine(this._lines[i, 0], this._lines[i, 1]);
      }
    }
  }
}
