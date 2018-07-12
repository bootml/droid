﻿using System;
using droid.Neodroid.Utilities.Enums;
using droid.Neodroid.Utilities.Interfaces;
using droid.Neodroid.Utilities.Structs;
using UnityEngine;

namespace droid.Neodroid.Prototyping.Observers.Transform {
  [AddComponentMenu(
       ObserverComponentMenuPath._ComponentMenuPath
       + "PositionObserver2D"
       + ObserverComponentMenuPath._Postfix), ExecuteInEditMode, Serializable]
  public class PositionObserver2D : Observer,
                                    IHasDouble {
    [Header("Specfic", order = 102), SerializeField]
    ObservationSpace _use_space = ObservationSpace.Environment_;

    [Header("Observation", order = 103), SerializeField]
    Vector2 _2_d_position;

    [SerializeField] Dimension2DCombination _dim_combination;

    [SerializeField] Space2 _position_space;

    public ObservationSpace UseSpace { get { return this._use_space; } }

    public override string PrototypingTypeName { get { return "DoublePosition"; } }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="position"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void SetPosition(Vector3 position) {
      Vector2 vector2_pos;
      switch (this._dim_combination) {
        case Dimension2DCombination.Xy_:
          vector2_pos = new Vector2(position.x, position.y);
          break;
        case Dimension2DCombination.Xz_:
          vector2_pos = new Vector2(position.x, position.z);
          break;
        case Dimension2DCombination.Yz_:
          vector2_pos = new Vector2(position.y, position.z);
          break;
        default: throw new ArgumentOutOfRangeException();
      }

      this._2_d_position = this.NormaliseObservation
                               ? this._position_space.ClipNormaliseRound(vector2_pos)
                               : vector2_pos;
    }

    public Vector2 ObservationValue { get { return this._2_d_position; } set { this._2_d_position = value; } }

    public Space2 ObservationSpace2D {
      get {
        return new Space2(this._position_space._Decimal_Granularity) {
            _Max_Values = new Vector2(this._position_space._Max_Values.x, this._position_space._Max_Values.y),
            _Min_Values = new Vector2(this._position_space._Min_Values.x, this._position_space._Min_Values.y)
        };
      }
    }

    public override void UpdateObservation() {
      if (this.ParentEnvironment && this._use_space == ObservationSpace.Environment_) {
        this.SetPosition(this.ParentEnvironment.TransformPosition(this.transform.position));
      } else if (this._use_space == ObservationSpace.Local_) {
        this.SetPosition(this.transform.localPosition);
      } else {
        this.SetPosition(this.transform.position);
      }

      this.FloatEnumerable = new[] {this._2_d_position.x, this._2_d_position.y};
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() {
      this.FloatEnumerable = new[] {this._2_d_position.x, this._2_d_position.y};
    }
  }
}
