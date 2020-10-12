namespace Messiah.Utility {
  using System;
  using System.IO;
  using System.Runtime.Serialization.Formatters.Binary;
  using FullSerializer;
  public static class ByteConverter {
    private static readonly fsSerializer _serializer = new fsSerializer();

    public static string Serialize<T>(object obj) {
      fsData data;
      _serializer.TrySerialize(typeof(T), obj, out data).AssertSuccessWithoutWarnings();

      // emit the data via JSON
      return fsJsonPrinter.CompressedJson(data);
    }

    public static T Deserialize<T>(string str) {
      fsData data = fsJsonParser.Parse(str);

      object deserialized = null;
      _serializer.TryDeserialize(data, typeof(T), ref deserialized).AssertSuccessWithoutWarnings();

      return (T)deserialized;
    }
  }
}