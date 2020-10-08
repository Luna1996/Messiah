#pragma warning disable 4014
namespace Messiah.UI {
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;
  using DG.Tweening;
  using Logic;
  using UnityEngine.UI;

  public class CardSelectionView : MonoBehaviour {
    public static CardSelectionView view;

    public Text title;
    public Transform content;

    RectTransform rect;
    float height;
    void Start() {
      view = this;
      rect = (RectTransform)transform;
      height = rect.GetLocalSize().y;
      rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, height);
    }

    public void Show(List<string> cards, string title = null, float d = 0.2f) {
      this.title.text = title;
      foreach (var card in cards) {
        var cardview = PrefabManager.Instanciate("Card", content).GetComponent<CardView>();
        cardview.SetLuaCard(card);
        cardview.inPanel = true;
      }
      rect.DOAnchorPosY(0, d);
    }

    public void Hide(float d = 0.2f) {
      rect.DOAnchorPosY(height, d).onComplete += () => Destroy(gameObject);
      if (UIMask.mask != null)
        UIMask.UnloadMask(d);
    }

    void Destroy() {
      view = null;
    }
  }
}
