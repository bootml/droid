using System;
using Neodroid.Messaging.Messages;
using UnityEngine;

namespace Neodroid.Prototyping.Motors.WheelColliderMotor {
  [AddComponentMenu(PrototypingComponentMenuPath._ComponentMenuPath + "Motors/WheelCollider/TorqueMotor")]
  [RequireComponent(typeof(WheelCollider))]
  public class TorqueMotor : Motor {
    [SerializeField] WheelCollider _wheel_collider;

    public override String Identifier { get { return this.name + "Torque"; } }

    protected override void Awake() {
      base.Awake();
      this._wheel_collider = this.GetComponent<WheelCollider>();
    }

    protected override void InnerApplyMotion(MotorMotion motion) {
      this._wheel_collider.motorTorque = motion.Strength;
    }

    void FixedUpdate() { this.ApplyLocalPositionToVisuals(this._wheel_collider); }

    void ApplyLocalPositionToVisuals(WheelCollider col) {
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
