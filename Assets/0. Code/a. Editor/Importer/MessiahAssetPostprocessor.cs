namespace Messiah.Editor {
  using System.IO;
  using UnityEngine;
  using UnityEditor;
  using Messiah.Utility;
  using System.Threading.Tasks;

  class MessiahAssetPostprocessor : AssetPostprocessor {
    static void OnPostprocessAllAssets(string[] imported, string[] deleted, string[] moved, string[] movedFrom) {
      Parallel.ForEach(imported, (path) => {
        var rpath = Utility.GetPathRelatedToAssetsFolder(path);
        if (Utility.IsConfigExcel(rpath)) {
          var xlsx_path = rpath.Substring(Constant.ExcelFolder.Length + 1);
          Utility.DeleteGeneratedJson(xlsx_path);
        }
      });
      
      Parallel.ForEach(imported, (path) => {
        var rpath = Utility.GetPathRelatedToAssetsFolder(path);
        if (Utility.IsConfigExcel(rpath)) {
          var xlsx_path = rpath.Substring(Constant.ExcelFolder.Length + 1);
          Utility.GenerateJson(xlsx_path);
        }
      });

      Parallel.For(0, moved.Length, (i) => {
        var rpath = Utility.GetPathRelatedToAssetsFolder(moved[i]);
        if (Utility.IsConfigExcel(rpath)) {
          var rpathold = Utility.GetPathRelatedToAssetsFolder(movedFrom[i]);
          if (Utility.IsConfigExcel(rpathold)) {
            var xlsx_path_old = rpathold.Substring(Constant.ExcelFolder.Length + 1);
            Utility.DeleteGeneratedJson(xlsx_path_old);
          }
          var xlsx_path = rpath.Substring(Constant.ExcelFolder.Length + 1);
          Utility.GenerateJson(xlsx_path);
        }
      });
    }
  }
}