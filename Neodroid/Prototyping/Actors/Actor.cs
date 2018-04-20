using System.Collections.Generic;
using Neodroid.Environments;
using Neodroid.Messaging.Messages;
using Neodroid.Prototyping.Motors;
using Neodroid.Utilities;
using Neodroid.Utilities.BoundingBoxes;
using Neodroid.Utilities.Interfaces;
using UnityEngine;

namespace Neodroid.Prototyping.Actors {
  [AddComponentMenu(PrototypingComponentMenuPath._ComponentMenuPath + "Actors/General")]
  [ExecuteInEditMode]
  //[RequireComponent (typeof(Collider))]
  public class Actor : MonoBehaviour,
                       IHasRegister<Motor>,
                       IRegisterable {
    [SerializeField] bool _alive = true;

    [SerializeField] Bounds _bounds;
    [SerializeField] bool _draw_bounds;

    public bool Alive { get { return this._alive; } }

    public Bounds ActorBounds {
      get {
        var col = this.GetComponent<BoxCollider>();
        this._bounds = new Bounds(this.transform.position, Vector3.zero); // position and size

        if (col) this._bounds.Encapsulate(col.bounds);

        foreach (var child_col in this.GetComponentsInChildren<Collider>()) {
          if (child_col != col)
            this._bounds.Encapsulate(child_col.bounds);
        }

        return this._bounds;
      }
    }

    void Awake() { this.Setup(); }

    void Setup() {
      if (this._motors == null)
        this._motors = new Dictionary<string, Motor>();
      if (this._environment != null)
        this._environment.UnRegisterActor(this.Identifier);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterComponent(this.ParentEnvironment, this);
    }

    void Update() {
      if (this._draw_bounds) {
        var corners = Corners.ExtractCorners(
            this.ActorBounds.center,
            this.ActorBounds.extents,
            this.transform);

        Corners.DrawBox(
            corners[0],
            corners[1],
            corners[2],
            corners[3],
            corners[4],
            corners[5],
            corners[6],
            corners[7],
            Color.gray);
      }
    }

    #if UNITY_EDITOR
    void OnValidate() {
      // Only called in the editor
      //Setup ();
    }
                #endif

    public void Kill() { this._alive = false; }

    public void ApplyMotion(MotorMotion motion) {
      if (this._alive) {
        if (this.Debugging)
          print("Applying " + motion + " To " + this.name + "'s motors");
        var motion_motor_name = motion.MotorName;
        if (this._motors.ContainsKey(motion_motor_name) && this._motors[motion_motor_name] != null)
          this._motors[motion_motor_name].ApplyMotion(motion);
        else {
          if (this.Debugging)
            print("Could find not motor with the specified name: " + motion_motor_name);
        }
      } else {
        if (this.Debugging)
          print("Actor is dead, cannot apply motion");
      }
    }

    public void AddMotor(Motor motor, string identifier) {
      if (this.Debugging)
        print("Actor " + this.name + " has motor " + identifier);
      if (this._motors == null)
        this._motors = new Dictionary<string, Motor>();
      if (!this._motors.ContainsKey(identifier))
        this._motors.Add(identifier, motor);
      else {
        if (this.Debugging)
          print($"A motor with the identifier {identifier} is already registered");
      }
    }

    public virtual void Reset() {
      if (this._motors != null) {
        foreach (var motor in this._motors.Values) {
          if (motor != null)
            motor.Reset();
        }
      }

      this._alive = true;
    }

    #region Fields

    [Header("References", order = 99)]
    [SerializeField]
    PrototypingEnvironment _environment;

    [Header("Development", order = 100)]
    [SerializeField]
    bool _debugging;

    [Header("General", order = 101)]
    [SerializeField]
    Dictionary<string, Motor> _motors;

    #endregion

    #region Getters

    public string Identifier { get { return this.name; } }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="motor"></param>
    public void Register(Motor motor) { this.AddMotor(motor, motor.Identifier); }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="motor"></param>
    /// <param name="identifier"></param>
    public void Register(Motor motor, string identifier) { this.AddMotor(motor, identifier); }

    public Dictionary<string, Motor> Motors { get { return this._motors; } }

    public void RefreshAwake() { this.Awake(); }

    public void RefreshStart() { }

    public PrototypingEnvironment ParentEnvironment {
      get { return this._environment; }
      set { this._environment = value; }
    }

    public bool Debugging { get { return this._debugging; } set { this._debugging = value; } }

    #endregion
  }
}
