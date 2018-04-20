using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Neodroid.Managers;
using Neodroid.Messaging.Messages;
using Neodroid.Prototyping.Actors;
using Neodroid.Prototyping.Configurables;
using Neodroid.Prototyping.Displayers;
using Neodroid.Prototyping.Evaluation;
using Neodroid.Prototyping.Observers;
using Neodroid.Prototyping.Resetables;
using Neodroid.Utilities;
using Neodroid.Utilities.BoundingBoxes;
using Neodroid.Utilities.Enums;
using Neodroid.Utilities.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Neodroid.Environments {
  /// <summary>
  /// Environment to be used with the prototyping components.
  /// </summary>
  [AddComponentMenu("Neodroid/Environments/Prototyping")]
  public class PrototypingEnvironment : NeodroidEnvironment,
                                        IHasRegister<Actor>,
                                        IHasRegister<Observer>,
                                        IHasRegister<ConfigurableGameObject>,
                                        IHasRegister<Resetable>,
                                        IHasRegister<Displayer> {
    #region UnityCallbacks

    /// <summary>
    /// 
    /// </summary>
    void Start() {
      this.InnerPreStart();
      if (!this._simulation_manager)
        this._simulation_manager = FindObjectOfType<NeodroidManager>();
      if (!this._objective_function)
        this._objective_function = FindObjectOfType<ObjectiveFunction>();
      this._simulation_manager = NeodroidUtilities.MaybeRegisterComponent(
          this._simulation_manager,
          (NeodroidEnvironment)this);
      this.SaveInitialPoses();
      this.StartCoroutine(this.SaveInitialBodiesIe());
    }

    /// <summary>
    /// 
    /// </summary>
    protected virtual void InnerPreStart() { }

    #endregion

    #region Fields

    [Header("References", order = 99)]
    [SerializeField]
    ObjectiveFunction _objective_function;

    [SerializeField] NeodroidManager _simulation_manager;

    [Header("Development", order = 100)]
    [SerializeField]
    bool _debugging;

    [Header("General", order = 101)]
    [SerializeField]
    Transform _coordinate_reference_point;

    [SerializeField] bool _track_only_children = true;

    [SerializeField] CoordinateSystem _coordinate_system = CoordinateSystem.Local_coordinates_;

    [SerializeField] int _episode_length = 1000;

    [Header("(Optional)", order = 102)]
    [SerializeField]
    BoundingBox _playable_area;

    #endregion

    #region PrivateMembers

    /// <summary>
    /// 
    /// </summary>
    Vector3[] _reset_positions;

    /// <summary>
    /// 
    /// </summary>
    Quaternion[] _reset_rotations;

    /// <summary>
    /// 
    /// </summary>
    GameObject[] _tracked_game_objects;

    /// <summary>
    /// 
    /// </summary>
    Vector3[] _reset_velocities;

    /// <summary>
    /// 
    /// </summary>
    Vector3[] _reset_angulars;

    /// <summary>
    /// 
    /// </summary>
    Rigidbody[] _bodies;

    /// <summary>
    /// 
    /// </summary>
    Transform[] _poses;

    /// <summary>
    /// 
    /// </summary>
    Pose[] _received_poses;

    /// <summary>
    /// 
    /// </summary>
    Body[] _received_bodies;

    /// <summary>
    /// 
    /// </summary>
    Configuration[] _configurations;

    float _lastest_reset_time;
    float _energy_spent;
    protected bool _Terminated;
    protected bool _Is_Terminated;
    string _termination_reason = "";
    protected bool _Configured;
    bool _describe;
    bool _terminable = true;
    bool _reset;

    public PrototypingEnvironment() { this.CurrentFrameNumber = 0; }

    #endregion

    #region PublicMethods

    #region Getters

    public Dictionary<string, Displayer> Displayers { get; } = new Dictionary<string, Displayer>();

    public Dictionary<string, ConfigurableGameObject> Configurables { get; } =
      new Dictionary<string, ConfigurableGameObject>();

    public Dictionary<string, Actor> Actors { get; } = new Dictionary<string, Actor>();

    public Dictionary<string, Observer> Observers { get; } = new Dictionary<string, Observer>();

    public int EpisodeLength { get { return this._episode_length; } set { this._episode_length = value; } }

    public Dictionary<string, Resetable> Resetables { get; } = new Dictionary<string, Resetable>();

    public override string Identifier { get { return this.name; } }

    public int CurrentFrameNumber { get; private set; }

    public float GetTimeSinceReset() {
      return Time.time - this._lastest_reset_time; //Time.realtimeSinceStartup;
    }

    public bool Debugging { get { return this._debugging; } set { this._debugging = value; } }

    public NeodroidManager Manager {
      get { return this._simulation_manager; }
      set { this._simulation_manager = value; }
    }

    public ObjectiveFunction ObjectiveFunction {
      get { return this._objective_function; }
      set { this._objective_function = value; }
    }

    public BoundingBox PlayableArea {
      get { return this._playable_area; }
      set { this._playable_area = value; }
    }

    public Transform CoordinateReferencePoint {
      get { return this._coordinate_reference_point; }
      set { this._coordinate_reference_point = value; }
    }

    public CoordinateSystem CoordinateSystem {
      get { return this._coordinate_system; }
      set { this._coordinate_system = value; }
    }

    #endregion

    public void Terminate(string reason = "None") {
      if (this._terminable) {
        if (this.Debugging)
          print($"Was interrupted, because {reason}");
        this._Terminated = true;
        this._termination_reason = reason;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public override void PostStep() {
      if (this._Terminated) this._Is_Terminated = true;

      if (this._reset) {
        this._reset = false;
        this.Reset();
      }

      if (this._Configured) {
        this._Configured = false;
        this.Configure();
      }

      this.UpdateConfigurableValues();
      this.UpdateObserversData();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override Reaction SampleReaction() {
      var motions = new List<MotorMotion>();

      foreach (var actor in this.Actors) {
        foreach (var motor in actor.Value.Motors) {
          var strength = Random.Range(
              (int)motor.Value.MotionValueSpace._Min_Value,
              (int)(motor.Value.MotionValueSpace._Max_Value + 1));
          motions.Add(new MotorMotion(actor.Key, motor.Key, strength));
        }
      }

      var rp = new ReactionParameters(true, true) {IsExternal = false};
      return new Reaction(rp, motions.ToArray(), null, null, null, "");
    }

    /// <summary>
    /// 
    /// </summary>
    protected void UpdateObserversData() {
      foreach (var obs in this.Observers.Values) {
        if (obs)
          obs.UpdateObservation();
      }
    }

    /// <summary>
    /// 
    /// </summary>
    protected void UpdateConfigurableValues() {
      foreach (var con in this.Configurables.Values) {
        if (con)
          con.UpdateObservation();
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="reaction"></param>
    /// <returns></returns>
    public override EnvironmentState React(Reaction reaction) {
      if (reaction.Parameters.IsExternal) {
        this._configurations = reaction.Configurations;
        this._Configured = reaction.Parameters.Configure;
        this._describe = reaction.Parameters.Describe;
        this._terminable = reaction.Parameters.Terminable;
        if (this._Configured && reaction.Unobservables != null) {
          this._received_poses = reaction.Unobservables.Poses;
          this._received_bodies = reaction.Unobservables.Bodies;
        }
      }

      this.SendToDisplayers(reaction);

      if (reaction.Parameters.Step)
        this.Step(reaction);
      else if (reaction.Parameters.Reset) {
        this.Terminate("Reaction was reset");
        this._reset = true;
      }

      return this.GetState();
    }

    #region Registration

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    public void Register(Displayer obj) {
      if (!this.Displayers.ContainsKey(obj.Identifier)) {
        if (this.Debugging) {
          Debug.Log($"Environment {this.name} has registered displayer {obj.Identifier}");
        }

        this.Displayers.Add(obj.Identifier, obj);
      } else {
        if (this.Debugging) {
          Debug.Log($"Environment {this.name} already has displayer {obj.Identifier} registered");
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="identifier"></param>
    public void Register(Displayer obj, String identifier) {
      if (!this.Displayers.ContainsKey(identifier)) {
        if (this.Debugging)
          Debug.Log($"Environment {this.name} has registered displayer {identifier}");
        this.Displayers.Add(identifier, obj);
      } else {
        if (this.Debugging)
          Debug.Log($"Environment {this.name} already has displayer {identifier} registered");
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="actor"></param>
    public void Register(Actor actor) {
      if (!this.Actors.ContainsKey(actor.Identifier)) {
        if (this.Debugging) {
          Debug.Log($"Environment {this.name} has registered actor {actor.Identifier}");
        }

        this.Actors.Add(actor.Identifier, actor);
      } else {
        if (this.Debugging) {
          Debug.Log($"Environment {this.name} already has actor {actor.Identifier} registered");
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="actor"></param>
    /// <param name="identifier"></param>
    public void Register(Actor actor, string identifier) {
      if (!this.Actors.ContainsKey(identifier)) {
        if (this.Debugging)
          Debug.Log($"Environment {this.name} has registered actor {identifier}");
        this.Actors.Add(identifier, actor);
      } else {
        if (this.Debugging)
          Debug.Log($"Environment {this.name} already has actor {identifier} registered");
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="observer"></param>
    public void Register(Observer observer) {
      if (!this.Observers.ContainsKey(observer.Identifier)) {
        if (this.Debugging) {
          Debug.Log($"Environment {this.name} has registered observer {observer.Identifier}");
        }

        this.Observers.Add(observer.Identifier, observer);
      } else {
        if (this.Debugging) {
          Debug.Log($"Environment {this.name} already has observer {observer.Identifier} registered");
        }
      }
    }

    public void Register(Observer observer, string identifier) {
      if (!this.Observers.ContainsKey(identifier)) {
        if (this.Debugging)
          Debug.Log($"Environment {this.name} has registered observer {identifier}");
        this.Observers.Add(identifier, observer);
      } else {
        if (this.Debugging) {
          Debug.Log($"Environment {this.name} already has observer {identifier} registered");
        }
      }
    }

    public void Register(ConfigurableGameObject configurable) {
      if (!this.Configurables.ContainsKey(configurable.Identifier)) {
        if (this.Debugging) {
          Debug.Log($"Environment {this.name} has registered configurable {configurable.Identifier}");
        }

        this.Configurables.Add(configurable.Identifier, configurable);
      } else {
        if (this.Debugging) {
          Debug.Log($"Environment {this.name} already has configurable {configurable.Identifier} registered");
        }
      }
    }

    public void Register(ConfigurableGameObject configurable, string identifier) {
      if (!this.Configurables.ContainsKey(identifier)) {
        if (this.Debugging)
          Debug.Log($"Environment {this.name} has registered configurable {identifier}");
        this.Configurables.Add(identifier, configurable);
      } else {
        if (this.Debugging) {
          Debug.Log($"Environment {this.name} already has configurable {identifier} registered");
        }
      }
    }

    public void Register(Resetable resetable, string identifier) {
      if (!this.Resetables.ContainsKey(identifier)) {
        if (this.Debugging)
          Debug.Log($"Environment {this.name} has registered resetables {identifier}");
        this.Resetables.Add(identifier, resetable);
      } else {
        if (this.Debugging) {
          Debug.Log($"Environment {this.name} already has configurable {identifier} registered");
        }
      }
    }

    public void Register(Resetable resetable) {
      if (!this.Resetables.ContainsKey(resetable.Identifier)) {
        if (this.Debugging) {
          Debug.Log($"Environment {this.name} has registered resetables {resetable.Identifier}");
        }

        this.Resetables.Add(resetable.Identifier, resetable);
      } else {
        if (this.Debugging) {
          Debug.Log($"Environment {this.name} already has configurable {resetable.Identifier} registered");
        }
      }
    }

    public void UnRegisterActor(string identifier) {
      if (this.Actors.ContainsKey(identifier)) {
        if (this.Debugging)
          Debug.Log($"Environment {this.name} unregistered actor {identifier}");
      }

      this.Actors.Remove(identifier);
    }

    public void UnRegisterObserver(string identifier) {
      if (this.Observers.ContainsKey(identifier)) {
        if (this.Debugging)
          Debug.Log($"Environment {this.name} unregistered observer {identifier}");
      }

      this.Observers.Remove(identifier);
    }

    public void UnRegisterConfigurable(string identifier) {
      if (this.Configurables.ContainsKey(identifier)) {
        if (this.Debugging)
          Debug.Log($"Environment {this.name} unregistered configurable {identifier}");
      }

      this.Configurables.Remove(identifier);
    }

    #endregion

    #region Transformations

    public Vector3 TransformPosition(Vector3 position) {
      if (this._coordinate_system == CoordinateSystem.Relative_to_reference_point_) {
        if (this._coordinate_reference_point)
          return this._coordinate_reference_point.transform.InverseTransformPoint(position);
        return position;
      }

      if (this._coordinate_system == CoordinateSystem.Local_coordinates_)
        return position - this.transform.position;
      return position;
    }

    public Vector3 InverseTransformPosition(Vector3 position) {
      if (this._coordinate_system == CoordinateSystem.Relative_to_reference_point_) {
        if (this._coordinate_reference_point)
          return this._coordinate_reference_point.transform.TransformPoint(position);
        return position;
      }

      if (this._coordinate_system == CoordinateSystem.Local_coordinates_)
        return position - this.transform.position;
      return position;
    }

    public Vector3 TransformDirection(Vector3 direction) {
      if (this._coordinate_system == CoordinateSystem.Relative_to_reference_point_) {
        if (this._coordinate_reference_point)
          return this._coordinate_reference_point.transform.InverseTransformDirection(direction);
        return direction;
      }

      if (this._coordinate_system == CoordinateSystem.Local_coordinates_)
        return this.transform.InverseTransformDirection(direction);
      return direction;
    }

    public Vector3 InverseTransformDirection(Vector3 direction) {
      if (this._coordinate_system == CoordinateSystem.Relative_to_reference_point_) {
        if (this._coordinate_reference_point)
          return this._coordinate_reference_point.transform.TransformDirection(direction);
        return direction;
      }

      if (this._coordinate_system == CoordinateSystem.Local_coordinates_)
        return this.transform.InverseTransformDirection(direction);
      return direction;
    }

    public Quaternion TransformRotation(Quaternion quaternion) {
      if (this._coordinate_system == CoordinateSystem.Relative_to_reference_point_) {
        if (this._coordinate_reference_point)
          return Quaternion.Inverse(this._coordinate_reference_point.rotation) * quaternion;
        //Quaternion.Euler(this._coordinate_reference_point.transform.TransformDirection(quaternion.forward));
      }

      return quaternion;
    }

    #endregion

    #endregion

    #region PrivateMethods

    void SaveInitialPoses() {
      var ignored_layer = LayerMask.NameToLayer("IgnoredByNeodroid");
      if (this._track_only_children)
        this._tracked_game_objects =
            NeodroidUtilities.RecursiveChildGameObjectsExceptLayer(this.transform, ignored_layer);
      else {
        this._tracked_game_objects = NeodroidUtilities.FindAllGameObjectsExceptLayer(ignored_layer);
      }

      this._reset_positions = new Vector3[this._tracked_game_objects.Length];
      this._reset_rotations = new Quaternion[this._tracked_game_objects.Length];
      this._poses = new Transform[this._tracked_game_objects.Length];
      for (var i = 0; i < this._tracked_game_objects.Length; i++) {
        this._reset_positions[i] = this._tracked_game_objects[i].transform.position;
        this._reset_rotations[i] = this._tracked_game_objects[i].transform.rotation;
        this._poses[i] = this._tracked_game_objects[i].transform;
        var maybe_joint = this._tracked_game_objects[i].GetComponent<Joint>();
        if (maybe_joint != null)
          maybe_joint.gameObject.AddComponent<JointFix>();
      }
    }

    void SaveInitialBodies() {
      var body_list = new List<Rigidbody>();
      foreach (var go in this._tracked_game_objects) {
        if (go != null) {
          var body = go.GetComponent<Rigidbody>();
          if (body)
            body_list.Add(body);
        }
      }

      //if (body_list.Count > 0) {
      this._bodies = body_list.ToArray();
      this._reset_velocities = new Vector3[this._bodies.Length];
      this._reset_angulars = new Vector3[this._bodies.Length];
      for (var i = 0; i < this._bodies.Length; i++) {
        this._reset_velocities[i] = this._bodies[i].velocity;
        this._reset_angulars[i] = this._bodies[i].angularVelocity;
      }

      //}
    }

    IEnumerator SaveInitialBodiesIe() {
      yield return new WaitForFixedUpdate();
      this.SaveInitialBodies();
    }

    EnvironmentState GetState() {
      foreach (var a in this.Actors.Values) {
        foreach (var m in a.Motors.Values)
          this._energy_spent += m.GetEnergySpend();
      }

      var signal = 0f;
      if (!this._Is_Terminated) {
        if (this._objective_function != null)
          signal = this._objective_function.Evaluate();
      }

      EnvironmentDescription description = null;
      if (this._describe) {
        var threshold = 0f;
        if (this._objective_function != null)
          threshold = this._objective_function.SolvedThreshold;
        description = new EnvironmentDescription(
            this.EpisodeLength,
            this._simulation_manager.Configuration,
            this.Actors,
            this.Configurables,
            threshold);
        this._describe = false;
      }

      var observables = new List<float>();
      foreach (var item in this.Observers.OrderBy(i => i.Key))
        observables.AddRange(item.Value.FloatEnumerable);

      return new EnvironmentState(
          this.Identifier,
          this._energy_spent,
          this.Observers,
          this.CurrentFrameNumber,
          signal,
          this._Terminated || this._Is_Terminated,
          observables.ToArray(),
          this._bodies,
          this._poses,
          this._termination_reason,
          description);
    }

    protected void Reset() {
      this._lastest_reset_time = Time.time;
      this.CurrentFrameNumber = 0;
      if (this._objective_function)
        this._objective_function.Reset();

      this.ResetRegisteredObjects();
      this.SetEnvironmentPoses(this._tracked_game_objects, this._reset_positions, this._reset_rotations);
      this.SetEnvironmentBodies(this._bodies, this._reset_velocities, this._reset_angulars);
      this._Is_Terminated = false;
      this._Terminated = false;
    }

    protected void Configure() {
      if (this._received_poses != null) {
        var positions = new Vector3[this._received_poses.Length];
        var rotations = new Quaternion[this._received_poses.Length];
        for (var i = 0; i < this._received_poses.Length; i++) {
          positions[i] = this._received_poses[i].position;
          rotations[i] = this._received_poses[i].rotation;
        }

        this.SetEnvironmentPoses(this._tracked_game_objects, positions, rotations);
      }

      if (this._received_bodies != null) {
        var vels = new Vector3[this._received_bodies.Length];
        var angs = new Vector3[this._received_bodies.Length];
        for (var i = 0; i < this._received_bodies.Length; i++) {
          vels[i] = this._received_bodies[i].Velocity;
          angs[i] = this._received_bodies[i].AngularVelocity;
        }

        this.SetEnvironmentBodies(this._bodies, vels, angs);
      }

      if (this._configurations != null) {
        foreach (var configuration in this._configurations) {
          if (this.Configurables.ContainsKey(configuration.ConfigurableName))
            this.Configurables[configuration.ConfigurableName].ApplyConfiguration(configuration);
          else {
            if (this.Debugging) {
              Debug.Log(
                  "Could find not configurable with the specified name: " + configuration.ConfigurableName);
            }
          }
        }
      }
    }

    void SendToDisplayers(Reaction reaction) {
      if (reaction.Displayables != null && reaction.Displayables.Length > 0) {
        foreach (var displayable in reaction.Displayables) {
          if (this.Debugging)
            Debug.Log("Applying " + displayable + " To " + this.name + "'s displayers");
          var displayable_name = displayable.DisplayableName;
          if (this.Displayers.ContainsKey(displayable_name) && this.Displayers[displayable_name] != null) {
            var v = displayable.DisplayableValue;
            this.Displayers[displayable_name].Display(v);
          } else {
            if (this.Debugging)
              Debug.Log("Could find not displayer with the specified name: " + displayable_name);
          }
        }
      }
    }

    void SendToMotors(Reaction reaction) {
      if (reaction.Motions != null && reaction.Motions.Length > 0) {
        foreach (var motion in reaction.Motions) {
          if (this.Debugging)
            Debug.Log("Applying " + motion + " To " + this.name + "'s actors");
          var motion_actor_name = motion.ActorName;
          if (this.Actors.ContainsKey(motion_actor_name) && this.Actors[motion_actor_name] != null)
            this.Actors[motion_actor_name].ApplyMotion(motion);
          else {
            if (this.Debugging)
              Debug.Log("Could find not actor with the specified name: " + motion_actor_name);
          }
        }
      }
    }

    void Step(Reaction reaction) {
      if (reaction.Parameters.EpisodeCount)
        this.CurrentFrameNumber++;

      this.SendToMotors(reaction);

      if (this.EpisodeLength > 0 && this.CurrentFrameNumber >= this.EpisodeLength) {
        if (this.Debugging)
          Debug.Log("Maximum episode length reached");

        this.Terminate("Maximum episode length reached");
      }

      this.UpdateObserversData();
    }

    protected void ResetRegisteredObjects() {
      if (this.Debugging)
        Debug.Log("Resetting registed objects");
      foreach (var resetable in this.Resetables.Values) {
        if (resetable != null)
          resetable.Reset();
      }

      foreach (var actor in this.Actors.Values) {
        if (actor)
          actor.Reset();
      }

      foreach (var observer in this.Observers.Values) {
        if (observer)
          observer.Reset();
      }
    }

    #region EnvironmentStateSetters

    void SetEnvironmentPoses(GameObject[] child_game_objects, Vector3[] positions, Quaternion[] rotations) {
      if (this._simulation_manager) {
        for (var iterations = 0;
             iterations < this._simulation_manager.Configuration.ResetIterations;
             iterations++) {
          for (var i = 0; i < child_game_objects.Length; i++) {
            if (child_game_objects[i] != null && i < positions.Length && i < rotations.Length) {
              var rigid_body = child_game_objects[i].GetComponent<Rigidbody>();
              if (rigid_body)
                rigid_body.Sleep();
              child_game_objects[i].transform.position = positions[i];
              child_game_objects[i].transform.rotation = rotations[i];
              if (rigid_body)
                rigid_body.WakeUp();

              var joint_fix = child_game_objects[i].GetComponent<JointFix>();
              if (joint_fix)
                joint_fix.Reset();
              var anim = child_game_objects[i].GetComponent<Animation>();
              if (anim)
                anim.Rewind();
            }
          }
        }
      }
    }

    void SetEnvironmentBodies(Rigidbody[] bodies, Vector3[] velocities, Vector3[] angulars) {
      if (bodies != null && bodies.Length > 0) {
        for (var i = 0; i < bodies.Length; i++) {
          if (i < bodies.Length && bodies[i] != null && i < velocities.Length && i < angulars.Length) {
            if (this.Debugging) {
              print(
                  $"Setting {bodies[i].name}, velocity to {velocities[i]} and angular velocity to {angulars[i]}");
            }

            bodies[i].Sleep();
            bodies[i].velocity = velocities[i];
            bodies[i].angularVelocity = angulars[i];
            bodies[i].WakeUp();
          }
        }
      }
    }

    #endregion

    #endregion
  }
}
