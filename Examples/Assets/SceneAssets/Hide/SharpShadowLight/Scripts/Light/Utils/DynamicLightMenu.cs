/****************************************************************************
 Copyright (c) 2014 Martin Ysa

 Permission is hereby granted, free of charge, to any person obtaining a copy
 of this software and associated documentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights
 to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:
 
 The above copyright notice and this permission notice shall be included in
 all copies or substantial portions of the Software.
 
 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 THE SOFTWARE.
 ****************************************************************************/

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace SceneAssets.Hide.SharpShadowLight.Scripts.Light.Utils {
  public class DynamicLightMenu : Editor {
    const string _menu_path = "GameObject/SharpShadowLight";

    internal static DynamicLight _Light;
    static string _folder_path;

    [MenuItem(_menu_path + "/Lights/ ☀ Radial No Material ", false, 21)]
    static void AddRadialNoMat() {
      _folder_path = EditorUtils.GetMainRelativepath();

      //Object prefab = AssetDatabase.LoadAssetAtPath(folderPath + "Prefabs/Lights/2DPointLight.prefab", typeof(GameObject));
      var hex = new GameObject(
          "Light2D"); //Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
      _Light = hex.AddComponent<DynamicLight>();
      _Light._Layer = 255;
      hex.transform.position = new Vector3(0, 0, 0);
    }

    [MenuItem(_menu_path + "/Lights/ ☀ Radial Procedural Gradient ", false, 31)]
    static void AddRadialGradient() {
      _folder_path = EditorUtils.GetMainRelativepath();

      var mate = AssetDatabase.LoadAssetAtPath(
                     _folder_path + "Materials/LightMaterialGradient.mat",
                     typeof(Material)) as Material;
      var hex = new GameObject(
          "Light2D"); //Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
      _Light = hex.AddComponent<DynamicLight>();
      _Light._Layer = 255;
      hex.transform.position = new Vector3(0, 0, 0);
      _Light._Light_Material = mate;
    }

    [MenuItem(_menu_path + "/Lights/ ☀ Pseudo Spot Light ", false, 41)]
    static void AddPseudo() {
      _folder_path = EditorUtils.GetMainRelativepath();

      var prefab = AssetDatabase.LoadAssetAtPath(
          _folder_path + "Prefabs/Lights/2DPseudoSpotLight.prefab",
          typeof(GameObject));
      var hex = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
      if (hex != null) {
        hex.transform.position = new Vector3(0, 0, 0);
        hex.name = "2DRadialGradientPoint";
        _Light = hex.GetComponent<DynamicLight>();
      }

      //light.layer = 255;
    }

    #region Casters Zone

    [MenuItem(_menu_path + "/Casters/Square", false, 66)]
    static void AddSquare() {
      _folder_path = EditorUtils.GetMainRelativepath();
      var prefab = AssetDatabase.LoadAssetAtPath(
          _folder_path + "Prefabs/Casters/square.prefab",
          typeof(GameObject));
      var hex = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
      if (hex != null) {
        hex.transform.position = new Vector3(5, 0, 0);
        //hex.layer = LayerMask.NameToLayer("shadows");
        hex.name = "Square";
      }
    }

    [MenuItem(_menu_path + "/Casters/Hexagon", false, 67)]
    static void AddHexagon() {
      _folder_path = EditorUtils.GetMainRelativepath();
      var prefab = AssetDatabase.LoadAssetAtPath(
          _folder_path + "Prefabs/Casters/hexagon.prefab",
          typeof(GameObject));
      var hex = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
      if (hex != null) {
        hex.transform.position = new Vector3(5, 0, 0);
        //hex.layer = LayerMask.NameToLayer("shadows");
        hex.name = "Hexagon";
      }
    }

    [MenuItem(_menu_path + "/Casters/Pacman", false, 68)]
    static void AddPacman() {
      _folder_path = EditorUtils.GetMainRelativepath();
      var prefab = AssetDatabase.LoadAssetAtPath(
          _folder_path + "Prefabs/Casters/pacman.prefab",
          typeof(GameObject));
      var hex = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
      if (hex != null) {
        hex.transform.position = new Vector3(5, 0, 0);
        //hex.layer = LayerMask.NameToLayer("shadows");
        hex.name = "Pacman";
      }
    }

    [MenuItem(_menu_path + "/Casters/Star", false, 69)]
    static void AddStar() {
      _folder_path = EditorUtils.GetMainRelativepath();
      var prefab = AssetDatabase.LoadAssetAtPath(
          _folder_path + "Prefabs/Casters/star.prefab",
          typeof(GameObject));
      var hex = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
      if (hex != null) {
        hex.transform.position = new Vector3(5, 0, 0);
        //hex.layer = LayerMask.NameToLayer("shadows");
        hex.name = "Star";
      }
    }

    #endregion
  }
}
#endif
