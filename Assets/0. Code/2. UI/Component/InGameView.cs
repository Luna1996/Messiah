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
    public TextFlyer[] textFlyers;

    public Transform buffpanel;
    public Transform charpanel;
    public Transform underHand;
    public Transform aboveHand;

    Sprite nextOn;
    Sprite nextOff;
    public Image nextBtn;

    public Text dayNum;
    public Text deadNum;

    void Awake() {
      GameManager.cardOnFly = GetComponentInChildren<CardOnFly>();
      Logic.GameManager.gameData = GameCore.userData.currentGameData;
      LuaManager.lua.Global.Set("GameData", GameCore.userData.currentGameData);
      handView = GetComponentInChildren<HandView>();
      top = (RectTransform)transform.Find("TopBar");
      topY = top.anchoredPosition.y;
      bottom = (RectTransform)transform.Find("BottomBar");
      bottomY = bottom.anchoredPosition.y;

      nextOn = AtlasManager.GetSprite("下一天s");
      nextOff = AtlasManager.GetSprite("下一天");

      EventService.Listen(GameEvent.EnterMainPhase, OnTurnStart);
      EventService.Listen(GameEvent.EnterConsumePhase, OnEnterConsumePhase);
      EventService.ListenAsync(GameEvent.EnterEventPhase, OnEnterEventPhase);
      EventService.Listen(GameEvent.IG_IdleWorkerChanged, OnHumanChanged);
      EventService.Listen(GameEvent.IG_MaxWorkerChanged, OnHumanChanged);
      EventService.ListenWithArg<int>(GameEvent.IG_ResourceModify, OnResourceChanged);
      EventService.Listen(GameEvent.EnterEndingPhase, OnEnterEndingPhase);
      EventService.Listen(GameEvent.IG_DeadWorkerChanged, OnDeadWorkerChanged);
    }

    void OnDestroy() {
      EventService.Ignore(GameEvent.EnterMainPhase, OnTurnStart);
      EventService.Ignore(GameEvent.EnterConsumePhase, OnEnterConsumePhase);
      EventService.IgnoreAsync(GameEvent.EnterEventPhase, OnEnterEventPhase);
      EventService.Ignore(GameEvent.IG_IdleWorkerChanged, OnHumanChanged);
      EventService.Ignore(GameEvent.IG_MaxWorkerChanged, OnHumanChanged);
      EventService.IgnoreWithArg<int>(GameEvent.IG_ResourceModify, OnResourceChanged);
      EventService.Ignore(GameEvent.EnterEndingPhase, OnEnterEndingPhase);
    }

    public async Task Show() {
      GameManager.inGameView = this;
      LuaManager.lua.Global.Set("HandView", handView);
      LuaManager.lua.Global.Set("InGameView", this);
      top.DOAnchorPosY(0, 0.5f);
      await bottom.DOAnchorPosY(0, 0.5f).AsyncWaitForCompletion();
      for (int i = 0; i < resouces.Length; i++)
        OnResourceChanged(i);
      OnHumanChanged();
      foreach (var buff in GameManager.gameData.buff)
        buff.SetUp();
      OnDeadWorkerChanged();

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
      foreach (var cardview in handView.hands)
        cardview.Dissolve();
      top.DOAnchorPosY(topY, 0.5f);
      await bottom.DOAnchorPosY(bottomY, 0.5f).AsyncWaitForCompletion();
      Destroy(gameObject);
    }

    public void OnTurnStart() {
      ToggleNextDay(true);
      UserData.Save();
      PrefabManager.Instanciate("NewDaySplash", transform);
      GameManager.DrawCard(GameManager.gameData.drawNum);
    }

    public void NextDay() {
      ToggleNextDay(false);
      CardSelectionView.UnloadView();
      if (DiscardPhaseView.NeedDiscard())
        PrefabManager.Instanciate("DiscardPhaseView", underHand);
      else
        GameCore.FAM.Fire(GameStateTrigger.NextPhase);
    }

    public void ToggleNextDay(bool active) {
      nextBtn.sprite = active ? nextOn : nextOff;
      nextBtn.GetComponent<Button>().interactable = active;
    }

    public void OnEnterConsumePhase() {
      PrefabManager.Instanciate("ConsumePhaseView", aboveHand);
    }

    public async Task OnEnterEventPhase() {
      if (GameManager.ShouldTriggerEvent()) {
        PrefabManager.Instanciate("EventPhaseView", transform);
        await Task.Delay(1000);
        PrefabManager.Instanciate("EventPhasePanel", aboveHand);
      } else
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
          txt.color = Color.green;
          img.color = Color.green;
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

    public void ToggleDrawPile() {
      var list = new List<string>(GameManager.gameData.drawPile);
      GameData.Shuffle(list);
      CardSelectionView.ToggleView(aboveHand, list, "抽 牌 堆");
    }

    public void ToggleDiscardPile() {
      CardSelectionView.ToggleView(aboveHand, GameManager.gameData.discardPile, "弃 牌 堆");
    }

    public void ToggleExilePile() {
      CardSelectionView.ToggleView(aboveHand, GameManager.gameData.exilePile, "放 逐 区");
    }

    public void BackToOutGame() {
      GameCore.FAM.Fire(GameStateTrigger.Back);
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
      tips.transform.parent.gameObject.SetActive(true);
    }

    public void HideTip() {
      tips.transform.parent.gameObject.SetActive(false);
    }

    public Text human;
    public Image humanBar;
    public void OnHumanChanged() {
      var max = GameManager.gameData.maxWorker;
      var idle = GameManager.gameData.idleWorker;
      humanBar.DOFillAmount((float)idle / (float)max, 0.2f);
      human.text = idle + " / " + max;
    }

    public void OnEnterEndingPhase() {
      PrefabManager.Instanciate("EndingPhasePanel", transform);
    }

    public async void OnDeadWorkerChanged() {
      deadNum.text = $"{GameManager.gameData.deadWorker}";
      await deadNum.transform.DOScale(new Vector3(1.1f, 1.1f, 1), 0.5f).SetEase(Ease.Linear).AsyncWaitForCompletion();
      await deadNum.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.Linear).AsyncWaitForCompletion();
    }
  }
}