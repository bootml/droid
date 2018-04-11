using System;
using Neodroid.Messaging.Messages;
using UnityEngine;

namespace Neodroid.Environments {
  public abstract class NeodroidEnvironment : MonoBehaviour {
    public abstract String Identifier { get; }
    public abstract void PostStep();

    public abstract Reaction SampleReaction();
    public abstract EnvironmentState React(Reaction reaction);
  }
}
