using System;
using Neodroid.Messaging.Messages;
using Neodroid.Utilities.Enums;
using UnityEngine;

namespace Neodroid.Prototyping.Motors.Particles {
  [AddComponentMenu(PrototypingComponentMenuPath._ComponentMenuPath+"Motors/Particles/Rocket")]
  [RequireComponent(typeof(ParticleSystem))]
  [RequireComponent(typeof(Rigidbody))]
  public class RocketMotor : SingleAxisRigidbodyMotor {
    [SerializeField] bool _fired_this_step;

    [SerializeField] protected ParticleSystem _Particle_System;

    public override String MotorIdentifier { get { return this.name + "Rocket" + this._Axis_Of_Motion; } }

    protected override void Start() {
      this._Rigidbody = this.GetComponent<Rigidbody>();
      this._Particle_System = this.GetComponent<ParticleSystem>();
      var valid_input = this.MotionValueSpace;
      valid_input._Min_Value = 0;
      this.MotionValueSpace = valid_input;
      this.RegisterComponent();
    }

    void LateUpdate() {
      if (this._fired_this_step) {
        if (!this._Particle_System.isPlaying)
          this._Particle_System.Play(true);
      } else
        this._Particle_System.Stop(true);

      this._fired_this_step = false;
    }

    protected override void InnerApplyMotion(MotorMotion motion) {
      if (motion.Strength < this.MotionValueSpace._Min_Value
          || motion.Strength > this.MotionValueSpace._Max_Value) {
        print(
            $"It does not accept input {motion.Strength}, outside allowed range {this.MotionValueSpace._Min_Value} to {this.MotionValueSpace._Max_Value}");
        return; // Do nothing
      }

      switch (this._Axis_Of_Motion) {
        case Axis.X_:
          if (this._Relative_To == Space.World)
            this._Rigidbody.AddForce(Vector3.left * motion.Strength);
          else
            this._Rigidbody.AddRelativeForce(Vector3.left * motion.Strength);
          break;
        case Axis.Y_:
          if (this._Relative_To == Space.World)
            this._Rigidbody.AddForce(Vector3.up * motion.Strength);
          else
            this._Rigidbody.AddRelativeForce(Vector3.up * motion.Strength);
          break;
        case Axis.Z_:
          if (this._Relative_To == Space.World)
            this._Rigidbody.AddForce(Vector3.forward * motion.Strength);
          else
            this._Rigidbody.AddRelativeForce(Vector3.up * motion.Strength);
          break;
        case Axis.Rot_x_:
          if (this._Relative_To == Space.World)
            this._Rigidbody.AddTorque(Vector3.left * motion.Strength);
          else
            this._Rigidbody.AddRelativeTorque(Vector3.left * motion.Strength);
          break;
        case Axis.Rot_y_:
          if (this._Relative_To == Space.World)
            this._Rigidbody.AddTorque(Vector3.up * motion.Strength);
          else
            this._Rigidbody.AddRelativeTorque(Vector3.up * motion.Strength);
          break;
        case Axis.Rot_z_:
          if (this._Relative_To == Space.World)
            this._Rigidbody.AddTorque(Vector3.forward * motion.Strength);
          else
            this._Rigidbody.AddRelativeTorque(Vector3.forward * motion.Strength);
          break;
        default:
          break;
      }

      this._fired_this_step = true;
    }
  }
}
