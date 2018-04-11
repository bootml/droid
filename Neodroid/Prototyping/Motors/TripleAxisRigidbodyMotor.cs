using System;
using Neodroid.Messaging.Messages;
using Neodroid.Utilities;
using UnityEngine;

namespace Neodroid.Prototyping.Motors {
  [AddComponentMenu(PrototypingComponentMenuPath._ComponentMenuPath+"Motors/TripleAxisRigidbody")]
  [RequireComponent(typeof(Rigidbody))]
  public class TripleAxisRigidbodyMotor : Motor {
    [SerializeField] protected Space _Relative_To = Space.Self;

    [SerializeField] protected Rigidbody _Rigidbody;

    [SerializeField] protected bool _Rotational_Motors;

    string _x;
    string _y;
    string _z;

    public override String MotorIdentifier { get { return this.name + "Rigidbody"; } }

    protected override void Setup() { this._Rigidbody = this.GetComponent<Rigidbody>(); }

    public override void RegisterComponent() {
      this._x = this.MotorIdentifier + "X";
      this._y = this.MotorIdentifier + "Y";
      this._z = this.MotorIdentifier + "Z";
      if (this._Rotational_Motors) {
        this._x = this.MotorIdentifier + "RotX";
        this._y = this.MotorIdentifier + "RotY";
        this._z = this.MotorIdentifier + "RotZ";
      }

      this.ParentActor =
          NeodroidUtilities.MaybeRegisterNamedComponent(this.ParentActor, (Motor)this, this._x);
      this.ParentActor =
          NeodroidUtilities.MaybeRegisterNamedComponent(this.ParentActor, (Motor)this, this._y);
      this.ParentActor =
          NeodroidUtilities.MaybeRegisterNamedComponent(this.ParentActor, (Motor)this, this._z);
    }

    protected override void InnerApplyMotion(MotorMotion motion) {
      if (!this._Rotational_Motors) {
        if (motion.MotorName == this._x) {
          if (this._Relative_To == Space.World)
            this._Rigidbody.AddForce(Vector3.left * motion.Strength);
          else
            this._Rigidbody.AddRelativeForce(Vector3.left * motion.Strength);
        } else if (motion.MotorName == this._y) {
          if (this._Relative_To == Space.World)
            this._Rigidbody.AddForce(Vector3.up * motion.Strength);
          else
            this._Rigidbody.AddRelativeForce(Vector3.up * motion.Strength);
        } else if (motion.MotorName == this._z) {
          if (this._Relative_To == Space.World)
            this._Rigidbody.AddForce(Vector3.forward * motion.Strength);
          else
            this._Rigidbody.AddRelativeForce(Vector3.forward * motion.Strength);
        }
      } else {
        if (motion.MotorName == this._x) {
          if (this._Relative_To == Space.World)
            this._Rigidbody.AddTorque(Vector3.left * motion.Strength);
          else
            this._Rigidbody.AddRelativeTorque(Vector3.left * motion.Strength);
        } else if (motion.MotorName == this._y) {
          if (this._Relative_To == Space.World)
            this._Rigidbody.AddTorque(Vector3.up * motion.Strength);
          else
            this._Rigidbody.AddRelativeTorque(Vector3.up * motion.Strength);
        } else if (motion.MotorName == this._z) {
          if (this._Relative_To == Space.World)
            this._Rigidbody.AddTorque(Vector3.forward * motion.Strength);
          else
            this._Rigidbody.AddRelativeTorque(Vector3.forward * motion.Strength);
        }
      }
    }
  }
}
