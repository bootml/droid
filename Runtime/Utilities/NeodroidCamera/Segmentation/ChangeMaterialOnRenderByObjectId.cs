﻿using System;
using System.Collections.Generic;
using droid.Runtime.Utilities.NeodroidCamera.Synthesis;
using droid.Runtime.Utilities.Structs;
using UnityEngine;
using UnityEngine.Serialization;
using Object = System.Object;
using Random = UnityEngine.Random;

namespace droid.Runtime.Utilities.NeodroidCamera.Segmentation {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  public class ChangeMaterialOnRenderByObjectId : Segmenter {
    /// <summary>
    /// </summary>
    Renderer[] _all_renders;

    /// <summary>
    /// </summary>
    MaterialPropertyBlock _block;

    [SerializeField] ColorByInstance[] instanceColorArray;
    [SerializeField] Shader segmentation_shader;
    [SerializeField] Camera _camera;

    /// <summary>
    /// </summary>
    public Dictionary<GameObject, Color> ColorsDictGameObject { get; set; } = new Dictionary<GameObject, Color>();

    /// <summary>
    /// </summary>
    public override Dictionary<String, Color> ColorsDict {
      get {
        var colors = new Dictionary<String, Color>();
        foreach (var key_val in this.ColorsDictGameObject) {
          colors.Add(key_val.Key.GetInstanceID().ToString(), key_val.Value);
        }

        return colors;
      }
    }

    // Use this for initialization
    /// <summary>
    /// </summary>
    void Start() { this.Setup(); }

    /// <summary>
    /// </summary>
    void Awake() {
      this._all_renders = FindObjectsOfType<Renderer>();
      this._block = new MaterialPropertyBlock();
      this.Setup();
    }

    /// <summary>
    /// </summary>
    void Update() {

    }

    void CheckBlock() {
      if (this._block == null) {
        this._block = new MaterialPropertyBlock();
      }
    }

    SynthesisUtils.CapturePass[] cps = {
      new SynthesisUtils.CapturePass
      {
        _Name = "_object_id", ReplacementMode =
          SynthesisUtils.ReplacementModes.Object_id_,
        _SupportsAntialiasing = false
      }
    };

    /// <summary>
    /// </summary>
    void Setup() {
      this._camera = this.GetComponent<Camera>();
      SynthesisUtils.SetupCapturePassesReplacementShader(this._camera,this.segmentation_shader, ref cps);

      this.CheckBlock();
      foreach (var r in this._all_renders) {
        var game_object = r.gameObject;
        var id = game_object.GetInstanceID();
        var layer = game_object.layer;
        var go_tag = game_object.tag;

        ColorsDictGameObject = new Dictionary<GameObject, Color>();
        this.ColorsDictGameObject.Add(game_object, ColorEncoding.EncodeIdAsColor(id));
        this._block.SetColor("_ObjectIdColor", ColorEncoding.EncodeIdAsColor(id));
        //this._block.SetColor("_CategoryIdColor", ColorEncoding.EncodeLayerAsColor(layer));
        //this._block.SetColor("_MaterialIdColor", ColorEncoding.EncodeIdAsColor(id));
        //this._block.SetColor("_CategoryColor", ColorEncoding.EncodeTagHashCodeAsColor(go_tag));
        r.SetPropertyBlock(this._block);
      }
    }
  }
}