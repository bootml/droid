﻿using System;
using System.Collections.Generic;
using Neodroid.Environments;
using Neodroid.Prototyping.Evaluation.Terms;
using Neodroid.Utilities.Interfaces;
using UnityEngine;

namespace Neodroid.Prototyping.Evaluation {
  [Serializable]
  public abstract class ObjectiveFunction : MonoBehaviour,
                                            IHasRegister<Term> {
    public bool Debugging { get { return this._debugging; } set { this._debugging = value; } }

    public float SolvedThreshold {
      get { return this._solved_threshold; }
      set { this._solved_threshold = value; }
    }

    public PrototypingEnvironment ParentEnvironment {
      get { return this._environment; }
      set { this._environment = value; }
    }

    public virtual void Register(Term term) {
      if (this.Debugging)
        print($"Term registered: {term}");
      this._extra_terms_dict.Add(term.name, term);
      this._extra_term_weights.Add(term, 1);
    }

    public virtual void Register(Term term, string identifier) {
      this._extra_terms_dict.Add(term.name, term);
      this._extra_term_weights.Add(term, 1);
    }

    void Awake() {
      foreach (var go in this._extra_terms)
        this.Register(go);

      if (!this.ParentEnvironment)
        this.ParentEnvironment = FindObjectOfType<PrototypingEnvironment>();
    }

    public virtual float InternalEvaluate() { return 0; }

    public float Evaluate() {
      var signal = 0.0f;
      signal += this.InternalEvaluate();
      signal += this.EvaluateExtraTerms();

      signal = signal * Mathf.Pow(this._in_mdp_discount_factor, this._environment.CurrentFrameNumber);
      if (this.Debugging)
        print(signal);
      return signal;
    }

    public void Reset() { this.InternalReset(); }

    public virtual void InternalReset() { }

    public virtual void AdjustExtraTermsWeights(Term term, float new_weight) {
      if (this._extra_term_weights.ContainsKey(term))
        this._extra_term_weights[term] = new_weight;
    }

    public virtual float EvaluateExtraTerms() {
      float extra_terms_output = 0;
      foreach (var term in this._extra_terms_dict.Values) {
        if (this.Debugging)
          print($"Extra term: {term}");
        extra_terms_output += this._extra_term_weights[term] * term.Evaluate();
      }

      if (this.Debugging)
        print($"Extra terms signal: {extra_terms_output}");
      return extra_terms_output;
    }

    #region Fields

    [Header("Development", order = 99)]
    [SerializeField]
    bool _debugging;

    [Header("References", order = 100)]
    [SerializeField]
    float _in_mdp_discount_factor = 1.0f;

    [SerializeField] PrototypingEnvironment _environment;

    [SerializeField] Term[] _extra_terms;

    [SerializeField] Dictionary<string, Term> _extra_terms_dict = new Dictionary<string, Term>();

    [SerializeField] Dictionary<Term, float> _extra_term_weights = new Dictionary<Term, float>();

    [Header("General", order = 101)]
    [SerializeField]
    float _solved_threshold;

    #endregion
  }
}
