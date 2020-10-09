using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public static class MonoBehaviourEx {
  public static void PassEvent<T>(this MonoBehaviour self, PointerEventData data, ExecuteEvents.EventFunction<T> function)
        where T : IEventSystemHandler {
    List<RaycastResult> results = new List<RaycastResult>();
    EventSystem.current.RaycastAll(data, results);
    GameObject current = self.gameObject;
    for (int i = 0; i < results.Count; ++i) {
      if (current != results[i].gameObject) {
        ExecuteEvents.Execute(results[i].gameObject, data, function);
      }
    }
  }
}