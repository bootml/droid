using System.Collections;
using UnityEngine;

namespace SceneAssets.Hide.SharpShadowLight.Scripts.Helpers {
  public class InterfaceTouch : MonoBehaviour {
    //GUIText UIlights;
    //GUIText UIvertex;

    [HideInInspector] public static int _Vertex_Count;

    GameObject _c_light;
    GameObject _cube_l;

    //int lightCount = 1;

    void Start() {
      this._c_light = GameObject.Find("2DLight");

      this.StartCoroutine(this.LoopUpdate());
    }

    // Update is called once per frame
    IEnumerator LoopUpdate() {
      while (true) {
        var pos = this._c_light.transform.position;
        pos.x += Input.GetAxis("Horizontal") * 30f * Time.deltaTime;
        pos.y += Input.GetAxis("Vertical") * 30f * Time.deltaTime;
        yield return new WaitForEndOfFrame();
        this._c_light.transform.position = pos;
      }
    }
  }
}
