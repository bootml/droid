using droid.Runtime.Messaging.Messages;
using droid.Runtime.Utilities.Enums;
using UnityEngine;

namespace droid.Runtime.Interfaces {
  /// <summary>
  ///
  /// </summary>
  public interface ISimulatorConfiguration {
    /// <summary>
    ///
    /// </summary>
    int ResetIterations { get; set; }

    /// <summary>
    ///
    /// </summary>
    bool AlwaysSerialiseIndividualObservables { get; set; }

    /// <summary>
    ///
    /// </summary>
    bool AlwaysSerialiseAggregatedFloatArray { get; set; }

    /// <summary>
    ///
    /// </summary>
    bool AlwaysSerialiseUnobservables { get; set; }

    /// <summary>
    ///
    /// </summary>
    SimulationType SimulationType { get; set; }

    /// <summary>
    ///
    /// </summary>
    int FrameSkips { get; set; }

    /// <summary>
    ///
    /// </summary>
    FrameFinishes FrameFinishes { get; set; }

    /// <summary>
    ///
    /// </summary>
    int TargetFrameRate { get; set; }

    /// <summary>
    ///
    /// </summary>
    int QualityLevel { get; set; }

    /// <summary>
    ///
    /// </summary>
    float TimeScale { get; set; }

    /// <summary>
    ///
    /// </summary>
    int NumOfEnvironments { get; set; }

    /// <summary>
    ///
    /// </summary>
    int Width { get; set; }

    /// <summary>
    ///
    /// </summary>
    bool FullScreen { get; set; }

    /// <summary>
    ///
    /// </summary>
    int Height { get; set; }

    /// <summary>
    ///
    /// </summary>
    ExecutionPhase StepExecutionPhase { get; set; }

    /// <summary>
    ///
    /// </summary>
    bool UpdateFixedTimeScale { get; set; }

    /// <summary>
    ///
    /// </summary>
    string IpAddress { get; set; }

    /// <summary>
    ///
    /// </summary>
    int Port { get; set; }

    /// <summary>
    ///
    /// </summary>
    bool ReplayReactionInSkips { get; set; }

    /// <summary>
    ///
    /// </summary>
    bool ApplyResolutionSettings { get; set; }

    /// <summary>
    ///
    /// </summary>
    bool ApplyQualitySettings { get; set; }

    bool ResizableWindow { get; set; }
    ColorSpace ColorSpace { get; set; }
    int VSyncCount { get; set; }
  }
}
