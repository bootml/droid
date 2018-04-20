namespace Neodroid.Messaging.Messages {
  /// <summary>
  /// 
  /// </summary>
  public enum ExecutionPhase {
    /// <summary>
    /// 
    /// </summary>
    Before_,
    /// <summary>
    /// 
    /// </summary>
    Middle_,
    /// <summary>
    /// 
    /// </summary>
    After_
  }

  /// <summary>
  /// 
  /// </summary>
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

    /// <summary>
    /// 
    /// </summary>
    public bool EpisodeCount { get; }

    /// <summary>
    /// 
    /// </summary>
    public ExecutionPhase Phase { get; set; } = ExecutionPhase.Middle_;

    /// <summary>
    /// 
    /// </summary>
    public bool IsExternal { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool Terminable { get; }

    /// <summary>
    /// 
    /// </summary>
    public bool Describe { get; }

    /// <summary>
    /// 
    /// </summary>
    public bool Reset { get; }

    /// <summary>
    /// 
    /// </summary>
    public bool Step { get; }

    /// <summary>
    /// 
    /// </summary>
    public bool Configure { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
      return "<ReactionParameters>\n "
             + $"Terminable:{this.Terminable},\nStep:{this.Step},\nReset:{this.Reset},\nConfigure:{this.Configure},\nDescribe:{this.Describe}\nEpisodeCount:{this.EpisodeCount}"
             + "\n</ReactionParameters>\n";
    }
  }
}
