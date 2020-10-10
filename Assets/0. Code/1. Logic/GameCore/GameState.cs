namespace Messiah.Logic.GameCoreNS {
  public enum GameState {
    OutGameState,
    InGameState,
    ConsumePhase,
    EventPhase,
    MainPhase,
    EndingPhase,
  }

  public enum GameStateTrigger {
    GameStart,
    NextPhase,
    GameEnd,
    Back
  }
}