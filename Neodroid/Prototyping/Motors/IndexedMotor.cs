using System;
using Neodroid.Messaging.Messages;
using UnityEngine;

namespace Neodroid.Prototyping.Motors {
  [AddComponentMenu(PrototypingComponentMenuPath._ComponentMenuPath+"Motors/Indexed")]
  public class IndexedMotor : Motor {
    [SerializeField] protected string _Layer_Mask = "Obstructions";
    [SerializeField] protected bool _No_Collisions = true;
    [SerializeField] protected Space _Relative_To = Space.Self;
    public override String MotorIdentifier { get { return this.name + "Index"; } }

    protected override void InnerApplyMotion(MotorMotion motion) {
      var layer_mask = 1 << LayerMask.NameToLayer(this._Layer_Mask);

      Vector3 vec;
      switch ((int)motion.Strength) {
        case 1:
          vec = Vector3.forward;
          break;
        case 2:
          vec = Vector3.back;
          break;
        case 3:
          vec = Vector3.left;
          break;
        case 4:
          vec = Vector3.right;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }

      if (this._No_Collisions) {
        if (!Physics.Raycast(this.transform.position, vec, Mathf.Abs(motion.Strength), layer_mask))
          this.transform.Translate(vec, this._Relative_To);
      } else
        this.transform.Translate(vec, this._Relative_To);
    }
  }
}
