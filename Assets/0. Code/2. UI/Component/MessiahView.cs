namespace Messiah.UI {
  using UnityEngine;
  using DG.Tweening;
  using Messiah.Logic.GameCoreNS;
  using UnityEngine.EventSystems;
  using Logic;
  using System.Collections.Generic;
  using Coffee.UIExtensions;
  using System.Threading.Tasks;
  using UnityEngine.UI;

  public class MessiahView : MonoBehaviour, IBeginDragHandler, IDragHandler {
    [SerializeField]
    public Vector2 ogpos;
    [SerializeField]
    public Vector3 ogscale;
    [SerializeField]
    public Vector2 igpos;
    [SerializeField]
    public Vector3 igscale;

    public bool canMove;

    public Transform buildings;

    public async System.Threading.Tasks.Task SwitchState(GameState gs) {
      var trans = (RectTransform)transform;
      switch (gs) {
        case GameState.OutGameState:
          if (trans.anchoredPosition != ogpos) {
            canMove = false;
            trans.DOAnchorPos(ogpos, 2);
            trans.DOScale(ogscale, 2);
            await System.Threading.Tasks.Task.Delay(2000);
            canMove = true;
          }
          break;
        case GameState.InGameState:
          UpdateAllBuildings();
          if (trans.anchoredPosition != igpos) {
            canMove = false;
            trans.DOAnchorPos(igpos, 2);
            trans.DOScale(igscale, 2);
            await System.Threading.Tasks.Task.Delay(2000);
            canMove = true;
          }
          break;
      }
    }

    public void UpdateAllBuildings() {
      var count = buildings.childCount;
      for (int i = 1; i < count; i++) buildings.GetChild(i).gameObject.SetActive(false);
      foreach (var b in GameManager.gameData.buildingAcquired)
        buildings.Find(b)?.gameObject.SetActive(true);
    }

    static Vector3 center = new Vector3(0, 0, -9);
    static Vector3 zoomInScale = new Vector3(2, 2, 1);
    public async Task Focus(string bname) {
      var go = buildings.Find(bname).gameObject;
      var rt = (RectTransform)transform;
      var rto = (RectTransform)go.transform;
      var size = rt.GetLocalSize();
      rt.DOPivot(rto.anchoredPosition / size, 0.5f);
      transform.DOMove(center, 0.5f);
      await transform.DOScale(zoomInScale, 0.5f).AsyncWaitForCompletion();
    }

    public async void Build(string bname) {
      canMove = false;
      var rt = (RectTransform)transform;
      var opivot = rt.pivot;
      var opos = transform.position;
      var oscale = transform.localScale;
      await Focus(bname);
      var go = buildings.Find(bname).gameObject;
      var shiny = go.AddComponent<UIShiny>();
      if (!go.activeSelf) {
        go.SetActive(true);
        await go.GetComponent<RawImage>().DOFade(1, 0.5f).AsyncWaitForCompletion();
      } else {
        go.transform.DOShakeScale(1);
      }
      shiny.duration = 1;
      shiny.Play();
      await Task.Delay(1000);
      rt.DOPivot(opivot, 0.5f);
      transform.DOMove(opos, 0.5f);
      await transform.DOScale(oscale, 0.5f).AsyncWaitForCompletion();
      Destroy(shiny);
      canMove = true;
    }

    static Vector3 pos;
    public void OnBeginDrag(PointerEventData p) {
      if (!canMove) return;
      pos = transform.position;
    }

    public void OnDrag(PointerEventData p) {
      if (!canMove) return;
      var t = transform.position;
      t.x = (pos + p.pointerCurrentRaycast.worldPosition - p.pointerPressRaycast.worldPosition).x;
      if (t.x > -0.47 && t.x < 0.47)
        transform.position = t;
      // var d = new Vector3(p.delta.x, 0, 0);
      // transform.position += d;
    }
  }
}