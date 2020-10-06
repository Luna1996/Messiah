namespace Messiah.UI {
  using UnityEngine;
  using UnityEngine.UI;
  using DG.Tweening;
  using Messiah.Logic;
  using Messiah.Logic.GameCoreNS;
  using Coffee.UIExtensions;

  public class InGameView : MonoBehaviour {
    RectTransform top;
    RectTransform bottom;
    float topY;
    float bottomY;


    async void Start() {
      top = (RectTransform)transform.Find("TopBar");
      topY = top.anchoredPosition.y;
      bottom = (RectTransform)transform.Find("BottomBar");
      bottomY = bottom.anchoredPosition.y;
      top.DOAnchorPosY(0, 0.5f);
      bottom.DOAnchorPosY(0, 0.5f);
      await System.Threading.Tasks.Task.Delay(500);
      if (GameCore.userData.currentGameData == null)
        GameCore.userData.currentGameData = GameData.NewGameData();
    }
  }
}