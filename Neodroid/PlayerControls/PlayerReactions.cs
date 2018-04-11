using System.Collections.Generic;
using Neodroid.Managers;
using Neodroid.Messaging.Messages;
using Neodroid.Utilities.ScriptableObjects;
using UnityEngine;

namespace Neodroid.PlayerControls {
  public class PlayerReactions : MonoBehaviour {
    [SerializeField] NeodroidManager _manager;
    [SerializeField] PlayerMotions _player_motions;

    [SerializeField] bool _debugging;

    void Start() { this._manager = FindObjectOfType<NeodroidManager>(); }

    void Update() {
      if (this._player_motions != null) {
        var motions = new List<MotorMotion>();
        foreach (var player_motion in this._player_motions._Motions) {
          if (Input.GetKey(player_motion._Key)) {
            if (this._debugging) {
              print($"{player_motion._Actor} {player_motion._Motor} {player_motion._Strength}");
            }

            var motion = new MotorMotion(player_motion._Actor, player_motion._Motor, player_motion._Strength);
            motions.Add(motion);
          }
        }

        var step = motions.Count > 0;
        var parameters = new ReactionParameters(true, step) {IsExternal = false};
        var reaction = new Reaction(parameters, motions.ToArray(), null, null, null, "");
        this._manager.React(reaction);
      } else {
        if (this._debugging)
          print("No playermotions scriptable object assigned");
      }
    }
  }
}
