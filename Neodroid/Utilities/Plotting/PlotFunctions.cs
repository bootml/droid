using System.Collections.Generic;
using Neodroid.Prototyping.Displayers;
using UnityEngine;

namespace Neodroid.Utilities.Plotting {
  public static class PlotFunctions {
    public static ScatterPlotDisplayer.ValuePoint[] SampleRandomSeries(float size) {
      var poin = new List<ScatterPlotDisplayer.ValuePoint>();
      for (var j = 0; j < 100; j++) {
        var point = new Vector3(j, UnityEngine.Random.Range(0, 10), 0);
        var vp = new ScatterPlotDisplayer.ValuePoint(point, j / 100f, size);
        poin.Add(vp);
      }

      var points = poin.ToArray();
      return points;
    }
  }
}
