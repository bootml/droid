using Neodroid.Utilities.Interfaces;
using UnityEngine;

namespace Neodroid.Prototyping.Configurables {
  [AddComponentMenu(PrototypingComponentMenuPath._ComponentMenuPath+"Configurables/QuaternionTransform")]
  public class QuaternionTransformConfigurable : ConfigurableGameObject,
                                                 IHasQuaternionTransform {
    [Header("Specfic", order = 102)]
    [SerializeField]
    Vector3 _position;

    [SerializeField] Quaternion _rotation;

    [SerializeField] string _x;

    [SerializeField] string _y;

    [SerializeField] string _z;

    public Quaternion Rotation { get { return this._rotation; } }

    public Vector3 Position { get { return this._position; } }
  }
}
