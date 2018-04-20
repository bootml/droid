using System;
using Neodroid.Messaging.Messages;
using Neodroid.Utilities.Interfaces;
using UnityEngine;

namespace Neodroid.Environments {
  /// <summary>
  /// 
  /// </summary>
  public abstract class NeodroidEnvironment : MonoBehaviour,
                                              IRegisterable {
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public abstract String Identifier { get; }
    /// <summary>
    /// 
    /// </summary>
    public abstract void PostStep();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public abstract Reaction SampleReaction();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="reaction"></param>
    /// <returns></returns>
    public abstract EnvironmentState React(Reaction reaction);
  }
}
