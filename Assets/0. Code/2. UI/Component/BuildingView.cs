#pragma warning disable 4014
namespace Messiah.UI {
  using UnityEngine;
  using UnityEngine.UI;
  using DG.Tweening;
  using Logic;

  public class BuildingView : MonoBehaviour {
    public static BuildingView view;

    public CardScrollView avalible;
    public CardScrollView tabview;


    RectTransform rect;
    float height;
    void Start() {
      view = this;
      rect = (RectTransform)transform;
      height = rect.GetLocalSize().y;
      rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, height);
    }

    public void Show(float d = 0.2f) {
      avalible.SetCards(GameManager.gameData.buildingAvaliable, 0);
      ShowAquiredBuilding();
      rect.DOAnchorPosY(0, d);
    }

    public void ToggleView(Toggle t) {
      if (t.isOn) ShowAquiredBuilding();
      else ShowBuildingDeck();
    }

    public void ShowBuildingDeck() {
      var l = GameManager.gameData.buildingDeck;
      GameData.Shuffle(l);
      tabview.SetCards(l);
    }
    public void ShowAquiredBuilding() {
      tabview.SetCards(GameManager.gameData.buildingAcquired);
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