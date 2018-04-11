namespace Neodroid.Messaging.Messages {
  public class Configuration {
    public Configuration(string configurable_name, float configurable_value) {
      this.ConfigurableName = configurable_name;
      this.ConfigurableValue = configurable_value;
    }

    public string ConfigurableName { get; }

    public float ConfigurableValue { get; }

    public override string ToString() {
      return "<Configuration> " + this.ConfigurableName + ", " + this.ConfigurableValue + " </Configuration>";
    }
  }
}
