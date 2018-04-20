using System;
using Neodroid.Utilities.Interfaces;
using UnityEngine;

namespace Neodroid.Prototyping.Observers {
  [AddComponentMenu(PrototypingComponentMenuPath._ComponentMenuPath + "Observers/LineOfSight")]
  [ExecuteInEditMode]
  [Serializable]
  public class LineOfSightObserver : Observer,
                                     IHasSingle {
    RaycastHit _hit;

    [SerializeField] float _obs_value;

    [Header("Specfic", order = 102)]
    [SerializeField]
    Transform _target;

    public override string Identifier { get { return this.name + "LineOfSight"; } }

    public Single ObservationValue {
      get { return this._obs_value; }
      private set { this._obs_value = value; }
    }

    protected override void InnerSetup() { this.FloatEnumerable = new[] {this.ObservationValue}; }

    public override void UpdateObservation() {
      var distance = Vector3.Distance(this.transform.position, this._target.position);
      if (Physics.Raycast(
          this.transform.position,
          this._target.position - this.transform.position,
          out this._hit,
          distance)) {
        if (this.Debugging)
          print(this._hit.distance);
        if (this._hit.collider.gameObject != this._target.gameObject)
          this.ObservationValue = 0;
        else
          this.ObservationValue = 1;
      } else
        this.ObservationValue = 1;

      this.FloatEnumerable = new[] {this.ObservationValue};
    }
  }
}
