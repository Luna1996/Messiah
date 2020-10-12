namespace Messiah.UI {
  using UnityEngine;
  using UnityEngine.UI;
  using Logic;
  using Logic.GameCoreNS;
  using DG.Tweening;

  public class ConsumePhaseView : UIMask {

    public Text[] texts;
    public Image confirm;

    bool skip = false;

    new async void Start() {
      await base.Start();
      float d = 1;

      var buildingNum = 1;
      foreach (var name in GameManager.gameData.buildingAcquired) {
        if (!name.StartsWith("住宅")) buildingNum += 1;
      }
      var energyMod = GameManager.gameData.resourcesModifitor[(int)ResourceType.Mine];
      var energyCost = buildingNum - energyMod;
      if (energyCost < 0) energyCost = 0;
      texts[0].text = $"今天{buildingNum}座建筑，共需{energyCost}能量";
      await texts[0].DOFade(1, skip ? 0.1f : d).AsyncWaitForCompletion();

      var energy = GameManager.GetResource(ResourceType.Mine);
      GameManager.SubResource(ResourceType.Mine, energyCost);
      if (energy < energyCost) {
        texts[1].text = "由于灯塔供能不足";
        await texts[1].DOFade(1, skip ? 0.1f : d).AsyncWaitForCompletion();
        texts[2].text = "在浓雾的侵染下";
        await texts[2].DOFade(1, skip ? 0.1f : d).AsyncWaitForCompletion();
        var dead = Random.Range(1, buildingNum - energy + 1);
        if (dead > GameManager.gameData.idleWorker) dead = GameManager.gameData.idleWorker;
        if (dead > 0)
          texts[3].text = $"{dead}人丧失了理智";
        else
          texts[3].text = $"无人丧失理智，真走运";

        await texts[3].DOFade(1, skip ? 0.1f : d).AsyncWaitForCompletion();
        GameManager.gameData.maxWorker -= dead;
        if (GameCore.FAM.State == GameState.EndingPhase) {
          await Close();
          return;
        }
      } else {
        texts[1].text = "在浓雾的侵染下";
        await texts[1].DOFade(1, skip ? 0.1f : d).AsyncWaitForCompletion();
        texts[2].text = "你的城市运转正常";
        await texts[2].DOFade(1, skip ? 0.1f : d).AsyncWaitForCompletion();
        texts[3].text = "愿明灯指引你前行";
        await texts[3].DOFade(1, skip ? 0.1f : d).AsyncWaitForCompletion();
      }

      var maxWorker = GameManager.gameData.maxWorker;
      var foodMod = GameManager.gameData.resourcesModifitor[(int)ResourceType.Food];
      var foodCost = maxWorker - foodMod;
      if (foodCost < 0) foodCost = 0;
      texts[4].text = $"今天{maxWorker}人需要{foodCost}食物";
      await texts[4].DOFade(1, skip ? 0.1f : d).AsyncWaitForCompletion();

      var food = GameManager.GetResource(ResourceType.Food);
      GameManager.SubResource(ResourceType.Food, foodCost);
      if (food < foodCost) {
        var dead = Random.Range(1, foodCost - food + 1);
        if (dead > GameManager.gameData.idleWorker) dead = GameManager.gameData.idleWorker;
        if (dead > 0)
          texts[5].text = $"又有{dead}人死于饥饿";
        else
          texts[5].text = $"人民食不果腹";
        await texts[5].DOFade(1, skip ? 0.1f : d).AsyncWaitForCompletion();
        GameManager.gameData.maxWorker -= dead;
        if (GameCore.FAM.State == GameState.EndingPhase) {
          await Close();
          return;
        }
        texts[6].text = $"明天？还有明天吗？";
        await texts[6].DOFade(1, skip ? 0.1f : d).AsyncWaitForCompletion();
      } else {
        texts[5].text = $"至少目前衣食无忧";
        await texts[5].DOFade(1, skip ? 0.1f : d).AsyncWaitForCompletion();
        texts[6].text = $"愿明灯永照弥赛亚";
        await texts[6].DOFade(1, skip ? 0.1f : d).AsyncWaitForCompletion();
      }

      confirm.GetComponent<Button>().interactable = true;
      await confirm.DOFade(1, skip ? 0.1f : d).AsyncWaitForCompletion();
    }

    public async void NextPhase() {
      await Close();
      GameCore.FAM.Fire(GameStateTrigger.NextPhase);
    }

    public void Skip() {
      skip = true;
    }
  }
}