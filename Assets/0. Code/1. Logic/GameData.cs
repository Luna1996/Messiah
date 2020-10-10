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
    public int drawNum = 4;
    public int keepNum = 3;

    int _maxWorker = 10;
    public int maxWorker {
      get { return _maxWorker; }
      set {
        if (value != _maxWorker) {
          _maxWorker = value;
          EventService.Notify(GameEvent.IG_MaxWorkerChanged);
        }
      }
    }

    int _idleWorker = 10;
    public int idleWorker {
      get { return _idleWorker; }
      set {
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

      gd.build = new List<string> {  "Building_house","GodCard","BasicMine01","BasicWood01","Building_mine_22"};
      gd.drawPile = new List<string>(gd.build);
      Shuffle(gd.drawPile);
      gd.buildingAvaliable = new List<string> { "Building_church_01", "Building_church", "Building_clinic" };
      gd.buildingAcquired = new List<string> {};
      gd.buildingDeck = new List<string> { "Building_church_01", "Building_church", "Building_clinic", "Building_church_01", "Building_church", "Building_clinic" };
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