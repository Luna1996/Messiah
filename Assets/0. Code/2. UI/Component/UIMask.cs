namespace Messiah.UI {
  using UnityEngine;
  using DG.Tweening;
  using System.Threading.Tasks;

  public class UIMask : MonoBehaviour {
    protected CanvasGroup canvasGroup;
    protected float duration = 0.2f;

    protected async Task Start() {
      canvasGroup = gameObject.GetComponent<CanvasGroup>();
      canvasGroup.alpha = 0;
      await canvasGroup.DOFade(1, duration).AsyncWaitForCompletion();
    }

    public async Task Close(bool destroy = true) {
      await canvasGroup.DOFade(0, duration).AsyncWaitForCompletion();
      if (destroy)
        Destroy(gameObject);
    }
  }
}