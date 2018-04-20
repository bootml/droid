using System;
using UnityEngine;

namespace Neodroid.Prototyping.Evaluation.Terms {
  [AddComponentMenu(PrototypingComponentMenuPath._ComponentMenuPath + "Evaluation/Terms/CollisionPunishment")]
  public class CollsionsPunishmentTerm : Term {
    [SerializeField] Collider _a;

    [SerializeField] Collider _b;

    public override float Evaluate() {
      if (this._a.bounds.Intersects(this._b.bounds))
        return -1;
      return 0;
    }

    public override String Identifier { get { return this.name + "CollisionPunishment"; } }
  }
}
