#if UNITY_EDITOR

using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;

namespace SceneAssets.Hide.SharpShadowLight.Scripts.Light.Utils {
  public class EditorUtils : Editor {
    public static EditorUtils _EditorUtils;

    internal static string _Relativepath;

    //EditorUtils();

    public static string GetMainRelativepath() {
      if (_EditorUtils == null) _EditorUtils = (EditorUtils)CreateInstance("EditorUtils");

      if (_Relativepath != null)
        return _Relativepath;
      var ms = MonoScript.FromScriptableObject(_EditorUtils);
      var m_script_file_path = AssetDatabase.GetAssetPath(ms);

      //If 2DDL FREE
      var name = "Scripts/2DLight/Editor/" + Path.GetFileName(m_script_file_path);

      //If 2DDL PRO
      //string _name = "2DLight/Editor/" + Path.GetFileName(m_ScriptFilePath);

      var rex = new Regex(name);
      var result = rex.Replace(m_script_file_path, "", 1);

      _Relativepath = result;
      return _Relativepath;
    }
  }
}
#endif
