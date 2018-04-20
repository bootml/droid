using Neodroid.Environments;
using Neodroid.Messaging.Messages;
using Neodroid.Utilities;
using Neodroid.Utilities.Interfaces;
using Neodroid.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Prototyping.Configurables {
  /// <summary>
  /// 
  /// </summary>
  [AddComponentMenu(PrototypingComponentMenuPath._ComponentMenuPath + "Configurables/General")]
  public class ConfigurableGameObject : Configurable,
                                        IRegisterable {
    /// <summary>
    /// 
    /// </summary>
    public bool RelativeToExistingValue { get { return this._relative_to_existing_value; } }

    /// <summary>
    /// 
    /// </summary>
    public ValueSpace ConfigurableValueSpace {
      get { return this._configurable_value_space; }
      set { this._configurable_value_space = value; }
    }

    /// <summary>
    /// 
    /// </summary>
    public PrototypingEnvironment ParentEnvironment {
      get { return this._environment; }
      set { this._environment = value; }
    }

    /// <summary>
    /// 
    /// </summary>
    public bool Debugging { get { return this._debugging; } set { this._debugging = value; } }

    /// <summary>
    /// 
    /// </summary>
    public override string Identifier { get { return this.name + "Configurable"; } }

    /// <summary>
    /// 
    /// </summary>
    public virtual void UpdateObservation() { }

    /// <summary>
    /// 
    /// </summary>
    protected virtual void Start() {
      this.InnerStart();
      this.UpdateObservation();
    }

    protected virtual void InnerStart() { }

    /// <summary>
    /// 
    /// </summary>
    protected virtual void Awake() { this.AddToEnvironment(); }

    /// <summary>
    /// 
    /// </summary>
    public void RefreshAwake() { this.Awake(); }

    /// <summary>
    /// 
    /// </summary>
    public void RefreshStart() { this.Start(); }

    /// <summary>
    /// 
    /// </summary>
    protected virtual void AddToEnvironment() {
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterComponent(this.ParentEnvironment, this);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="configuration"></param>
    public override void ApplyConfiguration(Configuration configuration) { }

    #region Fields

    /// <summary>
    /// 
    /// </summary>
    [Header("References", order = 99)]
    [SerializeField]
    PrototypingEnvironment _environment;

    /// <summary>
    /// 
    /// </summary>
    [Header("Development", order = 100)]
    [SerializeField]
    bool _debugging;

    /// <summary>
    /// 
    /// </summary>
    [Header("General", order = 101)]
    [SerializeField]
    ValueSpace _configurable_value_space =
        new ValueSpace {_Decimal_Granularity = 0, _Min_Value = 10, _Max_Value = 10};

    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    bool _relative_to_existing_value;

    #endregion
  }
}
