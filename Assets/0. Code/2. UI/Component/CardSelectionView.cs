#pragma warning disable 4014
namespace Messiah.UI {
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using UnityEngine;
  using DG.Tweening;
  using Logic;
  using UnityEngine.UI;
  using Utility;
  using Logic.GameCoreNS;
  using System.Threading;

  public class CardSelectionView : UIMask {
    static CardSelectionView view;

    public Text title;
    public CardScrollView cardScrollView;
    public Button confirm;

    RectTransform rect;
    float height;
    int max;
    XLua.LuaFunction cb;
    XLua.LuaFunction canchoose;

    List<Card> selection;
    CancellationTokenSource cs;

    new async void Start() {
      view = this;
      rect = (RectTransform)transform.GetChild(0);
      height = rect.GetLocalSize().y;
      rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, height);
      await base.Start();
      rect.DOAnchorPosY(0, 0.2f);
    }

    public void SetCards(IList<string> cards, string title) {
      this.title.text = title;
      cardScrollView.SetCards(cards);
    }

    public void SetCB(int max, XLua.LuaFunction cb, XLua.LuaFunction canfun) {
      this.max = max;
      this.cb = cb;
      this.canchoose = canfun;
      if (max == -1 || cb == null) {
        EventService.IgnoreWithArg<CardView>(GameEvent.IG_OnCardSelectionChanged, OnCardSelectionChanged);
        confirm.gameObject.SetActive(false);
        selection = null;
        foreach (var view in cardScrollView.cardViews)
          view.canSelect = false;
      } else {
        EventService.ListenWithArg<CardView>(GameEvent.IG_OnCardSelectionChanged, OnCardSelectionChanged);
        confirm.gameObject.SetActive(true);
        confirm.interactable = false;
        selection = new List<Card>();
        foreach (var view in cardScrollView.cardViews) {
          view.canSelect = true;
          view.canSelectFunc = canfun;
          view.GetComponent<Image>().raycastTarget = true;
        }
      }
    }

    public void OnCardSelectionChanged(CardView cv) {
      if (cv.mask) selection.Add(cv.luacard);
      else selection.Remove(cv.luacard);
      if ((max == 0 && selection.Count > 0) || (max > 0 && max == selection.Count)) {
        confirm.interactable = true;
      } else confirm.interactable = false;
    }

    public void OnConfirm() {
      cb?.Call(selection);
      UnloadView();
      if (cs != null) cs.Cancel();
    }

    public void CloseView() {
      cb?.Call(null);
      UnloadView();
      if (cs != null) cs.Cancel();
    }

    public static async Task ToggleView(Transform trans, IList<string> cards, string title, int max = -1, XLua.LuaFunction cb = null, XLua.LuaFunction canchoose = null) {
      if (view) {
        if (view.title.text != title) {
          await view.rect.DOAnchorPosY(view.height, 0.2f).AsyncWaitForCompletion();
          view.SetCards(cards, title);
          view.SetCB(max, cb, canchoose);
          await view.rect.DOAnchorPosY(0, 0.2f).AsyncWaitForCompletion();
        } else {
          await UnloadView();
          return;
        }
      } else {
        view = PrefabManager.Instanciate("CardSelectionView", trans).GetComponent<CardSelectionView>();
        view.SetCards(cards, title);
        view.SetCB(max, cb, canchoose);
      }
      if (max != -1) {
        view.cs = new CancellationTokenSource();
        try {
          await Task.Delay(-1, view.cs.Token);
        } catch (System.Exception) {
        }
      }
    }

    public static async Task UnloadView() {
      if (view) {
        view.Close();
        await view.rect.DOAnchorPosY(view.height, 0.2f).AsyncWaitForCompletion();
        GameObject.Destroy(view);
        view = null;
      }
    }

  }
}
