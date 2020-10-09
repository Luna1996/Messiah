namespace Messiah.UI {
  using UnityEngine;
  using DG.Tweening;
  using Messiah.Logic.GameCoreNS;
  using UnityEngine.EventSystems;
  using Logic;
  using System.Collections.Generic;
  using Coffee.UIExtensions;
  using System.Threading.Tasks;

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
        transform.Find(b)?.gameObject.SetActive(true);
    }

    static Vector3 zoomInScale = new Vector3(3, 3, 1);
    public async void Focus(string bname) {
      var go = transform.Find(bname).gameObject;
      Vector3 d = transform.position - go.transform.position;
      d.z = transform.position.z;
      var size = ((RectTransform)transform).GetLocalSize();
      ((RectTransform)transform).pivot = ((RectTransform)go.transform).anchoredPosition/size;
      transform.DOMove(d, 0.5f);
      await ((RectTransform)transform).DOScale(ogscale, 0.5f).AsyncWaitForCompletion();
    }

    public async void Build(string bname) {
      var go = transform.Find(bname).gameObject;
      go.SetActive(true);
      var shiny = go.AddComponent<UIShiny>();
      shiny.duration = 1;
      shiny.Play();
      await Task.Delay(1000);
    }
  }
}