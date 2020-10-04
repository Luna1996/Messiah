namespace Messiah.Utility {
  using System;
  using System.IO;
  using System.Runtime.Serialization.Formatters.Binary;
  public static class ByteConverter {
    static BinaryFormatter bf = new BinaryFormatter();

    public static string Serialize(object obj) {
      using (MemoryStream ms = new MemoryStream()) {
        bf.Serialize(ms, obj);
        return Convert.ToBase64String(ms.ToArray());
      }
    }

    public static T Deserialize<T>(string str) {
      var bytes = Convert.FromBase64String(str);
      using (MemoryStream ms = new MemoryStream(bytes)) {
        return (T)bf.Deserialize(ms);
      }
    }
  }
}