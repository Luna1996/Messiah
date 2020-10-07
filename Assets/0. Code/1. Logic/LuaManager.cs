namespace Messiah.Logic {
  using UnityEngine;
  using UnityEngine.AddressableAssets;
  using XLua;
  using System.Threading.Tasks;
  using System.Collections.Generic;

  public static class LuaManager {
    public static LuaEnv lua;
    public static Dictionary<string, TextAsset> cache;

    public static async Task Init() {
      // 加载lua文件
      cache = new Dictionary<string, TextAsset>();
      await Addressables.LoadAssetsAsync<TextAsset>(Constant.Label_Lua, (textAsset) => {
        cache[textAsset.name] = textAsset;
      }).Task;

      lua = new LuaEnv();
      lua.AddLoader(LuaLoader);

      // 设置xlua调试器
#if DEVELOPMENT_BUILD || UNITY_EDITOR
      lua.Global.Set("LUA_DEBUG", true);
#endif

      // xlua入口
      lua.DoString("require('LuaEntry')");

#if DEVELOPMENT_BUILD || UNITY_EDITOR
      Debug.Log("LuaManager Initialized");
#endif
    }

    public static byte[] LuaLoader(ref string path) {
      var split = path.LastIndexOf('/');
      if (split != -1)
        return cache[path.Substring(split + 1)].bytes;
      else
        return cache[path].bytes;
    }

    public static LuaTable CreateLuaObject(string path) {
      var args = path.Split(' ');
      if (args.Length == 1)
        return (LuaTable)lua.DoString($"require('{path}') return {path}()")[0];
      else
        return (LuaTable)lua.DoString($"require('{args[0]}') return {args[0]}('{args[1]}')")[0];
    }
  }
}