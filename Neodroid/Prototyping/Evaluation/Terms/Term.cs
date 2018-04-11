using System;
using UnityEngine;

namespace Neodroid.Prototyping.Evaluation.Terms {
  [Serializable]
  public abstract class Term : MonoBehaviour {
    public abstract float Evaluate();
  }
}
