using System;
using System.Collections.Generic;
using Neodroid.Environments;
using Neodroid.Utilities;
using Neodroid.Utilities.Interfaces;
using UnityEngine;

namespace Neodroid.Prototyping.Observers {
  [AddComponentMenu(PrototypingComponentMenuPath._ComponentMenuPath + "Observers/General")]
  [ExecuteInEditMode]
  [Serializable]
  public class Observer : MonoBehaviour,
                          IHasFloatEnumarable,
                          IRegisterable {
    public PrototypingEnvironment ParentEnvironment {
      get { return this._environment; }
      set { this._environment = value; }
    }

    public bool Debugging { get { return this._debugging; } set { this._debugging = value; } }

    public virtual string Identifier { get { return this.name + "Observer"; } }

    public Boolean NormaliseObservationUsingSpace {
      get { return this._normalise_observation_using_space; }
      set { this._normalise_observation_using_space = value; }
    }

    public virtual IEnumerable<float> FloatEnumerable { get; protected set; }

    protected virtual void Awake() { this.Setup(); }

    protected virtual void Start() { }

    public void RefreshAwake() { this.Awake(); }

    public void RefreshStart() { this.Start(); }

    protected void Setup() {
      this.RegisterComponent();
      this.FloatEnumerable = new float[] { };
      this.InnerSetup();
    }

    protected virtual void InnerSetup() { }

    public virtual void RegisterComponent() {
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterComponent(this.ParentEnvironment, this);
    }

    public virtual void UpdateObservation() { }

    public virtual void Reset() { }

    #region Fields

    [Header("References", order = 99)]
    [SerializeField]
    PrototypingEnvironment _environment;

    [Header("Normalisation", order = 100)]
    [SerializeField]
    bool _normalise_observation_using_space = true;

    [Header("Development", order = 101)]
    [SerializeField]
    bool _debugging;

    #endregion
  }
}
