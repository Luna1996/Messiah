namespace Messiah.Logic {
  using System;
  using UI;

  public interface Card {
    CardView cardView { get; set; }
    string className { get; set; }
    string name { get; set; }
    string frame { get; set; }
    string image { get; set; }
    string desc { get; set; }
    int cost { get; set; }
    void setCardView();
    int onPlay();
    int canPlay();
    void addToHand();
    void rmvFrHand();
    string getCardFullName();
  }
}