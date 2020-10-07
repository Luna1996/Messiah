namespace Messiah.UI {
  using Logic.GameCoreNS;
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using UnityEngine;
  using UnityEngine.UI;
  using UnityEngine.AddressableAssets;
  using Utility;

  public class ViewManager : MonoBehaviour {
    MessiahView messiah;
    OutGameView outGameView;
    InGameView inGameView;

    Text debugPanelText;

    public async Task Init() {
      EventService.Listen(GameEvent.EnterInGameState, SwitchToInGameView);
      EventService.Listen(GameEvent.EnterOutGameState, SwitchToOutGameView);
      messiah = GetComponentInChildren<MessiahView>();
      outGameView = GetComponentInChildren<OutGameView>();
      Logic.GameManager.viewManager = this;
      Logic.GameManager.messiahView = messiah;
      Logic.LuaManager.lua.Global.Set("ViewManager", this);
      Logic.LuaManager.lua.Global.Set("MessiahView", messiah);
      outGameView.Init();
      await Task.Delay(0);
    }

    public async void SwitchToOutGameView() {
      await inGameView.Hide();
      await messiah.SwitchState(GameState.OutGameState);
      outGameView = LoadPrefab("OutGameView").GetComponent<OutGameView>();
      await outGameView.Show();
    }

    public async void SwitchToInGameView() {
      await outGameView.Hide();
      await messiah.SwitchState(GameState.InGameState);
      inGameView = LoadPrefab("InGameView").GetComponent<InGameView>();
      await inGameView.Show();
    }

    public GameObject LoadPrefab(string name) {
      return Logic.PrefabManager.Instanciate(name, transform);
    }

#if DEVELOPMENT_BUILD
    public async Task SetupDebugPanel() {
      var debugPanelPrefab = await Addressables.LoadAssetAsync<GameObject>("Assets/1. Data/3. Prefab/DebugPanel.prefab").Task;
      debugPanelText = Instantiate(debugPanelPrefab, transform).GetComponent<Text>();
      debugPanelText.name = "DebubPanel";
      Application.logMessageReceived += LogHook;
      gameObject.ToggleSiblings(false);
    }

    void LogHook(string msg, string stack, LogType type) {
      string color = "red";
      switch (type) {
        case LogType.Log: color = "white"; break;
        case LogType.Warning: color = "yellow"; break;
      }
      debugPanelText.text += $"<color={color}>{msg}";
      if (type != LogType.Log)
        debugPanelText.text += $"\n{stack}";
      debugPanelText.text += "</color>\n";
    }
#endif
  }
}