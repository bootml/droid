using System;
using Neodroid.Messaging.Messages;
using Neodroid.Utilities;
using UnityEngine;

namespace Neodroid.Prototyping.Motors {
  [AddComponentMenu(PrototypingComponentMenuPath._ComponentMenuPath+"Motors/Rigidbody")]
  [RequireComponent(typeof(Rigidbody))]
  public class RigidbodyMotor : Motor {
    [SerializeField] protected Space _Relative_To = Space.Self;

    [SerializeField] protected Rigidbody _Rigidbody;

    string _rot_x;
    string _rot_y;
    string _rot_z;

    string _x;
    string _y;
    string _z;

    public override String MotorIdentifier { get { return this.name + "Rigidbody"; } }

    protected override void Setup() { this._Rigidbody = this.GetComponent<Rigidbody>(); }

    public override void RegisterComponent() {
      this.ParentActor = NeodroidUtilities.MaybeRegisterComponent(this.ParentActor, (Motor)this);

      this._x = this.MotorIdentifier + "X";
      this._y = this.MotorIdentifier + "Y";
      this._z = this.MotorIdentifier + "Z";
      this._rot_x = this.MotorIdentifier + "RotX";
      this._rot_y = this.MotorIdentifier + "RotY";
      this._rot_z = this.MotorIdentifier + "RotZ";
      this.ParentActor =
          NeodroidUtilities.MaybeRegisterNamedComponent(this.ParentActor, (Motor)this, this._x);
      this.ParentActor =
          NeodroidUtilities.MaybeRegisterNamedComponent(this.ParentActor, (Motor)this, this._y);
      this.ParentActor =
          NeodroidUtilities.MaybeRegisterNamedComponent(this.ParentActor, (Motor)this, this._z);
      this.ParentActor =
          NeodroidUtilities.MaybeRegisterNamedComponent(this.ParentActor, (Motor)this, this._rot_x);
      this.ParentActor =
          NeodroidUtilities.MaybeRegisterNamedComponent(this.ParentActor, (Motor)this, this._rot_y);
      this.ParentActor =
          NeodroidUtilities.MaybeRegisterNamedComponent(this.ParentActor, (Motor)this, this._rot_z);
    }

    protected override void InnerApplyMotion(MotorMotion motion) {
      if (motion.MotorName == this._x)
        this._Rigidbody.AddForce(Vector3.left * motion.Strength);
      else if (motion.MotorName == this._y)
        this._Rigidbody.AddForce(Vector3.up * motion.Strength);
      else if (motion.MotorName == this._z)
        this._Rigidbody.AddForce(Vector3.forward * motion.Strength);
      else if (motion.MotorName == this._rot_x)
        this._Rigidbody.AddTorque(Vector3.left * motion.Strength);
      else if (motion.MotorName == this._rot_y)
        this._Rigidbody.AddTorque(Vector3.up * motion.Strength);
      else if (motion.MotorName == this._rot_z)
        this._Rigidbody.AddTorque(Vector3.forward * motion.Strength);
    }
  }
}
