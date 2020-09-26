namespace Messiah.Editor {
  using System.IO;
  using UnityEditor;
  using UnityEngine;
  using Messiah.Utility;
  using System.Collections.Generic;
  using System.Threading.Tasks;

  static class MessiahMenu {
    [MenuItem("弥赛亚/重新载入配置表 %F2")]
    static void ReloadExcelConfig() {
      Utility.PurgeFolder(Constant.JsonFolder);
      Utility.PurgeFolder(Constant.CSharpFolder);
      ReloadHelper();
      AssetDatabase.Refresh();
    }

    static void ReloadHelper(DirectoryInfo dir = null, List<string> result = null) {
      if (dir == null) {
        ReloadHelper(
          new DirectoryInfo(Utility.GetFullPath(Constant.ExcelFolder)),
          new List<string>());
        return;
      }
      FileSystemInfo[] info = dir.GetFileSystemInfos();
      Parallel.ForEach(info, (i) => {
        if (i is DirectoryInfo)
          ReloadHelper(new DirectoryInfo(i.FullName), result);
        else {
          var path = Utility.GetRelatedPath(i.FullName, "Excel");
          if (path.EndsWith(".xlsx") && !path.Contains("~"))
            Utility.GenerateJson(path);
        }
      });
    }
  }
}