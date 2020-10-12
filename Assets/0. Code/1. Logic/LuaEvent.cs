namespace Messiah.Logic {
  using System;
  using System.Threading.Tasks;

  public interface LuaEvent {
    LuaEventState initState { get; set; }
    LuaEventState currentState { get; set; }
    string[] choices();
    Func<Task> choose(string choice);
  }
  public interface LuaEventState {
    string name { get; set; }
    string desc { get; set; }
    string image { get; set; }
    string[] choices { get; set; }
    LuaEventState choose(string choice);
  }
}