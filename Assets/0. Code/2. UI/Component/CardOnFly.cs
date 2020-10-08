namespace Messiah.UI {
  using Logic;
  using UnityEngine;
  using UnityEngine.UI;
  using DG.Tweening;
  using System.Threading.Tasks;

  public class CardOnFly : MonoBehaviour {
    public static float ratio = 0.1f;
    public static Vector3 EndScale = new Vector3(0.2f, 0.2f, 1);
    public static Vector3 MidScale = new Vector3(1, 1, 1);
    public RectTransform drawPile;
    public RectTransform discardPile;
    public RectTransform exilePile;

    void Start() {
      GameManager.cardOnFly = this;
    }

    public async void SendCardTo(CardView cardView, CardLocation loc, float d = 0.5f) {
      cardView.canPlay = false;
      cardView.transform.SetParent(transform);
      Vector3 endpos;
      Vector3 endscale;
      Quaternion endquat;
      GetLoc(loc, out endpos, out endscale, out endquat);
      cardView.transform.DOMove(endpos, d).SetEase(Ease.Linear);
      cardView.transform.DORotateQuaternion(endquat, d).SetEase(Ease.Linear);
      cardView.transform.DOScale(endscale, d).SetEase(Ease.Linear);
      await Task.Delay((int)(d * 1000));
      GameObject.Destroy(cardView.gameObject);
    }

    public async void SendCardFromTo(string cardName, CardLocation fromloc, CardLocation toloc, float d = 0.5f) {
      var card = PrefabManager.Instanciate("Card", transform);
      Vector3 pos, scale;
      Quaternion quat;
      GetLoc(fromloc, out pos, out scale, out quat);
      card.transform.position = pos;
      card.transform.rotation = quat;
      ((RectTransform)(card.transform)).localScale = scale;

      GetLocInBetween(fromloc, toloc, out pos, out scale, out quat);
      card.transform.DOMove(pos, d / 2).SetEase(Ease.Linear);
      card.transform.DOScale(scale, d / 2).SetEase(Ease.Linear);
      card.transform.DORotateQuaternion(quat, d / 2).SetEase(Ease.Linear);
      await Task.Delay((int)(d / 2 * 1000));

      GetLoc(toloc, out pos, out scale, out quat);
      card.transform.DOMove(pos, d / 2).SetEase(Ease.Linear);
      card.transform.DOScale(scale, d / 2).SetEase(Ease.Linear);
      card.transform.DORotateQuaternion(quat, d / 2).SetEase(Ease.Linear);
      await Task.Delay((int)(d / 2 * 1000));

      GameObject.Destroy(card);
    }

    public void GetLoc(CardLocation loc, out Vector3 pos, out Vector3 scale, out Quaternion quat) {
      quat = Quaternion.identity;
      scale = EndScale;
      switch (loc) {
        case CardLocation.DrawPile:
          pos = drawPile.position;
          break;
        case CardLocation.DiscardPile:
          pos = discardPile.position;
          break;
        case CardLocation.ExilePile:
          pos = exilePile.position;
          break;
        case CardLocation.Camera:
          pos = GameManager.handView.transData.cardgen;
          scale = Vector3.one;
          break;
        default:
          pos = Vector3.zero;
          break;
      }
    }

    public void GetLocInBetween(CardLocation loc1, CardLocation loc2, out Vector3 pos, out Vector3 scale, out Quaternion quat) {
      Vector3 pos1, pos2, scale1, scale2;
      GetLoc(loc1, out pos1, out scale1, out quat);
      GetLoc(loc2, out pos2, out scale2, out quat);
      pos = (pos1 + pos2) / 2;
      scale = (scale1 + scale2) / 2;
      pos = pos * ratio + transform.position * (1 - ratio);
      scale = scale * ratio + MidScale * (1 - ratio);
      quat = Quaternion.identity;
    }

    public async void test() {
      if (UIMask.mask == null)
        await UIMask.LoadMask(GameManager.viewManager.transform, "CardSelectionView", 0.2f);
      else
        await UIMask.UnloadMask(0.2f);
    }
  }

  public enum CardLocation {
    DrawPile,
    DiscardPile,
    ExilePile,
    Camera
  }
}