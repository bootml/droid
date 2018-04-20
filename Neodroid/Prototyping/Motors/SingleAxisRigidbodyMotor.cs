﻿using System;
using Neodroid.Messaging.Messages;
using Neodroid.Utilities.Enums;
using UnityEngine;

namespace Neodroid.Prototyping.Motors {
  /// <summary>
  /// 
  /// </summary>
  [AddComponentMenu(PrototypingComponentMenuPath._ComponentMenuPath + "Motors/SingleAxisRigidbodyMotor")]
  [RequireComponent(typeof(Rigidbody))]
  public class SingleAxisRigidbodyMotor : Motor {
    /// <summary>
    /// 
    /// </summary>
    [Header("General", order = 101)]
    [SerializeField]
    protected Axis _Axis_Of_Motion;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    protected Space _Relative_To = Space.Self;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    protected Rigidbody _Rigidbody;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField] protected ForceMode _ForceMode = ForceMode.Force;

    /// <summary>
    /// 
    /// </summary>
    public override String Identifier { get { return this.name + "Rigidbody" + this._Axis_Of_Motion; } }

    /// <summary>
    /// 
    /// </summary>
    protected override void Setup() { this._Rigidbody = this.GetComponent<Rigidbody>(); }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="motion"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    protected override void InnerApplyMotion(MotorMotion motion) {
      switch (this._Axis_Of_Motion) {
        case Axis.X_:
          if (this._Relative_To == Space.World)   
            this._Rigidbody.AddForce(Vector3.left * motion.Strength, this._ForceMode);
          else
            this._Rigidbody.AddRelativeForce(Vector3.left * motion.Strength, this._ForceMode);
          break;
        case Axis.Y_:
          if (this._Relative_To == Space.World)
            this._Rigidbody.AddForce(Vector3.up * motion.Strength, this._ForceMode);
          else
            this._Rigidbody.AddRelativeForce(Vector3.up * motion.Strength, this._ForceMode);
          break;
        case Axis.Z_:
          if (this._Relative_To == Space.World)
            this._Rigidbody.AddForce(Vector3.forward * motion.Strength, this._ForceMode);
          else
            this._Rigidbody.AddRelativeForce(Vector3.forward * motion.Strength, this._ForceMode);
          break;
        case Axis.Rot_x_:
          if (this._Relative_To == Space.World)
            this._Rigidbody.AddTorque(Vector3.left * motion.Strength, this._ForceMode);
          else
            this._Rigidbody.AddRelativeTorque(Vector3.left * motion.Strength, this._ForceMode);
          break;
        case Axis.Rot_y_:
          if (this._Relative_To == Space.World)
            this._Rigidbody.AddTorque(Vector3.up * motion.Strength, this._ForceMode);
          else
            this._Rigidbody.AddRelativeTorque(Vector3.up * motion.Strength, this._ForceMode);
          break;
        case Axis.Rot_z_:
          if (this._Relative_To == Space.World)
            this._Rigidbody.AddTorque(Vector3.forward * motion.Strength, this._ForceMode);
          else
            this._Rigidbody.AddRelativeTorque(Vector3.forward * motion.Strength, this._ForceMode);
          break;
        case Axis.Dir_x_: break;
        case Axis.Dir_y_: break;
        case Axis.Dir_z_: break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }
}
