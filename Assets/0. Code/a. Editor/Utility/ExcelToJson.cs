namespace Messiah.Utility {
  using System.IO;
  using UnityEditor;
  using UnityEngine;
  using Messiah.Editor;
  using System.Collections.Generic;

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
      RunCmd(excel2json, $"-e \"{tmpxlsx_fullpath}\" -j \"{json_fullpath}\" -p \"{cs_fullpath}\" -h 3 -s -x \"#\" -l -a").WaitForExit();
      File.Delete(tmpxlsx_fullpath);
      ModifyCSharpFile(cs_fullpath, json_fullpath);
      AssetDatabase.ImportAsset(GetPathRelatedToProject(json_fullpath));
      AssetDatabase.ImportAsset(GetPathRelatedToProject(cs_fullpath));
      Debug.Log($"生成配置表：{xlsx_path}");
    }

    public static void ModifyCSharpFile(string cs_fullpath, string json_fullpath) {
      var name = Path.GetFileNameWithoutExtension(cs_fullpath);
      var json_asset = GetPathRelatedToProject(json_fullpath).Replace(@"\", "/");
      var lines = new List<string>(File.ReadAllLines(cs_fullpath));
      lines.RemoveRange(0, 8);
      lines.RemoveRange(lines.Count - 3, 3);
      List<string> types = new List<string>();
      for (int i = 0; i < lines.Count; i++) {
        lines[i] = "  " + lines[i];
        if (lines[i].StartsWith("  public class")) {
          var words = lines[i].Split(' ');
          types.Add(words[words.Length - 1]);
          lines[i + 1] = lines[i] + " {";
          lines[i] = "  [Serializable]";
          i++;
        }
      }
      lines.Insert(0,
      "namespace Messiah.JsonData {\n" +
      "  using System;\n" +
      "  using System.Threading.Tasks;\n" +
      "  using System.Collections.Generic;\n" +
      "  using UnityEngine;\n" +
      "  using UnityEngine.AddressableAssets;\n");
      lines.Add(
        "\n" +
        "  [Serializable]\n" +
        "  public class " + name + " {"
      );
      foreach (var type in types)
        lines.Add($"    public {type}[] {type};");
      lines.Add(
        "  }\n" +
        "\n" +
        "  public static class " + name + "JsonData {\n" +
        "    static " + name + " " + name + ";\n" +
        "\n" +
        "    public static async Task Load() {\n" +
        "      var textAsset = await Addressables.LoadAssetAsync<TextAsset>(\"" + json_asset + "\").Task;\n" +
        "      " + name + " = JsonUtility.FromJson<" + name + ">(textAsset.text);\n");
      foreach (var type in types)
        lines.Add(
          $"      {type}Dic = new Dictionary<int, {type}>();\n" +
          $"      foreach (var entry in {name}.{type})\n" +
          $"        {type}Dic[entry.ID] = entry;\n"
        );
      lines.Add(
        $"      {name} = null;\n" +
        "    }\n" +
        "\n" +
        "    public static void UnLoad() {");
      foreach (var type in types)
        lines.Add($"      {type}Dic = null;");
      lines.Add("    }");

      foreach (var type in types)
        lines.Add(
          $"\n    static Dictionary<int, {type}> {type}Dic;\n" +
          $"    public static {type} Get{type}(int ID) {{ return {type}Dic[ID]; }}"
        );

      lines.Add(
        "  }\n" +
        "}");
      File.WriteAllLines(cs_fullpath, lines);
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