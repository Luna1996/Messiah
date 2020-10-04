namespace Messiah.Logic.GameCoreNS {
  public enum GameState {
    InitState,
    OutGameState,
    InGameState,
    ConsumePhase,
    EventPhase,
    MainPhase,
    EndingPhase,
  }

  public enum GameStateTrigger {
    NoLastGameData,
    FoundLastGameData,
    Login,
    Logout,
    GameStart,
    NextPhase,
    GameEnd,
  }
}