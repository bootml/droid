using System.Collections.Generic;
using Neodroid.Prototyping.Actors;
using Neodroid.Prototyping.Configurables;
using Neodroid.Utilities.ScriptableObjects;

namespace Neodroid.Messaging.Messages {
  public class EnvironmentDescription {
    public EnvironmentDescription(
        int max_steps,
        SimulatorConfiguration simulation_configuration,
        Dictionary<string, Actor> actors,
        Dictionary<string, ConfigurableGameObject> configurables,
        float solved_threshold) {
      this.Configurables = configurables;
      this.Actors = actors;
      this.MaxSteps = max_steps;
      this.FrameSkips = simulation_configuration.FrameSkips;
      this.SolvedThreshold = solved_threshold;
      this.ApiVersion = "0.1.2";
    }

    public string ApiVersion { get; set; }

    public Dictionary<string, Actor> Actors { get; }

    public Dictionary<string, ConfigurableGameObject> Configurables { get; }

    public int MaxSteps { get; }

    public int FrameSkips { get; }

    public float SolvedThreshold { get; }
  }
}
