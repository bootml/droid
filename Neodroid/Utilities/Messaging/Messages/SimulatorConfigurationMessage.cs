﻿using System;
using droid.Neodroid.Utilities.Messaging.FBS;
using droid.Neodroid.Utilities.ScriptableObjects;

namespace droid.Neodroid.Utilities.Messaging.Messages
{
  /// <summary>
  ///
  /// </summary>
  public partial class SimulatorConfigurationMessage {
     int _frame_skips;
     bool _full_screen;
     int _height;
     int _width;
     int _frame_finishes;
     int _num_of_environments;
     int _reset_iterations;

    /// <summary>
    ///
    /// </summary>
    public  int FrameSkips { get { return this._frame_skips;} set { this._frame_skips = value; } }

    public  float TimeScale { get; set; }

    public  bool FullScreen { get { return this._full_screen; } set { this._full_screen = value; } }

    public  int Height { get { return this._height; } set { this._height = value; } }

    public  int Width { get { return this._width; } set { this._width = value; } }

    public  int Finishes {
      get { return this._frame_finishes; }
      set { this._frame_finishes = value; }
    }

    public  int NumOfEnvironments {
      get { return this._num_of_environments; }
      set { this._num_of_environments = value; }
    }

    public  int ResetIterations {
      get { return this._reset_iterations; }
      set { this._reset_iterations = value; }
    }
    
    public  int QualityLevel { get; set; }
    public  float TargetFrameRate { get; set; }
    public  int SimulationType { get; set; }
  }

  /// <summary>
  ///
  /// </summary>
  public  partial class SimulatorConfigurationMessage {
    /// <summary>
    ///
    /// </summary>
    /// <param name="simulator_configuration"></param>
    public  SimulatorConfigurationMessage(SimulatorConfiguration simulator_configuration) {
      this._frame_skips = simulator_configuration.FrameSkips;
      this._full_screen = simulator_configuration.FullScreen;
      this._height = simulator_configuration.Height;
      this._width = simulator_configuration.Width;
      this._frame_finishes = (Int32)simulator_configuration.FrameFinishes;
      this._num_of_environments = simulator_configuration.NumOfEnvironments;
      this.TimeScale = simulator_configuration.TimeScale;
      this._reset_iterations = simulator_configuration.ResetIterations;
      this.QualityLevel = simulator_configuration.QualityLevel;
      this.TargetFrameRate = simulator_configuration.TargetFrameRate;
      this.SimulationType = (int)simulator_configuration.SimulationType;
      this.Finishes = (int)simulator_configuration.FrameFinishes;
      //TODO: CANT BE CHANGE while running
//TODO: Exhaust list!
    }

    public SimulatorConfigurationMessage() { }

    /// <summary>
    ///
    /// </summary>
    /// <param name="flat_simulator_configuration"></param>
    public  void FbsParse(FSimulatorConfiguration flat_simulator_configuration) {
      this._frame_skips = flat_simulator_configuration.FrameSkips;
      this._full_screen = flat_simulator_configuration.FullScreen;
      this._height = flat_simulator_configuration.Height;
      this._width = flat_simulator_configuration.Width;
      this._frame_finishes = (int)flat_simulator_configuration.WaitEvery;
      this._num_of_environments = flat_simulator_configuration.NumOfEnvironments;
      this.TimeScale = flat_simulator_configuration.TimeScale;
      this._reset_iterations = flat_simulator_configuration.ResetIterations;
      this.QualityLevel = flat_simulator_configuration.QualityLevel;
      this.TargetFrameRate = flat_simulator_configuration.TargetFrameRate;
      //this.SimulationType = flat_simulator_configuration.S //TODO: CANT BE CHANGE while running
//TODO: Exhaust list!
    }
  }
}