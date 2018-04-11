#if UNITY_EDITOR
using Neodroid.Environments;
using Neodroid.Managers;
using Neodroid.Prototyping.Actors;
using Neodroid.Prototyping.Configurables;
using Neodroid.Prototyping.Evaluation;
using Neodroid.Prototyping.Motors;
using Neodroid.Prototyping.Observers;
using Neodroid.Prototyping.Displayers;
using UnityEditor;
using UnityEngine;

namespace Neodroid.Editor.Windows {
  public class DebugWindow : EditorWindow {
    Actor[] _actors;

    ConfigurableGameObject[] _configurables;

    PrototypingEnvironment[] _environments;

    Texture _icon;

    Motor[] _motors;

    ObjectiveFunction[] _objective_functions;

    Observer[] _observers;
    
    Displayer[] _displayers;
  
    bool _show_actors_debug;
    bool _show_configurables_debug;
    bool _show_environments_debug;
    bool _show_motors_debug;
    bool _show_objective_functions_debug;
    bool _show_observers_debug;
    bool _show_simulation_manager_debug;
    bool _show_displayers_debug;

    PausableManager _time_simulation_manager;
  
    [MenuItem(  EditorWindowMenuPath._WindowMenuPath+"DebugWindow")]
    [MenuItem(  EditorWindowMenuPath._ToolMenuPath+"DebugWindow")]
    public static void ShowWindow() {
      GetWindow<DebugWindow>(); //Show existing window instance. If one doesn't exist, make one.
    }

    void OnEnable() {
      this.FindObjects();
      this._icon = (Texture2D)AssetDatabase.LoadAssetAtPath(
          "Assets/Neodroid/Gizmos/Icons/information.png",
          typeof(Texture2D));
      this.titleContent = new GUIContent("Neo:Debug", this._icon, "Window for controlling debug messages");
    }

    void FindObjects(){
        this._time_simulation_manager = FindObjectOfType<PausableManager>();
      this._environments = FindObjectsOfType<PrototypingEnvironment>();
      this._actors = FindObjectsOfType<Actor>();
      this._motors = FindObjectsOfType<Motor>();
      this._observers = FindObjectsOfType<Observer>();
      this._configurables = FindObjectsOfType<ConfigurableGameObject>();
      this._objective_functions = FindObjectsOfType<ObjectiveFunction>();
      this._displayers = FindObjectsOfType<Displayer>();
}
  
    void OnGUI() {
      this.FindObjects();

      this._show_simulation_manager_debug = EditorGUILayout.Toggle(
          "Debug simulation manager",
          this._show_simulation_manager_debug);
      this._show_environments_debug = EditorGUILayout.Toggle(
          "Debug all environments",
          this._show_environments_debug);
      this._show_actors_debug = EditorGUILayout.Toggle("Debug all actors", this._show_actors_debug);
      this._show_motors_debug = EditorGUILayout.Toggle("Debug all motors", this._show_motors_debug);
      this._show_observers_debug = EditorGUILayout.Toggle("Debug all observers", this._show_observers_debug);
      this._show_configurables_debug = EditorGUILayout.Toggle(
          "Debug all configurables",
          this._show_configurables_debug);
      this._show_objective_functions_debug = EditorGUILayout.Toggle(
          "Debug all objective functions",
          this._show_objective_functions_debug);
      this._show_displayers_debug = EditorGUILayout.Toggle(
          "Debug all displayers",
          this._show_displayers_debug);


      if (GUILayout.Button("Apply")) {
        if (this._time_simulation_manager != null)
          this._time_simulation_manager.Debugging = this._show_simulation_manager_debug;
        foreach (var environment in this._environments) environment.Debugging = this._show_environments_debug;
        foreach (var actor in this._actors) actor.Debugging = this._show_actors_debug;
        foreach (var motor in this._motors) motor.Debugging = this._show_motors_debug;
        foreach (var observer in this._observers) observer.Debugging = this._show_observers_debug;
        foreach (var configurable in this._configurables)
          configurable.Debugging = this._show_configurables_debug;
        foreach (var objective_functions in this._objective_functions)
          objective_functions.Debugging = this._show_objective_functions_debug;
        foreach (var displayer in this._displayers)
          displayer.Debugging = this._show_displayers_debug;
      }

      /*if (GUI.changed) {
      EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
      // Unity not tracking changes to properties of gameobject made through this window automatically and
      // are not saved unless other changes are made from a working inpector window
      }*/
    }

    public void OnInspectorUpdate() { this.Repaint(); }
  }
}
#endif
