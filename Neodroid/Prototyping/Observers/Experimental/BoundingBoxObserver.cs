using Neodroid.Utilities.BoundingBoxes;
using UnityEngine;

namespace Neodroid.Prototyping.Observers.Experimental {
  /// <summary>
  /// 
  /// </summary>
  [AddComponentMenu("Neodroid/Prototyping/Observers/NotUsed/BoundingBox")]
  [ExecuteInEditMode]
  [RequireComponent(typeof(BoundingBox))]
  public class BoundingBoxObserver : Observer {
    /// <summary>
    /// 
    /// </summary>
    public override string Identifier { get { return this.name + "BoundingBox"; } }
    //BoundingBox _bounding_box;

    /// <summary>
    /// 
    /// </summary>
    protected override void Start() {
      //_bounding_box = this.GetComponent<BoundingBox> ();
    }

    /// <summary>
    /// 
    /// </summary>
    public override void UpdateObservation() {
      //Data = Encoding.ASCII.GetBytes (_bounding_box.BoundingBoxCoordinatesAsJSON);
    }
  }
}
