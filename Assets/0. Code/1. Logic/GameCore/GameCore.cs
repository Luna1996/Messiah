namespace Messiah.Logic.GameCoreNS {
  using UnityEngine;
  using Stateless;
  using Utility;

  public static class GameCore {

    public static StateMachine<GameState, GameStateTrigger> FAM;
    public static UserData userData;

    public static void Init() {
      FAM = new StateMachine<GameState, GameStateTrigger>(GameState.OutGameState);

      FAM.Configure(GameState.OutGameState)
        .Permit(GameStateTrigger.GameStart, GameState.InGameState)
        .OnEntry(() => EventService.Notify(GameEvent.EnterOutGameState))
        .OnExit(() => EventService.Notify(GameEvent.ExitOutGameState));

      FAM.Configure(GameState.InGameState)
        .Permit(GameStateTrigger.GameStart, GameState.MainPhase)
        .OnEntry(() => {
          EventService.Notify(GameEvent.EnterInGameState);
        })
        .OnExit(() => EventService.Notify(GameEvent.ExitInGameState));

      FAM.Configure(GameState.ConsumePhase)
        .SubstateOf(GameState.InGameState)
        .Permit(GameStateTrigger.NextPhase, GameState.EventPhase)
        .Permit(GameStateTrigger.GameEnd, GameState.EndingPhase)
        .OnEntry(() => EventService.Notify(GameEvent.EnterConsumePhase))
        .OnExit(() => EventService.Notify(GameEvent.ExitConsumePhase));

      FAM.Configure(GameState.EventPhase)
        .SubstateOf(GameState.InGameState)
        .Permit(GameStateTrigger.NextPhase, GameState.MainPhase)
        .Permit(GameStateTrigger.GameEnd, GameState.EndingPhase)
        .OnEntry(() => EventService.Notify(GameEvent.EnterEventPhase))
        .OnExit(() => EventService.Notify(GameEvent.ExitEventPhase));

      FAM.Configure(GameState.MainPhase)
        .SubstateOf(GameState.InGameState)
        .Permit(GameStateTrigger.NextPhase, GameState.ConsumePhase)
        .Permit(GameStateTrigger.GameEnd, GameState.EndingPhase)
        .OnEntry(() => EventService.Notify(GameEvent.EnterMainPhase))
        .OnExit(() => EventService.Notify(GameEvent.ExitMainPhase));

      FAM.Configure(GameState.EndingPhase)
        .SubstateOf(GameState.InGameState)
        .Permit(GameStateTrigger.GameStart, GameState.InGameState)
        .Permit(GameStateTrigger.GameEnd, GameState.OutGameState)
        .OnEntry(() => EventService.Notify(GameEvent.EnterEndingPhase))
        .OnExit(() => EventService.Notify(GameEvent.ExitEndingPhase));

#if DEVELOPMENT_BUILD || UNITY_EDITOR
      Debug.Log("GameCore Initialized");
#endif
    }
  }
}