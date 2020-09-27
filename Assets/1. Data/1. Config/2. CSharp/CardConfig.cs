namespace Messiah.JsonData {
  using System;
  using System.Threading.Tasks;
  using System.Collections.Generic;
  using UnityEngine;
  using UnityEngine.AddressableAssets;

  [Serializable]
  public class Card {
  	public int ID; // 索引
  	public string name; // 
  	public string image; // 
  	public string desc; // 
  	public int[] desc_args; // 
  	public string frame; // 
  	public Effect[] effects; // 
  }

  [Serializable]
  public class CardConfig {
    public Card[] Card;
  }

  public static class CardConfigJsonData {
    static CardConfig CardConfig;

    public static async Task Load() {
      var textAsset = await Addressables.LoadAssetAsync<TextAsset>("Assets/1. Data/1. Config/1. Json/CardConfig.json").Task;
      CardConfig = JsonUtility.FromJson<CardConfig>(textAsset.text);

      CardDic = new Dictionary<int, Card>();
      foreach (var entry in CardConfig.Card)
        CardDic[entry.ID] = entry;

      CardConfig = null;
    }

    public static void UnLoad() {
      CardDic = null;
    }

    static Dictionary<int, Card> CardDic;
    public static Card GetCard(int ID) { return CardDic[ID]; }
  }
}
