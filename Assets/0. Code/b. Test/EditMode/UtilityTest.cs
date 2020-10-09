using NUnit.Framework;
using Messiah.Utility;
using Messiah.Editor;
using UnityEngine;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

static class UtilityTest {
  [Test]
  public static void PlayerPrefTest() {
    PlayerPrefs.DeleteAll();
    Debug.Log(PlayerPrefs.GetString("FUCKYOU") == "");
  }

  [Test]
  public static void RunCmdTest() {
    Utility.RunCmd(Constant.ExcelToJsonExe);
  }

  [Test]
  public static void ExcelToJsonTest() {
    Utility.GenerateJson("t.xlsx");
  }

  [Test]
  public static void PathTest() {
    Debug.Log(Path.GetFullPath("fuck"));
  }

  [Test]
  public static void ParallelForTest() {
    Task.Factory.StartNew(() => {
      Parallel.For(0, 10, (i) => {
        Debug.Log(i);
      });
    });
    Debug.Log("!!!!");
  }

  [Test]
  public static void JsonTest() {
    var d = JsonUtility.FromJson<Dictionary<string, string>>("{\"a\":\"1\", \"b\": \"2\"}");
    Debug.Log(d.Count);
  }

  [Test]
  public static void CSGeneratorTest() {
    Utility.ModifyCSharpFile(
      "C:/Users/yifuwang.TENCENT/Documents/GitHub/Messiah/Assets/1. Data/1. Config/2. CSharp/you.cs",
    "C:/Users/yifuwang.TENCENT/Documents/GitHub/Messiah/Assets/1. Data/1. Config/1. Json/you.json");
  }

  [Test]
  public static void ByteSerializationTest() {
    Messiah.Logic.UserData t = new Messiah.Logic.UserData();
    t.username = "你大爷";
    var str = ByteConverter.Serialize(t);
    var f = ByteConverter.Deserialize<Messiah.Logic.UserData>(str);
    Debug.Log(f.username);
  }

  [Test]
  public static void LocalLoginTest() {
    Messiah.Logic.UserData.CreateLocalUser("草泥马");
    var data = Messiah.Logic.UserData.GetUserData("草泥马");
    Debug.Log(data.username);
  }

  [Test]
  public static async void AsyncTest() {
    var ts = new List<Task>();
    for (int i = 0; i < 5; i++) ts.Add(t());
    foreach (var tt in ts) {
      tt.Start();
      await tt;
    }
  }


  public static Task t() {
    Debug.Log(2);
    return new Task(async () => {
      await Task.Delay(3000);
      Debug.Log(1);
    });
  }

}