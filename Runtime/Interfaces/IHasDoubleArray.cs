﻿using droid.Runtime.Utilities.Structs;
using UnityEngine;

namespace droid.Runtime.Interfaces {
  /// <summary>
  /// </summary>
  public interface IHasDoubleArray {
    /// <summary>
    /// </summary>
    Vector2[] ObservationArray { get; }

    Space1[] ObservationSpace { get; }
  }
}
