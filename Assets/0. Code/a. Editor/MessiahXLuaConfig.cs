namespace Messiah.Editor {
  using XLua;
  using System;
  using System.Collections.Generic;

  public static class MessiahXLuaConfig {
    [CSObjectWrapEditor.GenPath]
    public static readonly string XLuaGenPath = "Assets/2. Plugin/XLua/Gen";
    [CSharpCallLua]
    public static List<Type> CSharpCallLua = new List<Type>() {
      typeof(UI.T)
    };
  }
}
