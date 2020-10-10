#pragma warning disable 4014
namespace Messiah.Logic {
  using GameCoreNS;
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using UI;
  using UnityEngine;
  using Utility;
  using System;
  using XLua;

  public enum ResourceType {
    Mine = 0,
    Food = 1,
    Wood = 2,
    Iron = 3,
    Tech = 4,
    Faith = 5,
  }

  public enum DeckType {
    OriginalDeckAndDrawPile,
    DrawPile,
    DiscardPile,
    BuildingDeck,
    EventDeck,
  }

  public static class GameManager {
    public static ViewManager viewManager;
    public static InGameView inGameView;
    public static MessiahView messiahView;
    public static HandView handView;
    public static GameData gameData;
    public static CardOnFly cardOnFly;

    public static int[] turneventpool = new int[]{
      -1,-1,0,1,2,3,-1,8,-1,-1,4,-1,-1,-1,-1,6,-1,7,-1,-1,-1,4,-1,-1,-1,-1,5,-1,-1,2,-1,10,-1,4-1,6,7,-1,-1,4,-1,-1,-1,-1,-1,9
    };
    public static string[][] eventpool = new string[][] {
      new string[]{"Event998"},new string[]{"Event999"},
      new string[]{"Event1001"},new string[]{"Event1011",},new string[]{"Event1362","Event1021"},
      new string[]{"Event1402"},new string[]{"Event1362","Event1377"},new string[]{"Event1081","Event1011"},
      new string[]{"Event1271",},new string[]{"Event1383"},new string[]{"Event1301"}
    };


    public static bool ShouldTriggerEvent() {
      var i = (gameData.numberOfTurn - 1) % turneventpool.Length;
      return turneventpool[i] >= 0 && turneventpool[i] < eventpool.Length;
    }

    public static LuaEvent ChooseCurrentEvent() {
      var i = (gameData.numberOfTurn - 1) % turneventpool.Length;
      var pool = eventpool[turneventpool[i]];
      var r = UnityEngine.Random.Range(0, pool.Length);
      return LuaManager.GetLuaEvent(pool[r]);
    }


    public static void ModifyCost(int mod) {
      LuaManager.lua.DoString($"CostModifiter = {mod} + CostModifiter");
      EventService.Notify(GameEvent.IG_OnCostModifiterChanged);
    }

    public static void Build(Card card, string bname) {
      if (card != null) {
        var i = handView.RemoveCard(card);
        card.cardView.DissolveInCenter();
        handView.RemoveCard(card);
        gameData.buildingDeck.Add(card.getCardFullName());
      }
      if (!gameData.buildingAcquired.Contains(bname))
        gameData.buildingAcquired.Add(bname);
      messiahView.Build(bname);
    }


    public static void Discard(Card card) {
      var i = handView.RemoveCard(card);
      gameData.discardPile.Add(card.getCardFullName());
      SendCardTo(card.cardView, CardLocation.DiscardPile, 0.2f);
    }

    public static void Exile(Card card) {
      var i = handView.RemoveCard(card);
      gameData.exilePile.Add(card.getCardFullName());
      card.cardView.Dissolve();
    }

    public static async Task DrawCard(int num = 1) {
      var cards = new List<string>();
      for (int i = 0; i < num; i++) {
        if (gameData.drawPile.Count == 0) {
          RecycleDiscardPile();
        }
        if (gameData.drawPile.Count > 0) {
          var card = gameData.drawPile[0];
          cards.Add(card);
          gameData.drawPile.RemoveAt(0);
        }
      }
      foreach (var card in cards) {
        handView.AddCard(card);
        EventService.Notify(GameEvent.IG_DrawCard);
        await Task.Delay(100);
      }
    }

    public static async Task RecycleDiscardPile() {
      var cardOnFly = new List<string>(gameData.discardPile);
      gameData.drawPile.AddRange(gameData.discardPile);
      gameData.discardPile.Clear();
      GameData.Shuffle(gameData.drawPile);

      foreach (var card in cardOnFly) {
        SendCardFromTo(card, CardLocation.DiscardPile, CardLocation.DrawPile);
        await Task.Delay(50);
      }
    }

    public static void NextPhase() {
      GameCore.FAM.Fire(GameStateTrigger.NextPhase);
    }

    public static int GetResource(ResourceType rt) {
      return gameData.resources[(int)rt];
    }

