using System;
using UnityEngine;

namespace Neodroid.Prototyping.Resetables {
  public class TransformResetable : Resetable {
    public override String ResetableIdentifier { get; }
    public override void Reset() { throw new NotImplementedException(); }

    Vector3 _original_position;
    Quaternion _original_rotation;

    void Start() {
      
      this._original_position = this.transform.position;
      this._original_rotation = this.transform.rotation;
    }
  }
}
