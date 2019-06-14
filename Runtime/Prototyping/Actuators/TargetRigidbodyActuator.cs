﻿using System;
using droid.Runtime.Environments;
using droid.Runtime.Interfaces;
using droid.Runtime.Prototyping.Actors;
using droid.Runtime.Utilities.Misc;
using UnityEngine;

namespace droid.Runtime.Prototyping.Actuators {
  /// <inheritdoc cref="Actuator" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ActuatorComponentMenuPath._ComponentMenuPath
                    + "TargetRigidbody"
                    + ActuatorComponentMenuPath._Postfix)]
  [RequireComponent(typeof(Rigidbody))]
  public class TargetRigidbodyActuator : Actuator,
                                         IEnvironmentListener {
    string _movement;
    AbstractPrototypingEnvironment _parent_environment;

    /// <summary>
    /// </summary>
    [SerializeField]
    protected Rigidbody _Rigidbody;

    string _turn;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "TargetRigidbody"; } }

    /// <summary>
    ///
    /// </summary>
    public Single MovementSpeed { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Single RotationSpeed { get; set; }

    /// <summary>
    ///
    /// </summary>
    public void PreStep() { }

    /// <summary>
    ///
    /// </summary>
    public void Step() { this.OnStep(); }

    /// <summary>
    ///
    /// </summary>
    public void PostStep() { }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void Setup() {
      this._Rigidbody = this.GetComponent<Rigidbody>();

      this._movement = this.Identifier + "Movement_";
      this._turn = this.Identifier + "Turn_";
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.Parent =
          NeodroidUtilities.RegisterComponent((IHasRegister<IActuator>)this.Parent, (Actuator)this, this._movement);
      this.Parent =
          NeodroidUtilities.RegisterComponent((IHasRegister<IActuator>)this.Parent, (Actuator)this, this._turn);

      this._parent_environment =
          NeodroidUtilities.RegisterComponent(this._parent_environment,
                                              this);

      if (this._parent_environment != null) {
        this._parent_environment.PreStepEvent += this.PreStep;
        this._parent_environment.StepEvent += this.Step;
        this._parent_environment.PostStepEvent += this.PostStep;
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    protected override void InnerApplyMotion(IMotion motion) {
      if (motion.ActuatorName == this._movement) {
        this.ApplyMovement(motion.Strength);
      } else if (motion.ActuatorName == this._turn) {
        this.ApplyRotation(motion.Strength);
      }
    }

    void ApplyRotation(float rotation_change = 0f) { this.RotationSpeed = rotation_change; }

    void ApplyMovement(float movement_change = 0f) { this.MovementSpeed = movement_change; }

    void OnStep() {
      this._Rigidbody.velocity = Vector3.zero;
      this._Rigidbody.angularVelocity = Vector3.zero;

      // Move
      var movement = this.MovementSpeed * Time.deltaTime * this.transform.forward;
      this._Rigidbody.MovePosition(this._Rigidbody.position + movement);

      // Turn
      var turn = this.RotationSpeed * Time.deltaTime;
      var turn_rotation = Quaternion.Euler(0f, turn, 0f);
      this._Rigidbody.MoveRotation(this._Rigidbody.rotation * turn_rotation);
    }
  }
}
