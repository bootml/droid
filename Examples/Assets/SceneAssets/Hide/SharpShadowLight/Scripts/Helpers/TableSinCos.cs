using UnityEngine;

namespace SceneAssets.Hide.SharpShadowLight.Scripts.Helpers {
  public static class TableSinCos {
    static bool _instanced;

    public static float[] _Sen_Array;
    public static float[] _Cos_Array;

    public static void Init() {
      if (_instanced) return;

      _Sen_Array = new float[360];
      _Cos_Array = new float[360];

      for (var i = 0; i < 360; i++) {
        _Sen_Array[i] = Mathf.Sin(i * Mathf.Deg2Rad);
        _Cos_Array[i] = Mathf.Cos(i * Mathf.Deg2Rad);
      }

      _instanced = true;
    }
  }
}
