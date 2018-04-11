using UnityEngine;

namespace SceneAssets.Hide._2DLightAsset.Scripts {
  public class Rotate : MonoBehaviour {
    // Use this for initialization
    void Start() { }

    // Update is called once per frame
    void LateUpdate() {
      var euler = this.transform.localEulerAngles;
      euler.z += 2f;
      this.transform.localEulerAngles = euler;
    }
  }
}
