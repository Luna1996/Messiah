namespace Messiah.Utility {
  using System.IO;
  using UnityEditor;
  using UnityEngine;
  using Messiah.Editor;

  public static partial class Utility {
    public static bool IsConfigExcel(string rpath) {
      var name = Path.GetFileName(rpath);
      return rpath.StartsWith(Constant.ExcelFolder) && name.EndsWith(".xlsx") && !name.StartsWith("~");
    }

    public static void GenerateJson(string xlsx_path) {
      var excel2json = GetFullPath(Constant.ExcelToJsonExe);
      var xlsx_fullpath = GetFullPath(Constant.ExcelFolder, xlsx_path);
      var tmpxlsx_fullpath = xlsx_fullpath + ".tmp";
      var json_fullpath = GetJsonPath(xlsx_path);
      var cs_fullpath = GetCSharpPath(xlsx_path);
      File.Copy(xlsx_fullpath, tmpxlsx_fullpath, true);
      RunCmd(excel2json, $"-e \"{tmpxlsx_fullpath}\" -j \"{json_fullpath}\" -p \"{cs_fullpath}\" -h 3 -s -x \"#\" -l").WaitForExit();
      File.Delete(tmpxlsx_fullpath);
      // TODO modify cs file
      AssetDatabase.ImportAsset(GetPathRelatedToProject(json_fullpath));
      AssetDatabase.ImportAsset(GetPathRelatedToProject(cs_fullpath));
      Debug.Log($"生成配置表：{xlsx_path}");
    }

    public static void DeleteGeneratedJson(string xlsx_path) {
      var json_fullpath = Utility.GetJsonPath(xlsx_path);
      var cs_fullpath = Utility.GetCSharpPath(xlsx_path);
      AssetDatabase.DeleteAsset(GetPathRelatedToProject(json_fullpath));
      AssetDatabase.DeleteAsset(GetPathRelatedToProject(cs_fullpath));
      Debug.Log($"删除配置表: {xlsx_path}");
    }

    public static string GetJsonPath(string xlsx_path) {
      return GetFullPath(Constant.JsonFolder, xlsx_path.Replace(".xlsx", ".json"));
    }

    public static string GetCSharpPath(string xlsx_path) {
      return GetFullPath(Constant.CSharpFolder, xlsx_path.Replace(".xlsx", ".cs"));
    }
  }
}