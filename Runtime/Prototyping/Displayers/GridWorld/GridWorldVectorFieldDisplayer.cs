using System;
using droid.Runtime.Structs;
using droid.Runtime.Utilities.Drawing;
using UnityEngine;

namespace droid.Runtime.Prototyping.Displayers.GridWorld {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  [AddComponentMenu(DisplayerComponentMenuPath._ComponentMenuPath
                    + "GridWorldVectorField"
                    + DisplayerComponentMenuPath._Postfix)]
  public class GridWorldVectorFieldDisplayer : Displayer {
    /// <summary>
    /// </summary>
    protected override void Setup() { }

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    public override void Display(Double value) { }

    /// <summary>
    ///
    /// </summary>
    /// <param name="values"></param>
    public override void Display(float[] values) { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="values"></param>
    public override void Display(String values) { }

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="NotImplementedException"></exception>
    public override void Display(Vector3 value) { throw new NotImplementedException(); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    public override void Display(Vector3[] value) { this.ScatterPlot(value); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="points"></param>
    public override void Display(Points.ValuePoint points) { this.PlotSeries(new[] {points}); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="points"></param>
    public override void Display(Points.ValuePoint[] points) { }

    /// <summary>
    ///
    /// </summary>
    /// <param name="point"></param>
    /// <exception cref="NotImplementedException"></exception>
    public override void Display(Points.StringPoint point) { throw new NotImplementedException(); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="points"></param>
    /// <exception cref="NotImplementedException"></exception>
    public override void Display(Points.StringPoint[] points) { throw new NotImplementedException(); }

    //public override void Display(Object o) { throw new NotImplementedException(); }
    /// <summary>
    ///
    /// </summary>
    /// <param name="values"></param>
    public override void Display(float values) { }

    void Update() {
      if (this._RetainLastPlot) {
        if (this._Values != null) {
          PlotSeries(this._Values);
        }
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="points"></param>
    public void ScatterPlot(Vector3[] points) { }

    /*public override void PlotSeries(float[] points) {

    }*/

    /// <summary>
    /// </summary>
    /// <param name="points"></param>
    public override void PlotSeries(Points.ValuePoint[] points) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Plotting value points");
      }
      #endif

      this._Values = points;

      foreach (var point in points) {
        //point._Size
        switch ((int)point._Val) {
          case 0:
            NeodroidDrawingUtilities.ForDebug(point._Pos, Vector3.forward, Color.cyan);
            break;
          case 1:
            NeodroidDrawingUtilities.ForDebug(point._Pos, Vector3.back, Color.cyan);
            break;
          case 2:
            NeodroidDrawingUtilities.ForDebug(point._Pos, Vector3.up, Color.cyan);
            break;
          case 3:
            NeodroidDrawingUtilities.ForDebug(point._Pos, Vector3.down, Color.cyan);
            break;
          case 4:
            NeodroidDrawingUtilities.ForDebug(point._Pos, Vector3.left, Color.cyan);
            break;
          case 5:
            NeodroidDrawingUtilities.ForDebug(point._Pos, Vector3.right, Color.cyan);
            break;
        }
      }
    }
  }
}
