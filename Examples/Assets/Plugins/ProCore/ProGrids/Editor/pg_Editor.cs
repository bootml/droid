/**
 *	7/8/2013
 */

#define PRO

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ProGrids {
  [InitializeOnLoad]
  public static class pg_Initializer {
    /**
     * When opening Unity, remember whether or not ProGrids was open when Unity was shut down last.
     */
    static pg_Initializer() {
      if (EditorPrefs.GetBool(pg_Constant.ProGridsIsEnabled)) {
        if (pg_Editor.instance == null)
          pg_Editor.InitProGrids();
        else
          EditorApplication.delayCall += pg_Editor.instance.Initialize;
      }
    }
  }

  public class pg_Editor : ScriptableObject,
                           ISerializationCallbackReceiver {
    #region MEMBERS

    public static pg_Editor instance {
      get {
        if (_instance == null) {
          var editor = Resources.FindObjectsOfTypeAll<pg_Editor>();

          if (editor != null && editor.Length > 0) {
            _instance = editor[0];

            for (var i = 1; i < editor.Length; i++) DestroyImmediate(editor[i]);
          }
        }

        return _instance;
      }
      set { _instance = value; }
    }

    static pg_Editor _instance;

    Color oldColor;

    bool useAxisConstraints {
      get { return EditorPrefs.GetBool(pg_Constant.UseAxisConstraints); }
      set { EditorPrefs.SetBool(pg_Constant.UseAxisConstraints, value); }
    }

    [SerializeField] bool snapEnabled = true;
    [SerializeField] SnapUnit snapUnit = SnapUnit.Meter;
    #if PRO
    float snapValue = 1f; // the actual snap value, taking into account unit size
    float t_snapValue = 1f; // what the user sees
    #else
	private float snapValue = .25f;
	private float t_snapValue = .25f;
    #endif
    bool drawGrid = true;
    bool drawAngles;
    public float angleValue = 45f;
    bool gridRepaint = true;
    public bool predictiveGrid = true;

    bool _snapAsGroup = true;

    public bool snapAsGroup {
      get {
        return EditorPrefs.HasKey(pg_Constant.SnapAsGroup)
                   ? EditorPrefs.GetBool(pg_Constant.SnapAsGroup)
                   : true;
      }
      set {
        this._snapAsGroup = value;
        EditorPrefs.SetBool(pg_Constant.SnapAsGroup, this._snapAsGroup);
      }
    }

    public bool fullGrid { get; private set; }

    bool _scaleSnapEnabled;

    public bool ScaleSnapEnabled {
      get {
        return EditorPrefs.HasKey(pg_Constant.SnapScale) ? EditorPrefs.GetBool(pg_Constant.SnapScale) : false;
      }
      set {
        this._scaleSnapEnabled = value;
        EditorPrefs.SetBool(pg_Constant.SnapScale, this._scaleSnapEnabled);
      }
    }

    KeyCode m_IncreaseGridSizeShortcut = KeyCode.Equals;
    KeyCode m_DecreaseGridSizeShortcut = KeyCode.Minus;
    KeyCode m_NudgePerspectiveBackwardShortcut = KeyCode.LeftBracket;
    KeyCode m_NudgePerspectiveForwardShortcut = KeyCode.RightBracket;
    KeyCode m_NudgePerspectiveResetShortcut = KeyCode.Alpha0;
    KeyCode m_CyclePerspectiveShortcut = KeyCode.Backslash;

    bool lockGrid;
    Axis renderPlane = Axis.Y;

    #if PG_DEBUG
	private GameObject _pivotGo;
	public GameObject pivotGo
	{
		get
		{
			if(_pivotGo == null)
			{
				GameObject find = GameObject.Find("PG_PIVOT_CUBE");

				if(find == null)
				{
					_pivotGo = GameObject.CreatePrimitive(PrimitiveType.Cube);
					_pivotGo.name = "PG_PIVOT_CUBE";
				}
				else
					_pivotGo = find;
			}

			return _pivotGo;
		}

		set
		{
			_pivotGo = value;
		}
	}
    #endif

    #endregion

    #region CONSTANT

    const int VERSION = 22;

    #if PRO
    const int WINDOW_HEIGHT = 240;
    #else
	const int WINDOW_HEIGHT = 260;
    #endif

    const int DEFAULT_SNAP_MULTIPLIER = 2048;

    const int MAX_LINES = 150; // the maximum amount of lines to display on screen in either direction
    public static float alphaBump; // Every tenth line gets an alpha bump by this amount
    const int BUTTON_SIZE = 46;

    Texture2D icon_extendoClose, icon_extendoOpen;

    [SerializeField]
    pg_ToggleContent gc_SnapToGrid = new pg_ToggleContent("Snap", "", "Snaps all selected objects to grid.");

    [SerializeField]
    pg_ToggleContent gc_GridEnabled = new pg_ToggleContent(
        "Hide",
        "Show",
        "Toggles drawing of guide lines on or off.  Note that object snapping is not affected by this setting.");

    [SerializeField]
    pg_ToggleContent gc_SnapEnabled = new pg_ToggleContent("On", "Off", "Toggles snapping on or off.");

    [SerializeField]
    pg_ToggleContent gc_LockGrid = new pg_ToggleContent(
        "Lock",
        "Unlck",
        "Lock the perspective grid center in place.");

    [SerializeField]
    pg_ToggleContent gc_AngleEnabled = new pg_ToggleContent(
        "> On",
        "> Off",
        "If on, ProGrids will draw angled line guides.  Angle is settable in degrees.");

    [SerializeField]
    pg_ToggleContent gc_RenderPlaneX = new pg_ToggleContent("X", "X", "Renders a grid on the X plane.");

    [SerializeField]
    pg_ToggleContent gc_RenderPlaneY = new pg_ToggleContent("Y", "Y", "Renders a grid on the Y plane.");

    [SerializeField]
    pg_ToggleContent gc_RenderPlaneZ = new pg_ToggleContent("Z", "Z", "Renders a grid on the Z plane.");

    [SerializeField]
    pg_ToggleContent gc_RenderPerspectiveGrid = new pg_ToggleContent(
        "Full",
        "Plane",
        "Renders a 3d grid in perspective mode.");

    [SerializeField] GUIContent gc_ExtendMenu = new GUIContent("", "Show or hide the scene view menu.");
    [SerializeField] GUIContent gc_SnapIncrement = new GUIContent("", "Set the snap increment.");

    #endregion

    #region PREFERENCES

    /** Settings **/
    public Color gridColorX, gridColorY, gridColorZ;
    public Color gridColorX_primary, gridColorY_primary, gridColorZ_primary;

    // private bool lockOrthographic;

    public void LoadPreferences() {
      if ((EditorPrefs.HasKey(pg_Constant.PGVersion) ? EditorPrefs.GetInt(pg_Constant.PGVersion) : 0)
          != VERSION) {
        EditorPrefs.SetInt(pg_Constant.PGVersion, VERSION);
        pg_Preferences.ResetPrefs();
      }

      if (EditorPrefs.HasKey(pg_Constant.SnapEnabled))
        this.snapEnabled = EditorPrefs.GetBool(pg_Constant.SnapEnabled);

      this.menuOpen = EditorPrefs.GetBool(pg_Constant.ProGridsIsExtended, true);

      this.SetSnapValue(
          EditorPrefs.HasKey(pg_Constant.GridUnit)
              ? (SnapUnit)EditorPrefs.GetInt(pg_Constant.GridUnit)
              : SnapUnit.Meter,
          EditorPrefs.HasKey(pg_Constant.SnapValue) ? EditorPrefs.GetFloat(pg_Constant.SnapValue) : 1,
          EditorPrefs.HasKey(pg_Constant.SnapMultiplier)
              ? EditorPrefs.GetInt(pg_Constant.SnapMultiplier)
              : DEFAULT_SNAP_MULTIPLIER);

      this.m_IncreaseGridSizeShortcut = EditorPrefs.HasKey("pg_Editor::IncreaseGridSize")
                                            ? (KeyCode)EditorPrefs.GetInt("pg_Editor::IncreaseGridSize")
                                            : KeyCode.Equals;
      this.m_DecreaseGridSizeShortcut = EditorPrefs.HasKey("pg_Editor::DecreaseGridSize")
                                            ? (KeyCode)EditorPrefs.GetInt("pg_Editor::DecreaseGridSize")
                                            : KeyCode.Minus;
      this.m_NudgePerspectiveBackwardShortcut = EditorPrefs.HasKey("pg_Editor::NudgePerspectiveBackward")
                                                    ? (KeyCode)EditorPrefs.GetInt(
                                                        "pg_Editor::NudgePerspectiveBackward")
                                                    : KeyCode.LeftBracket;
      this.m_NudgePerspectiveForwardShortcut = EditorPrefs.HasKey("pg_Editor::NudgePerspectiveForward")
                                                   ? (KeyCode)EditorPrefs.GetInt(
                                                       "pg_Editor::NudgePerspectiveForward")
                                                   : KeyCode.RightBracket;
      this.m_NudgePerspectiveResetShortcut = EditorPrefs.HasKey("pg_Editor::NudgePerspectiveReset")
                                                 ? (KeyCode)EditorPrefs.GetInt(
                                                     "pg_Editor::NudgePerspectiveReset")
                                                 : KeyCode.Alpha0;
      this.m_CyclePerspectiveShortcut = EditorPrefs.HasKey("pg_Editor::CyclePerspective")
                                            ? (KeyCode)EditorPrefs.GetInt("pg_Editor::CyclePerspective")
                                            : KeyCode.Backslash;

      this.lockGrid = EditorPrefs.GetBool(pg_Constant.LockGrid);

      if (this.lockGrid) {
        if (EditorPrefs.HasKey(pg_Constant.LockedGridPivot)) {
          var piv = EditorPrefs.GetString(pg_Constant.LockedGridPivot);
          var pivsplit = piv.Replace("(", "").Replace(")", "").Split(',');

          float x, y, z;
          if (!float.TryParse(pivsplit[0], out x)) goto NoParseForYou;
          if (!float.TryParse(pivsplit[1], out y)) goto NoParseForYou;
          if (!float.TryParse(pivsplit[2], out z)) goto NoParseForYou;

          this.pivot.x = x;
          this.pivot.y = y;
          this.pivot.z = z;

          NoParseForYou: ; // appease the compiler
        }
      }

      this.fullGrid = EditorPrefs.GetBool(pg_Constant.PerspGrid);

      this.renderPlane = EditorPrefs.HasKey(pg_Constant.GridAxis)
                             ? (Axis)EditorPrefs.GetInt(pg_Constant.GridAxis)
                             : Axis.Y;

      alphaBump = EditorPrefs.HasKey("pg_alphaBump")
                      ? EditorPrefs.GetFloat("pg_alphaBump")
                      : pg_Preferences.ALPHA_BUMP;

      this.gridColorX = EditorPrefs.HasKey("gridColorX")
                            ? pg_Util.ColorWithString(EditorPrefs.GetString("gridColorX"))
                            : pg_Preferences.GRID_COLOR_X;
      this.gridColorX_primary = new Color(
          this.gridColorX.r,
          this.gridColorX.g,
          this.gridColorX.b,
          this.gridColorX.a + alphaBump);
      this.gridColorY = EditorPrefs.HasKey("gridColorY")
                            ? pg_Util.ColorWithString(EditorPrefs.GetString("gridColorY"))
                            : pg_Preferences.GRID_COLOR_Y;
      this.gridColorY_primary = new Color(
          this.gridColorY.r,
          this.gridColorY.g,
          this.gridColorY.b,
          this.gridColorY.a + alphaBump);
      this.gridColorZ = EditorPrefs.HasKey("gridColorZ")
                            ? pg_Util.ColorWithString(EditorPrefs.GetString("gridColorZ"))
                            : pg_Preferences.GRID_COLOR_Z;
      this.gridColorZ_primary = new Color(
          this.gridColorZ.r,
          this.gridColorZ.g,
          this.gridColorZ.b,
          this.gridColorZ.a + alphaBump);

      this.drawGrid = EditorPrefs.HasKey("showgrid")
                          ? EditorPrefs.GetBool("showgrid")
                          : pg_Preferences.SHOW_GRID;

      this.predictiveGrid = EditorPrefs.HasKey(pg_Constant.PredictiveGrid)
                                ? EditorPrefs.GetBool(pg_Constant.PredictiveGrid)
                                : true;

      this._snapAsGroup = this.snapAsGroup;
      this._scaleSnapEnabled = this.ScaleSnapEnabled;
    }

    GUISkin sixBySevenSkin;

    #endregion

    #region MENU

    [MenuItem("Tools/ProGrids/About", false, 0)]
    public static void MenuAboutProGrids() {
      pg_AboutWindow.Init("Assets/ProCore/ProGrids/About/pc_AboutEntry_ProGrids.txt", true);
    }

    [MenuItem("Tools/ProGrids/ProGrids Window", false, 15)]
    public static void InitProGrids() {
      if (instance == null) {
        EditorPrefs.SetBool(pg_Constant.ProGridsIsEnabled, true);
        instance = CreateInstance<pg_Editor>();
        instance.hideFlags = HideFlags.DontSave;
        EditorApplication.delayCall += instance.Initialize;
      } else
        CloseProGrids();

      SceneView.RepaintAll();
    }

    [MenuItem("Tools/ProGrids/Close ProGrids", true, 200)]
    public static bool VerifyCloseProGrids() {
      return instance != null || Resources.FindObjectsOfTypeAll<pg_Editor>().Length > 0;
    }

    [MenuItem("Tools/ProGrids/Close ProGrids")]
    public static void CloseProGrids() {
      foreach (var editor in Resources.FindObjectsOfTypeAll<pg_Editor>())
        editor.Close();
    }

    [MenuItem("Tools/ProGrids/Cycle SceneView Projection", false, 101)]
    public static void CyclePerspective() {
      if (instance == null) return;

      var scnvw = SceneView.lastActiveSceneView;
      if (scnvw == null) return;

      var nextOrtho = EditorPrefs.GetInt(pg_Constant.LastOrthoToggledRotation);
      switch (nextOrtho) {
        case 0:
          scnvw.orthographic = true;
          scnvw.LookAt(scnvw.pivot, Quaternion.Euler(Vector3.zero));
          nextOrtho++;
          break;

        case 1:
          scnvw.orthographic = true;
          scnvw.LookAt(scnvw.pivot, Quaternion.Euler(Vector3.up * -90f));
          nextOrtho++;
          break;

        case 2:
          scnvw.orthographic = true;
          scnvw.LookAt(scnvw.pivot, Quaternion.Euler(Vector3.right * 90f));
          nextOrtho++;
          break;

        case 3:
          scnvw.orthographic = false;
          scnvw.LookAt(scnvw.pivot, new Quaternion(-0.1f, 0.9f, -0.2f, -0.4f));
          nextOrtho = 0;
          break;
      }

      EditorPrefs.SetInt(pg_Constant.LastOrthoToggledRotation, nextOrtho);
    }

    [MenuItem("Tools/ProGrids/Cycle SceneView Projection", true, 101)]
    [MenuItem("Tools/ProGrids/Increase Grid Size", true, 203)]
    [MenuItem("Tools/ProGrids/Decrease Grid Size", true, 202)]
    public static bool VerifyGridSizeAdjustment() { return instance != null; }

    [MenuItem("Tools/ProGrids/Decrease Grid Size", false, 202)]
    public static void DecreaseGridSize() {
      if (instance == null) return;

      var multiplier = EditorPrefs.HasKey(pg_Constant.SnapMultiplier)
                           ? EditorPrefs.GetInt(pg_Constant.SnapMultiplier)
                           : DEFAULT_SNAP_MULTIPLIER;
      var val = EditorPrefs.HasKey(pg_Constant.SnapValue) ? EditorPrefs.GetFloat(pg_Constant.SnapValue) : 1f;

      if (multiplier > 1)
        multiplier /= 2;

      instance.SetSnapValue(instance.snapUnit, val, multiplier);

      SceneView.RepaintAll();
    }

    [MenuItem("Tools/ProGrids/Increase Grid Size", false, 203)]
    public static void IncreaseGridSize() {
      if (instance == null) return;

      var multiplier = EditorPrefs.HasKey(pg_Constant.SnapMultiplier)
                           ? EditorPrefs.GetInt(pg_Constant.SnapMultiplier)
                           : DEFAULT_SNAP_MULTIPLIER;
      var val = EditorPrefs.HasKey(pg_Constant.SnapValue) ? EditorPrefs.GetFloat(pg_Constant.SnapValue) : 1f;

      if (multiplier < int.MaxValue / 2)
        multiplier *= 2;

      instance.SetSnapValue(instance.snapUnit, val, multiplier);

      SceneView.RepaintAll();
    }

    [MenuItem("Tools/ProGrids/Nudge Perspective Backward", true, 304)]
    [MenuItem("Tools/ProGrids/Nudge Perspective Forward", true, 305)]
    [MenuItem("Tools/ProGrids/Reset Perspective Nudge", true, 306)]
    public static bool VerifyMenuNudgePerspective() {
      return instance != null && !instance.fullGrid && !instance.ortho && instance.lockGrid;
    }

    [MenuItem("Tools/ProGrids/Nudge Perspective Backward", false, 304)]
    public static void MenuNudgePerspectiveBackward() {
      if (!instance.lockGrid) return;
      instance.offset -= instance.snapValue;
      instance.gridRepaint = true;
      SceneView.RepaintAll();
    }

    [MenuItem("Tools/ProGrids/Nudge Perspective Forward", false, 305)]
    public static void MenuNudgePerspectiveForward() {
      if (!instance.lockGrid) return;
      instance.offset += instance.snapValue;
      instance.gridRepaint = true;
      SceneView.RepaintAll();
    }

    [MenuItem("Tools/ProGrids/Reset Perspective Nudge", false, 306)]
    public static void MenuNudgePerspectiveReset() {
      if (!instance.lockGrid) return;
      instance.offset = 0;
      instance.gridRepaint = true;
      SceneView.RepaintAll();
    }

    public static void ForceRepaint() {
      if (instance != null) {
        instance.gridRepaint = true;
        SceneView.RepaintAll();
      }
    }

    #endregion

    #region INITIALIZATION / SERIALIZATION

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize() {
      instance = this;
      SceneView.onSceneGUIDelegate += this.OnSceneGUI;
      EditorApplication.update += this.Update;
      EditorApplication.hierarchyChanged += this.hierarchyChanged;
    }

    void OnEnable() {
      instance.LoadGUIResources();
      #if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0 && !UNITY_5_1
      Selection.selectionChanged += this.OnSelectionChange;
      #endif
    }

    public void Initialize() {
      SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
      EditorApplication.update -= this.Update;
      EditorApplication.hierarchyChanged -= this.hierarchyChanged;

      SceneView.onSceneGUIDelegate += this.OnSceneGUI;
      EditorApplication.update += this.Update;
      EditorApplication.hierarchyChanged += this.hierarchyChanged;

      this.LoadGUIResources();
      this.LoadPreferences();
      instance = this;
      pg_GridRenderer.Init();

      this.SetMenuIsExtended(this.menuOpen);

      this.lastTime = Time.realtimeSinceStartup;

      // reset colors without changing anything
      this.menuOpen = !this.menuOpen;
      this.ToggleMenuVisibility();

      if (this.drawGrid)
        pg_Util.SetUnityGridEnabled(false);

      this.gridRepaint = true;
      this.RepaintSceneView();
    }

    void OnDestroy() { this.Close(true); }

    public void Close() {
      EditorPrefs.SetBool(pg_Constant.ProGridsIsEnabled, false);
      DestroyImmediate(this);
      #if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0 && !UNITY_5_1
      Selection.selectionChanged -= this.OnSelectionChange;
      #endif
    }

    public void Close(bool isBeingDestroyed) {
      pg_GridRenderer.Destroy();

      SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
      EditorApplication.update -= this.Update;
      EditorApplication.hierarchyChanged -= this.hierarchyChanged;

      instance = null;

      foreach (var listener in toolbarEventSubscribers)
        listener(false);

      pg_Util.SetUnityGridEnabled(true);

      SceneView.RepaintAll();
    }

    void LoadGUIResources() {
      if (this.gc_GridEnabled.image_on == null)
        this.gc_GridEnabled.image_on = pg_IconUtility.LoadIcon("ProGrids2_GUI_Vis_On.png");

      if (this.gc_GridEnabled.image_off == null)
        this.gc_GridEnabled.image_off = pg_IconUtility.LoadIcon("ProGrids2_GUI_Vis_Off.png");

      if (this.gc_SnapEnabled.image_on == null)
        this.gc_SnapEnabled.image_on = pg_IconUtility.LoadIcon("ProGrids2_GUI_Snap_On.png");

      if (this.gc_SnapEnabled.image_off == null)
        this.gc_SnapEnabled.image_off = pg_IconUtility.LoadIcon("ProGrids2_GUI_Snap_Off.png");

      if (this.gc_SnapToGrid.image_on == null)
        this.gc_SnapToGrid.image_on = pg_IconUtility.LoadIcon("ProGrids2_GUI_PushToGrid_Normal.png");

      if (this.gc_LockGrid.image_on == null)
        this.gc_LockGrid.image_on = pg_IconUtility.LoadIcon("ProGrids2_GUI_PGrid_Lock_On.png");

      if (this.gc_LockGrid.image_off == null)
        this.gc_LockGrid.image_off = pg_IconUtility.LoadIcon("ProGrids2_GUI_PGrid_Lock_Off.png");

      if (this.gc_AngleEnabled.image_on == null)
        this.gc_AngleEnabled.image_on = pg_IconUtility.LoadIcon("ProGrids2_GUI_AngleVis_On.png");

      if (this.gc_AngleEnabled.image_off == null)
        this.gc_AngleEnabled.image_off = pg_IconUtility.LoadIcon("ProGrids2_GUI_AngleVis_Off.png");

      if (this.gc_RenderPlaneX.image_on == null)
        this.gc_RenderPlaneX.image_on = pg_IconUtility.LoadIcon("ProGrids2_GUI_PGrid_X_On.png");

      if (this.gc_RenderPlaneX.image_off == null)
        this.gc_RenderPlaneX.image_off = pg_IconUtility.LoadIcon("ProGrids2_GUI_PGrid_X_Off.png");

      if (this.gc_RenderPlaneY.image_on == null)
        this.gc_RenderPlaneY.image_on = pg_IconUtility.LoadIcon("ProGrids2_GUI_PGrid_Y_On.png");

      if (this.gc_RenderPlaneY.image_off == null)
        this.gc_RenderPlaneY.image_off = pg_IconUtility.LoadIcon("ProGrids2_GUI_PGrid_Y_Off.png");

      if (this.gc_RenderPlaneZ.image_on == null)
        this.gc_RenderPlaneZ.image_on = pg_IconUtility.LoadIcon("ProGrids2_GUI_PGrid_Z_On.png");

      if (this.gc_RenderPlaneZ.image_off == null)
        this.gc_RenderPlaneZ.image_off = pg_IconUtility.LoadIcon("ProGrids2_GUI_PGrid_Z_Off.png");

      if (this.gc_RenderPerspectiveGrid.image_on == null)
        this.gc_RenderPerspectiveGrid.image_on = pg_IconUtility.LoadIcon("ProGrids2_GUI_PGrid_3D_On.png");

      if (this.gc_RenderPerspectiveGrid.image_off == null)
        this.gc_RenderPerspectiveGrid.image_off = pg_IconUtility.LoadIcon("ProGrids2_GUI_PGrid_3D_Off.png");

      if (this.icon_extendoOpen == null)
        this.icon_extendoOpen = pg_IconUtility.LoadIcon("ProGrids2_MenuExtendo_Open.png");

      if (this.icon_extendoClose == null)
        this.icon_extendoClose = pg_IconUtility.LoadIcon("ProGrids2_MenuExtendo_Close.png");
    }

    #endregion

    #region INTERFACE

    readonly GUIStyle gridButtonStyle = new GUIStyle();
    readonly GUIStyle extendoStyle = new GUIStyle();
    GUIStyle gridButtonStyleBlank = new GUIStyle();
    readonly GUIStyle backgroundStyle = new GUIStyle();
    bool guiInitialized;

    public float GetSnapIncrement() { return this.t_snapValue; }

    public void SetSnapIncrement(float inc) {
      this.SetSnapValue(this.snapUnit, Mathf.Max(inc, .001f), DEFAULT_SNAP_MULTIPLIER);
    }

    void RepaintSceneView() { SceneView.RepaintAll(); }

    int MENU_HIDDEN { get { return this.menuIsOrtho ? -192 : -173; } }

    const int MENU_EXTENDED = 8;
    const int PAD = 3;
    Rect r = new Rect(8, MENU_EXTENDED, 42, 16);
    Rect backgroundRect = new Rect(00, 0, 0, 0);
    Rect extendoButtonRect = new Rect(0, 0, 0, 0);
    bool menuOpen = true;
    float menuStart = MENU_EXTENDED;
    const float MENU_SPEED = 500f;
    float deltaTime;
    float lastTime;
    const float FADE_SPEED = 2.5f;
    float backgroundFade = 1f;
    bool mouseOverMenu;
    Color menuBackgroundColor = new Color(0f, 0f, 0f, .5f);
    Color extendoNormalColor = new Color(.9f, .9f, .9f, .7f);
    Color extendoHoverColor = new Color(0f, 1f, .4f, 1f);
    bool extendoButtonHovering;
    bool menuIsOrtho;

    void Update() {
      this.deltaTime = Time.realtimeSinceStartup - this.lastTime;
      this.lastTime = Time.realtimeSinceStartup;

      if (this.menuOpen && this.menuStart < MENU_EXTENDED
          || !this.menuOpen && this.menuStart > this.MENU_HIDDEN) {
        this.menuStart += this.deltaTime * MENU_SPEED * (this.menuOpen ? 1f : -1f);
        this.menuStart = Mathf.Clamp(this.menuStart, this.MENU_HIDDEN, MENU_EXTENDED);
        this.RepaintSceneView();
      }

      var a = this.menuBackgroundColor.a;
      this.backgroundFade = this.mouseOverMenu || !this.menuOpen ? FADE_SPEED : -FADE_SPEED;

      this.menuBackgroundColor.a = Mathf.Clamp(
          this.menuBackgroundColor.a + this.backgroundFade * this.deltaTime,
          0f,
          .5f);
      this.extendoNormalColor.a = this.menuBackgroundColor.a;
      this.extendoHoverColor.a = this.menuBackgroundColor.a / .5f;

      if (!Mathf.Approximately(this.menuBackgroundColor.a, a)) this.RepaintSceneView();
    }

    void DrawSceneGUI() {
      GUI.backgroundColor = this.menuBackgroundColor;
      this.backgroundRect.x = this.r.x - 4;
      this.backgroundRect.y = 0;
      this.backgroundRect.width = this.r.width + 8;
      this.backgroundRect.height = this.r.y + this.r.height + PAD;
      GUI.Box(this.backgroundRect, "", this.backgroundStyle);

      // when hit testing mouse for showing the background, add some leeway
      this.backgroundRect.width += 32f;
      this.backgroundRect.height += 32f;
      GUI.backgroundColor = Color.white;

      if (!this.guiInitialized) {
        this.extendoStyle.normal.background = this.menuOpen ? this.icon_extendoClose : this.icon_extendoOpen;
        this.extendoStyle.hover.background = this.menuOpen ? this.icon_extendoClose : this.icon_extendoOpen;

        this.guiInitialized = true;
        this.backgroundStyle.normal.background = EditorGUIUtility.whiteTexture;

        var icon_button_normal = pg_IconUtility.LoadIcon("ProGrids2_Button_Normal.png");
        var icon_button_hover = pg_IconUtility.LoadIcon("ProGrids2_Button_Hover.png");

        if (icon_button_normal == null)
          this.gridButtonStyleBlank = new GUIStyle("button");
        else {
          this.gridButtonStyleBlank.normal.background = icon_button_normal;
          this.gridButtonStyleBlank.hover.background = icon_button_hover;
          this.gridButtonStyleBlank.normal.textColor = icon_button_normal != null ? Color.white : Color.black;
          this.gridButtonStyleBlank.hover.textColor = new Color(.7f, .7f, .7f, 1f);
        }

        this.gridButtonStyleBlank.padding = new RectOffset(1, 2, 1, 2);
        this.gridButtonStyleBlank.alignment = TextAnchor.MiddleCenter;
      }

      this.r.y = this.menuStart;

      this.gc_SnapIncrement.text = this.t_snapValue.ToString("#.####");

      if (GUI.Button(this.r, this.gc_SnapIncrement, this.gridButtonStyleBlank)) {
        #if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        // On Mac ShowAsDropdown and ShowAuxWindow both throw stack pop exceptions when initialized.
		pg_ParameterWindow options = EditorWindow.GetWindow<pg_ParameterWindow>(true, "ProGrids Settings", true);
		Rect screenRect = SceneView.lastActiveSceneView.position;
		options.editor = this;
		options.position = new Rect(screenRect.x + r.x + r.width + PAD,
										screenRect.y + r.y + 24,
										256,
										174);
        #else
        var options = CreateInstance<pg_ParameterWindow>();
        var screenRect = SceneView.lastActiveSceneView.position;
        options.editor = this;
        options.ShowAsDropDown(
            new Rect(screenRect.x + this.r.x + this.r.width + PAD, screenRect.y + this.r.y + 24, 0, 0),
            new Vector2(256, 174));
        #endif
      }

      this.r.y += this.r.height + PAD;

      // Draw grid
      if (pg_ToggleContent.ToggleButton(
          this.r,
          this.gc_GridEnabled,
          this.drawGrid,
          this.gridButtonStyle,
          EditorStyles.miniButton)) this.SetGridEnabled(!this.drawGrid);

      this.r.y += this.r.height + PAD;

      // Snap enabled
      if (pg_ToggleContent.ToggleButton(
          this.r,
          this.gc_SnapEnabled,
          this.snapEnabled,
          this.gridButtonStyle,
          EditorStyles.miniButton)) this.SetSnapEnabled(!this.snapEnabled);

      this.r.y += this.r.height + PAD;

      // Push to grid
      if (pg_ToggleContent.ToggleButton(
          this.r,
          this.gc_SnapToGrid,
          true,
          this.gridButtonStyle,
          EditorStyles.miniButton)) this.SnapToGrid(Selection.transforms);

      this.r.y += this.r.height + PAD;

      // Lock grid
      if (pg_ToggleContent.ToggleButton(
          this.r,
          this.gc_LockGrid,
          this.lockGrid,
          this.gridButtonStyle,
          EditorStyles.miniButton)) {
        this.lockGrid = !this.lockGrid;
        EditorPrefs.SetBool(pg_Constant.LockGrid, this.lockGrid);
        EditorPrefs.SetString(pg_Constant.LockedGridPivot, this.pivot.ToString());

        // if we've modified the nudge value, reset the pivot here
        if (!this.lockGrid) this.offset = 0f;

        this.gridRepaint = true;

        this.RepaintSceneView();
      }

      if (this.menuIsOrtho) {
        this.r.y += this.r.height + PAD;

        if (pg_ToggleContent.ToggleButton(
            this.r,
            this.gc_AngleEnabled,
            this.drawAngles,
            this.gridButtonStyle,
            EditorStyles.miniButton)) this.SetDrawAngles(!this.drawAngles);
      }

      /**
       * Perspective Toggles
       */
      this.r.y += this.r.height + PAD + 4;

      if (pg_ToggleContent.ToggleButton(
          this.r,
          this.gc_RenderPlaneX,
          (this.renderPlane & Axis.X) == Axis.X && !this.fullGrid,
          this.gridButtonStyle,
          EditorStyles.miniButton)) this.SetRenderPlane(Axis.X);

      this.r.y += this.r.height + PAD;

      if (pg_ToggleContent.ToggleButton(
          this.r,
          this.gc_RenderPlaneY,
          (this.renderPlane & Axis.Y) == Axis.Y && !this.fullGrid,
          this.gridButtonStyle,
          EditorStyles.miniButton)) this.SetRenderPlane(Axis.Y);

      this.r.y += this.r.height + PAD;

      if (pg_ToggleContent.ToggleButton(
          this.r,
          this.gc_RenderPlaneZ,
          (this.renderPlane & Axis.Z) == Axis.Z && !this.fullGrid,
          this.gridButtonStyle,
          EditorStyles.miniButton)) this.SetRenderPlane(Axis.Z);

      this.r.y += this.r.height + PAD;

      if (pg_ToggleContent.ToggleButton(
          this.r,
          this.gc_RenderPerspectiveGrid,
          this.fullGrid,
          this.gridButtonStyle,
          EditorStyles.miniButton)) {
        this.fullGrid = !this.fullGrid;
        this.gridRepaint = true;
        EditorPrefs.SetBool(pg_Constant.PerspGrid, this.fullGrid);
        this.RepaintSceneView();
      }

      this.r.y += this.r.height + PAD;

      this.extendoButtonRect.x = this.r.x;
      this.extendoButtonRect.y = this.r.y;
      this.extendoButtonRect.width = this.r.width;
      this.extendoButtonRect.height = this.r.height;

      GUI.backgroundColor = this.extendoButtonHovering ? this.extendoHoverColor : this.extendoNormalColor;
      this.gc_ExtendMenu.text = this.icon_extendoOpen == null ? (this.menuOpen ? "Close" : "Open") : "";
      if (GUI.Button(
          this.r,
          this.gc_ExtendMenu,
          this.icon_extendoOpen ? this.extendoStyle : this.gridButtonStyleBlank)) {
        this.ToggleMenuVisibility();
        this.extendoButtonHovering = false;
      }

      GUI.backgroundColor = Color.white;
    }

    void ToggleMenuVisibility() {
      this.menuOpen = !this.menuOpen;
      EditorPrefs.SetBool(pg_Constant.ProGridsIsExtended, this.menuOpen);

      this.extendoStyle.normal.background = this.menuOpen ? this.icon_extendoClose : this.icon_extendoOpen;
      this.extendoStyle.hover.background = this.menuOpen ? this.icon_extendoClose : this.icon_extendoOpen;

      foreach (var listener in toolbarEventSubscribers)
        listener(this.menuOpen);

      this.RepaintSceneView();
    }

    // skip color fading and stuff
    void SetMenuIsExtended(bool isExtended) {
      this.menuOpen = isExtended;
      this.menuIsOrtho = this.ortho;
      this.menuStart = this.menuOpen ? MENU_EXTENDED : this.MENU_HIDDEN;

      this.menuBackgroundColor.a = 0f;
      this.extendoNormalColor.a = this.menuBackgroundColor.a;
      this.extendoHoverColor.a = this.menuBackgroundColor.a / .5f;

      this.extendoStyle.normal.background = this.menuOpen ? this.icon_extendoClose : this.icon_extendoOpen;
      this.extendoStyle.hover.background = this.menuOpen ? this.icon_extendoClose : this.icon_extendoOpen;

      foreach (var listener in toolbarEventSubscribers)
        listener(this.menuOpen);

      EditorPrefs.SetBool(pg_Constant.ProGridsIsExtended, this.menuOpen);
    }

    void OpenProGridsPopup() {
      if (EditorUtility.DisplayDialog(
              "Upgrade to ProGrids", // Title
              "Enables all kinds of super-cool features, like different snap values, more units of measurement, and angles.", // Message
              "Upgrade", // Okay
              "Cancel" // Cancel
          ))
          // #if UNITY_4
          // AssetStore.OpenURL(pg_Constant.ProGridsUpgradeURL);
          // #else
        Application.OpenURL(pg_Constant.ProGridsUpgradeURL);
      // #endif
    }

    #endregion

    #region ONSCENEGUI

    Transform lastTransform;
    const string AXIS_CONSTRAINT_KEY = "s";
    const string TEMP_DISABLE_KEY = "d";
    bool toggleAxisConstraint;
    bool toggleTempSnap;

    Vector3 lastPosition = Vector3.zero;

    // private Vector3 lastRotation = Vector3.zero;
    Vector3 lastScale = Vector3.one;
    Vector3 pivot = Vector3.zero, lastPivot = Vector3.zero;

    Vector3 camDir = Vector3.zero, prevCamDir = Vector3.zero;

    // Distance from camera to pivot at the last time the grid mesh was updated.
    float lastDistance;
    public float offset;

    bool firstMove = true;

    #if PROFILE_TIMES
	pb_Profiler profiler = new pb_Profiler();
    #endif

    public bool ortho { get; private set; }
    bool prevOrtho;

    float planeGridDrawDistance;

    public void OnSceneGUI(SceneView scnview) {
      var isCurrentView = scnview == SceneView.lastActiveSceneView;

      if (isCurrentView) {
        Handles.BeginGUI();
        this.DrawSceneGUI();
        Handles.EndGUI();
      }

      // don't snap stuff in play mode
      if (EditorApplication.isPlayingOrWillChangePlaymode)
        return;

      var e = Event.current;

      // repaint scene gui if mouse is near controls
      if (isCurrentView && e.type == EventType.MouseMove) {
        var tmp = this.extendoButtonHovering;
        this.extendoButtonHovering = this.extendoButtonRect.Contains(e.mousePosition);

        if (this.extendoButtonHovering != tmp) this.RepaintSceneView();

        this.mouseOverMenu = this.backgroundRect.Contains(e.mousePosition);
      }

      if (e.Equals(Event.KeyboardEvent(AXIS_CONSTRAINT_KEY))) this.toggleAxisConstraint = true;

      if (e.Equals(Event.KeyboardEvent(TEMP_DISABLE_KEY))) this.toggleTempSnap = true;

      if (e.isKey) {
        this.toggleAxisConstraint = false;
        this.toggleTempSnap = false;
        var used = true;

        if (e.keyCode == this.m_IncreaseGridSizeShortcut) {
          if (e.type == EventType.KeyUp)
            IncreaseGridSize();
        } else if (e.keyCode == this.m_DecreaseGridSizeShortcut) {
          if (e.type == EventType.KeyUp)
            DecreaseGridSize();
        } else if (e.keyCode == this.m_NudgePerspectiveBackwardShortcut) {
          if (e.type == EventType.KeyUp && VerifyMenuNudgePerspective())
            MenuNudgePerspectiveBackward();
        } else if (e.keyCode == this.m_NudgePerspectiveForwardShortcut) {
          if (e.type == EventType.KeyUp && VerifyMenuNudgePerspective())
            MenuNudgePerspectiveForward();
        } else if (e.keyCode == this.m_NudgePerspectiveResetShortcut) {
          if (e.type == EventType.KeyUp && VerifyMenuNudgePerspective())
            MenuNudgePerspectiveReset();
        } else if (e.keyCode == this.m_CyclePerspectiveShortcut) {
          if (e.type == EventType.KeyUp)
            CyclePerspective();
        } else
          used = false;

        if (used)
          e.Use();
      }

      var cam = Camera.current;

      if (cam == null)
        return;

      this.ortho = cam.orthographic && this.IsRounded(scnview.rotation.eulerAngles.normalized);

      this.camDir = pg_Util.CeilFloor(this.pivot - cam.transform.position);

      if (this.ortho && !this.prevOrtho || this.ortho != this.menuIsOrtho)
        this.OnSceneBecameOrtho(isCurrentView);

      if (!this.ortho && this.prevOrtho) this.OnSceneBecamePersp(isCurrentView);

      this.prevOrtho = this.ortho;

      var camDistance = Vector3.Distance(
          cam.transform.position,
          this.lastPivot); // distance from camera to pivot

      if (this.fullGrid)
        this.pivot = this.lockGrid || Selection.activeTransform == null
                         ? this.pivot
                         : Selection.activeTransform.position;
      else {
        var sceneViewPlanePivot = this.pivot;

        var ray = new Ray(cam.transform.position, cam.transform.forward);
        var plane = new Plane(Vector3.up, this.pivot);
        float dist;

        // the only time a locked grid should ever move is if it's pivot is out
        // of the camera's frustum.
        if (this.lockGrid && !cam.InFrustum(this.pivot)
            || !this.lockGrid
            || scnview != SceneView.lastActiveSceneView) {
          if (plane.Raycast(ray, out dist))
            sceneViewPlanePivot = ray.GetPoint(Mathf.Min(dist, this.planeGridDrawDistance / 2f));
          else
            sceneViewPlanePivot =
                ray.GetPoint(Mathf.Min(cam.farClipPlane / 2f, this.planeGridDrawDistance / 2f));
        }

        if (this.lockGrid)
          this.pivot = pg_Enum.InverseAxisMask(sceneViewPlanePivot, this.renderPlane)
                       + pg_Enum.AxisMask(this.pivot, this.renderPlane);
        else {
          this.pivot = Selection.activeTransform == null ? this.pivot : Selection.activeTransform.position;

          if (Selection.activeTransform == null || !cam.InFrustum(this.pivot))
            this.pivot = pg_Enum.InverseAxisMask(sceneViewPlanePivot, this.renderPlane)
                         + pg_Enum.AxisMask(
                             Selection.activeTransform == null
                                 ? this.pivot
                                 : Selection.activeTransform.position,
                             this.renderPlane);
        }
      }

      #if PG_DEBUG
		pivotGo.transform.position = pivot;
      #endif

      if (this.drawGrid) {
        if (this.ortho) {
          // ortho don't care about pivots
          this.DrawGridOrthographic(cam);
        } else {
          #if PROFILE_TIMES
				profiler.LogStart("DrawGridPerspective");
          #endif

          if (this.gridRepaint
              || this.pivot != this.lastPivot
              || Mathf.Abs(camDistance - this.lastDistance) > this.lastDistance / 2
              || this.camDir != this.prevCamDir) {
            this.prevCamDir = this.camDir;
            this.gridRepaint = false;
            this.lastPivot = this.pivot;
            this.lastDistance = camDistance;

            if (this.fullGrid) {
              //  if perspective and 3d, use pivot like normal
              pg_GridRenderer.DrawGridPerspective(
                  cam,
                  this.pivot,
                  this.snapValue,
                  new Color[3] {this.gridColorX, this.gridColorY, this.gridColorZ},
                  alphaBump);
            } else {
              if ((this.renderPlane & Axis.X) == Axis.X)
                this.planeGridDrawDistance = pg_GridRenderer.DrawPlane(
                    cam,
                    this.pivot + Vector3.right * this.offset,
                    Vector3.up,
                    Vector3.forward,
                    this.snapValue,
                    this.gridColorX,
                    alphaBump);

              if ((this.renderPlane & Axis.Y) == Axis.Y)
                this.planeGridDrawDistance = pg_GridRenderer.DrawPlane(
                    cam,
                    this.pivot + Vector3.up * this.offset,
                    Vector3.right,
                    Vector3.forward,
                    this.snapValue,
                    this.gridColorY,
                    alphaBump);

              if ((this.renderPlane & Axis.Z) == Axis.Z)
                this.planeGridDrawDistance = pg_GridRenderer.DrawPlane(
                    cam,
                    this.pivot + Vector3.forward * this.offset,
                    Vector3.up,
                    Vector3.right,
                    this.snapValue,
                    this.gridColorZ,
                    alphaBump);
            }
          }
          #if PROFILE_TIMES
				profiler.LogFinish("DrawGridPerspective");
          #endif
        }
      }

      // Always keep track of the selection
      if (!Selection.transforms.Contains(this.lastTransform)) {
        if (Selection.activeTransform) {
          this.lastTransform = Selection.activeTransform;
          this.lastPosition = Selection.activeTransform.position;
          this.lastScale = Selection.activeTransform.localScale;
        }
      }

      if (e.type == EventType.MouseUp) this.firstMove = true;

      if (!this.snapEnabled || GUIUtility.hotControl < 1)
        return;

      // Bugger.SetKey("Toggle Snap Off", toggleTempSnap);

      /**
       *	Snapping (for all the junk in PG, this method is literally the only code that actually affects anything).
       */
      if (Selection.activeTransform && pg_Util.SnapIsEnabled(Selection.activeTransform)) {
        if (!FuzzyEquals(this.lastTransform.position, this.lastPosition)) {
          var selected = this.lastTransform;

          if (!this.toggleTempSnap) {
            var old = selected.position;
            var mask = old - this.lastPosition;

            var constraintsOn =
                this.toggleAxisConstraint ? !this.useAxisConstraints : this.useAxisConstraints;

            if (constraintsOn)
              selected.position = pg_Util.SnapValue(old, mask, this.snapValue);
            else
              selected.position = pg_Util.SnapValue(old, this.snapValue);

            var offset = selected.position - old;

            if (this.predictiveGrid && this.firstMove && !this.fullGrid) {
              this.firstMove = false;
              var dragAxis = pg_Util.CalcDragAxis(offset, scnview.camera);

              if (dragAxis != Axis.None && dragAxis != this.renderPlane) this.SetRenderPlane(dragAxis);
            }

            if (this._snapAsGroup)
              this.OffsetTransforms(Selection.transforms, selected, offset);
            else {
              foreach (var t in Selection.transforms)
                t.position = constraintsOn
                                 ? pg_Util.SnapValue(t.position, mask, this.snapValue)
                                 : pg_Util.SnapValue(t.position, this.snapValue);
            }
          }

          this.lastPosition = selected.position;
        }

        if (!FuzzyEquals(this.lastTransform.localScale, this.lastScale) && this._scaleSnapEnabled) {
          if (!this.toggleTempSnap) {
            var old = this.lastTransform.localScale;
            var mask = old - this.lastScale;

            if (this.predictiveGrid) {
              var dragAxis = pg_Util.CalcDragAxis(
                  Selection.activeTransform.TransformDirection(mask),
                  scnview.camera);
              if (dragAxis != Axis.None && dragAxis != this.renderPlane) this.SetRenderPlane(dragAxis);
            }

            foreach (var t in Selection.transforms)
              t.localScale = pg_Util.SnapValue(t.localScale, mask, this.snapValue);

            this.lastScale = this.lastTransform.localScale;
          }
        }
      }
    }

    void OnSelectionChange() {
      // Means we don't have to wait for script reloads
      // to respect IgnoreSnap attribute, and keeps the
      // cache small.
      pg_Util.ClearSnapEnabledCache();
    }

    void OnSceneBecameOrtho(bool isCurrentView) {
      pg_GridRenderer.Destroy();

      if (isCurrentView && this.ortho != this.menuIsOrtho) this.SetMenuIsExtended(this.menuOpen);
    }

    void OnSceneBecamePersp(bool isCurrentView) {
      if (isCurrentView && this.ortho != this.menuIsOrtho) this.SetMenuIsExtended(this.menuOpen);
    }

    #endregion

    #region GRAPHICS

    GameObject go;

    void DrawGridOrthographic(Camera cam) {
      var camAxis =
          this.AxisWithVector(Camera.current.transform.TransformDirection(Vector3.forward).normalized);

      if (this.drawGrid) {
        switch (camAxis) {
          case Axis.X:
          case Axis.NegX:
            this.DrawGridOrthographic(cam, camAxis, this.gridColorX_primary, this.gridColorX);
            break;

          case Axis.Y:
          case Axis.NegY:
            this.DrawGridOrthographic(cam, camAxis, this.gridColorY_primary, this.gridColorY);
            break;

          case Axis.Z:
          case Axis.NegZ:
            this.DrawGridOrthographic(cam, camAxis, this.gridColorZ_primary, this.gridColorZ);
            break;
        }
      }
    }

    int PRIMARY_COLOR_INCREMENT = 10;
    Color previousColor;

    void DrawGridOrthographic(Camera cam, Axis camAxis, Color primaryColor, Color secondaryColor) {
      this.previousColor = Handles.color;
      Handles.color = primaryColor;

      var bottomLeft = pg_Util.SnapToFloor(cam.ScreenToWorldPoint(Vector2.zero), this.snapValue);
      var bottomRight = pg_Util.SnapToFloor(
          cam.ScreenToWorldPoint(new Vector2(cam.pixelWidth, 0f)),
          this.snapValue);
      var topLeft = pg_Util.SnapToFloor(
          cam.ScreenToWorldPoint(new Vector2(0f, cam.pixelHeight)),
          this.snapValue);
      var topRight = pg_Util.SnapToFloor(
          cam.ScreenToWorldPoint(new Vector2(cam.pixelWidth, cam.pixelHeight)),
          this.snapValue);

      var axis = this.VectorWithAxis(camAxis);

      var width = Vector3.Distance(bottomLeft, bottomRight);
      var height = Vector3.Distance(bottomRight, topRight);

      // Shift lines to 10m forward of the camera
      bottomLeft += axis * 10f;
      topRight += axis * 10f;
      bottomRight += axis * 10f;
      topLeft += axis * 10f;

      /**
       *	Draw Vertical Lines
       */
      var cam_right = cam.transform.right;
      var cam_up = cam.transform.up;

      var _snapVal = this.snapValue;

      var segs = (int)Mathf.Ceil(width / _snapVal) + 2;

      var n = 2f;
      while (segs > MAX_LINES) {
        _snapVal = _snapVal * n;
        segs = (int)Mathf.Ceil(width / _snapVal) + 2;
        n++;
      }

      /// Screen start and end
      var bl = cam_right.Sum() > 0
                   ? pg_Util.SnapToFloor(bottomLeft, cam_right, _snapVal * this.PRIMARY_COLOR_INCREMENT)
                   : pg_Util.SnapToCeil(bottomLeft, cam_right, _snapVal * this.PRIMARY_COLOR_INCREMENT);
      var start = bl - cam_up * (height + _snapVal * 2);
      var end = bl + cam_up * (height + _snapVal * 2);

      segs += this.PRIMARY_COLOR_INCREMENT;

      /// The current line start and end
      var line_start = Vector3.zero;
      var line_end = Vector3.zero;

      for (var i = -1; i < segs; i++) {
        line_start = start + i * (cam_right * _snapVal);
        line_end = end + i * (cam_right * _snapVal);
        Handles.color = i % this.PRIMARY_COLOR_INCREMENT == 0 ? primaryColor : secondaryColor;
        Handles.DrawLine(line_start, line_end);
      }

      /**
       * Draw Horizontal Lines
       */
      segs = (int)Mathf.Ceil(height / _snapVal) + 2;

      n = 2;
      while (segs > MAX_LINES) {
        _snapVal = _snapVal * n;
        segs = (int)Mathf.Ceil(height / _snapVal) + 2;
        n++;
      }

      var tl = cam_up.Sum() > 0
                   ? pg_Util.SnapToCeil(topLeft, cam_up, _snapVal * this.PRIMARY_COLOR_INCREMENT)
                   : pg_Util.SnapToFloor(topLeft, cam_up, _snapVal * this.PRIMARY_COLOR_INCREMENT);
      start = tl - cam_right * (width + _snapVal * 2);
      end = tl + cam_right * (width + _snapVal * 2);

      segs += this.PRIMARY_COLOR_INCREMENT;

      for (var i = -1; i < segs; i++) {
        line_start = start + i * (-cam_up * _snapVal);
        line_end = end + i * (-cam_up * _snapVal);
        Handles.color = i % this.PRIMARY_COLOR_INCREMENT == 0 ? primaryColor : secondaryColor;
        Handles.DrawLine(line_start, line_end);
      }

      #if PRO
      if (this.drawAngles) {
        var cen = pg_Util.SnapValue((topRight + bottomLeft) / 2f, this.snapValue);

        var half = width > height ? width : height;

        var opposite = Mathf.Tan(Mathf.Deg2Rad * this.angleValue) * half;

        var up = cam.transform.up * opposite;
        var right = cam.transform.right * half;

        var bottomLeftAngle = cen - (up + right);
        var topRightAngle = cen + (up + right);

        var bottomRightAngle = cen + (right - up);
        var topLeftAngle = cen + (up - right);

        Handles.color = primaryColor;

        // y = 1x+1
        Handles.DrawLine(bottomLeftAngle, topRightAngle);

        // y = -1x-1
        Handles.DrawLine(topLeftAngle, bottomRightAngle);
      }
      #endif

      Handles.color = this.previousColor;
    }

    #endregion

    #region ENUM UTILITY

    public SnapUnit SnapUnitWithString(string str) {
      foreach (SnapUnit su in Enum.GetValues(typeof(SnapUnit))) {
        if (su.ToString() == str)
          return su;
      }

      return 0;
    }

    public Axis AxisWithVector(Vector3 val) {
      var v = new Vector3(Mathf.Abs(val.x), Mathf.Abs(val.y), Mathf.Abs(val.z));

      if (v.x > v.y && v.x > v.z) {
        if (val.x > 0)
          return Axis.X;
        return Axis.NegX;
      }

      if (v.y > v.x && v.y > v.z) {
        if (val.y > 0)
          return Axis.Y;
        return Axis.NegY;
      }

      if (val.z > 0)
        return Axis.Z;
      return Axis.NegZ;
    }

    public Vector3 VectorWithAxis(Axis axis) {
      switch (axis) {
        case Axis.X:
          return Vector3.right;
        case Axis.Y:
          return Vector3.up;
        case Axis.Z:
          return Vector3.forward;
        case Axis.NegX:
          return -Vector3.right;
        case Axis.NegY:
          return -Vector3.up;
        case Axis.NegZ:
          return -Vector3.forward;

        default:
          return Vector3.forward;
      }
    }

    public bool IsRounded(Vector3 v) {
      return Mathf.Approximately(v.x, 1f)
             || Mathf.Approximately(v.y, 1f)
             || Mathf.Approximately(v.z, 1f)
             || v == Vector3.zero;
    }

    public Vector3 RoundAxis(Vector3 v) { return this.VectorWithAxis(this.AxisWithVector(v)); }

    #endregion

    #region MOVING TRANSFORMS

    static bool FuzzyEquals(Vector3 lhs, Vector3 rhs) {
      return Mathf.Abs(lhs.x - rhs.x) < .001f
             && Mathf.Abs(lhs.y - rhs.y) < .001f
             && Mathf.Abs(lhs.z - rhs.z) < .001f;
    }

    public void OffsetTransforms(Transform[] trsfrms, Transform ignore, Vector3 offset) {
      foreach (var t in trsfrms) {
        if (t != ignore)
          t.position += offset;
      }
    }

    void hierarchyChanged() {
      if (Selection.activeTransform != null) this.lastPosition = Selection.activeTransform.position;
    }

    #endregion

    #region SETTINGS

    public void SetSnapEnabled(bool enable) {
      EditorPrefs.SetBool(pg_Constant.SnapEnabled, enable);

      if (Selection.activeTransform) {
        this.lastTransform = Selection.activeTransform;
        this.lastPosition = Selection.activeTransform.position;
      }

      this.snapEnabled = enable;
      this.gridRepaint = true;
      this.RepaintSceneView();
    }

    public void SetSnapValue(SnapUnit su, float val, int multiplier) {
      var clamp_multiplier = Mathf.Min(Mathf.Max(1, multiplier), int.MaxValue);

      var value_multiplier = clamp_multiplier / (float)DEFAULT_SNAP_MULTIPLIER;

      /**
       * multiplier is a value modifies the snap val.  100 = no change,
       * 50 is half val, 200 is double val, etc.
       */
      this.snapValue = pg_Enum.SnapUnitValue(su) * val * value_multiplier;
      this.RepaintSceneView();

      EditorPrefs.SetInt(pg_Constant.GridUnit, (int)su);
      EditorPrefs.SetFloat(pg_Constant.SnapValue, val);
      EditorPrefs.SetInt(pg_Constant.SnapMultiplier, clamp_multiplier);

      // update gui (only necessary when calling with editorpref values)
      this.t_snapValue = val * value_multiplier;
      this.snapUnit = su;

      switch (su) {
        case SnapUnit.Inch:
          this.PRIMARY_COLOR_INCREMENT = 12; // blasted imperial units
          break;

        case SnapUnit.Foot:
          this.PRIMARY_COLOR_INCREMENT = 3;
          break;

        default:
          this.PRIMARY_COLOR_INCREMENT = 10;
          break;
      }

      if (EditorPrefs.GetBool(pg_Constant.SyncUnitySnap, true)) {
        EditorPrefs.SetFloat("MoveSnapX", this.snapValue);
        EditorPrefs.SetFloat("MoveSnapY", this.snapValue);
        EditorPrefs.SetFloat("MoveSnapZ", this.snapValue);

        if (EditorPrefs.GetBool(pg_Constant.SnapScale, true))
          EditorPrefs.SetFloat("ScaleSnap", this.snapValue);

        // If Unity snap sync is enabled, refresh the Snap Settings window if it's open. 
        var snapSettings = typeof(EditorWindow).Assembly.GetType("UnityEditor.SnapSettings");

        if (snapSettings != null) {
          var snapInitialized = snapSettings.GetField(
              "s_Initialized",
              BindingFlags.NonPublic | BindingFlags.Static);

          if (snapInitialized != null) {
            snapInitialized.SetValue(null, false);

            var win = Resources.FindObjectsOfTypeAll<EditorWindow>()
                .FirstOrDefault(x => x.ToString().Contains("SnapSettings"));

            if (win != null)
              win.Repaint();
          }
        }
      }

      this.gridRepaint = true;
    }

    public void SetRenderPlane(Axis axis) {
      this.offset = 0f;
      this.fullGrid = false;
      this.renderPlane = axis;
      EditorPrefs.SetBool(pg_Constant.PerspGrid, this.fullGrid);
      EditorPrefs.SetInt(pg_Constant.GridAxis, (int)this.renderPlane);
      this.gridRepaint = true;
      this.RepaintSceneView();
    }

    public void SetGridEnabled(bool enable) {
      this.drawGrid = enable;

      if (!this.drawGrid)
        pg_GridRenderer.Destroy();
      else
        pg_Util.SetUnityGridEnabled(false);

      EditorPrefs.SetBool("showgrid", enable);

      this.gridRepaint = true;
      this.RepaintSceneView();
    }

    public void SetDrawAngles(bool enable) {
      this.drawAngles = enable;
      this.gridRepaint = true;
      this.RepaintSceneView();
    }

    void SnapToGrid(Transform[] transforms) {
      Undo.RecordObjects(transforms, "Snap to Grid");

      foreach (var t in transforms)
        t.position = pg_Util.SnapValue(t.position, this.snapValue);

      this.gridRepaint = true;

      this.PushToGrid(this.snapValue);
    }

    #endregion

    #region GLOBAL SETTING

    internal bool GetUseAxisConstraints() {
      return this.toggleAxisConstraint ? !this.useAxisConstraints : this.useAxisConstraints;
    }

    internal float GetSnapValue() { return this.snapValue; }
    internal bool GetSnapEnabled() { return this.toggleTempSnap ? !this.snapEnabled : this.snapEnabled; }

    /**
     * Returns the value of useAxisConstraints, accounting for the shortcut key toggle.
     */
    public static bool UseAxisConstraints() {
      return instance != null ? instance.GetUseAxisConstraints() : false;
    }

    /**
     * Return the current snap value.
     */
    public static float SnapValue() { return instance != null ? instance.GetSnapValue() : 0f; }

    /**
     * Return true if snapping is enabled, false otherwise.
     */
    public static bool SnapEnabled() { return instance == null ? false : instance.GetSnapEnabled(); }

    public static void AddPushToGridListener(Action<float> listener) { pushToGridListeners.Add(listener); }

    public static void RemovePushToGridListener(Action<float> listener) {
      pushToGridListeners.Remove(listener);
    }

    public static void AddToolbarEventSubscriber(Action<bool> listener) {
      toolbarEventSubscribers.Add(listener);
    }

    public static void RemoveToolbarEventSubscriber(Action<bool> listener) {
      toolbarEventSubscribers.Remove(listener);
    }

    public static bool SceneToolbarActive() { return instance != null; }

    [SerializeField] static readonly List<Action<float>> pushToGridListeners = new List<Action<float>>();
    [SerializeField] static readonly List<Action<bool>> toolbarEventSubscribers = new List<Action<bool>>();

    void PushToGrid(float snapValue) {
      foreach (var listener in pushToGridListeners)
        listener(snapValue);
    }

    public static void OnHandleMove(Vector3 worldDirection) {
      if (instance != null)
        instance.OnHandleMove_Internal(worldDirection);
    }

    void OnHandleMove_Internal(Vector3 worldDirection) {
      if (this.predictiveGrid && this.firstMove && !this.fullGrid) {
        this.firstMove = false;
        var dragAxis = pg_Util.CalcDragAxis(worldDirection, SceneView.lastActiveSceneView.camera);

        if (dragAxis != Axis.None && dragAxis != this.renderPlane) this.SetRenderPlane(dragAxis);
      }
    }

    #endregion
  }
}
