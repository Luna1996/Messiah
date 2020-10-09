namespace Messiah.UI {
  using UnityEngine;
  using UnityEngine.UI;
  using DG.Tweening;
  using Utility;
  using Logic;
  using Logic.GameCoreNS;

  public class FogView : MonoBehaviour {
    void Start() {
      EventService.Listen(GameEvent.IG_ResourceModify, OnResourceChange);
    }

    static int[] energyThreshold = new int[] { 10, 50, 100, 200, 300, 500, 1000 };
    static float[] scaleLevel = new float[] { };
    void OnResourceChange() { }
  }
}