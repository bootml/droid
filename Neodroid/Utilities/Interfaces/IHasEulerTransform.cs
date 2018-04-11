using UnityEngine;

namespace Neodroid.Utilities.Interfaces {
  public interface IHasEulerTransform {
    Vector3 Position { get; }

    Vector3 Direction { get; }

    Vector3 Rotation { get; }
  }
}
