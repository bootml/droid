﻿using Neodroid.Runtime.Prototyping.Actors;
using UnityEngine;

namespace Neodroid.Runtime.Prototyping.Evaluation {
  [AddComponentMenu(
      EvaluationComponentMenuPath._ComponentMenuPath
      + "EuclideanDistance"
      + EvaluationComponentMenuPath._Postfix)]
  class EuclideanDistance : ObjectiveFunction {
    [SerializeField] Transform _g1;
    [SerializeField] Transform _g2;

    public override void InternalReset() { }

    public override float InternalEvaluate() {
      return Vector3.Distance(this._g1.position, this._g2.position);
    }

    protected override void PostSetup() {
      if (this._g1 == null) {
        this._g1 = FindObjectOfType<Actor>().transform;
      }

      if (this._g2 == null) {
        this._g2 = this.transform;
      }
    }
  }
}