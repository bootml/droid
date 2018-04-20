using Neodroid.Messaging.Messages;
using UnityEngine;
using Random = System.Random;

namespace Neodroid.Environments {
  [AddComponentMenu("Neodroid/Environments/Randomised")]
  public class RandomisedEnvironment : PrototypingEnvironment {
    Random _random_generator = new Random();

    void RandomiseEnvironment() {
      foreach (var configurable in this.Configurables) {
        var valid_range = configurable.Value.ConfigurableValueSpace;
        float value = this._random_generator.Next((int)valid_range._Min_Value, (int)valid_range._Max_Value);
        configurable.Value.ApplyConfiguration(new Configuration(configurable.Key, Mathf.Round(value)));
      }
    }

    protected override void InnerPreStart() {
      base.InnerPreStart();
      this.RandomiseEnvironment();
    }

    public override void PostStep() {
      if (this._Terminated) {
        this._Terminated = false;
        this.Reset();

        this.RandomiseEnvironment();
      }

      if (this._Configured) {
        this._Configured = false;
        this.Configure();
      }

      this.UpdateConfigurableValues();
      this.UpdateObserversData();
    }
  }
}
