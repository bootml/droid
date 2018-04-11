using UnityEngine;

namespace SceneAssets.ScriptedGrasper {
  [ExecuteInEditMode]
  public class SimpleBlur : MonoBehaviour {
    [SerializeField] Material _background;

    void OnRenderImage(RenderTexture src, RenderTexture dst) { Graphics.Blit(src, dst, this._background); }
  }
}
