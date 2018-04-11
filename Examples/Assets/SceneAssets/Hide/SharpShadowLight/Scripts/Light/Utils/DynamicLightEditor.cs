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
  [CustomEditor(typeof(DynamicLight))]
  //[CanEditMultipleObjects]
  public class DynamicLightEditor : Editor {
    internal static DynamicLight _Light;

    int _ad_count;
    GUIStyle _title_style, _sub_title_style, _bg_style, _btn_style, _ad_text_style;
    SerializedProperty _version, _lmaterial, _radius, _segments, _layer;

    internal void InitStyles() {
      this._title_style = new GUIStyle(GUI.skin.label);
      this._title_style.fontSize = 15;
      this._title_style.fontStyle = FontStyle.Bold;
      this._title_style.alignment = TextAnchor.MiddleLeft;
      this._title_style.margin = new RectOffset(4, 4, 10, 0);

      this._sub_title_style = new GUIStyle(GUI.skin.label);
      this._sub_title_style.fontSize = 13;
      this._sub_title_style.fontStyle = FontStyle.Bold;
      this._sub_title_style.alignment = TextAnchor.MiddleLeft;
      this._sub_title_style.margin = new RectOffset(4, 4, 10, 0);

      this._ad_text_style = new GUIStyle(GUI.skin.box);
      this._ad_text_style.fontSize = 11;
      this._ad_text_style.normal.textColor = Color.magenta;
      this._ad_text_style.fontStyle = FontStyle.Bold;
      this._ad_text_style.alignment = TextAnchor.MiddleLeft;
      this._ad_text_style.margin = new RectOffset(4, 4, 10, 0);
      this._ad_text_style.stretchWidth = true;

      this._bg_style = new GUIStyle(GUI.skin.button);
      this._bg_style.margin = new RectOffset(4, 4, 0, 4);
      this._bg_style.padding = new RectOffset(1, 1, 1, 2);
      this._bg_style.fixedHeight = 30f;
    }

    internal void OnEnable() {
      _Light = this.target as DynamicLight;

      this._version = this.serializedObject.FindProperty("version");
      this._lmaterial = this.serializedObject.FindProperty("lightMaterial");
      this._radius = this.serializedObject.FindProperty("lightRadius");
      this._segments = this.serializedObject.FindProperty("lightSegments");
      this._layer = this.serializedObject.FindProperty("layer");

      this._ad_count = Random.Range(0, 7);
    }

    public override void OnInspectorGUI() {
      if (_Light == null) return;

      this.InitStyles();

      EditorGUI.BeginChangeCheck();
      {
        var fradius = this._radius.floatValue;
        if (fradius < 0)
          fradius *= -1;

        if (this._radius.floatValue != fradius) this._radius.floatValue = fradius;

        var v = this._version.stringValue;

        GUILayout.Label("2DDL Setup", this._title_style);

        EditorGUILayout.Separator();
        GUILayout.Label("Main Prefs", this._sub_title_style);

        EditorGUILayout.Separator();

        EditorGUILayout.PropertyField(this._radius, new GUIContent("Radius", "Size of light radius"));
        EditorGUILayout.IntSlider(
            this._segments,
            3,
            20,
            new GUIContent(
                "Segments",
                "Quantity of line segments is used for build mesh render of 2DLight. 3 at least"));
        EditorGUILayout.PropertyField(
            this._lmaterial,
            new GUIContent("Light Material", "Material Object used for render into light mesh"));
        EditorGUILayout.Separator();

        EditorGUILayout.PropertyField(this._layer, new GUIContent("Layer", "Layer"));
        EditorGUILayout.Separator();

        var ad_text = "";
        this._ad_text_style.normal.textColor = Color.black;
        if (this._ad_count == 0) {
          //adTextStyle.normal.textColor = Color.green;
          ad_text = "Need Spots Lights or angular restriction?";
        } else if (this._ad_count == 1) {
          ad_text = "You need to Edit while you're designing?";
          //adTextStyle.normal.textColor = Color.green;
        } else if (this._ad_count == 2)
          ad_text = "Need Fog of War setup?";
        else if (this._ad_count == 3) {
          ad_text = "Need Field of View detection?";
          //adTextStyle.normal.textColor = Color.cyan;
        } else if (this._ad_count == 4) {
          ad_text = "You need More speed for Mobile target?";
          //adTextStyle.normal.textColor = Color.yellow;
        } else if (this._ad_count == 5)
          ad_text = "Need reveal hidden objects with Lights 2D?";
        else if (this._ad_count == 6)
          ad_text = "Need cookies 2D Lights or texturized 2D Lights?";
        else if (this._ad_count == 7) {
          ad_text = "Need Fast support?";
          //adTextStyle.normal.textColor = Color.cyan;
        }

        #if UNITY_TEAM_LICENSE
        //adTextStyle.normal.textColor = Color.blue;
        #endif

        GUILayout.Label(ad_text, this._ad_text_style);

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        //if (GUILayout.Button("Get Premium", this.bgStyle)) {
        //  AssetStore.Open("/content/25933");
        //Application.OpenURL("https://www.assetstore.unity3d.com/en/#!/content/25933");
        //}

        GUILayout.Label("Info", this._sub_title_style);
        EditorGUILayout.HelpBox("2DDL Free version: " + v, MessageType.None);
        EditorGUILayout.Separator();

        EditorGUILayout.Separator();
        //if (GUILayout.Button("Support")) Application.OpenURL("mailto:info@martinysa.com");
      }

      if (EditorGUI.EndChangeCheck()) {
        EditorUtility.SetDirty(this.target);
        this.ApplyChanges();
      }
    }

    void ApplyChanges() {
      Undo.RecordObject(_Light, "Apply changes");

      foreach (var o in this.targets) {
        var s = (DynamicLight)o;
        s._Light_Material = (Material)this._lmaterial.objectReferenceValue;
        s._Light_Radius = this._radius.floatValue;
        s._Light_Segments = this._segments.intValue;
        s._Layer = this._layer.intValue;
      }
    }
  }
}
#endif
