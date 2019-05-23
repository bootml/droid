using droid.Runtime.Messaging.Messages;

namespace droid.Runtime.Interfaces {
  /// <summary>
  ///
  /// </summary>
  public interface IEnvironment : IRegisterable {
    /// <summary>
    ///
    /// </summary>
    Reaction LastReaction { get; }
    /// <summary>
    ///
    /// </summary>
    int CurrentFrameNumber { get; }
    /// <summary>
    ///
    /// </summary>
    bool Terminated { get; }
    /// <summary>
    ///
    /// </summary>
    string LastTerminationReason { get; }
    /// <summary>
    ///
    /// </summary>
    bool IsResetting { get; }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    EnvironmentState CollectState();
    /// <summary>
    ///
    /// </summary>
    /// <param name="reaction"></param>
    void React(Reaction reaction);
    /// <summary>
    ///
    /// </summary>
    /// <param name="reaction"></param>
    /// <returns></returns>
    EnvironmentState ReactAndCollectState(Reaction reaction);
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    Reaction SampleReaction();
    /// <summary>
    ///
    /// </summary>
    void Tick();
    /// <summary>
    ///
    /// </summary>
    void PostStep();
  }
}
