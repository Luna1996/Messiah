namespace Messiah.UI {
  using UnityEngine;
  using UnityEngine.UI;
  using Logic;
  using DG.Tweening;
  using System.Threading.Tasks;

  public class UIMask : MonoBehaviour {
    public static Transform mask;

    static CanvasGroup canvasGroup;

    public static async Task<GameObject> LoadMask(Transform trans, string prefab = null, float t = 0.1f, int index = -1) {
      var go = PrefabManager.Instanciate("UIMask", trans);
      GameObject ret = null;
      mask = go.transform;
      if (index != -1) mask.SetSiblingIndex(index);
      if (!string.IsNullOrEmpty(prefab))
        ret = PrefabManager.Instanciate(prefab, mask);
      canvasGroup = go.GetComponent<CanvasGroup>();
      canvasGroup.alpha = 0;
      await canvasGroup.DOFade(1, t).AsyncWaitForCompletion();
      return ret;
    }

    public static async Task UnloadMask(float t = 0.1f) {
      var tween = canvasGroup.DOFade(0, t);
      await tween.AsyncWaitForCompletion();
      Destroy(mask.gameObject);
    }
  }
}