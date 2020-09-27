namespace Messiah.JsonData {
  using System;
  using System.Threading.Tasks;
  using System.Collections.Generic;
  using UnityEngine;
  using UnityEngine.AddressableAssets;

  [Serializable]
  public class NPC {
    public int ID; // 编号
    public string Name; // 名称
    public string AssetName; // 资源编号
    public int HP; // 血
    public int Attack; // 攻击
    public int Defence; // 防御
  }

  [Serializable]
  public class Item {
    public int ID; // 编号
    public string Name; // 名称
    public string AssetName; // 资源编号
  }

  [Serializable]
  public class fuck {
    public NPC[] NPC;
    public Item[] Item;
  }

  public static class fuckJsonData {
    static fuck fuck;

    public static async Task Load() {
      var textAsset = await Addressables.LoadAssetAsync<TextAsset>("Assets/1. Data/1. Config/1. Json/fuck.json").Task;
      fuck = JsonUtility.FromJson<fuck>(textAsset.text);

      NPCDic = new Dictionary<int, NPC>();
      foreach (var entry in fuck.NPC)
        NPCDic[entry.ID] = entry;

      ItemDic = new Dictionary<int, Item>();
      foreach (var entry in fuck.Item)
        ItemDic[entry.ID] = entry;

      fuck = null;
    }

    public static void UnLoad() {
      NPCDic = null;
      ItemDic = null;
    }

    static Dictionary<int, NPC> NPCDic;
    public static NPC GetNPC(int ID) { return NPCDic[ID]; }

    static Dictionary<int, Item> ItemDic;
    public static Item GetItem(int ID) { return ItemDic[ID]; }
  }
}
