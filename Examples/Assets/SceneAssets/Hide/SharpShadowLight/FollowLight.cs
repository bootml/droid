using UnityEngine;

namespace SceneAssets.Hide.SharpShadowLight {
  public class FollowLight : MonoBehaviour {
    public GameObject _To_Follow;

    // Update is called once per frame
    void Update() { this.gameObject.transform.position = this._To_Follow.transform.position; }
  }
}
