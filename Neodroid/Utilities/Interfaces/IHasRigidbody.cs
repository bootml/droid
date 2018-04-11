using UnityEngine;

namespace Neodroid.Utilities.Interfaces {
  public interface IHasRigidbody {
    Vector3 Velocity { get; }

    Vector3 AngularVelocity { get; }
  }
}
