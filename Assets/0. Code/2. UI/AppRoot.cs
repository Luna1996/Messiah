namespace Messiah.UI {
  using UnityEngine;
  using Logic.GameCoreNS;
  using Logic;

  public class AppRoot : MonoBehaviour {
    public ViewManager viewManager;

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
      await viewManager.Init();

      LuaManager.lua.Global.Set("AppRoot", this);
    }

    void AutoLogin() {
      GameCore.userData = UserData.GetLastUserData();
      if (GameCore.userData == null)
        UserData.LocalLogin("localuser");
    }


  }
}