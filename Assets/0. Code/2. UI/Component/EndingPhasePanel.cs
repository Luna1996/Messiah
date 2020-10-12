namespace Messiah.UI {
  using Logic;
  using Logic.GameCoreNS;
  using UnityEngine.UI;

  public class EndingPhasePanel : UIMask {
    public Text desc;

    new async void Start() {
      var dead = GameManager.gameData.deadWorker;
      if (GameManager.gameData.maxWorker == 0) {
        desc.text = $"<color=red>曾有{dead}人来到弥赛亚\n但现在\n你已孤身一人,\n是否是因为\n你错过了\n某些至关重要的朋友？\n没有他们帮助\n你永远也不可能渡过浩劫</color>";
      } else {
        var max = GameManager.gameData.maxWorker;
        desc.text = $"<color=green>浓雾已远离弥赛亚\n在你的带领下\n{max}人活了下来\n这一切要感谢{dead}人的牺牲\n以及\n米西亚\n泰娅\n安德\n罗兰\n的帮助</color>";
      }
      await base.Start();
    }

    public async void EndGame() {
      await Close();
      GameCore.FAM.Fire(GameStateTrigger.Back);
    }
  }
}