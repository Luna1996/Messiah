namespace Messiah.UI {
  using UnityEngine;
  using UnityEngine.UI;
  using System;
  using System.Collections.Generic;
  using Logic;
  using XLua;
  using Utility;
  using DG.Tweening;
  using Coffee.UIExtensions;
  using System.Threading.Tasks;

  [Serializable]
  public class Buff {
    public string buff;
    public Enum trigger;
    public BuffType type;
    public string icon;
    public string tips;
    public string tipsFunc;
    public string args;
    public int time;
    public int maxtime;

    [NonSerialized]
    GameObject buffIcon;

    static List<string> characters = new List<string> { "buff401", "buff402", "buff403", "buff404" };


    public Buff(string buff, Enum trigger, BuffType type, int maxtime, string icon, string tips, string tipsFunc = null, string args = null) {
      GameManager.gameData.buff.Add(this);
      this.buff = buff;
      this.trigger = trigger;
      this.type = type;
      this.icon = icon;
      this.tips = tips;
      this.tipsFunc = tipsFunc;
      this.args = args;
      this.time = 0;
      this.maxtime = maxtime;

      SetUp();
    }

    public async void SetUp() {
      if (!string.IsNullOrEmpty(icon)) {
        Transform trans;
        if (characters.Contains(icon)) trans = GameManager.inGameView.charpanel;
        else trans = GameManager.inGameView.buffpanel;
        buffIcon = PrefabManager.Instanciate("BuffIcon", trans);
        var texture = (Texture2D)AtlasManager.GetTexture(icon);
        var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
        buffIcon.GetComponent<Image>().sprite = sprite;
        if (string.IsNullOrEmpty(tipsFunc))
          buffIcon.GetComponent<Tip>().text = tips;
        else {
          var ret = LuaManager.lua.DoString($"return {tipsFunc}({time}, '{args}')");
          buffIcon.GetComponent<Tip>().text = ret[0].ToString();
          if (ret.Length > 1) {
            buffIcon.GetComponentInChildren<Text>(true).gameObject.SetActive(true);
            buffIcon.GetComponentInChildren<Text>(true).text = ret[1].ToString();
          }
        }
        buffIcon.GetComponent<CanvasGroup>().alpha = 0;
        await buffIcon.GetComponent<CanvasGroup>().DOFade(1, 0.5f).AsyncWaitForCompletion();
        var shiny = buffIcon.AddComponent<UIShiny>();
        shiny.duration = 1;
        shiny.Play();
        await Task.Delay(1000);
        GameObject.Destroy(shiny);
      }

      EventService.Listen(trigger, CallBack);
    }

    static Vector3 big = new Vector3(1.2f, 1.2f, 1.2f);
    public async void CallBack() {
      time++;
      if (!string.IsNullOrEmpty(tipsFunc)) {
        var ret = LuaManager.lua.DoString($"return {tipsFunc}({time}, '{args}')");
        buffIcon.GetComponent<Tip>().text = ret[0].ToString();
        if (ret.Length > 1)
          buffIcon.GetComponentInChildren<Text>(true).text = ret[1].ToString();
      }
      if (type == BuffType.Repeat || time == maxtime) {
        LuaManager.lua.DoString($"{buff}({time}, '{args}')");
        if (buffIcon) {
          await buffIcon.transform.DOPunchScale(big, 1f).AsyncWaitForCompletion();
        }
      }
      if (time == maxtime)
        Remove();
    }

    public async void Remove() {
      EventService.Ignore(trigger, CallBack);
      GameManager.gameData.buff.Remove(this);
      if (buffIcon) {
        await buffIcon.GetComponent<CanvasGroup>().DOFade(0, 0.5f).AsyncWaitForCompletion();
        GameObject.Destroy(buffIcon);
      }

    }
  }

  public enum BuffType {
    OneShot,
    Repeat,
  }
}