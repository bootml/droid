using System;
using UnityEngine;

namespace Neodroid.Prototyping.Resetables {
  /// <summary>
  /// 
  /// </summary>
  public class TransformResetable : Resetable {
    /// <summary>
    /// 
    /// </summary>
    public override void Reset() {       
      this.transform.position = this._original_position;
      this.transform.rotation = this._original_rotation;
    }
    /// <summary>
    /// 
    /// </summary>
    public override String Identifier { get { return this.name + "Transform"; } }

    /// <summary>
    /// 
    /// </summary>
    Vector3 _original_position;
    /// <summary>
    /// 
    /// </summary>
    Quaternion _original_rotation;

    /// <summary>
    /// 
    /// </summary>
    void Start() {
      this._original_position = this.transform.position;
      this._original_rotation = this.transform.rotation;
    }
  }
}
