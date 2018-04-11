#if UNITY_EDITOR
using UnityEngine;

/**
 * Despite the MonoBehaviour inheritance, this is an Editor-only script.
 */
[ExecuteInEditMode]
public class pg_SceneMeshRender : MonoBehaviour {
  public Material material;

  public Mesh mesh;

  // HideFlags.DontSaveInEditor isn't exposed for whatever reason, so do the bit math on ints 
  // and just cast to HideFlags.
  // HideFlags.HideInHierarchy | HideFlags.DontSaveInEditor | HideFlags.NotEditable
  readonly HideFlags SceneCameraHideFlags = (HideFlags)(1 | 4 | 8);

  void OnDestroy() {
    if (this.mesh) DestroyImmediate(this.mesh);
    if (this.material) DestroyImmediate(this.material);
  }

  void OnRenderObject() {
    // instead of relying on 'SceneCamera' string comparison, check if the hideflags match.
    // this could probably even just check for one bit match, since chances are that any 
    // game view camera isn't going to have hideflags set.
    if ((Camera.current.gameObject.hideFlags & this.SceneCameraHideFlags) != this.SceneCameraHideFlags
        || Camera.current.name != "SceneCamera")
      return;

    if (this.material == null || this.mesh == null) {
      DestroyImmediate(this.gameObject);
      // Debug.Log("NULL MESH || MATERIAL");
      return;
    }

    this.material.SetPass(0);
    Graphics.DrawMeshNow(this.mesh, Vector3.zero, Quaternion.identity, 0);
  }
}
#endif
