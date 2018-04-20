using Neodroid.Messaging.Messages;
using Neodroid.Utilities;
using UnityEngine;

namespace Neodroid.Prototyping.Configurables {
  [AddComponentMenu(PrototypingComponentMenuPath._ComponentMenuPath + "Configurables/Color")]
  [RequireComponent(typeof(Renderer))]
  public class ColorConfigurable : ConfigurableGameObject {
    /// <summary>
    /// Alpha
    /// </summary>
    string _a;

    /// <summary>
    /// Blue
    /// </summary>
    string _b;

    /// <summary>
    /// Green
    /// </summary>
    string _g;

    /// <summary>
    /// Red
    /// </summary>
    string _r;

    /// <summary>
    /// 
    /// </summary>
    Renderer _renderer;

    /// <summary>
    /// 
    /// </summary>
    public override string Identifier { get { return this.name + "Color"; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void InnerStart() { this._renderer = this.GetComponent<Renderer>(); }

    /// <summary>
    /// 
    /// </summary>
    protected override void AddToEnvironment() {
      this._r = this.Identifier + "R";
      this._g = this.Identifier + "G";
      this._b = this.Identifier + "B";
      this._a = this.Identifier + "A";
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._r);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._g);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._b);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._a);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="configuration"></param>
    public override void ApplyConfiguration(Configuration configuration) {
      if (this.Debugging)
        print("Applying " + configuration + " To " + this.Identifier);
      foreach (var mat in this._renderer.materials) {
        var c = mat.color;

        if (configuration.ConfigurableName == this._r)
          c.r = configuration.ConfigurableValue;
        else if (configuration.ConfigurableName == this._g)
          c.g = configuration.ConfigurableValue;
        else if (configuration.ConfigurableName == this._b)
          c.b = configuration.ConfigurableValue;
        else if (configuration.ConfigurableName == this._a)
          c.a = configuration.ConfigurableValue;

        mat.color = c;
      }
    }
  }
}
