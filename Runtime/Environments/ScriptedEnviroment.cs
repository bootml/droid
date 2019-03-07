﻿using System;
using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Managers;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Utilities.EventRecipients.droid.Neodroid.Utilities.Unsorted;
using droid.Runtime.Utilities.Misc;
using UnityEngine;
using Random = UnityEngine.Random;

namespace droid.Runtime.Environments {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu("Neodroid/Environments/ScriptedEnvironment")]
  public class ScriptedEnvironment : NeodroidEnvironment {
    /// <summary>
    /// </summary>
    [SerializeField]
    Renderer _actor_renderer = null;

    /// <summary>
    /// </summary>
    [SerializeField]
    int _actor_x = 0;

    /// <summary>
    /// </summary>
    [SerializeField]
    int _actor_y = 0;

    /// <summary>
    /// </summary>
    [SerializeField]
    Renderer _goal_renderer = null;

    /// <summary>
    /// </summary>
    [SerializeField]
    int _goal_x = 0;

    /// <summary>
    /// </summary>
    [SerializeField]
    int _goal_y = 0;

    /// <summary>
    /// </summary>
    int[,] _grid = null;

    /// <summary>
    /// </summary>
    [SerializeField]
    int _height = 0;

    List<IMotorMotion> _motions = new List<IMotorMotion>();

    /// <summary>
    /// </summary>
    [SerializeField]
    IManager _time_simulation_manager = null;

    /// <summary>
    /// </summary>
    [SerializeField]
    int _width = 0;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "ScriptedEnvironment"; } }

    /// <summary>
    /// </summary>
    public int ActorX {
      get { return this._actor_x; }
      set { this._actor_x = Mathf.Max(0, Mathf.Min(this._width - 1, value)); }
    }

    /// <summary>
    /// </summary>
    public int ActorY {
      get { return this._actor_y; }
      set { this._actor_y = Mathf.Max(0, Mathf.Min(this._height - 1, value)); }
    }

    /// <summary>
    /// </summary>
    public int GoalX {
      get { return this._goal_x; }
      set { this._goal_x = Mathf.Max(0, Mathf.Min(this._width - 1, value)); }
    }

    /// <summary>
    /// </summary>
    public int GoalY {
      get { return this._goal_y; }
      set { this._goal_y = Mathf.Max(0, Mathf.Min(this._height - 1, value)); }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void Setup() {
      this._grid = new int[this._width, this._height];

      var k = 0;
      for (var i = 0; i < this._width; i++) {
        for (var j = 0; j < this._height; j++) {
          this._grid[i, j] = k++;
        }
      }

      this._time_simulation_manager =
          NeodroidUtilities.RegisterComponent((NeodroidManager)this._time_simulation_manager, this);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PostStep() {
      if (this._goal_renderer) {
        this._goal_renderer.transform.position = new Vector3(this.GoalX, 0, this.GoalY);
      }

      if (this._actor_renderer) {
        this._actor_renderer.transform.position = new Vector3(this.ActorX, 0, this.ActorY);
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override Reaction SampleReaction() {
      this._motions.Clear();

      var strength = Random.Range(0, 4);
      this._motions.Add(new MotorMotion("", "", strength));

      var rp = new ReactionParameters(true, true, episode_count : true) {IsExternal = false};
      return new Reaction(rp, this._motions.ToArray(), null, null, null, "");
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="reaction"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public override EnvironmentState ReactAndCollectState(Reaction reaction) {
      this.React(reaction);
      return this.CollectState();
    }

    public override void React(Reaction reaction) {
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
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Tick() { }

    public override EnvironmentState CollectState() {
      var actor_idx = this._grid[this.ActorX, this.ActorY];
      var goal_idx = this._grid[this.GoalX, this.GoalY];

      var terminated = actor_idx == goal_idx;
      var signal = terminated ? 1 : 0;

      var time = Time.time - this._Lastest_Reset_Time;

      var observables = new float[] {actor_idx};

      return new EnvironmentState(this.Identifier, 0, 0, time, signal, terminated, ref observables);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="recipient"></param>
    public override void ObservationsString(DataPoller recipient) {
      recipient.PollData(this.CollectState().ToString());
    }

    public override void EnvironmentReset() { }
  }
}
