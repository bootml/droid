﻿using Neodroid.Utilities.BoundingBoxes;
using UnityEngine;

namespace Neodroid.Prototyping.Observers.NotUsed {
  [AddComponentMenu("Neodroid/Prototyping/Observers/NotUsed/BoundingBox")]
  [ExecuteInEditMode]
  [RequireComponent(typeof(BoundingBox))]
  public class BoundingBoxObserver : Observer {
    public override string ObserverIdentifier { get { return this.name + "BoundingBox"; } }
    //BoundingBox _bounding_box;

    protected override void Start() {
      //_bounding_box = this.GetComponent<BoundingBox> ();
    }

    public override void UpdateObservation() {
      //Data = Encoding.ASCII.GetBytes (_bounding_box.BoundingBoxCoordinatesAsJSON);
    }
  }
}
