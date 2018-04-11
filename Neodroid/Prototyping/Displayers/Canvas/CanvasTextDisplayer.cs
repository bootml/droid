using System;
using UnityEngine;
using UnityEngine.UI;

namespace Neodroid.Prototyping.Displayers.Canvas {
  [ExecuteInEditMode]
  [RequireComponent(typeof(Text))]
  [AddComponentMenu("Neodroid/Displayers/Canvas/CanvasText")]
  public class CanvasTextDisplayer : Displayer {
    Text _text_component;

    protected override void Setup() {
      this._text_component = this.GetComponent<Text>();
      this._text_component.text = "TEEEEEXT!";
    }

    public override void Display(Single value) { throw new NotImplementedException(); }
    public override void Display(Double value) { throw new NotImplementedException(); }
    public override void Display(Single[] values) { throw new NotImplementedException(); }

    public override void Display(String value) {
      if (this.Debugging)
        print("Applying " + value + " To " + this.name);
      this.SetText(value);
    }

    public void SetText(string text) { this._text_component.text = text; }
  }
}
