using System.Collections;
using SceneAssets.Hide.SharpShadowLight.Scripts.Light;
using UnityEngine;

namespace SceneAssets.Hide.SharpShadowLight.Scripts.Helpers {
  public class InterfaceTouch2 : MonoBehaviour {
    //GUIText UIlights;
    //GUIText UIvertex;

    [HideInInspector] public static int _Vertex_Count;
    int _layer;
    DynamicLight _2_ddl;
    bool _ctrl_down;

    bool _mouse_click;
    Camera _cam;

    GameObject _c_light;
    GameObject _cube_l;

    int _light_count = 1;

    void Start() {
      this._cam = GameObject.Find("Camera").GetComponent<Camera>();

      this._c_light = GameObject.Find("2DLight");

      this.StartCoroutine(this.LoopUpdate());
    }

    void Update() {
      this._mouse_click = Input.GetMouseButtonDown(0);
      this._ctrl_down = Input.GetKey(KeyCode.LeftControl);
    }

    // Update is called once per frame
    IEnumerator LoopUpdate() {
      while (true) {
        //cLight = GameObject.Find("2DLight");
        //if(Input.GetAxis("Horizontal")){
        //light.transform.position = new Vector3 (Input.mousePosition.x -Screen.width*.5f, Input.mousePosition.y -Screen.height*.5f);
        var pos = this._c_light.transform.position;
        pos.x += Input.GetAxis("Horizontal") * 30f * Time.deltaTime;
        pos.y += Input.GetAxis("Vertical") * 30f * Time.deltaTime;

        if (this._mouse_click) {
          Vector2 p = this._cam.ScreenToWorldPoint(Input.mousePosition);

          if (this._ctrl_down) {
            this._2_ddl = this._c_light.GetComponent<DynamicLight>();
            this._layer = this._2_ddl._Layer;
            var m = new Material(this._2_ddl._Light_Material);

            var n_light = new GameObject();
            n_light.transform.parent = this._c_light.transform;

            this._2_ddl = n_light.AddComponent<DynamicLight>();
            //m.color = new Color(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f));
            this._2_ddl._Light_Material = m;
            n_light.transform.position = p;
            this._2_ddl._Light_Radius = 40;
            this._2_ddl._Layer = this._layer;

            var quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            quad.transform.parent = n_light.transform;
            quad.transform.localPosition = Vector3.zero;
            this._light_count++;
          }
        }

        yield return new WaitForEndOfFrame();
        this._c_light.transform.position = pos;
      }
    }
  }
}
