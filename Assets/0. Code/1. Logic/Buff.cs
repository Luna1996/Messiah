namespace Messiah.Logic {
  using System;
  using Utility;
  using XLua;

  public enum BuffType {
    Repeat,
    OneShot,
  }

  public class Buff {
    Enum id;
    BuffType buffType;
    int time;
    LuaFunction callback;
    int t;

    public Buff(Enum id, BuffType buffType, int time, LuaFunction callback) {
      t = 0;
      this.id = id;
      this.buffType = buffType;
      this.time = time;
      this.callback = callback;
      EventService.Listen(id, CallBack);
    }

    public void CallBack() {
      t++;
      if (buffType == BuffType.Repeat || t == time)
        callback.Call(t);
      if (t == time) {
        EventService.Ignore(id, CallBack);
      }
    }
  }
}