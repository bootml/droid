using Neodroid.Managers;
using Neodroid.Messaging.Messages;
using Neodroid.Utilities;
using UnityEngine;

namespace Neodroid.Prototyping.Configurables {
  [AddComponentMenu(PrototypingComponentMenuPath._ComponentMenuPath + "Configurables/Simulation")]
  [RequireComponent(typeof(PausableManager))]
  public class SimulationConfigurable : ConfigurableGameObject {
    string _fullscreen;
    string _height;

    string _quality_level;
    string _target_frame_rate;
    string _time_scale;
    string _width;

    public override string Identifier { get { return this.name + "Simulation"; } }

    protected override void AddToEnvironment() {
      this._quality_level = this.Identifier + "QualityLevel";
      this._target_frame_rate = this.Identifier + "TargetFrameRate";
      this._time_scale = this.Identifier + "TimeScale";
      this._width = this.Identifier + "Width";
      this._height = this.Identifier + "Height";
      this._fullscreen = this.Identifier + "Fullscreen";
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._quality_level);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._target_frame_rate);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._width);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._height);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._fullscreen);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._time_scale);
    }

    public override void ApplyConfiguration(Configuration configuration) {
      if (this.Debugging)
        print("Applying " + configuration + " To " + this.Identifier);

      if (configuration.ConfigurableName == this._quality_level)
        QualitySettings.SetQualityLevel((int)configuration.ConfigurableValue, true);
      else if (configuration.ConfigurableName == this._target_frame_rate)
        Application.targetFrameRate = (int)configuration.ConfigurableValue;
      else if (configuration.ConfigurableName == this._width)
        Screen.SetResolution((int)configuration.ConfigurableValue, Screen.height, false);
      else if (configuration.ConfigurableName == this._height)
        Screen.SetResolution(Screen.width, (int)configuration.ConfigurableValue, false);
      else if (configuration.ConfigurableName == this._fullscreen)
        Screen.SetResolution(Screen.width, Screen.height, (int)configuration.ConfigurableValue != 0);
      else if (configuration.ConfigurableName == this._time_scale)
        Time.timeScale = configuration.ConfigurableValue;
    }
  }
}
