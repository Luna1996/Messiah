namespace Messiah.UI {
  using Messiah.Logic.GameCoreNS;
  using System.Threading.Tasks;
  using UnityEngine;
  using UnityEngine.UI;

  public class OutGameView : MonoBehaviour {
    public GameObject newGame;
    public GameObject oldGame;

    AlphaEffect effect;

    void Start() {
      effect = GetComponent<AlphaEffect>();
      if (GameCore.userData?.currentGameData != null)
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
      GameCore.FAM.Fire(GameStateTrigger.GameStart);
    }
  }
}