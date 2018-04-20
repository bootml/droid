using System;
using Neodroid.Utilities.Interfaces;
using UnityEngine;

namespace Neodroid.Prototyping.Evaluation.Terms {
  [Serializable]
  public abstract class Term : MonoBehaviour,
                               IRegisterable {
    public abstract float Evaluate();
    public abstract String Identifier { get; }
  }
}
