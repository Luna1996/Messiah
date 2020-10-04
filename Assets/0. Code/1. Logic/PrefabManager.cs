namespace Messiah.Logic {
  using UnityEngine;
  using UnityEngine.AddressableAssets;
  using XLua;
  using System.Threading.Tasks;
  using System.Collections.Generic;

  public static class PrefabManager {
    
    public static Dictionary<string, GameObject> cache;

    public static async Task Init() {
      cache = new Dictionary<string, GameObject>();
      await Addressables.LoadAssetsAsync<GameObject>(Constant.Label_Prefab,
        (go) => {
          cache[go.name] = go;
        })
      .Task;

#if DEVELOPMENT_BUILD || UNITY_EDITOR
      Debug.Log("PrefabManager Initialized");
#endif
    }

    public static GameObject Instanciate(string name) {
      return GameObject.Instantiate(cache[name]);
    }

  }
}