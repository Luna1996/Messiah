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

    public static Texture GetTexture(string name) {
      return cache[name];
    }

    public static Sprite GetSprite(string name) { 
      var t = (Texture2D)cache[name];
      return Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0.5f, 0.5f), 100f);
    }

  }
}