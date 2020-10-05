namespace Messiah.Logic {
  using System;
  using UI;

  public interface Card {
    CardView cardView { get; set; }
    string name { get; set; }
    string frame { get; set; }
    string image { get; set; }
    string desc { get; set; }
    Action onPlay { get; set; }
    Action<int> canPlay { get; set; }
    Action addToHand { get; set; }
    Action rmvFrHand { get; set; }
  }
}