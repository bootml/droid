using System;
using Neodroid.Messaging.Messages;
using UnityEngine;

namespace Neodroid.Prototyping.Configurables {
  [AddComponentMenu(PrototypingComponentMenuPath._ComponentMenuPath + "Configurables/Difficulty")]
  public class DifficultyConfigurable : ConfigurableGameObject {
    public override void ApplyConfiguration(Configuration configuration) {
      if (Math.Abs(configuration.ConfigurableValue - 1) < double.Epsilon) {
        //print ("Increased Difficulty");
      } else if (Math.Abs(configuration.ConfigurableValue - -1) < double.Epsilon) {
        //print ("Decreased Difficulty");
      }
    }
  }
}
