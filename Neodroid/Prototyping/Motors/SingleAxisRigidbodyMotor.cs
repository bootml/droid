using System;
using Neodroid.Messaging.Messages;
using Neodroid.Utilities.Enums;
using UnityEngine;

namespace Neodroid.Prototyping.Motors {
  [AddComponentMenu(PrototypingComponentMenuPath._ComponentMenuPath+"Motors/SingleAxisRigidbody")]
  [RequireComponent(typeof(Rigidbody))]
  public class SingleAxisRigidbodyMotor : Motor {
    [Header("General", order = 101)]
    [SerializeField]
    protected Axis _Axis_Of_Motion;

    [SerializeField] protected Space _Relative_To = Space.Self;

    [SerializeField] protected Rigidbody _Rigidbody;

    public override String MotorIdentifier { get { return this.name + "Rigidbody" + this._Axis_Of_Motion; } }

    protected override void Setup() { this._Rigidbody = this.GetComponent<Rigidbody>(); }

    protected override void InnerApplyMotion(MotorMotion motion) {
      switch (this._Axis_Of_Motion) {
        case Axis.X_:
          if (this._Relative_To == Space.World)
            this._Rigidbody.AddForce(Vector3.left * motion.Strength);
          else
            this._Rigidbody.AddRelativeForce(Vector3.left * motion.Strength);
          break;
        case Axis.Y_:
          if (this._Relative_To == Space.World)
            this._Rigidbody.AddForce(Vector3.up * motion.Strength);
          else
            this._Rigidbody.AddRelativeForce(Vector3.up * motion.Strength);
          break;
        case Axis.Z_:
          if (this._Relative_To == Space.World)
            this._Rigidbody.AddForce(Vector3.forward * motion.Strength);
          else
            this._Rigidbody.AddRelativeForce(Vector3.up * motion.Strength);
          break;
        case Axis.Rot_x_:
          if (this._Relative_To == Space.World)
            this._Rigidbody.AddTorque(Vector3.left * motion.Strength);
          else
            this._Rigidbody.AddRelativeTorque(Vector3.left * motion.Strength);
          break;
        case Axis.Rot_y_:
          if (this._Relative_To == Space.World)
            this._Rigidbody.AddTorque(Vector3.up * motion.Strength);
          else
            this._Rigidbody.AddRelativeTorque(Vector3.up * motion.Strength);
          break;
        case Axis.Rot_z_:
          if (this._Relative_To == Space.World)
            this._Rigidbody.AddTorque(Vector3.forward * motion.Strength);
          else
            this._Rigidbody.AddRelativeTorque(Vector3.forward * motion.Strength);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }
}
