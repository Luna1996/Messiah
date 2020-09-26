namespace Messiah.Utility {
  using System.Diagnostics;
  public static partial class Utility {
    public static Process RunCmd(string exe, params string[] args) {
      Process process = new Process();
      ProcessStartInfo startInfo = new ProcessStartInfo();
      startInfo.WindowStyle = ProcessWindowStyle.Hidden;
      startInfo.FileName = exe;
      startInfo.Arguments = string.Join(" ", args);
      process.StartInfo = startInfo;
      process.Start();
      return process;
    }
  }
}