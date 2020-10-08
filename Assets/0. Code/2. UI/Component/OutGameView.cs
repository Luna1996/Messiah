namespace Messiah.UI {
  using Messiah.Logic.GameCoreNS;
  using System.Threading.Tasks;
  using UnityEngine;
  using UnityEngine.UI;

  public class OutGameView : MonoBehaviour {
    public GameObject newGame;
    public GameObject oldGame;

    AlphaEffect effect;

    public void Init() {
      effect = GetComponent<AlphaEffect>();
      newGame.SetActive(true);
      if (GameCore.userData.currentGameData != null)
        oldGame.SetActive(true);
    }

    public async Task Show() {
      Logic.LuaManager.lua.Global.Set("OutGameView", this);
      await effect.Show(0.5f);
    }

    public async Task Hide() {
      Logic.LuaManager.lua.Global.Set("OutGameView", false);
      await effect.Hide(0.5f);
      Destroy(gameObject);
    }

    public void NewGame() {
      GameCore.userData.currentGameData = null;
      GameCore.FAM.Fire(GameStateTrigger.GameStart);
    }

    public void OldGame() {
      GameCore.FAM.Fire(GameStateTrigger.GameStart);
    }
  }
}