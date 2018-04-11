using System;

namespace Neodroid.Messaging.Messages.Displayables {
  public abstract class Displayable {
    public abstract string DisplayableName { get; }
    public abstract dynamic DisplayableValue { get; }
  }
}
