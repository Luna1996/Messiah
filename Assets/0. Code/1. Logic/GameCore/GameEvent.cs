namespace Messiah.Logic.GameCoreNS {
  public enum GameEvent {
    EnterOutGameState,
    ExitOutGameState,
    EnterInGameState,
    ExitInGameState,
    EnterConsumePhase,
    ExitConsumePhase,
    EnterEventPhase,
    ExitEventPhase,
    EnterMainPhase,
    ExitMainPhase,
    EnterEndingPhase,
    ExitEndingPhase,

    ResourceModify,

    Game_DrawCard,
    Game_MaxWorkerChanged,
    Game_IdleWorkerChanged,
    Game_OccupiedWorkerChanged,
    Game_SickWorkerChanged,
  }
}