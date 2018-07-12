﻿using droid.Neodroid.Utilities.Interfaces;
using droid.Neodroid.Utilities.Structs;
using UnityEngine;

namespace droid.Neodroid.Prototyping.Observers {
  [AddComponentMenu(
       ObserverComponentMenuPath._ComponentMenuPath + "Value" + ObserverComponentMenuPath._Postfix),
   ExecuteInEditMode]
  public class ValueObserver : Observer,
                               IHasSingle {
    [Header("Observation", order = 103), SerializeField]
    float _observation_value;

    [SerializeField] ValueSpace _observation_value_space;

    public ValueSpace SingleSpace { get { return this._observation_value_space; } }

    public override string PrototypingTypeName { get { return "Value"; } }

    public float ObservationValue {
      get { return this._observation_value; }
      set {
        this._observation_value = this.NormaliseObservation
                                      ? this._observation_value_space.ClipNormaliseRound(value)
                                      : value;
      }
    }

    protected override void PreSetup() { this.FloatEnumerable = new[] {this.ObservationValue}; }

    public override void UpdateObservation() { this.FloatEnumerable = new[] {this.ObservationValue}; }
  }
}
