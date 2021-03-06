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


    IG_OnCardSelectionChanged,
    IG_OnCostModifiterChanged,


    IG_ResourceModify,
    IG_DrawCard,
    IG_MaxWorkerChanged,
    IG_IdleWorkerChanged,
    IG_OccupiedWorkerChanged,
    IG_SickWorkerChanged,
    IG_DeadWorkerChanged,

    IG_WeatherCardDraw,
    IG_BuildingsBuit,
    IG_BasicCardPlay,
    IG_ResourceCardPlay,
    IG_MineEqualsZero,
  }
}