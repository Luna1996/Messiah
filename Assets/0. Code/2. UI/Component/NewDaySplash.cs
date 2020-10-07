namespace Messiah.UI {
  using UnityEngine;
  using UnityEngine.UI;
  using Logic;

  public class NewDaySplash : MonoBehaviour {
    void Start() {
      var text = GetComponent<Text>();
      GameManager.gameData.numberOfTurn += 1;
      text.text = $"第 {GameManager.gameData.numberOfTurn} 天";
    }
  }
}