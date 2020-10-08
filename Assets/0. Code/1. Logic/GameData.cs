namespace Messiah.Logic {
  using GameCoreNS;
  using System;
  using System.Collections.Generic;
  using Utility;

  [Serializable]
  public class GameData {
    public int numberOfTurn = 0;

    public List<string> build;
    public List<string> drawPile;
    public List<string> discardPile = new List<string>();
    public List<string> exilePile = new List<string>();
    public List<string> hands = new List<string>();
    public int drawNum = 7;
    public int keepNum = 3;

    int _maxWorker = 10;
    public int maxWorker {
      get { return _maxWorker; }
      set {
        if (value != _maxWorker) {
          value = _maxWorker;
          EventService.Notify(GameEvent.IG_MaxWorkerChanged);
        }
      }
    }

    int _idleWorker = 10;
    public int idleWorker {
      get { return _idleWorker; }
      set {
        if (value != _idleWorker) {
          value = _idleWorker;
          EventService.Notify(GameEvent.IG_IdleWorkerChanged);
        }
      }
    }

    int _occupiedWorker = 0;
    public int ocupiedWorker {
      get { return _occupiedWorker; }
      set {
        if (value != _occupiedWorker) {
          value = _occupiedWorker;
          EventService.Notify(GameEvent.IG_OccupiedWorkerChanged);
        }
      }
    }

    int _sickWorker = 0;
    public int sickWorker {
      get { return _idleWorker; }
      set {
        if (value != _idleWorker) {
          value = _idleWorker;
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

    public List<string> buf = new List<string>();
    public List<string> relic = new List<string>();

    public int[] resources = { 0, 0, 0, 0, 0, 0 };

    public static GameData NewGameData() {
      var gd = new GameData();
      //gd.build = new List<string> { "BasicMine01", "DrawCard", "Tech_build_0", "Mine_02", "Tech_build_1", "BasicMinePile", "BasicIronPile", "DrawCard","Wood_02" };
      gd.build = new List<string> { "Curse_creed","Curse_gluttony","Curse_envy","Curse_lust","Curse_sloth","Curse_wrath","Curse_pride" };
      gd.drawPile = new List<string>(gd.build);
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