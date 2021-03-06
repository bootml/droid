﻿using System;
using Neodroid.Runtime.Environments;
using Neodroid.Runtime.Interfaces;
using Neodroid.Runtime.Messaging.Messages;
using Neodroid.Runtime.Utilities.Misc.Drawing;
using Neodroid.Runtime.Utilities.Misc.Grasping;
using Neodroid.Runtime.Utilities.Structs;
using UnityEngine;
using Random = System.Random;

namespace Neodroid.Runtime.Prototyping.Configurables {
  [AddComponentMenu(
      ConfigurableComponentMenuPath._ComponentMenuPath + "Position" + ConfigurableComponentMenuPath._Postfix)]
  public class PositionConfigurable : Configurable,
                                      IHasTriple {
    [Header("Observation", order = 103), SerializeField]
    Vector3 _position;

    [SerializeField] bool _use_environments_space;

    /// <summary>
    ///
    /// </summary>
    string _x;

    /// <summary>
    ///
    /// </summary>
    string _y;

    /// <summary>
    ///
    /// </summary>
    string _z;

    protected override void PreSetup() {
      this._x = this.Identifier + "X_";
      this._y = this.Identifier + "Y_";
      this._z = this.Identifier + "Z_";
    }

    /// <summary>
    ///
    /// </summary>
    public override string PrototypingTypeName {
      get { return "PositionConfigurable"; }
    }

    /// <summary>
    ///
    /// </summary>
    public Vector3 ObservationValue {
      get { return this._position; }
    }

    public Space3 TripleSpace { get; } = new Space3();

    /// <summary>
    ///
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          (Configurable)this);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          (Configurable)this,
          this._x);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          (Configurable)this,
          this._y);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          (Configurable)this,
          this._z);
    }

    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment == null) return;
      this.ParentEnvironment.UnRegister(this);
      this.ParentEnvironment.UnRegister(this, this._x);
      this.ParentEnvironment.UnRegister(this, this._y);
      this.ParentEnvironment.UnRegister(this, this._z);
    }

    public override void UpdateCurrentConfiguration() {
      if (this._use_environments_space) {
        this._position = this.ParentEnvironment.TransformPosition(this.transform.position);
      } else {
        this._position = this.transform.position;
      }
    }

    public override void ApplyConfiguration(IConfigurableConfiguration simulator_configuration) {
      var pos = this.transform.position;
      if (this._use_environments_space) {
        pos = this.ParentEnvironment.TransformPosition(this.transform.position);
      }

      var v = simulator_configuration.ConfigurableValue;
      if (this.TripleSpace._Decimal_Granularity >= 0) {
        v = (int)Math.Round(v, this.TripleSpace._Decimal_Granularity);
      }

      if (this.TripleSpace._Min_Values[0].CompareTo(this.TripleSpace._Max_Values[0]) != 0) {
        //TODO NOT IMPLEMENTED CORRECTLY VelocitySpace should not be index but should check all pairwise values, TripleSpace._Min_Values == TripleSpace._Max_Values
        if (v < this.TripleSpace._Min_Values[0] || v > this.TripleSpace._Max_Values[0]) {
          Debug.Log(
              $"Configurable does not accept input{v}, outside allowed range {this.TripleSpace._Min_Values[0]} to {this.TripleSpace._Max_Values[0]}");
          return; // Do nothing
        }
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log($"Applying {v} to {simulator_configuration.ConfigurableName} configurable");
      }
      #endif

      if (this.RelativeToExistingValue) {
        if (simulator_configuration.ConfigurableName == this._x) {
          pos.Set(v - pos.x, pos.y, pos.z);
        } else if (simulator_configuration.ConfigurableName == this._y) {
          pos.Set(pos.x, v - pos.y, pos.z);
        } else if (simulator_configuration.ConfigurableName == this._z) {
          pos.Set(pos.x, pos.y, v - pos.z);
        }
      } else {
        if (simulator_configuration.ConfigurableName == this._x) {
          pos.Set(v, pos.y, pos.z);
        } else if (simulator_configuration.ConfigurableName == this._y) {
          pos.Set(pos.x, v, pos.z);
        } else if (simulator_configuration.ConfigurableName == this._z) {
          pos.Set(pos.x, pos.y, v);
        }
      }

      var inv_pos = pos;
      if (this._use_environments_space) {
        inv_pos = this.ParentEnvironment.InverseTransformPosition(inv_pos);
      }

      this.transform.position = inv_pos;
    }

    public override IConfigurableConfiguration SampleConfiguration(Random random_generator) {
      var random_vector3 = this.TripleSpace.RandomVector3();

      return new Configuration(this._x, random_vector3.x);
    }
  }
}