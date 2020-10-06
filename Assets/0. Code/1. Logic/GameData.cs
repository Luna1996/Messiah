namespace Messiah.Logic {
  using System;
  using System.Collections.Generic;

  [Serializable]
  public class GameData {
    public List<string> build = new List<string>();
    public List<string> drawPile = new List<string>();
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
    public List<int> resources = new List<int>();

    public static GameData NewGameData() {
      var gd = new GameData();
      string[] InitBuild = {"BasicMine01","BasicMine01","BasicMine01","BasicWood01","BasicWood01"};
      foreach (string element in InitBuild){
        gd.build.Add(element);
      }
      Shuffle(gd.build, gd.drawPile);
      return gd;
    }

    public static void Shuffle<T>(List<T> src, List<T> dist) {
      dist.Clear();
      dist.AddRange(src);
      for (int i = dist.Count - 1; i >= 1; i--) {
        var j = UnityEngine.Random.Range(0, i + 1);
        var t = dist[j];
        dist[j] = dist[i];
        dist[i] = t;
      }
    }

  }
}