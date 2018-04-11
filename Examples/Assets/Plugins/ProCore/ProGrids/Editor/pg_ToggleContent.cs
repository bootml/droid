using System;
using UnityEngine;

namespace ProGrids {
  /**
   * A substitute for GUIContent that offers some additional functionality.
   */
  [Serializable]
  public class pg_ToggleContent {
    readonly GUIContent gc = new GUIContent();
    public Texture2D image_on, image_off;
    public string text_on, text_off;
    public string tooltip;

    public pg_ToggleContent(string t_on, string t_off, string tooltip) {
      this.text_on = t_on;
      this.text_off = t_off;
      this.image_on = null;
      this.image_off = null;
      this.tooltip = tooltip;

      this.gc.tooltip = tooltip;
    }

    public pg_ToggleContent(string t_on, string t_off, Texture2D i_on, Texture2D i_off, string tooltip) {
      this.text_on = t_on;
      this.text_off = t_off;
      this.image_on = i_on;
      this.image_off = i_off;
      this.tooltip = tooltip;

      this.gc.tooltip = tooltip;
    }

    public static bool ToggleButton(
        Rect r,
        pg_ToggleContent content,
        bool enabled,
        GUIStyle imageStyle,
        GUIStyle altStyle) {
      content.gc.image = enabled ? content.image_on : content.image_off;
      content.gc.text = content.gc.image == null ? (enabled ? content.text_on : content.text_off) : "";

      return GUI.Button(r, content.gc, content.gc.image != null ? imageStyle : altStyle);
    }
  }
}
