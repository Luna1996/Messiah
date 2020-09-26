using NUnit.Framework;
using Messiah.Utility;
using Messiah.Editor;
using UnityEngine;
using System.IO;
using System.Threading.Tasks;

static class UtilityTest {
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
}