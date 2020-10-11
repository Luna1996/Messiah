#pragma warning disable 4014
namespace Messiah.UI {
  using UnityEngine;
  using UnityEngine.UI;
  using Logic;
  using DG.Tweening;

  public class NewDaySplash : UIMask {
    new async void Start() {
      duration = 0.5f;
      GameManager.gameData.numberOfTurn += 1;
      var text = GetComponentInChildren<Text>();
      text.text = $"第 {GameManager.gameData.numberOfTurn} 天";
      GameManager.inGameView.dayNum.text = $"{GameManager.gameData.numberOfTurn}";
      GameManager.inGameView.dayNum.transform.DOPunchScale(new Vector3(1.1f, 1.1f, 1), 1);
      await base.Start();
      Close();
    }
  }
}