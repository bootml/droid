using UnityEngine;

namespace SceneAssets.LunarLander.Scripts {
  [ExecuteInEditMode]
  public class FollowTarget : MonoBehaviour {
    public Vector3 _Offset = new Vector3(0f, 7.5f, 0f);

    public Transform _Target;

    void LateUpdate() { this.transform.position = this._Target.position + this._Offset; }
  }
}
