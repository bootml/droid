using Neodroid.Environments;
using UnityEngine;

namespace Neodroid.Prototyping.Evaluation.Terms {
  [AddComponentMenu(PrototypingComponentMenuPath._ComponentMenuPath+"Evaluation/Terms/Rush")]
  public class RushTerm : Term {
    [SerializeField]  float _penalty_size = 0.01f;
    [SerializeField] PrototypingEnvironment _env;

    void Awake() {
      if (!this._env) this._env = FindObjectOfType<PrototypingEnvironment>();
    }

    public override float Evaluate() {
      if (this._env) return -(1f / this._env.EpisodeLength);

      return -this._penalty_size;
    }
  }
}
