namespace Messiah.UI {
  using UnityEngine;
  using Logic;
  using UnityEngine.EventSystems;
  using System;

  public class Tip : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    [TextArea]
    public string text = "提示文本";
    public void OnPointerDown(PointerEventData p) {
      GameManager.inGameView.ShowTip(p.pointerCurrentRaycast.worldPosition, text);
    }
    public void OnPointerUp(PointerEventData p) {
      GameManager.inGameView.HideTip();
    }
  }
}