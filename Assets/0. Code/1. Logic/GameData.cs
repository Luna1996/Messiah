namespace Messiah.Logic {
  using System;
  using System.Collections.Generic;

  [Serializable]
  public class GameData {
    public List<string> build;
    public List<string> drawPile;
    public List<string> graveyardPile = new List<string>();
    public List<string> exilePile = new List<string>();
    public List<string> hands = new List<string>();
    public int drawNum = 6;
    public int keepNum = 10;

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
      gd.build = new List<string> { "BasicMine01", "BasicMine01", "BasicMine01", "BasicWood01", "BasicWood01" };
      gd.drawPile = new List<string>(gd.build);
      gd.resources = new List<int> { 10, 100, 0, 0, 0, 0, 0 };
      gd.resourcesMaxLimit = new List<int> { 10, 100, 0, 0, 0, 0, 0 };
      Shuffle(gd.drawPile);
      return gd;
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