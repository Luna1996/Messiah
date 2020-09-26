namespace Messiah.Utility {
  using System;
  using System.Collections.Generic;

  public class EventService {
    public static EventService INSTANCE = new EventService();
    private EventService() { }

    private Dictionary<Enum, Delegate> actions = new Dictionary<Enum, Delegate>();

    #region 零
    public void Notify(Enum id) {
      Delegate callbacks;
      if (actions.TryGetValue(id, out callbacks))
        (callbacks as Action).Invoke();
    }

    public void Listen(Enum id, Action callback) {
      Delegate callbacks;
      if (actions.TryGetValue(id, out callbacks)) {
        var action = callbacks as Action;
        action += callback;
      } else
        actions.Add(id, callback);
    }

    public void Ignore(Enum id, Action callback = null) {
      Delegate callbacks;
      if (actions.TryGetValue(id, out callbacks)) {

        if (callback == null) {
          actions.Remove(id);
          return;
        }

        var action = (callbacks as Action);
        action -= callback;
        if (action.GetInvocationList().Length == 0) {
          actions.Remove(id);
        }
      }
    }
    #endregion

    #region 一
    public void Notify<A>(Enum id, A a) {
      Delegate callbacks;
      if (actions.TryGetValue(id, out callbacks))
        (callbacks as Action<A>)(a);
    }

    public void Listen<A>(Enum id, Action<A> callback) {
      Delegate callbacks;
      if (actions.TryGetValue(id, out callbacks)) {
        var action = callbacks as Action<A>;
        action += callback;
      } else
        actions.Add(id, callback);
    }

    public void Ignore<A>(Enum id, Action<A> callback = null) {
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

    ~EventService() => actions.Clear();
  }
}