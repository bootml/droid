using Neodroid.Utilities;
using Neodroid.Utilities.Interfaces;
using UnityEngine;

namespace Neodroid.Prototyping.Observers {
  [AddComponentMenu(PrototypingComponentMenuPath._ComponentMenuPath + "Observers/GoalCell")]
  public class GoalCellObserver : Observer,
                                  IHasTriple {
    [SerializeField] EmptyCell _current_goal;
    [SerializeField] Vector3 _current_goal_position;

    [SerializeField] bool _draw_names = true;

    [SerializeField] int _order_index;

    public int OrderIndex { get { return this._order_index; } set { this._order_index = value; } }

    public bool DrawNames { get { return this._draw_names; } set { this._draw_names = value; } }

    public override string Identifier { get { return this.name + "GoalObserver"; } }

    public EmptyCell CurrentGoal {
      get {
        this.UpdateObservation();
        return this._current_goal;
      }
      set { this._current_goal = value; }
    }

    public Vector3 ObservationValue {
      get { return this._current_goal_position; }
      private set { this._current_goal_position = value; }
    }

    public override void UpdateObservation() {
      this._current_goal_position = this._current_goal.transform.position;
    }

    #if UNITY_EDITOR
    void OnDrawGizmosSelected() {
      if (this.DrawNames) {
        if (this._current_goal) {
          NeodroidUtilities.DrawString(
              this._current_goal.name,
              this._current_goal.transform.position,
              Color.green);
        }
      }
    }
                #endif
  }
}
