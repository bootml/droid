using System;
using Neodroid.Utilities.ScriptableObjects;
using UnityEngine;

namespace Neodroid.Managers {
  [AddComponentMenu("Neodroid/Managers/Pausable")]
  public class PausableManager : NeodroidManager {
    #region UnityCallbacks

    protected new void Awake() {
      base.Awake();
      if (this.Configuration.SimulationType == SimulationType.Frame_dependent_) {
        this.EarlyUpdateEvent += this.PauseSimulation;
        this.UpdateEvent += this.MaybeResume;
      }

      if (this.Configuration.SimulationType == SimulationType.Physics_dependent_) {
        #if UNITY_EDITOR
        if (this._allow_in_editor_blockage) {
          this.EarlyFixedUpdateEvent +=
              this.Receive; // Receive blocks the main thread and therefore also the unity editor.
        } else
          print("Blocking in editor is not enabled and is therefore not receiving or sending anything.");
                #else
        this.EarlyFixedUpdateEvent += this.Receive;
        #endif
      }
    }

    #endregion

    #region Fields

    [SerializeField] bool _blocked;
    #if UNITY_EDITOR
    [SerializeField]  bool _allow_in_editor_blockage = false;
        #endif

    #endregion

    #region PrivateMethods

    void MaybeResume() {
      if (this.TestMotors || this.CurrentReaction.Parameters.Step)
        this.ResumeSimulation(this._Configuration.TimeScale);
    }

    public Boolean IsSimulationPaused { get { return !(this.SimulationTime > 0); } }

    void PauseSimulation() { this.SimulationTime = 0; }

    void ResumeSimulation(float simulation_time_scale) {
      if (simulation_time_scale > 0)
        this.SimulationTime = simulation_time_scale;
      else
        this.SimulationTime = 1;
    }

    void Receive() {
      var reaction = this._Message_Server.Receive(TimeSpan.Zero);
      this.SetReactionFromExternalSource(reaction);
    }

    #endregion
  }
}
