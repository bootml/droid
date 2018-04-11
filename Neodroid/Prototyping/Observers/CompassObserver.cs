using System;
using Neodroid.Utilities.Interfaces;
using Neodroid.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Prototyping.Observers {
  [AddComponentMenu(PrototypingComponentMenuPath._ComponentMenuPath+"Observers/Compass")]
  [ExecuteInEditMode]
  [Serializable]
  public class CompassObserver : Observer,
                                 IHasDouble {
    [SerializeField] Vector2 _2_d_position;

    [Header("Observation", order = 103)]
    [SerializeField]
    Vector3 _position;

    [SerializeField]
    Space3 _position_space = new Space3 {
        _Decimal_Granularity = 1,
        _Max_Values = Vector3.one,
        _Min_Values = -Vector3.one
    };

    [Header("Specfic", order = 102)]
    [SerializeField]
    Transform _target;

    public override string ObserverIdentifier { get { return this.name + "Compass"; } }

    public Vector3 Position {
      get { return this._position; }
      set {
        this._position =  this.NormaliseObservationUsingSpace ?  this._position_space.ClipNormaliseRound(value):value;
        this._2_d_position = new Vector2(this._position.x, this._position.z);
      }
    }

    public Vector2 ObservationValue {
      get { return this._2_d_position; }
      set { this._2_d_position = value; }
    }

    protected override void InnerSetup() { this.FloatEnumerable = new[] {this.Position.x, this.Position.z}; }

    public override void UpdateObservation() {
      this.Position = this.transform.InverseTransformVector(this.transform.position - this._target.position)
          .normalized;

      this.FloatEnumerable = new[] {this.Position.x, this.Position.z};
    }
  }
}
