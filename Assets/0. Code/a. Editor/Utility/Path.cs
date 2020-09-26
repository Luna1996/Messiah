namespace Messiah.Utility {
  using System.IO;
  using Messiah.Editor;
  public static partial class Utility {
    public static void PurgeFolder(string path) {
      try {
        DirectoryInfo dir = new DirectoryInfo(GetFullPath(path));
        FileSystemInfo[] info = dir.GetFileSystemInfos();
        foreach (FileSystemInfo i in info) {
          if (i is DirectoryInfo) {
            DirectoryInfo subdir = new DirectoryInfo(i.FullName);
            subdir.Delete(true);
          } else {
            File.Delete(i.FullName);
          }
        }
      } catch { }
    }

    public static string GetFullPath(params string[] pathRelateToAssets) {
      return Path.GetFullPath(Path.Combine(Constant.AssetsFolder, string.Join($"{Path.DirectorySeparatorChar}", pathRelateToAssets)));
    }

    public static string GetPathRelatedToAssetsFolder(string path) {
      var i = path.IndexOf(Constant.AssetsFolder);
      if (i != -1) return path.Substring(i + Constant.AssetsFolder.Length + 1);
      else return path;
    }

    public static string GetPathRelatedToProject(string path) {
      var i = path.IndexOf(Constant.AssetsFolder);
      if (i != -1) return path.Substring(i);
      else return Path.Combine(Constant.AssetsFolder, path);
    }

    public static string GetRelatedPath(string path, string basepath) {
      var i = path.IndexOf(basepath);
      if (i != -1) return path.Substring(i + basepath.Length + 1);
      return path;
    }
  }
}