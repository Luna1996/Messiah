namespace Messiah.Logic {
  using System;
  using Utility;
  using UnityEngine;

  [Serializable]
  public class UserData {
    public string username;
    public GameData currentGameData;

    public static UserData GetLastUserData() {
      return GetUserData(PlayerPrefs.GetString(Constant.Pref_LastUsername));
    }

    public static UserData GetUserData(string name) {
      if (string.IsNullOrEmpty(name)) return null;
      var base64 = PlayerPrefs.GetString(Constant.Pref_UserDataPrefix + name);

      try {
        if (base64 == "") return null;
        else return ByteConverter.Deserialize<UserData>(base64);
      } catch {
        return null;
      }

    }

    public static void LocalLogin(string username) {
      var oldData = GetUserData(username);
      if (oldData == null)
        GameCoreNS.GameCore.userData = CreateLocalUser(username);
      else
        GameCoreNS.GameCore.userData = oldData;
      PlayerPrefs.SetString(Constant.Pref_LastUsername, GameCoreNS.GameCore.userData.username);
    }

    public static UserData CreateLocalUser(string username) {
      var newData = new UserData();
      newData.username = username;
      // var base64 = ByteConverter.Serialize(newData);
      // PlayerPrefs.SetString(Constant.Pref_UserDataPrefix + username, base64);
      return newData;
    }

    public static void Save() {
      var base64 = ByteConverter.Serialize(GameCoreNS.GameCore.userData);
      PlayerPrefs.SetString(Constant.Pref_UserDataPrefix + GameCoreNS.GameCore.userData.username, base64);
    }
  }
}