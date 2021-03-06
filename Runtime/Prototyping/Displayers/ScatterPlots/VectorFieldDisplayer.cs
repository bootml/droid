using System;
using System.Linq;
using Neodroid.Runtime.Utilities.Misc.Drawing;
using Neodroid.Runtime.Utilities.Plotting;
using Neodroid.Runtime.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Runtime.Prototyping.Displayers.ScatterPlots {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode,
   AddComponentMenu(
       DisplayerComponentMenuPath._ComponentMenuPath + "VectorField" + DisplayerComponentMenuPath._Postfix)]
  public class VectorFieldDisplayer : Displayer {
    


    dynamic _vals;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void Setup() { }

    public override void Display(Double value) { }

    public override void Display(float[] values) { }

    public override void Display(String values) { }

    public override void Display(Vector3 value) { throw new NotImplementedException(); }
    public override void Display(Vector3[] value) { this.ScatterPlot(value); }

    public override void Display(Points.ValuePoint points) { this.PlotSeries(new[] {points}); }

    public override void Display(Points.ValuePoint[] points) { }

    public override void Display(Points.StringPoint point) { throw new NotImplementedException(); }
    public override void Display(Points.StringPoint[] points) { throw new NotImplementedException(); }

    public override void Display(float values) { }

    void Update() {
      if (this._RetainLastPlot) {
        if (this._vals != null) {
          PlotSeries(this._vals);
        }
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="points"></param>
    public void ScatterPlot(Vector3[] points) { }

    /*public void PlotSeries(float[] points) {

    }*/

    #if UNITY_EDITOR
    void OnDrawGizmos() {
      if (this.enabled) {
        if (this._values == null || this._values.Length == 0) {
          if (this._PlotRandomSeries) {
            var vs = PlotFunctions.SampleRandomSeries(9);
            this._values = vs.Select(v => v._Val).ToArray();
            this.PlotSeries(vs);
          }
        }
      }
    }
    #endif

    /// <summary>
    ///
    /// </summary>
    /// <param name="points"></param>
    public override void PlotSeries(Points.ValuePoint[] points) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Plotting value points");
      }
      #endif

      this._vals = points;

      foreach (var point in points) {
        //point._Size
        switch ((int)(point._Val)) {
          case 0:
            DrawArrow.ForDebug(point._Pos, Vector3.forward, Color.cyan);
            break;
          case 1:
            DrawArrow.ForDebug(point._Pos, Vector3.back, Color.cyan);
            break;
          case 2:
            DrawArrow.ForDebug(point._Pos, Vector3.up, Color.cyan);
            break;
          case 3:
            DrawArrow.ForDebug(point._Pos, Vector3.down, Color.cyan);
            break;
          case 4:
            DrawArrow.ForDebug(point._Pos, Vector3.left, Color.cyan);
            break;
          case 5:
            DrawArrow.ForDebug(point._Pos, Vector3.right, Color.cyan);
            break;
        }
      }
    }
  }
}