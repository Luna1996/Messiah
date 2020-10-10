#pragma warning disable 4014
namespace Messiah.UI {
  using UnityEngine.UI;
  using Logic;

  public class NewDaySplash : UIMask {
    new async void Start() {
      duration = 0.5f;
      GameManager.gameData.numberOfTurn += 1;
      var text = GetComponentInChildren<Text>();
      text.text = $"第 {GameManager.gameData.numberOfTurn} 天";
      GameManager.inGameView.dayNum.text = $"{GameManager.gameData.numberOfTurn}";
      await base.Start();
      Close();
    }
  }
}