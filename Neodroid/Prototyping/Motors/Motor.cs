using System;
using Neodroid.Messaging.Messages;
using Neodroid.Prototyping.Actors;
using Neodroid.Utilities;
using Neodroid.Utilities.Interfaces;
using Neodroid.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Prototyping.Motors {
  //[AddComponentMenu(PrototypingComponentMenuPath._ComponentMenuPath+"Motors/General/Motor")]
  [ExecuteInEditMode]
  [Serializable]
  public class Motor : MonoBehaviour,
                       IRegisterable {
    public Actor ParentActor { get { return this._actor; } set { this._actor = value; } }

    public float EnergySpendSinceReset {
      get { return this._energy_spend_since_reset; }
      set { this._energy_spend_since_reset = value; }
    }

    public float EnergyCost { get { return this._energy_cost; } set { this._energy_cost = value; } }

    public ValueSpace MotionValueSpace {
      get { return this._motion_value_space; }
      set { this._motion_value_space = value; }
    }

    public bool Debugging { get { return this._debugging; } set { this._debugging = value; } }

    public virtual String Identifier { get { return this.name + "Motor"; } }

    protected virtual void Awake() {
      this.RegisterComponent();
      this.Setup();
    }

    public virtual void RegisterComponent() {
      this._actor = NeodroidUtilities.MaybeRegisterComponent(this._actor, this);
    }

    #if UNITY_EDITOR
    void OnValidate() {
      // Only called in the editor
      //RegisterComponent ();
    }
                #endif

    protected virtual void Start() { this.Setup(); }

    protected virtual void Setup() { }

    public void RefreshAwake() { this.Awake(); }

    public void RefreshStart() { this.Start(); }

    public void ApplyMotion(MotorMotion motion) {
      if (this.Debugging)
        print("Applying " + motion + " To " + this.name);
      if (motion.Strength < this.MotionValueSpace._Min_Value
          || motion.Strength > this.MotionValueSpace._Max_Value) {
        print(
            $"It does not accept input {motion.Strength}, outside allowed range {this.MotionValueSpace._Min_Value} to {this.MotionValueSpace._Max_Value}");
        return; // Do nothing
      }

      this.InnerApplyMotion(motion);
      this.EnergySpendSinceReset += Mathf.Abs(this.EnergyCost * motion.Strength);
    }

    protected virtual void InnerApplyMotion(MotorMotion motion) { }

    public virtual float GetEnergySpend() { return this._energy_spend_since_reset; }

    public override string ToString() { return this.Identifier; }

    public virtual void Reset() { this._energy_spend_since_reset = 0; }

    #region Fields

    [Header("References", order = 99)]
    [SerializeField]
    Actor _actor;

    [Header("Development", order = 100)]
    [SerializeField]
    bool _debugging;

    [Header("General", order = 101)]
    [SerializeField]
    ValueSpace _motion_value_space =
        new ValueSpace {_Decimal_Granularity = 0, _Min_Value = -10, _Max_Value = 10};

    [SerializeField] float _energy_spend_since_reset;

    [SerializeField] float _energy_cost;

    #endregion
  }
}
