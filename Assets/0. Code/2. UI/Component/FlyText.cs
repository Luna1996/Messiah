namespace Messiah.UI {
  using UnityEngine;
  using UnityEngine.UI;
  using DG.Tweening;

  public class FlyText : MonoBehaviour {
    static Vector3 escale = new Vector3(0.7f, 0.7f, 1);
    public async void Fly(string str, bool dir) {
      var text = GetComponent<Text>();
      text.text = str;
      var rect = ((RectTransform)transform);
      var y = rect.anchoredPosition.y;
      if (dir) rect.DOAnchorPosY(y + 100, 1).SetEase(Ease.Linear);//上
      else rect.DOAnchorPosY(y - 100, 1).SetEase(Ease.Linear);//下
      transform.DOScale(escale, 1).SetEase(Ease.Linear);
      await text.DOFade(0, 1).SetEase(Ease.Linear).AsyncWaitForCompletion();
      Destroy(gameObject);
    }
  }
}