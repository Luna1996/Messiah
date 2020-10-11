#pragma warning disable 4014
namespace Messiah.UI {
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using UnityEngine;
  using DG.Tweening;
  using Logic;
  using UnityEngine.UI;
  using System.Collections.ObjectModel;

  public class CardSelectionView : UIMask {
    static CardSelectionView view;

    public Text title;
    public CardScrollView cardScrollView;

    RectTransform rect;
    float height;
    new async void Start() {
      view = this;
      rect = (RectTransform)transform.GetChild(0);
      height = rect.GetLocalSize().y;
      rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, height);
      await base.Start();
      rect.DOAnchorPosY(0, 0.2f);
    }

    public void SetCards(ObservableCollection<string> cards, string title) {
      this.title.text = title;
      cardScrollView.SetCards(cards);
    }

    public void CloseView() { Close(); }

    public static async Task ToggleView(Transform trans, ObservableCollection<string> cards, string title) {
      if (view) {
        if (view.title.text != title) {
          await view.rect.DOAnchorPosY(view.height, 0.2f).AsyncWaitForCompletion();
          view.SetCards(cards, title);
          await view.rect.DOAnchorPosY(0, 0.2f).AsyncWaitForCompletion();
        } else {
          await UnloadView();
        }
      } else {
        view = PrefabManager.Instanciate("CardSelectionView", trans).GetComponent<CardSelectionView>();
        view.SetCards(cards, title);
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
