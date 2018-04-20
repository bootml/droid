using UnityEngine;

namespace Neodroid.Utilities.Interfaces {
  /// <summary>
  /// 
  /// </summary>
  public interface IHasEulerTransform {
    /// <summary>
    /// 
    /// </summary>
    Vector3 Position { get; }

    /// <summary>
    /// 
    /// </summary>
    Vector3 Direction { get; }

    /// <summary>
    /// 
    /// </summary>
    Vector3 Rotation { get; }
  }
}
