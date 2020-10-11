namespace Messiah.UI {
  using Logic;
  using Logic.GameCoreNS;
  using UnityEngine.UI;

  public class EndingPhasePanel : UIMask {
    public Text desc;

    new async void Start() {
      var dead = GameManager.gameData.deadWorker;
      if (GameManager.gameData.maxWorker == 0) {
        desc.text = $"曾有{dead}人来到弥赛亚\n但现在\n你已孤身一人";
      } else {
        var max = GameManager.gameData.maxWorker;
        desc.text = $"浓雾已远离弥赛亚\n在你的带领下\n{max}人活了下来\n这一切要感谢{dead}的牺牲";
      }
      await base.Start();
    }

    public async void EndGame() {
      await Close();
      GameCore.FAM.Fire(GameStateTrigger.Back);
    }
  }
}