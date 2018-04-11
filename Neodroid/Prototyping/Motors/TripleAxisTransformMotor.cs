using System;
using Neodroid.Messaging.Messages;
using Neodroid.Utilities;
using UnityEngine;

namespace Neodroid.Prototyping.Motors {
  [AddComponentMenu(PrototypingComponentMenuPath._ComponentMenuPath+"Motors/TripleAxisEulerTransform")]
  public class TripleAxisTransformMotor : Motor {
    [SerializeField] protected string _Layer_Mask = "Obstructions";

    [SerializeField] protected bool _No_Collisions = true;
    [SerializeField] protected Space _Relative_To = Space.Self;

    [SerializeField] protected bool _Rotational_Motors;
    [SerializeField] protected bool _Use_Mask = true;

    string _x;
    string _y;
    string _z;

    public override String MotorIdentifier { get { return this.name + "Transform"; } }

    public override void RegisterComponent() {
      if (!this._Rotational_Motors) {
        this._x = this.MotorIdentifier + "X";
        this._y = this.MotorIdentifier + "Y";
        this._z = this.MotorIdentifier + "Z";
      } else {
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
      var layer_mask = 1 << LayerMask.NameToLayer(this._Layer_Mask);
      if (!this._Rotational_Motors) {
        if (motion.MotorName == this._x) {
          var vec = Vector3.right * motion.Strength;
          if (this._No_Collisions) {
            if (!Physics.Raycast(this.transform.position, vec, Mathf.Abs(motion.Strength), layer_mask))
              this.transform.Translate(vec, this._Relative_To);
          } else
            this.transform.Translate(vec, this._Relative_To);
        } else if (motion.MotorName == this._y) {
          var vec = -Vector3.up * motion.Strength;
          if (this._No_Collisions) {
            if (!Physics.Raycast(this.transform.position, vec, Mathf.Abs(motion.Strength), layer_mask))
              this.transform.Translate(vec, this._Relative_To);
          } else
            this.transform.Translate(vec, this._Relative_To);
        } else if (motion.MotorName == this._z) {
          var vec = -Vector3.forward * motion.Strength;
          if (this._No_Collisions) {
            if (!Physics.Raycast(this.transform.position, vec, Mathf.Abs(motion.Strength), layer_mask))
              this.transform.Translate(vec, this._Relative_To);
          } else
            this.transform.Translate(vec, this._Relative_To);
        }
      } else {
        if (motion.MotorName == this._x)
          this.transform.Rotate(Vector3.left, motion.Strength, this._Relative_To);
        else if (motion.MotorName == this._y)
          this.transform.Rotate(Vector3.up, motion.Strength, this._Relative_To);
        else if (motion.MotorName == this._z)
          this.transform.Rotate(Vector3.forward, motion.Strength, this._Relative_To);
      }
    }
  }
}
