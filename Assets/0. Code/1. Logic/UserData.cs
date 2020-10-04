namespace Messiah.Logic {
  using System;
  using Utility;
  using UnityEngine;

  [Serializable]
  public class UserData {
    public string username;
    public GameData currentGameData;

    public static UserData GetLastUser() {
      var base64 = PlayerPrefs.GetString(Constant.Pref_LastUserData);
      try {
        if (base64 == "") return new UserData();
        else return ByteConverter.Deserialize<UserData>(base64);
      } catch {
        return new UserData();
      }
    }
  }
}