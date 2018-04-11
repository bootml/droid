using Neodroid.Environments;
using Neodroid.Utilities;
using UnityEngine;

namespace Neodroid.Prototyping.Resetables {
  //public interface Resetable {
  [ExecuteInEditMode]
  public abstract class Resetable : MonoBehaviour {
    public PrototypingEnvironment _Parent_Environment;

    public abstract string ResetableIdentifier { get; }

    public abstract void Reset();

    protected virtual void Awake() { this.RegisterComponent(); }

    protected virtual void RegisterComponent() {
      this._Parent_Environment = NeodroidUtilities.MaybeRegisterComponent(this._Parent_Environment, this);
    }
  }
}
