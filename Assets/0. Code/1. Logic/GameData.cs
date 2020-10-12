namespace Messiah.Logic {
  using GameCoreNS;
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using Utility;
  using UI;

  [Serializable]
  public class GameData {
    public int numberOfTurn = 0;

    public ObservableCollection<string> build;
    public ObservableCollection<string> drawPile;
    public ObservableCollection<string> discardPile = new ObservableCollection<string>();
    public ObservableCollection<string> exilePile = new ObservableCollection<string>();
    public ObservableCollection<string> hands = new ObservableCollection<string>();
    public int drawNum = 3;
    public int keepNum = 3;

    int _maxWorker = 5;
    public int maxWorker {
      get { return _maxWorker; }
      set {
        if (value < 0) value = 0;
        if (value != _maxWorker) {
          var dif = value - _maxWorker;
          if (dif < 0) deadWorker -= dif;
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

    int _deadWorker = 0;
    public int deadWorker {
      get { return _deadWorker; }
      set {
        if (value < 0) value = 0;
        if (value != _deadWorker) {
          var dif = value - _deadWorker;
          if (dif < 0) maxWorker -= dif;
          _deadWorker = value;
          EventService.Notify(GameEvent.IG_DeadWorkerChanged);
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

    public ObservableCollection<string> buildingDeck = new ObservableCollection<string>();
    public ObservableCollection<string> buildingAvaliable = new ObservableCollection<string>();
    public ObservableCollection<string> buildingAcquired = new ObservableCollection<string>();

    public string phase = "start";
    public ObservableCollection<string> eventDeck = new ObservableCollection<string>();
    public ObservableCollection<string> eventStack = new ObservableCollection<string>();

    public ObservableCollection<Buff> buff = new ObservableCollection<Buff>();
    public ObservableCollection<string> relic = new ObservableCollection<string>();

    public int costMod = 0;
    public int[] resources = { 10, 10, 10, 10, 10, 10 };
    public int[] resourcesModifitor = { 0, 0, 0, 0, 0, 0 };

    public static GameData NewGameData() {
      var gd = new GameData();

      var testpile = new List<string> { };
      var mustdraw = new List<string> { "BasicFoodPile 5", "BasicMinePile 5", "BasicFoodPile 5", };
      var build = new List<string> { "BasicIronPile 5", "BasicWoodPile 5", "Building_house", "Building_mine", "Building_wood", };
      Shuffle(build);
      mustdraw.AddRange(build);
      testpile.AddRange(mustdraw);

      gd.drawPile = new ObservableCollection<string>(testpile);
      gd.build = new ObservableCollection<string>(testpile);

      gd.buildingAcquired = new ObservableCollection<string> { };
      return gd;
    }

    public static void ResetGame() {
    }

    public static void Shuffle<L>(L list) where L : System.Collections.IList {
      for (int i = list.Count - 1; i >= 1; i--) {
        var j = UnityEngine.Random.Range(0, i + 1);
        var t = list[j];
        list[j] = list[i];
        list[i] = t;
      }
    }
  }
}