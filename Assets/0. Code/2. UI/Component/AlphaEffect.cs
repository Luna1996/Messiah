namespace Messiah.UI {
  using DG.Tweening;
  using DG.Tweening.Core;
  using System.Threading.Tasks;
  using UnityEngine;
  using UnityEngine.UI;

  public class AlphaEffect : MonoBehaviour {
    Tween alpha;
    DOGetter<float> getter;
    DOSetter<float> setter;
    bool flag;

    public float duration;
    public float min;
    public float max;
    public bool playOnStar;

    void Start() {
      flag = true;
      var canvasGroup = GetComponent<CanvasGroup>();
      if (canvasGroup != null) {
        getter = () => canvasGroup.alpha;
        setter = (a) => canvasGroup.alpha = a;
        goto OK;
      }
      var rawImage = GetComponent<RawImage>();
      if (rawImage != null) {
        getter = () => rawImage.color.a;
        setter = (a) => rawImage.color = new Color(1, 1, 1, a);
        goto OK;
      }
      var image = GetComponent<Image>();
      if (image != null) {
        getter = () => image.color.a;
        setter = (a) => image.color = new Color(1, 1, 1, a);
        goto OK;
      }
      return;
    OK:
      if (playOnStar)
        DoLoop();
    }

    public async Task Show(float d) {
      if (getter == null) Start();
      var canvasGroup = GetComponent<CanvasGroup>();
      canvasGroup.alpha = 0;
      alpha = DOTween.To(getter, setter, 1, d);
      await alpha.AsyncWaitForCompletion();
    }

    public async Task Hide(float d) {
      if (getter == null) Start();
      alpha = DOTween.To(getter, setter, 0, d);
      await alpha.AsyncWaitForCompletion();
    }

    public async void DoLoop() {
      while (flag) {
        var t = Random.Range(duration, duration + 1);
        alpha = DOTween.To(getter, setter, min, t);
        await alpha.AsyncWaitForCompletion();
        if (!flag) break;
        t = Random.Range(duration, duration + 1);
        alpha = DOTween.To(getter, setter, max, t);
        await alpha.AsyncWaitForCompletion();
      }
    }

    void OnDestroy() {
      flag = false;
      alpha.Kill();
    }
  }
}