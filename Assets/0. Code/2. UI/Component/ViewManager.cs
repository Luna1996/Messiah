namespace Messiah.UI {
  using Logic.GameCoreNS;
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using TMPro;
  using UnityEngine;
  using UnityEngine.AddressableAssets;
  using Utility;

  public class ViewManager : MonoBehaviour {
    HandView hand;
    MessiahView messiah;

    public async Task Init() {
      EventService.Listen(GameEvent.EnterInGameState, SwitchToInGameView);
      EventService.Listen(GameEvent.EnterOutGameState, SwitchToOutGameView);
      // 近镜头 聚焦灯塔。
      hand = GetComponentInChildren<HandView>();
      messiah = GetComponentInChildren<MessiahView>();
      await Task.Delay(0);
    }

    public async void SwitchToOutGameView() {
      // 生成手牌
      messiah.SwitchState(GameState.OutGameState);
      List<string> cards = new List<string>();
      if (string.IsNullOrEmpty(GameCore.userData.username))
        cards.Add("LoginCard");
      else {
        cards.Add("LogoutCard");
        cards.Add(null);
        cards.Add("NewGameCard");
        if (GameCore.userData.currentGameData != null)
          cards.Add("ContinueCard");
      }
      await hand.SetHands(cards);
    }

    public async void SwitchToInGameView() {
      // 收起手牌 -> 拉近镜头 -> 生成手牌
      messiah.SwitchState(GameState.InGameState);
      await hand.SetHands(null);
    }

#if DEVELOPMENT_BUILD
    private TextMeshProUGUI debugPanelText;

    public async Task SetupDebugPanel() {
      var debugPanelPrefab = await Addressables.LoadAssetAsync<GameObject>("Assets/1. Data/3. Prefab/DebugPanel.prefab").Task;
      debugPanelText = Instantiate(debugPanelPrefab, transform).GetComponent<TextMeshProUGUI>();
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