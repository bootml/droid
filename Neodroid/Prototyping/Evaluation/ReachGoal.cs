﻿using Neodroid.Prototyping.Actors;
using Neodroid.Utilities;
using UnityEngine;

namespace Neodroid.Prototyping.Evaluation {
  [AddComponentMenu(PrototypingComponentMenuPath._ComponentMenuPath+"Evaluation/ReachGoal")]
  public class ReachGoal : ObjectiveFunction {
    [SerializeField] Actor _actor;

    [SerializeField] bool _based_on_tags;

    [SerializeField] EmptyCell _goal;

    //Used for.. if outside playable area then reset
    [SerializeField] ActorOverlapping _overlapping = ActorOverlapping.Outside_area_;

    public override float InternalEvaluate() {
      var distance = Mathf.Abs(
          Vector3.Distance(this._goal.transform.position, this._actor.transform.position));

      if (this._overlapping == ActorOverlapping.Inside_area_ || distance < 0.5f) {
        this.ParentEnvironment.Terminate("Inside goal area");
        return 1f;
      }

      return 0f;
    }

    public override void InternalReset() {
      this.Setup();
      this._overlapping = ActorOverlapping.Outside_area_;
    }

    public void SetGoal(EmptyCell goal) {
      this._goal = goal;
      this.InternalReset();
    }

    void Setup() {
      if (!this._goal)
        this._goal = FindObjectOfType<EmptyCell>();
      if (!this._actor)
        this._actor = FindObjectOfType<Actor>();
      if (this._goal) {
        NeodroidUtilities.RegisterCollisionTriggerCallbacksOnChildren(
            this,
            this._goal.transform,
            null,
            this.OnTriggerEnterChild,
            debug : this.Debugging);
      }

      if (this._actor) {
        NeodroidUtilities.RegisterCollisionTriggerCallbacksOnChildren(
            this,
            this._actor.transform,
            null,
            this.OnTriggerEnterChild,
            debug : this.Debugging);
      }
    }

    void OnTriggerEnterChild(GameObject child_game_object, Collider other_game_object) {
      print("triggered");
      if (this._actor) {
        if (this._based_on_tags) {
          if (other_game_object.CompareTag(this._actor.tag)) {
            if (this.Debugging)
              Debug.Log("Actor is inside area");
            this._overlapping = ActorOverlapping.Inside_area_;
          }
        } else {
          if (child_game_object == this._goal.gameObject
              && other_game_object.gameObject == this._actor.gameObject) {
            if (this.Debugging)
              Debug.Log("Actor is inside area");
            this._overlapping = ActorOverlapping.Inside_area_;
          }
        }
      }
    }

    enum ActorOverlapping {
      Inside_area_,
      Outside_area_
    }
  }
}
