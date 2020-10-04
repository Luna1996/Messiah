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
        action += callback;
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
        }
      }
    }
    #endregion

    #region 一
    public static void Notify<A>(Enum id, A a) {
      Delegate callbacks;
      if (actions.TryGetValue(id, out callbacks))
        (callbacks as Action<A>)(a);
    }

    public static void Listen<A>(Enum id, Action<A> callback) {
      Delegate callbacks;
      if (actions.TryGetValue(id, out callbacks)) {
        var action = callbacks as Action<A>;
        action += callback;
      } else
        actions.Add(id, callback);
    }

    public static void Ignore<A>(Enum id, Action<A> callback = null) {
      Delegate callbacks;
      if (actions.TryGetValue(id, out callbacks)) {
        if (callback == null) {
          actions.Remove(id);
          return;
        }

        var action = callbacks as Action<A>;
        action -= callback;
        if (action == null)
          actions.Remove(id);
      }
    }
    #endregion
  }
}