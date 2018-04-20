using UnityEngine;

namespace Neodroid.Prototyping.Observers {
  enum Category {
    One_,
    Two_,
    Three_
  }

  [AddComponentMenu(PrototypingComponentMenuPath._ComponentMenuPath + "Observers/Categorical")]
  public class CategoricalObserver : Observer {
    void OneHotEncoding() { }
  }
}
