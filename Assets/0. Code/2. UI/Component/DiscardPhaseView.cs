namespace Messiah.UI {
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;
  using UnityEngine.UI;
  using Utility;
  using Logic;
  using Logic.GameCoreNS;
  using System.Threading.Tasks;

  public class DiscardPhaseView : MonoBehaviour {
    public Text desc;
    public Button confirm;

    int keepNum;
    int discNum;

    List<CardView> cards;

    void Start() {
      cards = new List<CardView>(GameManager.handView.hands);
      foreach (var card in cards) card.inPanel = true;
      keepNum = GameManager.gameData.keepNum;
      discNum = GameManager.handView.hands.Count - keepNum;
      UpdataDesc();
      EventService.ListenWithArg<CardView>(GameEvent.IG_OnCardSelectionChanged, OnCardSelectionChanged);
    }

    void UpdataDesc() {
      desc.text = $"当前手牌上限为<color=green>{keepNum}</color>\n还需丢弃<color=red>{discNum}</color>张牌";
      if (discNum == 0) confirm.interactable = true;
      else confirm.interactable = false;
      foreach (var card in cards)
        card.canSelect = !confirm.interactable;
    }

    void OnCardSelectionChanged(CardView cv) {
      if (cv.mask) discNum--;
      else discNum++;
      UpdataDesc();
    }

    public async void OnConfirm() {
      foreach (var card in cards) {
        card.inPanel = false;
        if (card.mask) {
          GameManager.Discard(card.luacard);
          await Task.Delay(100);
        }
      }
      EventService.IgnoreWithArg<CardView>(GameEvent.IG_OnCardSelectionChanged, OnCardSelectionChanged);
      await UIMask.UnloadMask(0.2f);
      GameCore.FAM.Fire(GameStateTrigger.NextPhase);
    }

    public static bool NeedDiscard() {
      var keepNum = GameManager.gameData.keepNum;
      var discNum = GameManager.handView.hands.Count - keepNum;
      return discNum != 0;
    }

  }

}