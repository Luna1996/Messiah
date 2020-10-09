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

    public Transform buffpanel;
    public Transform charpanel;

    void Awake() {
      GameManager.cardOnFly = GetComponentInChildren<CardOnFly>();
      Logic.GameManager.gameData = GameCore.userData.currentGameData;
      LuaManager.lua.Global.Set("GameData", GameCore.userData.currentGameData);
      handView = GetComponentInChildren<HandView>();
      top = (RectTransform)transform.Find("TopBar");
      topY = top.anchoredPosition.y;
      bottom = (RectTransform)transform.Find("BottomBar");
      bottomY = bottom.anchoredPosition.y;

      EventService.ListenAsync(GameEvent.EnterMainPhase, OnTurnStart);
      EventService.ListenAsync(GameEvent.EnterConsumePhase, OnEnterConsumePhase);
      EventService.ListenAsync(GameEvent.EnterEventPhase, OnEnterEventPhase);
      EventService.Listen(GameEvent.IG_IdleWorkerChanged, OnHumanChanged);
      EventService.Listen(GameEvent.IG_MaxWorkerChanged, OnHumanChanged);
      EventService.ListenWithArg<int>(GameEvent.IG_ResourceModify, OnResourceChanged);
    }

    public async Task Show() {
      GameManager.inGameView = this;
      LuaManager.lua.Global.Set("HandView", handView);
      LuaManager.lua.Global.Set("InGameView", this);
      top.DOAnchorPosY(0, 0.5f);
      bottom.DOAnchorPosY(0, 0.5f);
      for (int i = 0; i < resouces.Length; i++)
        OnResourceChanged(i);
      OnHumanChanged();
      foreach (var buff in GameManager.gameData.buff)
        buff.SetUp();
      Debug.Log(LuaManager.GetLuaEvent("Event1011").currentState.name);
      await System.Threading.Tasks.Task.Delay(500);

      handView.Init();
      foreach (var card in GameManager.gameData.hands) {
        handView.AddCard(card);
        await Task.Delay(100);
      }
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
      // LuaManager.lua.DoString("CostModifiter = 0");
      EventService.Notify(GameEvent.IG_OnCostModifiterChanged);
      UserData.Save();
      await UIMask.LoadMask(transform, "NewDaySplash", 0.2f, 3);
      GameManager.DrawCard(GameManager.gameData.drawNum);
      await Task.Delay(500);
      await UIMask.UnloadMask(0.2f);
    }

    public void NextDay() {
      if (GameCore.FAM.State == GameState.MainPhase)
        GameCore.FAM.Fire(GameStateTrigger.NextPhase);
    }

    public async Task OnEnterConsumePhase() {
      if (DiscardPhaseView.NeedDiscard())
        await UIMask.LoadMask(transform, "DiscardPhaseView", 0.2f, 2);
      else
        GameCore.FAM.Fire(GameStateTrigger.NextPhase);
    }

    public async Task OnEnterEventPhase() {
      handView.transform.SetSiblingIndex(2);
      await UIMask.LoadMask(transform, "EventPhaseView", 0.2f, 3);
      await Task.Delay(500);
      await UIMask.UnloadMask(0.2f);
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

    public async void ToggleDrawPile() {
      if (CardSelectionView.view)
        CardSelectionView.view.Hide();
      else {
        var list = new List<string>(GameManager.gameData.drawPile);
        GameData.Shuffle(list);
        (await UIMask.LoadMask(transform, "CardSelectionView", 0.1f, 3))
          .GetComponent<CardSelectionView>()
          .Show(list, "抽 牌 堆");
      }
    }

    public async void ToggleDiscardPile() {
      if (CardSelectionView.view)
        CardSelectionView.view.Hide();
      else
        (await UIMask.LoadMask(transform, "CardSelectionView", 0.1f, 3))
          .GetComponent<CardSelectionView>()
          .Show(GameManager.gameData.discardPile, "弃 牌 堆");
    }

    public async void ToggleExilePile() {
      if (CardSelectionView.view)
        CardSelectionView.view.Hide();
      else
        (await UIMask.LoadMask(transform, "CardSelectionView", 0.1f, 3))
          .GetComponent<CardSelectionView>()
          .Show(GameManager.gameData.exilePile, "放 逐 区");
    }

    public async void ToggleBuildingPanel() {
      if (BuildingView.view)
        BuildingView.view.Hide();
      else
        (await UIMask.LoadMask(transform, "BuildingView", 0.1f, 3))
          .GetComponent<BuildingView>()
          .Show();
    }

    public GameObject tips;
    public Text tiptext;

    public void ShowTip(Vector3 pos, string text) {
      var d = Vector3.zero;
      if (pos.x < 0) d.x = 0.05f;
      else d.x = -0.05f;
      if (pos.y < 0) d.y = 0.05f;
      else d.y = -0.05f;
      tips.transform.position = pos + d;
      tiptext.text = text;
      tips.SetActive(true);
    }

    public void HideTip() {
      tips.SetActive(false);
    }

    public Text human;
    public Image humanBar;
    public void OnHumanChanged() {
      var max = GameManager.gameData.maxWorker;
      var idle = GameManager.gameData.idleWorker;
      humanBar.DOFillAmount((float)idle / (float)max, 0.2f);
      human.text = idle + " / " + max;
    }
  }
}