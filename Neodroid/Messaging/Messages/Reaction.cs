using System;
using Neodroid.Messaging.Messages.Displayables;
using UnityEngine;

namespace Neodroid.Messaging.Messages {
  [Serializable]
  public class Reaction {
    public override string ToString() {
      var motions_str = "";
      if (this.Motions != null) {
        foreach (var motion in this.Motions)
          motions_str += motion + "\n";
      }

      var configurations_str = "";
      if (this.Configurations != null) {
        foreach (var configuration in this.Configurations)
          configurations_str += configuration + "\n";
      }

      var displayables_str = "";
      if (this.Displayables != null) {
        foreach (var displayable in this.Displayables)
          displayables_str += displayable + "\n";
      }

      return "<Reaction>\n "
             + $"{this.Parameters},{motions_str},{configurations_str},{this.Unobservables},{displayables_str},{this.SerialisedMessage}"
             + "\n</Reaction>";
    }

    #region Constructors

    public Reaction(
        ReactionParameters parameters,
        MotorMotion[] motions,
        Configuration[] configurations,
        Unobservables unobservables,
        Displayable[] displayables,
        String serialised_message) {
      this.Parameters = parameters;
      this.Motions = motions;
      this.Configurations = configurations;
      this.Unobservables = unobservables;
      this.Displayables = displayables;
      this.SerialisedMessage = serialised_message;
    }

    public string SerialisedMessage { get; }

    public Reaction() {
      this.Parameters.IsExternal = false;
    }

    #endregion

    #region Getters

    public Displayable[] Displayables { get; }

    public MotorMotion[] Motions { get; }

    public Configuration[] Configurations { get; }

    public ReactionParameters Parameters { get; } = new ReactionParameters();

    public Unobservables Unobservables { get; } = new Unobservables(null, new Transform[] { });

    #endregion
  }
}
