namespace Messiah.UI {
  using UnityEngine;
  using UnityEngine.UI;
  using Logic;
  using DG.Tweening;
  using System.Threading.Tasks;

  public class UIMask : MonoBehaviour {
    public static Transform mask;

    static CanvasGroup canvasGroup;

    public static async Task LoadMask(Transform trans, string prefab = null, float t = 0.1f, int index = -1) {
      var go = PrefabManager.Instanciate("UIMask", trans);
      mask = go.transform;
      if (index != -1) mask.SetSiblingIndex(index);
      if (!string.IsNullOrEmpty(prefab))
        PrefabManager.Instanciate(prefab, mask);
      canvasGroup = go.GetComponent<CanvasGroup>();
      canvasGroup.alpha = 0;
      canvasGroup.DOFade(1, t);
      await Task.Delay((int)(t * 1000));
    }

    public static async Task UnloadMask(float t = 0.1f) {
      canvasGroup.DOFade(0, t);
      await Task.Delay((int)(t * 1000));
      GameObject.Destroy(mask.gameObject);
    }
  }
}