// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

using System;
using System.Text;
using FlatBuffers;

namespace droid.Runtime.Messaging.FBS
{
  public enum FDisplayableValue : byte
{
 NONE = 0,
 FValue = 1,
 FValues = 2,
 FVector3s = 3,
 FValuedVector3s = 4,
 FString = 5,
 FByteArray = 6,
};

public struct FReaction : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return this.__p.bb; } }
  public static FReaction GetRootAsFReaction(ByteBuffer _bb) { return GetRootAsFReaction(_bb, new FReaction()); }
  public static FReaction GetRootAsFReaction(ByteBuffer _bb, FReaction obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public static bool FReactionBufferHasIdentifier(ByteBuffer _bb) { return Table.__has_identifier(_bb, "REAC"); }
  public void __init(int _i, ByteBuffer _bb) { this.__p.bb_pos = _i; this.__p.bb = _bb; }
  public FReaction __assign(int _i, ByteBuffer _bb) { this.__init(_i, _bb); return this; }

  public string EnvironmentName { get { int o = this.__p.__offset(4); return o != 0 ? this.__p.__string(o + this.__p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetEnvironmentNameBytes() { return __p.__vector_as_span(4); }
#else
  public ArraySegment<byte>? GetEnvironmentNameBytes() { return this.__p.__vector_as_arraysegment(4); }
#endif
  public byte[] GetEnvironmentNameArray() { return this.__p.__vector_as_array<byte>(4); }
  public FReactionParameters? Parameters { get { int o = this.__p.__offset(6); return o != 0 ? (FReactionParameters?)(new FReactionParameters()).__assign(o + this.__p.bb_pos, this.__p.bb) : null; } }
  public FMotion? Motions(int j) { int o = this.__p.__offset(8); return o != 0 ? (FMotion?)(new FMotion()).__assign(this.__p.__indirect(this.__p.__vector(o) + j * 4), this.__p.bb) : null; }
  public int MotionsLength { get { int o = this.__p.__offset(8); return o != 0 ? this.__p.__vector_len(o) : 0; } }
  public FMotion? MotionsByKey(string key) { int o = this.__p.__offset(8); return o != 0 ? FMotion.__lookup_by_key(this.__p.__vector(o), key, this.__p.bb) : null; }
  public FDisplayable? Displayables(int j) { int o = this.__p.__offset(10); return o != 0 ? (FDisplayable?)(new FDisplayable()).__assign(this.__p.__indirect(this.__p.__vector(o) + j * 4), this.__p.bb) : null; }
  public int DisplayablesLength { get { int o = this.__p.__offset(10); return o != 0 ? this.__p.__vector_len(o) : 0; } }
  public FDisplayable? DisplayablesByKey(string key) { int o = this.__p.__offset(10); return o != 0 ? FDisplayable.__lookup_by_key(this.__p.__vector(o), key, this.__p.bb) : null; }
  public FUnobservables? Unobservables { get { int o = this.__p.__offset(12); return o != 0 ? (FUnobservables?)(new FUnobservables()).__assign(this.__p.__indirect(o + this.__p.bb_pos), this.__p.bb) : null; } }
  public FConfiguration? Configurations(int j) { int o = this.__p.__offset(14); return o != 0 ? (FConfiguration?)(new FConfiguration()).__assign(this.__p.__indirect(this.__p.__vector(o) + j * 4), this.__p.bb) : null; }
  public int ConfigurationsLength { get { int o = this.__p.__offset(14); return o != 0 ? this.__p.__vector_len(o) : 0; } }
  public FConfiguration? ConfigurationsByKey(string key) { int o = this.__p.__offset(14); return o != 0 ? FConfiguration.__lookup_by_key(this.__p.__vector(o), key, this.__p.bb) : null; }
  public string SerialisedMessage { get { int o = this.__p.__offset(16); return o != 0 ? this.__p.__string(o + this.__p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetSerialisedMessageBytes() { return __p.__vector_as_span(16); }
#else
  public ArraySegment<byte>? GetSerialisedMessageBytes() { return this.__p.__vector_as_arraysegment(16); }
#endif
  public byte[] GetSerialisedMessageArray() { return this.__p.__vector_as_array<byte>(16); }

  public static void StartFReaction(FlatBufferBuilder builder) { builder.StartObject(7); }
  public static void AddEnvironmentName(FlatBufferBuilder builder, StringOffset environmentNameOffset) { builder.AddOffset(0, environmentNameOffset.Value, 0); }
  public static void AddParameters(FlatBufferBuilder builder, Offset<FReactionParameters> parametersOffset) { builder.AddStruct(1, parametersOffset.Value, 0); }
  public static void AddMotions(FlatBufferBuilder builder, VectorOffset motionsOffset) { builder.AddOffset(2, motionsOffset.Value, 0); }
  public static VectorOffset CreateMotionsVector(FlatBufferBuilder builder, Offset<FMotion>[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static VectorOffset CreateMotionsVectorBlock(FlatBufferBuilder builder, Offset<FMotion>[] data) { builder.StartVector(4, data.Length, 4); builder.Add(data); return builder.EndVector(); }
  public static void StartMotionsVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static void AddDisplayables(FlatBufferBuilder builder, VectorOffset displayablesOffset) { builder.AddOffset(3, displayablesOffset.Value, 0); }
  public static VectorOffset CreateDisplayablesVector(FlatBufferBuilder builder, Offset<FDisplayable>[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static VectorOffset CreateDisplayablesVectorBlock(FlatBufferBuilder builder, Offset<FDisplayable>[] data) { builder.StartVector(4, data.Length, 4); builder.Add(data); return builder.EndVector(); }
  public static void StartDisplayablesVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static void AddUnobservables(FlatBufferBuilder builder, Offset<FUnobservables> unobservablesOffset) { builder.AddOffset(4, unobservablesOffset.Value, 0); }
  public static void AddConfigurations(FlatBufferBuilder builder, VectorOffset configurationsOffset) { builder.AddOffset(5, configurationsOffset.Value, 0); }
  public static VectorOffset CreateConfigurationsVector(FlatBufferBuilder builder, Offset<FConfiguration>[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static VectorOffset CreateConfigurationsVectorBlock(FlatBufferBuilder builder, Offset<FConfiguration>[] data) { builder.StartVector(4, data.Length, 4); builder.Add(data); return builder.EndVector(); }
  public static void StartConfigurationsVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static void AddSerialisedMessage(FlatBufferBuilder builder, StringOffset serialisedMessageOffset) { builder.AddOffset(6, serialisedMessageOffset.Value, 0); }
  public static Offset<FReaction> EndFReaction(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    builder.Required(o, 4);  // environment_name
    builder.Required(o, 6);  // parameters
    return new Offset<FReaction>(o);
  }
  public static void FinishFReactionBuffer(FlatBufferBuilder builder, Offset<FReaction> offset) { builder.Finish(offset.Value, "REAC"); }
  public static void FinishSizePrefixedFReactionBuffer(FlatBufferBuilder builder, Offset<FReaction> offset) { builder.FinishSizePrefixed(offset.Value, "REAC"); }

  public static VectorOffset CreateSortedVectorOfFReaction(FlatBufferBuilder builder, Offset<FReaction>[] offsets) {
    Array.Sort(offsets, (Offset<FReaction> o1, Offset<FReaction> o2) => Table.CompareStrings(Table.__offset(4, o1.Value, builder.DataBuffer), Table.__offset(4, o2.Value, builder.DataBuffer), builder.DataBuffer));
    return builder.CreateVectorOfTables(offsets);
  }

  public static FReaction? __lookup_by_key(int vectorLocation, string key, ByteBuffer bb) {
    byte[] byteKey = Encoding.UTF8.GetBytes(key);
    int span = bb.GetInt(vectorLocation - 4);
    int start = 0;
    while (span != 0) {
      int middle = span / 2;
      int tableOffset = Table.__indirect(vectorLocation + 4 * (start + middle), bb);
      int comp = Table.CompareStrings(Table.__offset(4, bb.Length - tableOffset, bb), byteKey, bb);
      if (comp > 0) {
        span = middle;
      } else if (comp < 0) {
        middle++;
        start += middle;
        span -= middle;
      } else {
        return new FReaction().__assign(tableOffset, bb);
      }
    }
    return null;
  }
};

public struct FReactionParameters : IFlatbufferObject
{
  private Struct __p;
  public ByteBuffer ByteBuffer { get { return this.__p.bb; } }
  public void __init(int _i, ByteBuffer _bb) { this.__p.bb_pos = _i; this.__p.bb = _bb; }
  public FReactionParameters __assign(int _i, ByteBuffer _bb) { this.__init(_i, _bb); return this; }

  public bool Terminable { get { return 0!=this.__p.bb.Get(this.__p.bb_pos + 0); } }
  public bool Step { get { return 0!=this.__p.bb.Get(this.__p.bb_pos + 1); } }
  public bool Reset { get { return 0!=this.__p.bb.Get(this.__p.bb_pos + 2); } }
  public bool Configure { get { return 0!=this.__p.bb.Get(this.__p.bb_pos + 3); } }
  public bool Describe { get { return 0!=this.__p.bb.Get(this.__p.bb_pos + 4); } }
  public bool EpisodeCount { get { return 0!=this.__p.bb.Get(this.__p.bb_pos + 5); } }

  public static Offset<FReactionParameters> CreateFReactionParameters(FlatBufferBuilder builder, bool Terminable, bool Step, bool Reset, bool Configure, bool Describe, bool EpisodeCount) {
    builder.Prep(1, 6);
    builder.PutBool(EpisodeCount);
    builder.PutBool(Describe);
    builder.PutBool(Configure);
    builder.PutBool(Reset);
    builder.PutBool(Step);
    builder.PutBool(Terminable);
    return new Offset<FReactionParameters>(builder.Offset);
  }
};

public struct FMotion : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return this.__p.bb; } }
  public static FMotion GetRootAsFMotion(ByteBuffer _bb) { return GetRootAsFMotion(_bb, new FMotion()); }
  public static FMotion GetRootAsFMotion(ByteBuffer _bb, FMotion obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { this.__p.bb_pos = _i; this.__p.bb = _bb; }
  public FMotion __assign(int _i, ByteBuffer _bb) { this.__init(_i, _bb); return this; }

  public string ActorName { get { int o = this.__p.__offset(4); return o != 0 ? this.__p.__string(o + this.__p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetActorNameBytes() { return __p.__vector_as_span(4); }
#else
  public ArraySegment<byte>? GetActorNameBytes() { return this.__p.__vector_as_arraysegment(4); }
#endif
  public byte[] GetActorNameArray() { return this.__p.__vector_as_array<byte>(4); }
  public string ActuatorName { get { int o = this.__p.__offset(6); return o != 0 ? this.__p.__string(o + this.__p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetActuatorNameBytes() { return __p.__vector_as_span(6); }
#else
  public ArraySegment<byte>? GetActuatorNameBytes() { return this.__p.__vector_as_arraysegment(6); }
#endif
  public byte[] GetActuatorNameArray() { return this.__p.__vector_as_array<byte>(6); }
  public double Strength { get { int o = this.__p.__offset(8); return o != 0 ? this.__p.bb.GetDouble(o + this.__p.bb_pos) : (double)0.0; } }

  public static Offset<FMotion> CreateFMotion(FlatBufferBuilder builder,
      StringOffset actor_nameOffset = default(StringOffset),
      StringOffset actuator_nameOffset = default(StringOffset),
      double strength = 0.0) {
    builder.StartObject(3);
    AddStrength(builder, strength);
    AddActuatorName(builder, actuator_nameOffset);
    AddActorName(builder, actor_nameOffset);
    return EndFMotion(builder);
  }

  public static void StartFMotion(FlatBufferBuilder builder) { builder.StartObject(3); }
  public static void AddActorName(FlatBufferBuilder builder, StringOffset actorNameOffset) { builder.AddOffset(0, actorNameOffset.Value, 0); }
  public static void AddActuatorName(FlatBufferBuilder builder, StringOffset actuatorNameOffset) { builder.AddOffset(1, actuatorNameOffset.Value, 0); }
  public static void AddStrength(FlatBufferBuilder builder, double strength) { builder.AddDouble(2, strength, 0.0); }
  public static Offset<FMotion> EndFMotion(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    builder.Required(o, 4);  // actor_name
    builder.Required(o, 6);  // actuator_name
    return new Offset<FMotion>(o);
  }

  public static VectorOffset CreateSortedVectorOfFMotion(FlatBufferBuilder builder, Offset<FMotion>[] offsets) {
    Array.Sort(offsets, (Offset<FMotion> o1, Offset<FMotion> o2) => Table.CompareStrings(Table.__offset(4, o1.Value, builder.DataBuffer), Table.__offset(4, o2.Value, builder.DataBuffer), builder.DataBuffer));
    return builder.CreateVectorOfTables(offsets);
  }

  public static FMotion? __lookup_by_key(int vectorLocation, string key, ByteBuffer bb) {
    byte[] byteKey = Encoding.UTF8.GetBytes(key);
    int span = bb.GetInt(vectorLocation - 4);
    int start = 0;
    while (span != 0) {
      int middle = span / 2;
      int tableOffset = Table.__indirect(vectorLocation + 4 * (start + middle), bb);
      int comp = Table.CompareStrings(Table.__offset(4, bb.Length - tableOffset, bb), byteKey, bb);
      if (comp > 0) {
        span = middle;
      } else if (comp < 0) {
        middle++;
        start += middle;
        span -= middle;
      } else {
        return new FMotion().__assign(tableOffset, bb);
      }
    }
    return null;
  }
};

public struct FConfiguration : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return this.__p.bb; } }
  public static FConfiguration GetRootAsFConfiguration(ByteBuffer _bb) { return GetRootAsFConfiguration(_bb, new FConfiguration()); }
  public static FConfiguration GetRootAsFConfiguration(ByteBuffer _bb, FConfiguration obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { this.__p.bb_pos = _i; this.__p.bb = _bb; }
  public FConfiguration __assign(int _i, ByteBuffer _bb) { this.__init(_i, _bb); return this; }

  public string ConfigurableName { get { int o = this.__p.__offset(4); return o != 0 ? this.__p.__string(o + this.__p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetConfigurableNameBytes() { return __p.__vector_as_span(4); }
#else
  public ArraySegment<byte>? GetConfigurableNameBytes() { return this.__p.__vector_as_arraysegment(4); }
#endif
  public byte[] GetConfigurableNameArray() { return this.__p.__vector_as_array<byte>(4); }
  public double ConfigurableValue { get { int o = this.__p.__offset(6); return o != 0 ? this.__p.bb.GetDouble(o + this.__p.bb_pos) : (double)0.0; } }

  public static Offset<FConfiguration> CreateFConfiguration(FlatBufferBuilder builder,
      StringOffset configurable_nameOffset = default(StringOffset),
      double configurable_value = 0.0) {
    builder.StartObject(2);
    AddConfigurableValue(builder, configurable_value);
    AddConfigurableName(builder, configurable_nameOffset);
    return EndFConfiguration(builder);
  }

  public static void StartFConfiguration(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddConfigurableName(FlatBufferBuilder builder, StringOffset configurableNameOffset) { builder.AddOffset(0, configurableNameOffset.Value, 0); }
  public static void AddConfigurableValue(FlatBufferBuilder builder, double configurableValue) { builder.AddDouble(1, configurableValue, 0.0); }
  public static Offset<FConfiguration> EndFConfiguration(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    builder.Required(o, 4);  // configurable_name
    return new Offset<FConfiguration>(o);
  }

  public static VectorOffset CreateSortedVectorOfFConfiguration(FlatBufferBuilder builder, Offset<FConfiguration>[] offsets) {
    Array.Sort(offsets, (Offset<FConfiguration> o1, Offset<FConfiguration> o2) => Table.CompareStrings(Table.__offset(4, o1.Value, builder.DataBuffer), Table.__offset(4, o2.Value, builder.DataBuffer), builder.DataBuffer));
    return builder.CreateVectorOfTables(offsets);
  }

  public static FConfiguration? __lookup_by_key(int vectorLocation, string key, ByteBuffer bb) {
    byte[] byteKey = Encoding.UTF8.GetBytes(key);
    int span = bb.GetInt(vectorLocation - 4);
    int start = 0;
    while (span != 0) {
      int middle = span / 2;
      int tableOffset = Table.__indirect(vectorLocation + 4 * (start + middle), bb);
      int comp = Table.CompareStrings(Table.__offset(4, bb.Length - tableOffset, bb), byteKey, bb);
      if (comp > 0) {
        span = middle;
      } else if (comp < 0) {
        middle++;
        start += middle;
        span -= middle;
      } else {
        return new FConfiguration().__assign(tableOffset, bb);
      }
    }
    return null;
  }
};

public struct FDisplayable : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return this.__p.bb; } }
  public static FDisplayable GetRootAsFDisplayable(ByteBuffer _bb) { return GetRootAsFDisplayable(_bb, new FDisplayable()); }
  public static FDisplayable GetRootAsFDisplayable(ByteBuffer _bb, FDisplayable obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { this.__p.bb_pos = _i; this.__p.bb = _bb; }
  public FDisplayable __assign(int _i, ByteBuffer _bb) { this.__init(_i, _bb); return this; }

  public string DisplayableName { get { int o = this.__p.__offset(4); return o != 0 ? this.__p.__string(o + this.__p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetDisplayableNameBytes() { return __p.__vector_as_span(4); }
#else
  public ArraySegment<byte>? GetDisplayableNameBytes() { return this.__p.__vector_as_arraysegment(4); }
#endif
  public byte[] GetDisplayableNameArray() { return this.__p.__vector_as_array<byte>(4); }
  public FDisplayableValue DisplayableValueType { get { int o = this.__p.__offset(6); return o != 0 ? (FDisplayableValue)this.__p.bb.Get(o + this.__p.bb_pos) : FDisplayableValue.NONE; } }
  public TTable? DisplayableValue<TTable>() where TTable : struct, IFlatbufferObject { int o = this.__p.__offset(8); return o != 0 ? (TTable?)this.__p.__union<TTable>(o) : null; }

  public static Offset<FDisplayable> CreateFDisplayable(FlatBufferBuilder builder,
      StringOffset displayable_nameOffset = default(StringOffset),
      FDisplayableValue displayable_value_type = FDisplayableValue.NONE,
      int displayable_valueOffset = 0) {
    builder.StartObject(3);
    AddDisplayableValue(builder, displayable_valueOffset);
    AddDisplayableName(builder, displayable_nameOffset);
    AddDisplayableValueType(builder, displayable_value_type);
    return EndFDisplayable(builder);
  }

  public static void StartFDisplayable(FlatBufferBuilder builder) { builder.StartObject(3); }
  public static void AddDisplayableName(FlatBufferBuilder builder, StringOffset displayableNameOffset) { builder.AddOffset(0, displayableNameOffset.Value, 0); }
  public static void AddDisplayableValueType(FlatBufferBuilder builder, FDisplayableValue displayableValueType) { builder.AddByte(1, (byte)displayableValueType, 0); }
  public static void AddDisplayableValue(FlatBufferBuilder builder, int displayableValueOffset) { builder.AddOffset(2, displayableValueOffset, 0); }
  public static Offset<FDisplayable> EndFDisplayable(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    builder.Required(o, 4);  // displayable_name
    return new Offset<FDisplayable>(o);
  }

  public static VectorOffset CreateSortedVectorOfFDisplayable(FlatBufferBuilder builder, Offset<FDisplayable>[] offsets) {
    Array.Sort(offsets, (Offset<FDisplayable> o1, Offset<FDisplayable> o2) => Table.CompareStrings(Table.__offset(4, o1.Value, builder.DataBuffer), Table.__offset(4, o2.Value, builder.DataBuffer), builder.DataBuffer));
    return builder.CreateVectorOfTables(offsets);
  }

  public static FDisplayable? __lookup_by_key(int vectorLocation, string key, ByteBuffer bb) {
    byte[] byteKey = Encoding.UTF8.GetBytes(key);
    int span = bb.GetInt(vectorLocation - 4);
    int start = 0;
    while (span != 0) {
      int middle = span / 2;
      int tableOffset = Table.__indirect(vectorLocation + 4 * (start + middle), bb);
      int comp = Table.CompareStrings(Table.__offset(4, bb.Length - tableOffset, bb), byteKey, bb);
      if (comp > 0) {
        span = middle;
      } else if (comp < 0) {
        middle++;
        start += middle;
        span -= middle;
      } else {
        return new FDisplayable().__assign(tableOffset, bb);
      }
    }
    return null;
  }
};


}
