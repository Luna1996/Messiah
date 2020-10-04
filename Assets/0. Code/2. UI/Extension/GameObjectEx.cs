using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class GameObjectEx {
  public static void ToggleSiblings(this GameObject self, bool show) {
    if (self.transform.parent == null)
      foreach (var go in self.scene.GetRootGameObjects()) {
        if (go == self) continue;
        if (!show) go.hideFlags |= HideFlags.HideInHierarchy;
        else go.hideFlags &= ~HideFlags.HideInHierarchy;
      }
    else
      foreach (Transform trans in self.transform.parent) {
        if (trans == self.transform) continue;
        if (!show) trans.gameObject.hideFlags |= HideFlags.HideInHierarchy;
        else trans.gameObject.hideFlags &= ~HideFlags.HideInHierarchy;
      }
  }
}