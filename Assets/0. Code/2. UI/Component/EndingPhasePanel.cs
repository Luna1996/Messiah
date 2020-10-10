namespace Messiah.UI {
  using Logic;
  using Logic.GameCoreNS;
  using UnityEngine.UI;

  public class EndingPhasePanel : UIMask {
    public Text desc;

    new async void Start() {
      if (GameManager.gameData.maxWorker == 0)
        desc.text = "你已孤身一人";
      else
        desc.text = "浓雾已远离弥赛亚";
      await base.Start();
    }

    public async void EndGame() {
      await Close();
      GameCore.FAM.Fire(GameStateTrigger.GameEnd);
    }
  }
}