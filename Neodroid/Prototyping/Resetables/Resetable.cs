using System;
using Neodroid.Environments;
using Neodroid.Utilities;
using Neodroid.Utilities.Interfaces;
using UnityEngine;

namespace Neodroid.Prototyping.Resetables {
  /// <summary>
  /// 
  /// </summary>
  [ExecuteInEditMode]
  public abstract class Resetable : MonoBehaviour,
                                    IRegisterable {
    /// <summary>
    /// 
    /// </summary>
    public PrototypingEnvironment _Parent_Environment;

    /// <summary>
    /// 
    /// </summary>
    public abstract void Reset();

    /// <summary>
    /// 
    /// </summary>
    protected virtual void Awake() { this.RegisterComponent(); }

    /// <summary>
    /// 
    /// </summary>
    protected virtual void RegisterComponent() {
      this._Parent_Environment = NeodroidUtilities.MaybeRegisterComponent(this._Parent_Environment, this);
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract String Identifier { get; }
  }
}