    public static void SetResource(ResourceType rt, int v) {
      var ov = GetResource(rt);
      if (ov != v) {
        gameData.resources[(int)rt] = v;
        EventService.NotifyWithArg(GameEvent.IG_ResourceModify, (int)rt);
      }
    }

    public static void AddResource(ResourceType rt, int v) {
      var ov = GetResource(rt);
      v = v + gameData.resourcesModifitor[(int)rt];
      if (v >= 0)
        inGameView.textFlyers[(int)rt].FlyText($"<color=green>+{v}</color> ");
      else
        inGameView.textFlyers[(int)rt].FlyText($"<color=green>{v}</color> ");
      SetResource(rt, v + ov);
    }

    public static void SubResource(ResourceType rt, int v) {
      var ov = GetResource(rt);
      v = v + gameData.resourcesModifitor[(int)rt];
      if (v >= 0)
        inGameView.textFlyers[(int)rt].FlyText($"<color=red>-{v}</color> ");
      else
        inGameView.textFlyers[(int)rt].FlyText($"<color=red>+{-v}</color> ");
      SetResource(rt, v > ov ? 0 : (ov - v));
    }

    // public static void CreateBuff(Enum id, BuffType type, int time, LuaFunction callback) {
    // new Buff(id, type, time, callback);
    // }

    public static void AddCard(DeckType deckType, string[] card) {
      switch (deckType) {
        case DeckType.OriginalDeckAndDrawPile:
          AddCardTo(gameData.build, card);
          goto case DeckType.DrawPile;
        case DeckType.DrawPile:
          AddCardTo(gameData.drawPile, card);
          break;
        case DeckType.DiscardPile:
          AddCardTo(gameData.discardPile, card);
          break;
        case DeckType.BuildingDeck:
          AddCardTo(gameData.buildingDeck, card);
          break;
        case DeckType.EventDeck:
          AddCardTo(gameData.eventDeck, card);
          break;
      }
    }

    static void AddCardTo(List<string> list, string[] card) {
      list.AddRange(card);
      GameData.Shuffle(list);
    }

    public static void ReplaceCard(DeckType deckType, string src, string dst) {
      switch (deckType) {
        case DeckType.OriginalDeckAndDrawPile:
          ReplaceCardTo(gameData.build, src, dst);
          goto case DeckType.DrawPile;
        case DeckType.DrawPile:
          ReplaceCardTo(gameData.drawPile, src, dst);
          break;
        case DeckType.DiscardPile:
          ReplaceCardTo(gameData.discardPile, src, dst);
          break;
        case DeckType.BuildingDeck:
          ReplaceCardTo(gameData.buildingDeck, src, dst);
          break;
        case DeckType.EventDeck:
          ReplaceCardTo(gameData.eventDeck, src, dst);
          break;
      }
    }

    static void ReplaceCardTo(List<string> list, string src, string dst) {
      for (int i = 0; i < list.Count; i++) {
        if (list[i].StartsWith(src)) {
          list[i] = dst;
        }
      }
    }

    public static void RemoveCard(DeckType deckType, string[] card) {
      switch (deckType) {
        case DeckType.OriginalDeckAndDrawPile:
          RemoveCardTo(gameData.build, card);
          goto case DeckType.DrawPile;
        case DeckType.DrawPile:
          RemoveCardTo(gameData.drawPile, card);
          break;
        case DeckType.DiscardPile:
          RemoveCardTo(gameData.discardPile, card);
          break;
        case DeckType.BuildingDeck:
          RemoveCardTo(gameData.buildingDeck, card);
          break;
        case DeckType.EventDeck:
          RemoveCardTo(gameData.eventDeck, card);
          break;
      }
    }

    static void RemoveCardTo(List<string> list, string[] card) {
      foreach (var c in card) {
        for (int i = 0; i < list.Count; i++)
          if (list[i].StartsWith(c)) list.RemoveAt(i);
      }
    }

    static async Task SendCardTo(CardView cardView, CardLocation loc, float d = 0.5f) {
      await cardOnFly.SendCardTo(cardView, loc, d);
    }

    static async Task SendCardFromTo(string cardname, CardLocation fromloc, CardLocation toloc, float d = 0.5f) {
      await cardOnFly.SendCardFromTo(cardname, fromloc, toloc, d);
    }
  }
}