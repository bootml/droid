using System;
using Neodroid.Messaging.Messages;
using UnityEngine;

namespace Neodroid.Prototyping.Motors.WheelColliderMotor {
  /// <summary>
  /// 
  /// </summary>
  [AddComponentMenu(PrototypingComponentMenuPath._ComponentMenuPath + "Motors/WheelCollider/SteeringMotor")]
  [RequireComponent(typeof(WheelCollider))]
  public class SteeringMotor : Motor {
    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    WheelCollider _wheel_collider;

    /// <summary>
    /// 
    /// </summary>
    public override String Identifier { get { return this.name + "Steering"; } }

    /// <summary>
    /// 
    /// </summary>
    protected override void Awake() {
      base.Awake();
      this._wheel_collider = this.GetComponent<WheelCollider>();
    }

    /// <summary>
    /// 
    /// </summary>
    void FixedUpdate() { ApplyLocalPositionToVisuals(this._wheel_collider); }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="motion"></param>
    protected override void InnerApplyMotion(MotorMotion motion) {
      this._wheel_collider.steerAngle = motion.Strength;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="col"></param>
    static void ApplyLocalPositionToVisuals(WheelCollider col) {
      if (col.transform.childCount == 0)
        return;

      var visual_wheel = col.transform.GetChild(0);

      Vector3 position;
      Quaternion rotation;
      col.GetWorldPose(out position, out rotation);

      visual_wheel.transform.position = position;
      visual_wheel.transform.rotation = rotation;
    }
  }
}
