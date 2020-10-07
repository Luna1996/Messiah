namespace Messiah.Logic {
  using GameCoreNS;
  using System;
  using System.Collections.Generic;
  using Utility;

  [Serializable]
  public class GameData {
    public int numberOfTurn = 1;

    public List<string> build;
    public List<string> drawPile;
    public List<string> discardPile = new List<string>();
    public List<string> exilePile = new List<string>();
    public List<string> hands = new List<string>();
    public int drawNum = 6;
    public int keepNum = 3;

    int _maxWorker = 10;
    public int maxWorker {
      get { return _maxWorker; }
      set {
        if (value != _maxWorker) {
          value = _maxWorker;
          EventService.Notify(GameEvent.Game_MaxWorkerChanged);
        }
      }
    }

    int _idleWorker = 10;
    public int idleWorker {
      get { return _idleWorker; }
      set {
        if (value != _idleWorker) {
          value = _idleWorker;
          EventService.Notify(GameEvent.Game_IdleWorkerChanged);
        }
      }
    }

    int _occupiedWorker = 0;
    public int ocupiedWorker {
      get { return _occupiedWorker; }
      set {
        if (value != _occupiedWorker) {
          value = _occupiedWorker;
          EventService.Notify(GameEvent.Game_OccupiedWorkerChanged);
        }
      }
    }

    int _sickWorker = 0;
    public int sickWorker {
      get { return _idleWorker; }
      set {
        if (value != _idleWorker) {
          value = _idleWorker;
          EventService.Notify(GameEvent.Game_SickWorkerChanged);
        }
      }
    }

    public List<string> buildingDeck = new List<string>();
    public List<string> buildingAvaliable = new List<string>();
    public List<string> buildingAcquired = new List<string>();

    public string phase = "start";
    public List<string> eventDeck = new List<string>();
    public List<string> eventStack = new List<string>();

    public List<string> buf = new List<string>();
    public List<string> relic = new List<string>();
    public List<int> resources;
    public List<int> resourcesMaxLimit;

    public static GameData NewGameData() {
      var gd = new GameData();
      gd.build = new List<string> { "DrawCard", "DrawCard", "DrawCard", "DrawCard", "DrawCard", "DrawCard", "DrawCard", "DrawCard" };
      gd.drawPile = new List<string>(gd.build);
      gd.resources = new List<int> { 10, 100, 0, 0, 0, 0, 0 };
      gd.resourcesMaxLimit = new List<int> { 10, 100, 0, 0, 0, 0, 0 };
      Shuffle(gd.drawPile);
      return gd;
    }

    public static void ResetGame() {
    }

    public static void Shuffle<T>(List<T> list) {
      for (int i = list.Count - 1; i >= 1; i--) {
        var j = UnityEngine.Random.Range(0, i + 1);
        var t = list[j];
        list[j] = list[i];
        list[i] = t;
      }
    }

  }
}