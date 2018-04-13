using System.Collections;
using System.Collections.Generic;
using Neodroid.Environments;
using Neodroid.Messaging;
using Neodroid.Messaging.Messages;
using Neodroid.Utilities.Interfaces;
using Neodroid.Utilities.ScriptableObjects;
using UnityEngine;

namespace Neodroid.Managers {
  [AddComponentMenu("Neodroid/Managers/General")]
  public class NeodroidManager : MonoBehaviour,
                                 IHasRegister<NeodroidEnvironment> {
 
    /// <summary>
    /// 
    /// </summary>
    [Header("General", order = 101)]
    [SerializeField]
    protected bool _Awaiting_Reply;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField] protected SimulatorConfiguration _Configuration;

    [Header("Development", order = 99)]
    [SerializeField]
    bool _debugging;

    [Header("Connection", order = 100)]
    [SerializeField]
    string _ip_address = "localhost";

    [SerializeField] int _port = 5555;
    [SerializeField] int _skip_frame_i;

    [SerializeField] bool _testing_motors;

    [Header("Simulation", order = 101)]

    // When _update_fixed_time_scale is true, MAJOR slow downs due to PHYSX updates on change.
    [SerializeField]
    bool _update_fixed_time_scale;

    /// <summary>
    /// 
    /// </summary>
    public static NeodroidManager Instance { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public SimulatorConfiguration Configuration {
      get {
        if (this._Configuration == null)
          this.Configuration = new SimulatorConfiguration();

        return this._Configuration;
      }
      set { this._Configuration = value; }
    }

    /// <summary>
    /// 
    /// </summary>
    public event System.Action EarlyFixedUpdateEvent;
    /// <summary>
    /// 
    /// </summary>
    public event System.Action FixedUpdateEvent;
    /// <summary>
    /// 
    /// </summary>
    public event System.Action LateFixedUpdateEvent;

    /// <summary>
    /// 
    /// </summary>
    public event System.Action EarlyUpdateEvent;
    /// <summary>
    /// 
    /// </summary>
    public event System.Action UpdateEvent;
    /// <summary>
    /// 
    /// </summary>
    public event System.Action LateUpdateEvent;
    /// <summary>
    /// 
    /// </summary>
    public event System.Action OnPostRenderEvent;
    /// <summary>
    /// 
    /// </summary>
    public event System.Action OnRenderImageEvent;
    /// <summary>
    /// 
    /// </summary>
    public event System.Action OnEndOfFrameEvent;

    public event System.Action OnReceiveEvent;

    void FetchCommmandLineArguments() {
      var arguments = System.Environment.GetCommandLineArgs();

      for (var i = 0; i < arguments.Length; i++) {
        if (arguments[i] == "-ip")
          this.IpAddress = arguments[i + 1];
        if (arguments[i] == "-port")
          this.Port = int.Parse(arguments[i + 1]);
      }
    }

    void CreateMessagingServer() {
      if (this.IpAddress != "" || this.Port != 0) //TODO: close application is port is already in use.
        this._Message_Server = new MessageServer(this.IpAddress, this.Port, false, this.Debugging);
      else
        this._Message_Server = new MessageServer(this.Debugging);
    }

    void StartMessagingServer(bool threaded = false) {
      if (threaded)
        this._Message_Server.ListenForClientToConnect(this.OnConnectCallback, this.OnDebugCallback);
      if (this.Debugging) {
        print("Started Messaging Server in a new thread");
      } else {
        this._Message_Server.ListenForClientToConnect(this.OnDebugCallback);
        if (this.Debugging) {
          print("Started Messaging Server");
        }
      }
    }

    #region Getter And Setters

    public Reaction CurrentReaction {
      get { return this._Current_Reaction; }
      set { this._Current_Reaction = value; }
    }

    public bool TestMotors { get { return this._testing_motors; } set { this._testing_motors = value; } }

    public string IpAddress { get { return this._ip_address; } set { this._ip_address = value; } }

    public int Port { get { return this._port; } set { this._port = value; } }

    public bool Debugging { get { return this._debugging; } set { this._debugging = value; } }

    #endregion

    #region PrivateMembers

    /// <summary>
    /// 
    /// </summary>
    protected Dictionary<string, NeodroidEnvironment> _Environments =
        new Dictionary<string, NeodroidEnvironment>();

    protected MessageServer _Message_Server;

    protected Reaction _Current_Reaction = new Reaction();

    #endregion

    #region UnityCallbacks

    /// <summary>
    /// 
    /// </summary>
    protected void Awake() {
      if (Instance == null)
        Instance = this;
      else {
        Debug.Log(
            "Warning: multiple managers in the scene! using "
            + Instance);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    protected void Start() {
      this.FetchCommmandLineArguments();

      if (this.Configuration == null) {
        this.Configuration = ScriptableObject.CreateInstance<SimulatorConfiguration>();
      }
      
      this.ApplyConfiguration();
      this.CreateMessagingServer();

      if (this.Configuration.SimulationType == SimulationType.Physics_dependent_) {
        this.EarlyFixedUpdateEvent += this.PreStep;
        this.FixedUpdateEvent += this.Step;
        this.LateFixedUpdateEvent += this.PostStep;
        this.StartCoroutine(this.LateFixedUpdate());
      } else {
        this.EarlyUpdateEvent += this.PreStep;
        this.UpdateEvent += this.Step;
        switch (this._Configuration.FrameFinishes) {
          case FrameFinishes.Late_update_:
            this.LateUpdateEvent += this.PostStep;
            break;
          case FrameFinishes.On_post_render_:
            this.OnPostRenderEvent += this.PostStep;
            break;
          case FrameFinishes.On_render_image_:
            this.OnRenderImageEvent += this.PostStep;
            break;
          case FrameFinishes.End_of_frame_:
            this.StartCoroutine(this.EndOfFrame());
            this.OnEndOfFrameEvent += this.PostStep;
            break;
        }
      }

      if (this.Configuration.SimulationType == SimulationType.Physics_dependent_) {
        this.StartMessagingServer(); // Remember to manually bind receive to an event in a derivation
      } else {
        this.StartMessagingServer(true);
      }
    }

    public void ApplyConfiguration() {
      QualitySettings.SetQualityLevel(this._Configuration.QualityLevel, true);
      this.SimulationTime = this._Configuration.TimeScale;
      Application.targetFrameRate = this._Configuration.TargetFrameRate;
      QualitySettings.vSyncCount = 0;

      #if !UNITY_EDITOR
      Screen.SetResolution(
          width : this._Configuration.Width,
          height : this._Configuration.Height,
          fullscreen : this._Configuration.FullScreen);
      #endif
    }

    /// <summary>
    /// 
    /// </summary>
    public float SimulationTime {
      get { return Time.timeScale; }
      set {
        Time.timeScale = value;
        if (this._update_fixed_time_scale)
          Time.fixedDeltaTime = 0.02F * Time.timeScale;
      }
    }

    void OnPostRender() { this.OnPostRenderEvent?.Invoke(); }

    void OnRenderImage(RenderTexture src, RenderTexture dest) {
      this.OnRenderImageEvent?.Invoke(); //TODO: May not work
    }

    /// <summary>
    /// 
    /// </summary>
    protected void FixedUpdate() {
      this.EarlyFixedUpdateEvent?.Invoke();

      this.FixedUpdateEvent?.Invoke();
    }

    IEnumerator LateFixedUpdate() {
      while (true) {
        yield return new WaitForFixedUpdate();
        if (this.Debugging) print("LateFixedUpdate");
        this.LateFixedUpdateEvent?.Invoke();
      }

      // ReSharper disable once IteratorNeverReturns
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected IEnumerator EndOfFrame() {
      while (true) {
        yield return new WaitForEndOfFrame();

        this.OnEndOfFrameEvent?.Invoke();
      }

      // ReSharper disable once IteratorNeverReturns
    }

    /// <summary>
    /// 
    /// </summary>
    protected void Update() {
      this.EarlyUpdateEvent?.Invoke();

      this.UpdateEvent?.Invoke();
    }

    protected void LateUpdate() { this.LateUpdateEvent?.Invoke(); }

    #endregion

    #region PrivateMethods

    /// <summary>
    /// 
    /// </summary>
    protected void PreStep() {
      if (this._Awaiting_Reply && this.CurrentReaction.Parameters.Phase == ExecutionPhase.Before_)
        this.ReactReply();
    }

    /// <summary>
    /// 
    /// </summary>
    protected void Step() {
      if (this.TestMotors)
        this.React(this.SampleRandomReaction());

      if (this._Awaiting_Reply && this.CurrentReaction.Parameters.Phase == ExecutionPhase.Middle_)
        this.ReactReply();
    }

    /// <summary>
    /// 
    /// </summary>
    protected void PostStep() {
      if (this._Awaiting_Reply && this.CurrentReaction.Parameters.Phase == ExecutionPhase.After_)
        this.ReactReply();

      foreach (var environment in this._Environments.Values)
        environment.PostStep();

      this.ResetReaction();
    }

    /// <summary>
    /// 
    /// </summary>
    protected void ReactReply() {
      var state = this.React(this.CurrentReaction);

      if (this._skip_frame_i >= this.Configuration.FrameSkips) {
        //&&this._last_reply_time + this._configuration.MaxReplyInterval > Time.time) {
        this.Reply(state);
        this._Awaiting_Reply = false;
        this._skip_frame_i = 0;
        //this._last_reply_time = Time.time;
      } else {
        this._skip_frame_i += 1;
        if (this.Debugging)
          print("Skipping frame");
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected Reaction SampleRandomReaction() {
      var reaction = new Reaction();
      foreach (var environment in this._Environments.Values)
        reaction = environment.SampleReaction();

      return reaction;
    }

    //TODO: EnvironmentState[][] states for aggregation of frame skip states
    /// <summary>
    /// 
    /// </summary>
    /// <param name="states"></param>
    protected void Reply(EnvironmentState[] states) { this._Message_Server.SendStates(states); }

    void ResetReaction() {
      this.CurrentReaction = new Reaction();
      this.CurrentReaction.Parameters.IsExternal = false;
    }

    #endregion

    #region PublicMethods

    /// <summary>
    /// 
    /// </summary>
    /// <param name="reaction"></param>
    /// <returns></returns>
    public EnvironmentState[] React(Reaction reaction) {
      var states = new EnvironmentState[this._Environments.Values.Count];
      var i = 0;
      foreach (var environment in this._Environments.Values)
        states[i++] = environment.React(reaction);
      return states;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string GetStatus() {
      return this._Message_Server._Client_Connected ? "Connected" : "Not Connected";
    }

    #endregion

    #region Registration

    /// <summary>
    /// 
    /// </summary>
    /// <param name="environment"></param>
    public void Register(NeodroidEnvironment environment) {
      if (this.Debugging)
        Debug.Log($"Manager {this.name} has environment {environment.Identifier}");
      this._Environments.Add(environment.Identifier, environment);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="environment"></param>
    /// <param name="identifier"></param>
    public void Register(NeodroidEnvironment environment, string identifier) {
      if (this.Debugging)
        Debug.Log($"Manager {this.name} has environment {identifier}");
      this._Environments.Add(identifier, environment);
    }

    #endregion

    #region MessageServerCallbacks

    void OnReceiveCallback(Reaction reaction) {
      if (this.Debugging)
        print("Received: " + reaction);
      this.SetReactionFromExternalSource(reaction);

      this.OnReceiveEvent?.Invoke();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="reaction"></param>
    protected void SetReactionFromExternalSource(Reaction reaction) {
      this.CurrentReaction = reaction;
      this.CurrentReaction.Parameters.IsExternal = true;
      this._Awaiting_Reply = true;
    }

    void OnDisconnectCallback() {
      if (this.Debugging)
        Debug.Log("Client disconnected.");
    }

    void OnDebugCallback(string error) {
      if (this.Debugging)
        Debug.Log("DebugCallback: " + error);
    }

    void OnConnectCallback() {
      if (this.Debugging)
        Debug.Log("Client connected");
      this._Message_Server.StartReceiving(
          this.OnReceiveCallback,
          this.OnDisconnectCallback,
          this.OnDebugCallback);
    }

    #endregion

    #region Deconstruction

    void OnApplicationQuit() { this._Message_Server.CleanUp(); }

    void OnDestroy() { this._Message_Server.Destroy(); }

    #endregion
  }
}
