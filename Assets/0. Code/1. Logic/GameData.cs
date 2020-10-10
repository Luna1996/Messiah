namespace Messiah.Logic {
  using GameCoreNS;
  using System;
  using System.Collections.Generic;
  using Utility;
  using UI;

  [Serializable]
  public class GameData {
    public int numberOfTurn = 0;

    public List<string> build;
    public List<string> drawPile;
    public List<string> discardPile = new List<string>();
    public List<string> exilePile = new List<string>();
    public List<string> hands = new List<string>();
    public int drawNum = 3;
    public int keepNum = 3;

    int _maxWorker = 5;
    public int maxWorker {
      get { return _maxWorker; }
      set {
        if (value < 0) value = 0;
        if (value != _maxWorker) {
          var dif = value - _maxWorker;
          _maxWorker = value;
          idleWorker += dif;
          EventService.Notify(GameEvent.IG_MaxWorkerChanged);
        }
        if (_maxWorker == 0)
          GameCore.FAM.Fire(GameStateTrigger.GameEnd);
      }
    }

    int _idleWorker = 5;
    public int idleWorker {
      get { return _idleWorker; }
      set {
        if (value < 0) value = 0;
        if (value != _idleWorker) {
          _idleWorker = value;
          EventService.Notify(GameEvent.IG_IdleWorkerChanged);
        }
      }
    }

    int _occupiedWorker = 0;
    public int occupiedWorker {
      get { return _occupiedWorker; }
      set {
        if (value != _occupiedWorker) {
          _occupiedWorker = value;
          EventService.Notify(GameEvent.IG_OccupiedWorkerChanged);
        }
      }
    }

    int _sickWorker = 0;
    public int sickWorker {
      get { return _idleWorker; }
      set {
        if (value != _idleWorker) {
          _idleWorker = value;
          EventService.Notify(GameEvent.IG_SickWorkerChanged);
        }
      }
    }

    public List<string> buildingDeck = new List<string>();
    public List<string> buildingAvaliable = new List<string>();
    public List<string> buildingAcquired = new List<string>();

    public string phase = "start";
    public List<string> eventDeck = new List<string>();
    public List<string> eventStack = new List<string>();

    public List<Buff> buff = new List<Buff>();
    public List<string> relic = new List<string>();

    public int costMod = 0;
    public int[] resources = { 0, 0, 0, 0, 0, 0 };
    public int[] resourcesModifitor = { 0, 0, 0, 0, 0, 0 };

    public static GameData NewGameData() {
      var gd = new GameData();

      var testpile = new List<string> { };
      var mustdraw = new List<string> { "BasicFoodPile", "BasicMinePile","BasicFoodPile" };
      var build = new List<string> { "BasicWoodPile", "BasicWoodPile", "Building_house", "Building_mine" };
      Shuffle(build);
      mustdraw.AddRange(build);
      testpile.AddRange(mustdraw);

      gd.drawPile = new List<string>(testpile);
      gd.build = new List<string>(testpile);

      gd.buildingAcquired = new List<string> { };
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