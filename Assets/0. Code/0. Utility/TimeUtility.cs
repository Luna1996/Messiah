namespace Messiah.Utility {
  using System;
  public static class TimeUtility {
    public static long GetTimeStamp() {
      return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
    }
  }
}