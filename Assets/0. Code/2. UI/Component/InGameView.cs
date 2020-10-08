#pragma warning disable 4014
namespace Messiah.UI {
  using Coffee.UIExtensions;
  using DG.Tweening;
  using Messiah.Logic;
  using Messiah.Logic.GameCoreNS;
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using UnityEngine;
  using UnityEngine.UI;
  using Utility;

  public class InGameView : MonoBehaviour {
    RectTransform top;
    RectTransform bottom;
    float topY;
    float bottomY;

    HandView handView;

    public GameObject[] resouces;

    async void Awake() {
      if (GameCore.userData.currentGameData == null) {
        var gameData = GameData.NewGameData();
        GameCore.userData.currentGameData = gameData;
      }
      Logic.GameManager.gameData = GameCore.userData.currentGameData;
      LuaManager.lua.Global.Set("GameData", GameCore.userData.currentGameData);
      handView = GetComponentInChildren<HandView>();
      top = (RectTransform)transform.Find("TopBar");
      topY = top.anchoredPosition.y;
      bottom = (RectTransform)transform.Find("BottomBar");
      bottomY = bottom.anchoredPosition.y;

      for (int i = 0; i < resouces.Length; i++)
        OnResourceChanged(i);

      foreach (var card in GameManager.gameData.hands) {
        handView.AddCard(card);
        await Task.Delay(100);
      }

      EventService.ListenAsync(GameEvent.EnterMainPhase, OnTurnStart);
      EventService.ListenAsync(GameEvent.EnterConsumePhase, DiscardHand);
      EventService.ListenAsync(GameEvent.EnterEventPhase, ResolveRandomEvents);
      EventService.ListenWithArg<int>(GameEvent.IG_ResourceModify, OnResourceChanged);
    }

    public async Task Show() {
      LuaManager.lua.Global.Set("HandView", handView);
      LuaManager.lua.Global.Set("InGameView", this);
      top.DOAnchorPosY(0, 0.5f);
      bottom.DOAnchorPosY(0, 0.5f);
      await System.Threading.Tasks.Task.Delay(500);
      GameCore.FAM.Fire(GameStateTrigger.GameStart);
    }

    public async Task Hide() {
      LuaManager.lua.Global.Set("HandView", false);
      Logic.LuaManager.lua.Global.Set("InGameView", false);
      top.DOAnchorPosY(topY, 0.5f);
      bottom.DOAnchorPosY(bottomY, 0.5f);
      await System.Threading.Tasks.Task.Delay(500);
      Destroy(gameObject);
    }

    public async Task OnTurnStart() {
      UserData.Save();
      handView.transform.SetAsFirstSibling();
      await UIMask.LoadMask(transform, "NewDaySplash", 0.2f, 1);
      GameManager.DrawCard(GameManager.gameData.drawNum);
      await Task.Delay(500);
      await UIMask.UnloadMask(0.2f);
      handView.transform.SetAsFirstSibling();
    }

    public void NextDay() {
      if (GameCore.FAM.State == GameState.MainPhase)
        GameCore.FAM.Fire(GameStateTrigger.NextPhase);
    }

    public async Task DiscardHand() {
      handView.transform.SetAsFirstSibling();
      await UIMask.LoadMask(transform, "DiscardPhaseView", 0.2f, 1);
      await Task.Delay(1000);
      await UIMask.UnloadMask(0.2f);
      handView.transform.SetAsFirstSibling();
      GameCore.FAM.Fire(GameStateTrigger.NextPhase);
    }

    public async Task ResolveRandomEvents() {
      handView.transform.SetAsFirstSibling();
      await UIMask.LoadMask(transform, "EventPhaseView", 0.2f, 1);
      await Task.Delay(1000);
      await UIMask.UnloadMask(0.2f);
      handView.transform.SetAsFirstSibling();
      GameCore.FAM.Fire(GameStateTrigger.NextPhase);
    }

    static int[] count = { 0, 0, 0, 0, 0, 0 };
    static Vector3 bigscale = new Vector3(1.3f, 1.3f, 1);
    public async void OnResourceChanged(int i) {
      var newvalue = GameManager.gameData.resources[i];

      var go = resouces[i];
      var img = go.GetComponentInChildren<Image>();
      var txt = go.GetComponentInChildren<Text>();


      if (!string.IsNullOrEmpty(txt.text)) {
        var oldvalue = int.Parse(txt.text);
        txt.text = $"{newvalue}";
        if (oldvalue == newvalue) return;
        if (oldvalue > newvalue) {
          txt.color = Color.red;
          img.color = Color.red;
        } else if (oldvalue < newvalue) {
          txt.color = new Color(134f / 255f, 1, 248f / 255f);
          img.color = new Color(134f / 255f, 1, 248f / 255f);
        }
        count[i]++;
        ((RectTransform)go.transform).localScale = Vector3.one;
        go.transform.DOScale(bigscale, 0.25f).SetEase(Ease.OutQuad);
        await Task.Delay(250);
        go.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InQuad);
        await Task.Delay(250);
        count[i]--;
        if (count[i] == 0) {
          txt.color = Color.white;
          img.color = Color.white;
        }
      } else {
        txt.text = $"{newvalue}";
      }
    }

  }
}