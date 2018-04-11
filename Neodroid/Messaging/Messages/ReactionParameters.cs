namespace Neodroid.Messaging.Messages {
  public enum ExecutionPhase {
    Before_,
    Middle_,
    After_
  }

  public class ReactionParameters {
    public ReactionParameters(
        bool terminable = false,
        bool step = false,
        bool reset = false,
        bool configure = false,
        bool describe = false,
        bool episode_count = true) {
      this.IsExternal = false;
      this.Terminable = terminable;
      this.Reset = reset;
      this.Step = step;
      this.Configure = configure;
      this.Describe = describe;
      this.EpisodeCount = episode_count;
    }

    public bool EpisodeCount { get; }

    public ExecutionPhase Phase { get; set; } = ExecutionPhase.Middle_;

    public bool IsExternal { get; set; }

    public bool Terminable { get; }

    public bool Describe { get; }

    public bool Reset { get; }

    public bool Step { get; }

    public bool Configure { get; }

    public override string ToString() {
      return "<ReactionParameters>\n "
             + $"Terminable:{this.Terminable},\nStep:{this.Step},\nReset:{this.Reset},\nConfigure:{this.Configure},\nDescribe:{this.Describe}\nEpisodeCount:{this.EpisodeCount}"
             + "\n</ReactionParameters>\n";
    }
  }
}
