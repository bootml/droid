using System;
using Neodroid.Messaging.Messages;
using Neodroid.Utilities;
using Neodroid.Utilities.Interfaces;
using UnityEngine;

namespace Neodroid.Prototyping.Configurables {
  [AddComponentMenu(PrototypingComponentMenuPath._ComponentMenuPath + "Configurables/Position")]
  public class PositionConfigurable : ConfigurableGameObject,
                                      IHasTriple {
    [Header("Observation", order = 103)]
    [SerializeField]
    Vector3 _position;

    [SerializeField] bool _use_environments_space;

    string _x;
    string _y;
    string _z;

    public override string Identifier { get { return this.name + "Position"; } }

    public Vector3 ObservationValue { get { return this._position; } }

    protected override void AddToEnvironment() {
      this._x = this.Identifier + "X";
      this._y = this.Identifier + "Y";
      this._z = this.Identifier + "Z";
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._x);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._y);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._z);
    }

    public override void UpdateObservation() {
      if (this._use_environments_space)
        this._position = this.ParentEnvironment.TransformPosition(this.transform.position);
      else
        this._position = this.transform.position;
    }

    public override void ApplyConfiguration(Configuration configuration) {
      var pos = this.transform.position;
      if (this._use_environments_space)
        pos = this.ParentEnvironment.TransformPosition(this.transform.position);
      var v = configuration.ConfigurableValue;
      if (this.ConfigurableValueSpace._Decimal_Granularity >= 0)
        v = (int)Math.Round(v, this.ConfigurableValueSpace._Decimal_Granularity);
      if (this.ConfigurableValueSpace._Min_Value.CompareTo(this.ConfigurableValueSpace._Max_Value) != 0) {
        if (v < this.ConfigurableValueSpace._Min_Value || v > this.ConfigurableValueSpace._Max_Value) {
          print(
              string.Format(
                  "Configurable does not accept input{2}, outside allowed range {0} to {1}",
                  this.ConfigurableValueSpace._Min_Value,
                  this.ConfigurableValueSpace._Max_Value,
                  v));
          return; // Do nothing
        }
      }

      if (this.Debugging)
        print($"Applying {v} to {configuration.ConfigurableName} configurable");
      if (this.RelativeToExistingValue) {
        if (configuration.ConfigurableName == this._x)
          pos.Set(v - pos.x, pos.y, pos.z);
        else if (configuration.ConfigurableName == this._y)
          pos.Set(pos.x, v - pos.y, pos.z);
        else if (configuration.ConfigurableName == this._z)
          pos.Set(pos.x, pos.y, v - pos.z);
      } else {
        if (configuration.ConfigurableName == this._x)
          pos.Set(v, pos.y, pos.z);
        else if (configuration.ConfigurableName == this._y)
          pos.Set(pos.x, v, pos.z);
        else if (configuration.ConfigurableName == this._z)
          pos.Set(pos.x, pos.y, v);
      }

      var inv_pos = pos;
      if (this._use_environments_space)
        inv_pos = this.ParentEnvironment.InverseTransformPosition(inv_pos);
      this.transform.position = inv_pos;
    }
  }
}
