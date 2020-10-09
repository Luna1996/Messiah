using UnityEngine;

public static class RectTransformEx {
  static Vector3[] rect = new Vector3[4];
  public static Vector2 GetLocalSize(this RectTransform self) {
    self.GetLocalCorners(rect);
    return new Vector2(rect[3].x - rect[0].x, rect[1].y - rect[0].y);
  }
}