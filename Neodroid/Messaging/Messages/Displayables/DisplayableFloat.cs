using System;
using UnityEngine;

namespace Neodroid.Messaging.Messages.Displayables {
  class DisplayableFloat : Displayable {
    public DisplayableFloat(String displayable_name, double displayable_value) {
      this.DisplayableName = displayable_name;
      this.DisplayableValue = displayable_value;
    }

    public DisplayableFloat(String displayable_name, Double? displayable_value) {
      this.DisplayableName = displayable_name;
      this.DisplayableValue = displayable_value.GetValueOrDefault();
    }

    public DisplayableFloat(String displayable_name, float displayable_value) {
      this.DisplayableName = displayable_name;
      this.DisplayableValue = displayable_value;
    }

    public override string ToString() {
      return "<Displayable> " + this.DisplayableName + ", " + this.DisplayableValue + " </Displayable>";
    }

    public override String DisplayableName { get; }
    public override dynamic DisplayableValue { get; }
  }
}
