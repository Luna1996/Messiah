namespace Messiah.UI {
  using UnityEngine;
  using UnityEngine.UI;
  using DG.Tweening;
  using Utility;
  using Logic;
  using Logic.GameCoreNS;

  public class FogView : MonoBehaviour {
    public new Image light;
    public float currentScale = 0.25f;

    void Start() {
      EventService.Listen(GameEvent.EnterInGameState, DoFogShake);
      EventService.ListenWithArg<int>(GameEvent.IG_ResourceModify, OnResourceChange);
    }

    void OnDestroy() {
      EventService.Ignore(GameEvent.EnterInGameState, DoFogShake);
      EventService.IgnoreWithArg<int>(GameEvent.IG_ResourceModify, OnResourceChange);
    }

    static int[] energyThreshold = new int[] { 10, 50, 100, 200, 300, 500, 1000 };
    static float[] scaleLevel = new float[] { 0.25f, 0.27f, 0.3f, 0.4f, 0.6f, 0.8f, 1f, 1.2f };
    void OnResourceChange(int rt) {
      if ((ResourceType)rt == ResourceType.Mine) {
        var mine = GameManager.GetResource(ResourceType.Mine);
        int i;
        for (i = 0; i < energyThreshold.Length; i++)
          if (mine < energyThreshold[i])
            break;
        currentScale = scaleLevel[i];
      }
    }

    async void DoFogShake() {
      while (gameObject.activeInHierarchy) {
        var s1 = new Vector3(currentScale, currentScale, 1);
        var s2 = new Vector3(currentScale + 0.03f, currentScale + 0.03f, 1);
        light.DOFade(1, 2.5f);
        await transform.DOScale(s2, 2.5f).AsyncWaitForCompletion();
        light.DOFade(0.2f, 2.5f);
        await transform.DOScale(s1, 2.5f).AsyncWaitForCompletion();
      }
    }
  }
}