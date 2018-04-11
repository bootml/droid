using Neodroid.Prototyping.Actors;
using Neodroid.Utilities;
using Neodroid.Utilities.BoundingBoxes;
using UnityEngine;

namespace Neodroid.Prototyping.Evaluation {
  [AddComponentMenu(PrototypingComponentMenuPath._ComponentMenuPath+"Evaluation/PoseDeviance")]
  public class PoseDeviance : ObjectiveFunction {
    [SerializeField]  float _goal_reward = 1.0f;
    [SerializeField] float _default_reward;

    public override float InternalEvaluate() {
      var reward = this._default_reward;

      if (this._playable_area != null && !this._playable_area._Bounds.Intersects(this._actor.ActorBounds)) {
        if (this.Debugging)
          print("Outside playable area");
        this.ParentEnvironment.Terminate("Outside playable area");
      }

      var distance = Mathf.Abs(
          Vector3.Distance(this._goal.transform.position, this._actor.transform.position));
      var angle = Quaternion.Angle(this._goal.transform.rotation, this._actor.transform.rotation);

      if (!this._sparse) {
        reward += 1 / Mathf.Abs(distance + 1);
        reward += 1 / Mathf.Abs(angle + 1);
        if (this._state_full) {
          if (reward <= this._peak_reward)
            reward = 0.0f;
          else
            this._peak_reward = reward;
        }
      }

      if (distance < 0.5) {
        if (this.Debugging)
          print("Within range of goal");
        reward = this._goal_reward;
        this.ParentEnvironment.Terminate("Within range of goal");
      }

      return reward;
    }

    public override void InternalReset() { this._peak_reward = 0.0f; }

    void Start() {
      if (!this._goal)
        this._goal = FindObjectOfType<Transform>();
      if (!this._actor)
        this._actor = FindObjectOfType<Actor>();

      if (this._obstructions.Length <= 0)
        this._obstructions = FindObjectsOfType<Obstruction>();
      if (!this._playable_area)
        this._playable_area = FindObjectOfType<BoundingBox>();
    }

    #region Fields

    [Header("Specific", order = 102)]
    [SerializeField]
    float _peak_reward;

    [SerializeField]  bool _sparse = true;

    [SerializeField] Transform _goal;

    [SerializeField] Actor _actor;

    [SerializeField] BoundingBox _playable_area;

    [SerializeField] Obstruction[] _obstructions;

    [SerializeField] bool _state_full;

    #endregion
  }
}
