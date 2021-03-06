﻿using System;
using Neodroid.Runtime.Utilities.GameObjects;
using Neodroid.Runtime.Utilities.Misc.Drawing;
using Neodroid.Runtime.Utilities.Misc.Grasping;

namespace Neodroid.Runtime.Prototyping.Evaluation.Terms {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [Serializable]
  public abstract class Term : PrototypingGameObject {
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public abstract float Evaluate();

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public abstract override String PrototypingTypeName { get; }

    /// <summary>
    /// 
    /// </summary>
    ObjectiveFunction _objective_function;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this._objective_function = NeodroidUtilities.MaybeRegisterComponent(this._objective_function, this);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this._objective_function) {
        this._objective_function.UnRegister(this);
      }
    }
  }
}