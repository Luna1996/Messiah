namespace Messiah.UI {
  using UnityEngine;
  using UnityEngine.UI;
  using Logic;
  using Logic.GameCoreNS;

  public class EventPanel : UIMask {
    public Text title;
    public Text desc;
    public Image image;
    public Transform choices;

    LuaEvent evt;

    new async void Start() {
      await base.Start();
      SetLuaEvent(GameManager.ChooseCurrentEvent());
    }

    void SetLuaEvent(LuaEvent evt) {
      this.evt = evt;
      UpdateState();
    }

    void UpdateState() {
      if (evt.currentState == null) {
        EventEnd();
        // GameCore.FAM.Fire(GameStateTrigger.NextPhase);
        return;
      }
      title.text = evt.currentState.name;
      desc.text = evt.currentState.desc;
      if (!string.IsNullOrEmpty(evt.currentState.image)) {
        var texture = (Texture2D)AtlasManager.GetTexture(evt.currentState.image);
        image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
      }
      while (choices.childCount != 0)
        DestroyImmediate(choices.GetChild(0).gameObject);
      foreach (var choice in evt.currentState.choices) {
        var btn = PrefabManager.Instanciate("EventChoiceButton", choices);
        btn.GetComponentInChildren<Text>().text = choice;
        btn.GetComponent<Button>().onClick.AddListener(() => {
          evt.choose(choice);
          UpdateState();
        });
      }
    }

    async void EventEnd() {
      await Close();
      GameCore.FAM.Fire(GameStateTrigger.NextPhase);
    }
  }
}