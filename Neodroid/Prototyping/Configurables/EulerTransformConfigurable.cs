﻿using System;
using Neodroid.Messaging.Messages;
using Neodroid.Utilities;
using Neodroid.Utilities.Interfaces;
using UnityEngine;

namespace Neodroid.Prototyping.Configurables {
  [AddComponentMenu(PrototypingComponentMenuPath._ComponentMenuPath + "Configurables/EulerTransform")]
  public class EulerTransformConfigurable : ConfigurableGameObject,
                                            IHasEulerTransform {
    string _dir_x;
    string _dir_y;
    string _dir_z;

    [SerializeField] Vector3 _direction;

    [Header("Observation", order = 103)]
    [SerializeField]
    Vector3 _position;

    string _rot_x;
    string _rot_y;
    string _rot_z;

    [SerializeField] Vector3 _rotation;

    [SerializeField] bool _use_environments_space;

    string _x;
    string _y;
    string _z;

    public override string Identifier { get { return this.name + "EulerTransform"; } }

    public Vector3 Position { get { return this._position; } set { this._position = value; } }

    public Vector3 Direction { get { return this._direction; } set { this._direction = value; } }

    public Vector3 Rotation { get { return this._rotation; } set { this._rotation = value; } }

    public override void UpdateObservation() {
      if (this._use_environments_space) {
        this.Position = this.ParentEnvironment.TransformPosition(this.transform.position);
        this.Direction = this.ParentEnvironment.TransformDirection(this.transform.forward);
        this.Rotation = this.ParentEnvironment.TransformDirection(this.transform.up);
      } else {
        this.Position = this.transform.position;
        this.Direction = this.transform.forward;
        this.Rotation = this.transform.up;
      }
    }

    protected override void AddToEnvironment() {
      this._x = this.Identifier + "X";
      this._y = this.Identifier + "Y";
      this._z = this.Identifier + "Z";
      this._dir_x = this.Identifier + "DirX";
      this._dir_y = this.Identifier + "DirY";
      this._dir_z = this.Identifier + "DirZ";
      this._rot_x = this.Identifier + "RotX";
      this._rot_y = this.Identifier + "RotY";
      this._rot_z = this.Identifier + "RotZ";
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
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._dir_x);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._dir_y);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._dir_z);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._rot_x);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._rot_y);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._rot_z);
    }

    public override void ApplyConfiguration(Configuration configuration) {
      var pos = this.transform.position;
      var dir = this.transform.forward;
      var rot = this.transform.up;
      if (this._use_environments_space) {
        pos = this.ParentEnvironment.TransformPosition(pos);
        dir = this.ParentEnvironment.TransformDirection(dir);
        rot = this.ParentEnvironment.TransformDirection(rot);
      }

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
        print("Applying " + v + " To " + this.Identifier);
      if (this.RelativeToExistingValue) {
        if (configuration.ConfigurableName == this._x)
          pos.Set(v - pos.x, pos.y, pos.z);
        else if (configuration.ConfigurableName == this._y)
          pos.Set(pos.x, v - pos.y, pos.z);
        else if (configuration.ConfigurableName == this._z)
          pos.Set(pos.x, pos.y, v - pos.z);
        else if (configuration.ConfigurableName == this._dir_x)
          dir.Set(v - dir.x, dir.y, dir.z);
        else if (configuration.ConfigurableName == this._dir_y)
          dir.Set(dir.x, v - dir.y, dir.z);
        else if (configuration.ConfigurableName == this._dir_z)
          dir.Set(dir.x, dir.y, v - dir.z);
        else if (configuration.ConfigurableName == this._rot_x)
          rot.Set(v - rot.x, rot.y, rot.z);
        else if (configuration.ConfigurableName == this._rot_y)
          rot.Set(rot.x, v - rot.y, rot.z);
        else if (configuration.ConfigurableName == this._rot_z)
          rot.Set(rot.x, rot.y, v - rot.z);
      } else {
        if (configuration.ConfigurableName == this._x)
          pos.Set(v, pos.y, pos.z);
        else if (configuration.ConfigurableName == this._y)
          pos.Set(pos.x, v, pos.z);
        else if (configuration.ConfigurableName == this._z)
          pos.Set(pos.x, pos.y, v);
        else if (configuration.ConfigurableName == this._dir_x)
          dir.Set(v, dir.y, dir.z);
        else if (configuration.ConfigurableName == this._dir_y)
          dir.Set(dir.x, v, dir.z);
        else if (configuration.ConfigurableName == this._dir_z)
          dir.Set(dir.x, dir.y, v);
        else if (configuration.ConfigurableName == this._rot_x)
          rot.Set(v, rot.y, rot.z);
        else if (configuration.ConfigurableName == this._rot_y)
          rot.Set(rot.x, v, rot.z);
        else if (configuration.ConfigurableName == this._rot_z)
          rot.Set(rot.x, rot.y, v);
      }

      var inv_pos = pos;
      var inv_dir = dir;
      var inv_rot = rot;
      if (this._use_environments_space) {
        inv_pos = this.ParentEnvironment.InverseTransformPosition(pos);
        inv_dir = this.ParentEnvironment.InverseTransformDirection(dir);
        inv_rot = this.ParentEnvironment.InverseTransformDirection(rot);
      }

      this.transform.position = inv_pos;
      this.transform.rotation = Quaternion.identity;
      this.transform.rotation = Quaternion.LookRotation(inv_dir, inv_rot);
    }
  }
}
