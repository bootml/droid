using UnityEngine;

namespace SceneAssets.LunarLander.Scripts {
  [ExecuteInEditMode]
  [RequireComponent(typeof(LineRenderer))]
  public class ConnectedLine : MonoBehaviour {
    public Transform _Connection_To;
    LineRenderer _line_renderer;

    public Vector3 _Offset = Vector3.up;

    // Use this for initialization
    void Start() {
      this._line_renderer = this.GetComponent<LineRenderer>();
      if (!this._Connection_To) this._Connection_To = this.GetComponentInParent<Transform>();
    }

    // Update is called once per frame
    void Update() {
      if (this._Connection_To) {
        this._line_renderer.SetPosition(
            1,
            this.transform.InverseTransformPoint(
                this._Connection_To.TransformPoint(this._Connection_To.localPosition + this._Offset)));
      }
    }
  }
}
