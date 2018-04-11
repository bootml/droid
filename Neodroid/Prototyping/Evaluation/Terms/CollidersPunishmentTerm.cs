using UnityEngine;

namespace Neodroid.Prototyping.Evaluation.Terms {
  [AddComponentMenu(PrototypingComponentMenuPath._ComponentMenuPath+"Evaluation/Terms/CollidersPunishment")]
  public class CollidersPunishmentTerm : Term {
    [SerializeField] Collider[] _as;

    [SerializeField] Collider _b;

    [SerializeField] bool _debugging;

    public override float Evaluate() {
      foreach (var a in this._as) {
        if (a.bounds.Intersects(this._b.bounds))
          return -1;
      }

      return 0;
    }
  }
}
