namespace Messiah.Logic {
  using System;
  using System.Collections.Generic;

  [Serializable]
  public class GameData {
    public List<string> build;
    public List<string> drawPile;
    public List<string> graveyardPile;
    public List<string> exilePile;
    public List<string> hands;

    public List<string> buildingDeck;
    public List<string> buildingAvaliable;
    public List<string> buildingAcquired;

    public string phase;
    public List<string> eventDeck;
    public List<string> eventStack;

    public List<string> buf;
    public List<string> relic;
    public List<int> resources;

    public static GameData NewGameData() {
      var gd = new GameData();
      gd.build = new List<string>();
      return gd;
    }
  }
}