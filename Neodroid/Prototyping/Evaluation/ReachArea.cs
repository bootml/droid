using Neodroid.Prototyping.Actors;
using Neodroid.Prototyping.Observers;
using Neodroid.Utilities;
using Neodroid.Utilities.BoundingBoxes;
using UnityEngine;

namespace Neodroid.Prototyping.Evaluation {
  enum ActorOverlapping {
    Inside_area_,
    Outside_area_
  }

  enum ActorColliding {
    Not_colliding_,
    Colliding_
  }

  //[RequireComponent (typeof(BoundingBox))]
  //[RequireComponent (typeof(BoxCollider))]
  [AddComponentMenu(PrototypingComponentMenuPath._ComponentMenuPath + "Evaluation/ReachArea")]
  public class ReachArea : ObjectiveFunction {
    [SerializeField] Collider _actor;

    [SerializeField] Collider _area;

    [SerializeField] bool _based_on_tags;
    [SerializeField] ActorColliding _colliding = ActorColliding.Not_colliding_;

    [SerializeField] Obstruction[] _obstructions;

    //Used for.. if outside playable area then reset
    [SerializeField] ActorOverlapping _overlapping = ActorOverlapping.Outside_area_;

    [SerializeField] BoundingBox _playable_area;

    public override float InternalEvaluate() {
      /*var regularising_term = 0f;

            foreach (var ob in _obstructions) {
              RaycastHit ray_hit;
              Physics.Raycast (_actor.transform.position, (ob.transform.position - _actor.transform.position).normalized, out ray_hit, LayerMask.NameToLayer ("Obstruction"));
              regularising_term += -Mathf.Abs (Vector3.Distance (ray_hit.point, _actor.transform.position));
              //regularising_term += -Mathf.Abs (Vector3.Distance (ob.transform.position, _actor.transform.position));
            }

            reward += 0.2 * regularising_term;*/

      //reward += 1 / Mathf.Abs (Vector3.Distance (_area.transform.position, _actor.transform.position)); // Inversely porpotional to the absolute distance, closer higher reward

      if (this._overlapping == ActorOverlapping.Inside_area_) {
        this.ParentEnvironment.Terminate("Inside goal area");
        return 1f;
      }

      if (this._colliding == ActorColliding.Colliding_)
        this.ParentEnvironment.Terminate("Actor colliding with obstruction");
      if (this._playable_area && this._actor) {
        if (!this._playable_area._Bounds.Intersects(this._actor.GetComponent<Collider>().bounds))
          this.ParentEnvironment.Terminate("Actor is outside playable area");
      }

      return 0f;
    }

    void Start() {
      if (!this._area)
        this._area = FindObjectOfType<Observer>().gameObject.GetComponent<Collider>();
      if (!this._actor)
        this._actor = FindObjectOfType<Actor>().gameObject.GetComponent<Collider>();
      if (this._obstructions.Length <= 0)
        this._obstructions = FindObjectsOfType<Obstruction>();
      if (!this._playable_area)
        this._playable_area = FindObjectOfType<BoundingBox>();

      NeodroidUtilities.RegisterCollisionTriggerCallbacksOnChildren(
          this,
          this._area.transform,
          this.OnCollisionEnterChild,
          this.OnTriggerEnterChild,
          this.OnCollisionExitChild,
          this.OnTriggerExitChild,
          this.OnCollisionStayChild,
          this.OnTriggerStayChild,
          this.Debugging);

      NeodroidUtilities.RegisterCollisionTriggerCallbacksOnChildren(
          this,
          this._actor.transform,
          this.OnCollisionEnterChild,
          this.OnTriggerEnterChild,
          this.OnCollisionExitChild,
          this.OnTriggerExitChild,
          this.OnCollisionStayChild,
          this.OnTriggerStayChild,
          this.Debugging);
    }

    void OnTriggerEnterChild(GameObject child_game_object, Collider other_game_object) {
      if (this._actor) {
        if (this._based_on_tags) {
          if (child_game_object.tag == this._area.tag && other_game_object.tag == this._actor.tag) {
            if (this.Debugging)
              Debug.Log("Actor is inside area");
            this._overlapping = ActorOverlapping.Inside_area_;
          }

          if (child_game_object.tag == this._actor.tag && other_game_object.tag == "Obstruction") {
            if (this.Debugging)
              Debug.Log("Actor is colliding");
            this._colliding = ActorColliding.Colliding_;
          }
        } else {
          if (child_game_object == this._area.gameObject
              && other_game_object.gameObject == this._actor.gameObject) {
            if (this.Debugging)
              Debug.Log("Actor is inside area");
            this._overlapping = ActorOverlapping.Inside_area_;
          }

          if (child_game_object == this._actor.gameObject && other_game_object.tag == "Obstruction") {
            if (this.Debugging)
              Debug.Log("Actor is colliding");
            this._colliding = ActorColliding.Colliding_;
          }
        }
      }
    }

    void OnTriggerStayChild(GameObject child_game_object, Collider other_game_object) {
      if (this._actor) {
        if (this._based_on_tags) {
          if (child_game_object.tag == this._area.tag && other_game_object.tag == this._actor.tag) {
            if (this.Debugging)
              Debug.Log("Actor is inside area");
            this._overlapping = ActorOverlapping.Inside_area_;
          }

          if (child_game_object.tag == this._actor.tag && other_game_object.tag == "Obstruction") {
            if (this.Debugging)
              Debug.Log("Actor is colliding");
            this._colliding = ActorColliding.Colliding_;
          }
        } else {
          if (child_game_object == this._area.gameObject
              && other_game_object.gameObject == this._actor.gameObject) {
            if (this.Debugging)
              Debug.Log("Actor is inside area");
            this._overlapping = ActorOverlapping.Inside_area_;
          }

          if (child_game_object == this._actor.gameObject && other_game_object.tag == "Obstruction") {
            if (this.Debugging)
              Debug.Log("Actor is colliding");
            this._colliding = ActorColliding.Colliding_;
          }
        }
      }
    }

    void OnTriggerExitChild(GameObject child_game_object, Collider other_game_object) {
      if (this._actor) {
        if (this._based_on_tags) {
          if (child_game_object.tag == this._area.tag && other_game_object.tag == this._actor.tag) {
            if (this.Debugging)
              Debug.Log("Actor is outside area");
            this._overlapping = ActorOverlapping.Outside_area_;
          }

          if (child_game_object.tag == this._actor.tag && other_game_object.tag == "Obstruction") {
            if (this.Debugging)
              Debug.Log("Actor is not colliding");
            this._colliding = ActorColliding.Not_colliding_;
          }
        } else {
          if (child_game_object == this._area.gameObject
              && other_game_object.gameObject == this._actor.gameObject) {
            if (this.Debugging)
              Debug.Log("Actor is outside area");
            this._overlapping = ActorOverlapping.Outside_area_;
          }

          if (child_game_object == this._actor.gameObject && other_game_object.tag == "Obstruction") {
            if (this.Debugging)
              Debug.Log("Actor is not colliding");
            this._colliding = ActorColliding.Not_colliding_;
          }
        }
      }
    }

    void OnCollisionEnterChild(GameObject child_game_object, Collision collision) { }

    void OnCollisionStayChild(GameObject child_game_object, Collision collision) { }

    void OnCollisionExitChild(GameObject child_game_object, Collision collision) { }
  }
}
