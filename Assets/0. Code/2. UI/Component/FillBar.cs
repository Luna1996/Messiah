namespace Messiah.UI {
  using UnityEngine;
  using DG.Tweening;

  [AddComponentMenu("弥赛亚/计量条")]
  public class FillBar : MonoBehaviour {
    [Range(0, 1)]
    [SerializeField]
    public float percent = 0;

    RectTransform bg;
    RectTransform fg;

    void Reset() { SetPercent(0); }
    void Start() { UpdatePercent(); }

    void GrabComp() {
      bg = transform.Find("背景条") as RectTransform;
      fg = transform.Find("背景条/前景条") as RectTransform;
    }

    public void SetPercent(float p) { percent = p; UpdatePercent(); }
    public void UpdatePercent() {
      if (bg == null || fg == null) GrabComp();
      var rect = bg.GetLocalSize();
      fg.DOSizeDelta(new Vector2((rect.x + rect.y) * percent, rect.y), 0.2f);
    }
  }
}