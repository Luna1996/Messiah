namespace Messiah.Logic {
  using System.Collections.Generic;

  public interface LuaEvent {
    LuaEventState currentState { get; set; }
    string[] choices();
    void choose(string choice);
  }
  public interface LuaEventState {
    string name { get; set; }
    string desc { get; set; }
    string image { get; set; }
    string[] choices { get; set; }
    LuaEventState choose(string choice);
  }
}