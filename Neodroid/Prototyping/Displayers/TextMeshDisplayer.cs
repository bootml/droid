using System;
using TMPro;
using UnityEngine;

namespace Neodroid.Prototyping.Displayers {
  [ExecuteInEditMode]
  [AddComponentMenu("Neodroid/Displayers/TextMesh")]
  public class TextMeshDisplayer : Displayer {
    TextMeshPro _text;

    protected override void Setup() {
      this._text = this.GetComponent<TextMeshPro>();

      this._text.SetText("TEEEEXT{0}", 1);

      this.SetText("SAJD");
    }

    public void SetText(string text) {
      if (this.Debugging)
        print("Applying " + text + " To " + this.name);
      this._text.SetText(text);
    }

    public override void Display(Single value) { throw new NotImplementedException(); }
    public override void Display(Double value) { throw new NotImplementedException(); }
    public override void Display(Single[] values) { throw new NotImplementedException(); }

    public override void Display(String value) {
      if (this.Debugging)
        print("Applying " + value + " To " + this.name);
      this.SetText(value);
    }
  }
}
