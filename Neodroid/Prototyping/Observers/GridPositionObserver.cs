using Neodroid.Utilities.Interfaces;
using Neodroid.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Prototyping.Observers {
  [AddComponentMenu(PrototypingComponentMenuPath._ComponentMenuPath + "Observers/GridPosition")]
  public class GridPositionObserver : Observer,
                                      IHasSingle {
    int[,] _grid;
    [SerializeField] int _height;

    [Header("Observation", order = 103)]
    [SerializeField]
    float _observation_value;

    [SerializeField] ValueSpace _observation_value_space;
    [SerializeField] int _width;

    public override string Identifier { get { return this.name + "Value"; } }

    public float ObservationValue {
      get { return this._observation_value; }
      set {
        this._observation_value = this.NormaliseObservationUsingSpace
                                      ? this._observation_value_space.ClipNormaliseRound(value)
                                      : value;
      }
    }

    protected override void InnerSetup() {
      this._grid = new int[this._width, this._height];

      var k = 0;
      for (var i = 0; i < this._width; i++) {
        for (var j = 0; j < this._height; j++) this._grid[i, j] = k++;
      }

      this.FloatEnumerable = new[] {this.ObservationValue};
    }

    public override void UpdateObservation() {
      var x = this.transform.position.x + this._width;
      var z = this.transform.position.z + this._height;

      this.ObservationValue = this._grid[(int)x, (int)z];

      this.FloatEnumerable = new[] {this.ObservationValue};
    }
  }
}
