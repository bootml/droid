using System;
using Neodroid.Managers;
using Neodroid.Utilities.Interfaces;
using Neodroid.Utilities.ScriptableObjects;
using UnityEngine;

namespace Neodroid.Prototyping.Observers {
  enum ImageFormat {
    Jpg_,
    Png_,
    Exr_
  }

  [AddComponentMenu(PrototypingComponentMenuPath._ComponentMenuPath+"Observers/Camera")]
  [ExecuteInEditMode]
  [RequireComponent(typeof(Camera))]
  public class CameraObserver : Observer,
                                IHasByteArray {
    [SerializeField]  ImageFormat _image_format = ImageFormat.Jpg_;
    [SerializeField] [Range(0, 100)]  int _jpeg_quality = 75;

    [Header("Observation", order = 103)]
    //[SerializeField]
    byte[] _bytes = { };

    [Header("Specific", order = 102)]
    [SerializeField]
    Camera _camera;

    bool _grab = true;

    NeodroidManager _manager;
    [SerializeField] Texture2D _texture;

    public override string ObserverIdentifier { get { return this.name + "Camera"; } }

    public byte[] Bytes { get { return this._bytes; } private set { this._bytes = value; } }

    protected override void InnerSetup() {
      this._manager = FindObjectOfType<NeodroidManager>();
      this._camera = this.GetComponent<Camera>();
      this._texture = new Texture2D(this._camera.targetTexture.width, this._camera.targetTexture.height);
    }

    protected virtual void OnPostRender() { this.UpdateBytes(); }

    protected virtual void UpdateBytes() {
      if (!this._grab)
        return;
      this._grab = false;

      var current_render_texture = RenderTexture.active;
      RenderTexture.active = this._camera.targetTexture;

      this._texture.ReadPixels(
          new Rect(0, 0, this._camera.targetTexture.width, this._camera.targetTexture.height),
          0,
          0);
      this._texture.Apply();

      switch (this._image_format) {
        case ImageFormat.Jpg_:
          this._bytes = this._texture.EncodeToJPG(this._jpeg_quality);
          break;
        case ImageFormat.Png_:
          this._bytes = this._texture.EncodeToPNG();
          break;
        case ImageFormat.Exr_:
          this._bytes = this._texture.EncodeToEXR();
          break;
        default: throw new ArgumentOutOfRangeException();
      }

      RenderTexture.active = current_render_texture;
    }

    public override void UpdateObservation() {
      if (this._manager.Configuration.SimulationType != SimulationType.Frame_dependent_)
        print("WARNING! Camera Observations may be out of sync other data");

      this._grab = true;
    }
  }
}
