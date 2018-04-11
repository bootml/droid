using System;
using UnityEngine;
using UnityEngine.UI;

namespace Neodroid.Prototyping.Displayers.Canvas {
  [ExecuteInEditMode]
  [RequireComponent(typeof(Image))]
  [AddComponentMenu("Neodroid/Displayers/Canvas/CanvasBar")]
  public class CanvasBarDisplayer : Displayer {
    Image _image;
    [SerializeField] [Range(0.0f, 1.0f)] float _value;

    protected override void Setup() {
      this._image = this.GetComponent<Image>();
      this._image.fillAmount = 0.6f;
    }

    public void SetFillAmount(float amount) { this._image.fillAmount = amount; }

    public override void Display(Single value) {
      if (this.Debugging)
        print("Applying " + value + " To " + this.name);
      this.SetFillAmount(value);
    }

    public override void Display(Double value) {
      if (this.Debugging)
        print("Applying " + value + " To " + this.name);
      this.SetFillAmount((float)value);
    }

    public override void Display(Single[] values) { throw new NotImplementedException(); }
    public override void Display(String value) { throw new NotImplementedException(); }
  }
}
