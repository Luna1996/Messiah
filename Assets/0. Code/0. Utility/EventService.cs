namespace Messiah.Utility {
  using System;
  using System.Collections.Generic;

  public static class EventService {
    private static Dictionary<Enum, Delegate> actions = new Dictionary<Enum, Delegate>();

    #region 零
    public static void Notify(Enum id) {
      Delegate callbacks;
      if (actions.TryGetValue(id, out callbacks))
        (callbacks as Action).Invoke();
    }

    public static void Listen(Enum id, Action callback) {
      Delegate callbacks;
      if (actions.TryGetValue(id, out callbacks)) {
        var action = callbacks as Action;
        action += (Action)(callback as Delegate);
        actions[id] = action;
      } else
        actions.Add(id, callback);
    }

    public static void Ignore(Enum id, Action callback = null) {
      Delegate callbacks;
      if (actions.TryGetValue(id, out callbacks)) {

        if (callback == null) {
          actions.Remove(id);
          return;
        }

        var action = (callbacks as Action);
        action -= callback;
        if (action == null) {
          actions.Remove(id);
        } else { actions[id] = action; }
      }
    }
    #endregion

    #region 一
    public static void NotifyWithArg(Enum id, object a) {
      Delegate callbacks;
      if (actions.TryGetValue(id, out callbacks))
        (callbacks as Action<object>)(a);
    }

    public static void ListenWithArg(Enum id, Action<object> callback) {
      Delegate callbacks;
      if (actions.TryGetValue(id, out callbacks)) {
        var action = callbacks as Action<object>;
        action += callback;
        actions[id] = action;
      } else
        actions.Add(id, callback);
    }

    public static void IgnoreWithArg(Enum id, Action<object> callback = null) {
      Delegate callbacks;
      if (actions.TryGetValue(id, out callbacks)) {
        if (callback == null) {
          actions.Remove(id);
          return;
        }

        var action = callbacks as Action<object>;
        action -= callback;
        if (action == null)
          actions.Remove(id);
        else actions[id] = action;
      }
    }
    #endregion
  }
}