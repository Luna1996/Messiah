namespace Messiah.Editor {
  using XLua;
  using System;
  using System.Collections.Generic;

  public static class MessiahXLuaConfig {
    [CSObjectWrapEditor.GenPath]
    public static readonly string XLuaGenPath = "Assets/2. Plugin/XLua/Gen";
    [CSharpCallLua]
    public static List<Type> CSharpCallLua = new List<Type>() {
      typeof(Messiah.Logic.Card),
      typeof(System.Action),
      typeof(System.Action<int,string>),
      typeof(Messiah.Logic.LuaEvent),
      typeof(Messiah.Logic.LuaEventState)
    };
  }
}
