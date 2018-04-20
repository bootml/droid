using System;
using Neodroid.Messaging.Messages;
using Neodroid.Utilities;
using UnityEngine;

namespace Neodroid.Prototyping.Motors {
  [AddComponentMenu(PrototypingComponentMenuPath._ComponentMenuPath + "Motors/TripleAxisEulerTransformMotor")]
  public class TripleAxisTransformMotor : Motor {
    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    protected string _Layer_Mask = "Obstructions";

    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    protected bool _No_Collisions = true;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    protected Space _Relative_To = Space.Self;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    protected bool _Rotational_Motors;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    protected bool _Use_Mask = true;

    /// <summary>
    /// XAxisIdentier
    /// </summary>
    string _x;

    /// <summary>
    /// YAxisIdentier
    /// </summary>
    string _y;

    /// <summary>
    /// ZAxisIdentier
    /// </summary>
    string _z;

    /// <summary>
    /// 
    /// </summary>
    public override String Identifier { get { return this.name + "Transform"; } }

    /// <summary>
    /// 
    /// </summary>
    public override void RegisterComponent() {
      if (!this._Rotational_Motors) {
        this._x = this.Identifier + "X";
        this._y = this.Identifier + "Y";
        this._z = this.Identifier + "Z";
      } else {
        this._x = this.Identifier + "RotX";
        this._y = this.Identifier + "RotY";
        this._z = this.Identifier + "RotZ";
      }

      this.ParentActor =
          NeodroidUtilities.MaybeRegisterNamedComponent(this.ParentActor, (Motor)this, this._x);
      this.ParentActor =
          NeodroidUtilities.MaybeRegisterNamedComponent(this.ParentActor, (Motor)this, this._y);
      this.ParentActor =
          NeodroidUtilities.MaybeRegisterNamedComponent(this.ParentActor, (Motor)this, this._z);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="motion"></param>
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
