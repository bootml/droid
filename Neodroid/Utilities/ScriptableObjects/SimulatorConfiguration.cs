using System;
using UnityEngine;

namespace Neodroid.Utilities.ScriptableObjects {
  public enum SimulationType {
    Frame_dependent_,
    // Waiting for frame instead means stable physics(Multiple fixed updates) and camera has updated their rendertextures. Pauses the game after every reaction until next reaction is received.

    Physics_dependent_,
    // Camera observers should be manually rendered to ensure validity and freshness with camera.Render()

    Independent_
    // Continue simulation
  }

  public enum FrameFinishes {
    Late_update_,
    On_post_render_,
    On_render_image_,
    End_of_frame_
  }

  [Serializable]
  public class SimulatorConfiguration { //: ScriptableObject {
    [SerializeField] FrameFinishes _frame_finishes;
    [SerializeField] [Range(0, 99)] int _frame_skips;
    [Header("Graphics")] [SerializeField] bool _full_screen;

    [SerializeField] [Range(0, 9999)] int _height;
    [SerializeField] [Range(0, 9999)] float _max_reply_interval;
    [SerializeField] [Range(1, 4)] int _quality_level;

    [SerializeField] [Range(1, 99)] int _reset_iterations;

    [Header("Simulation")]
    [SerializeField]
    SimulationType _simulation_type;

    [SerializeField] [Range(-1, 9999)] int _target_frame_rate;

    [SerializeField]
    [Range(0f, float.MaxValue)]
    float _time_scale;

    [SerializeField] [Range(0, 9999)] int _width;

    public SimulatorConfiguration() {
      this.Width = 500;
      this.Height = 500;
      this.FullScreen = false;
      this.QualityLevel = 1;
      this.TimeScale = 1;
      this.TargetFrameRate = -1;
      this.SimulationType = SimulationType.Frame_dependent_;
      this.FrameFinishes = FrameFinishes.Late_update_;
      this.FrameSkips = 0;
      this.ResetIterations = 10;
      this.MaxReplyInterval = 0;
    }

    #region Getter Setters

    public int FrameSkips {
      get { return this._frame_skips; }
      set {
        if (value >= 0)
          this._frame_skips = value;
      }
    }

    public int ResetIterations {
      get { return this._reset_iterations; }
      set {
        if (value >= 1)
          this._reset_iterations = value;
      }
    }
    //When resetting transforms we run multiple times to ensure that we properly reset hierachies of objects

    public SimulationType SimulationType {
      get { return this._simulation_type; }
      set { this._simulation_type = value; }
    }

    public int Width {
      get { return this._width; }
      set {
        if (value >= 0)
          this._width = value;
      }
    }

    public int Height {
      get { return this._height; }
      set {
        if (value >= 0)
          this._height = value;
      }
    }

    public bool FullScreen { get { return this._full_screen; } set { this._full_screen = value; } }

    public int TargetFrameRate {
      get { return this._target_frame_rate; }
      set {
        if (value >= -1)
          this._target_frame_rate = value;
      }
    }

    public int QualityLevel {
      get { return this._quality_level; }
      set {
        if (value >= 1 && value <= 4)
          this._quality_level = value;
      }
    }

    public float TimeScale {
      get { return this._time_scale; }
      set {
        if (value >= 0)
          this._time_scale = value;
      }
    }

    public Single MaxReplyInterval {
      get { return this._max_reply_interval; }
      set { this._max_reply_interval = value; }
    }

    public FrameFinishes FrameFinishes {
      get { return this._frame_finishes; }
      set { this._frame_finishes = value; }
    }

    #endregion
  }
}
