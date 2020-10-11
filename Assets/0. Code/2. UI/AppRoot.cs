namespace Messiah.UI {
  using UnityEngine;
  using Logic.GameCoreNS;
  using Logic;
  using UnityEngine.AddressableAssets;

  public class AppRoot : MonoBehaviour {
    public ViewManager viewManager;
    public GameObject loading;
    public AudioSource shufflesound;

    async void Start() {
      GameManager.appRoot = this;
      DontDestroyOnLoad(gameObject);
      AutoLogin();

      float W = Screen.width;
      float H = Screen.height;
      float w = 1080;
      float h = 2400;
      if (h / w > H / W) {
        float d = (1 - w * H / (h * W)) / 2;
        Camera.main.rect = new Rect(d, 0, 1 - 2 * d, 1);
      } else {
        float d = (1 - h * W / (w * H)) / 2;
        Camera.main.rect = new Rect(0, d, 1, 1 - 2 * d);
      }

#if UNITY_EDITOR
      gameObject.ToggleSiblings(false);
#elif DEVELOPMENT_BUILD
      await viewManager.SetupDebugPanel();
#endif

      GameCore.Init();

      await LuaManager.Init();
      await AtlasManager.Init();
      await PrefabManager.Init();
      viewManager.Init();
      Destroy(loading);
    }

    void AutoLogin() {
      GameCore.userData = UserData.GetLastUserData();
      if (GameCore.userData == null)
        UserData.LocalLogin("localuser");
    }
  }
}