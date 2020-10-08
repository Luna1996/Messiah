namespace Messiah.UI {
  using UnityEngine;
  using DG.Tweening;
  using Messiah.Logic.GameCoreNS;
  using UnityEngine.EventSystems;
  using Logic;
  using System.Collections.Generic;

  public class MessiahView : MonoBehaviour {
    [SerializeField]
    public Vector2 ogpos;
    [SerializeField]
    public Vector3 ogscale;
    [SerializeField]
    public Vector2 igpos;
    [SerializeField]
    public Vector3 igscale;

    public async System.Threading.Tasks.Task SwitchState(GameState gs) {
      var trans = (RectTransform)transform;
      switch (gs) {
        case GameState.OutGameState:
          if (trans.anchoredPosition != ogpos) {
            trans.DOAnchorPos(ogpos, 2);
            trans.DOScale(ogscale, 2);
            await System.Threading.Tasks.Task.Delay(2000);
          }
          break;
        case GameState.InGameState:
          UpdateAllBuildings();
          if (trans.anchoredPosition != igpos) {
            trans.DOAnchorPos(igpos, 2);
            trans.DOScale(igscale, 2);
            await System.Threading.Tasks.Task.Delay(2000);
          }
          break;
      }
    }

    public void UpdateAllBuildings() {
      var count = transform.childCount;
      for (int i = 1; i < count; i++) transform.GetChild(i).gameObject.SetActive(false);
      foreach (var b in GameManager.gameData.buildingAcquired)
        transform.Find(b).gameObject.SetActive(true);
    }

    public void SetBuildingVisibility(string buildingname, bool flag) {
      transform.Find(buildingname).gameObject.SetActive(flag);
    }
  }
}