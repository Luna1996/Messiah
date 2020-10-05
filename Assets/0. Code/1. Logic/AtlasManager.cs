namespace Messiah.Logic {
  using UnityEngine;
  using UnityEngine.AddressableAssets;
  using XLua;
  using System.Threading.Tasks;
  using System.Collections.Generic;

  public static class AtlasManager {

    public static Dictionary<string, Texture> cache;

    public static async Task Init() {
      cache = new Dictionary<string, Texture>();
      await Addressables.LoadAssetsAsync<Texture>(Constant.Label_Atlas,
        (go) => {
          cache[go.name] = go;
        })
      .Task;

#if DEVELOPMENT_BUILD || UNITY_EDITOR
      Debug.Log("Atlas Initialized");
#endif
    }

    public static Texture GetTextrue(string name) {
      return cache[name];
    }

  }
}