﻿using UnityEngine;

namespace Neodroid.Messaging.Messages {
  public class Body {
    public Body(Vector3 vel, Vector3 ang) {
      this.Velocity = vel;
      this.AngularVelocity = ang;
    }

    public Vector3 Velocity { get; }

    public Vector3 AngularVelocity { get; }
  }
}
