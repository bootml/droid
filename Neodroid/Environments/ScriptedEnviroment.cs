using System;
using System.Collections.Generic;
using Neodroid.Managers;
using Neodroid.Messaging.Messages;
using Neodroid.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Neodroid.Environments {
  [AddComponentMenu("Neodroid/Environments/Scripted")]
  public class ScriptedEnviroment : NeodroidEnvironment {
    [SerializeField] Renderer _actor_renderer;

    [SerializeField] int _actor_x;

    [SerializeField] int _actor_y;

    [SerializeField] Renderer _goal_renderer;

    [SerializeField] int _goal_x;

    [SerializeField] int _goal_y;

    int[,] _grid;

    [SerializeField] int _height;

    [SerializeField] NeodroidManager _time_simulation_manager;
    [SerializeField] int _width;

    public override string Identifier { get { return this.name; } }

    public Int32 ActorX {
      get { return this._actor_x; }
      set { this._actor_x = Mathf.Max(0, Mathf.Min(this._width - 1, value)); }
    }

    public Int32 ActorY {
      get { return this._actor_y; }
      set { this._actor_y = Mathf.Max(0, Mathf.Min(this._height - 1, value)); }
    }

    public Int32 GoalX {
      get { return this._goal_x; }
      set { this._goal_x = Mathf.Max(0, Mathf.Min(this._width - 1, value)); }
    }

    public Int32 GoalY {
      get { return this._goal_y; }
      set { this._goal_y = Mathf.Max(0, Mathf.Min(this._height - 1, value)); }
    }

    void Start() {
      this._grid = new Int32[this._width, this._height];

      var k = 0;
      for (var i = 0; i < this._width; i++) {
        for (var j = 0; j < this._height; j++) this._grid[i, j] = k++;
      }

      this._time_simulation_manager = NeodroidUtilities.MaybeRegisterComponent(
          this._time_simulation_manager,
          (NeodroidEnvironment)this);
    }

    public override void PostStep() {
      if (this._goal_renderer)
        this._goal_renderer.transform.position = new Vector3(this.GoalX, 0, this.GoalY);

      if (this._actor_renderer)
        this._actor_renderer.transform.position = new Vector3(this.ActorX, 0, this.ActorY);
    }

    public override Reaction SampleReaction() {
      var motions = new List<MotorMotion>();

      var strength = Random.Range(0, 4);
      motions.Add(new MotorMotion("", "", strength));

      var rp = new ReactionParameters(true, true) {IsExternal = false};
      return new Reaction(rp, motions.ToArray(), null, null, null,"");
    }

    public override EnvironmentState React(Reaction reaction) {
      foreach (var motion in reaction.Motions) {
        switch ((int)motion.Strength) {
          case 0:
            this.ActorY += 1;
            break;
          case 1:
            this.ActorX += 1;
            break;
          case 2:
            this.ActorY -= 1;
            break;
          case 3:
            this.ActorX -= 1;
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }

      var actor_idx = this._grid[this.ActorX, this.ActorY];
      var goal_idx = this._grid[this.GoalX, this.GoalY];

      var terminated = actor_idx == goal_idx;
      var signal = terminated ? 1 : 0;

      return new EnvironmentState(
          this.Identifier,
          0,
          signal,
          terminated,
          new Single[] {actor_idx},
          new Rigidbody[] { },
          new Transform[] { });
    }
  }
}
