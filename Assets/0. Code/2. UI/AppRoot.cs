namespace Messiah.UI {
  using UnityEngine;
  using Logic.GameCoreNS;
  using Logic;
  using UnityEngine.AddressableAssets;

  public class AppRoot : MonoBehaviour {
    public ViewManager viewManager;
    public GameObject loading;

    async void Start() {
      DontDestroyOnLoad(gameObject);
      AutoLogin();

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