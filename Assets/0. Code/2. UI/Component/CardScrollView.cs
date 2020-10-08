namespace Messiah.UI {
  using System.Collections.Generic;
  using UnityEngine;
  using UnityEngine.UI;
  using Logic;
  using DG.Tweening;

  public class CardScrollView : MonoBehaviour {
    public ScrollRect scrollRect;

    public void SetCards(List<string> cards, int dir = 2) {
      scrollRect.horizontal = (dir & 1) != 0;
      scrollRect.vertical = (dir & 2) != 0;
      foreach (Transform trans in scrollRect.content.transform)
        Destroy(trans.gameObject);
      for (int i = 0; i < cards.Count; i++) {
        var cardview = PrefabManager.Instanciate("Card", scrollRect.content).GetComponent<CardView>();
        cardview.gameObject.name = $"{i}";
        cardview.SetLuaCard(cards[i]);
        cardview.inPanel = true;
      }
    }

    public void TakeOut(CardView cv, Transform trans) {
      cv.transform.SetParent(trans);
      var i = int.Parse(cv.gameObject.name);
      var go = new GameObject(cv.gameObject.name, typeof(RectTransform));
      go.transform.SetParent(scrollRect.content);
      go.transform.SetSiblingIndex(i);
      go.name = cv.gameObject.name;
    }

    public async void PutBack(CardView cv, float d = 0.2f) {
      var i = int.Parse(cv.gameObject.name);
      var go = scrollRect.content.GetChild(i);
      cv.transform.SetParent(gameObject.transform);
      cv.transform.DOMove(go.transform.position, d);
      cv.transform.DOScale(go.transform.localScale, d);
      await cv.transform.DORotateQuaternion(go.transform.rotation, d).AsyncWaitForCompletion();
      Destroy(go);
      cv.transform.SetParent(scrollRect.content.transform);
      cv.transform.SetSiblingIndex(i);
    }

  }
}