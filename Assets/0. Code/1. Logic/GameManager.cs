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
    public static MessiahView messiahView;
    public static HandView handView;
    public static GameData gameData;
    public static CardOnFly cardOnFly;

    public static void ResetDeck() {
      gameData.drawPile.Clear();
    }

    public static void Discard(Card card) {
      handView.RemoveCard(card);
      gameData.discardPile.Add(card.getCardFullName());
      SendCardTo(card.cardView, CardLocation.DiscardPile, 0.2f);
    }

    public static void Exile(Card card) {
      handView.RemoveCard(card);
      gameData.exilePile.Add(card.getCardFullName());
      card.cardView.Dissolve();
    }

    public static async Task DrawCard(int num = 1) {
      if (num != 1) {
        for (int i = 0; i < num; i++) {
          await DrawCard();
          await Task.Delay(100);
        }
      } else {
        if (gameData.drawPile.Count == 0) {
          await RecycleDiscardPile();
        }
        if (gameData.drawPile.Count > 0) {
          var card = gameData.drawPile[0];
          gameData.hands.Add(card);
          handView.AddCard(card);
          gameData.drawPile.RemoveAt(0);
        }
      }
    }

    public static async Task RecycleDiscardPile() {
      while (gameData.discardPile.Count != 0) {
        var cardname = gameData.discardPile[0];
        gameData.drawPile.Add(cardname);
        gameData.discardPile.RemoveAt(0);
        SendCardFromTo(cardname, CardLocation.DiscardPile, CardLocation.DrawPile);
        await Task.Delay(50);
      }
      GameData.Shuffle(gameData.drawPile);
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
      SetResource(rt, v + ov);
    }

    public static void SubResource(ResourceType rt, int v) {
      var ov = GetResource(rt);
      SetResource(rt, v > ov ? 0 : (ov - v));
    }

    public static void CreateBuff(Enum id, BuffType type, int time, LuaFunction callback) {
      new Buff(id, type, time, callback);
    }

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

    static void SendCardTo(CardView cardView, CardLocation loc, float d = 0.5f) {
      cardOnFly.SendCardTo(cardView, loc, d);
    }

    static void SendCardFromTo(string cardname, CardLocation fromloc, CardLocation toloc, float d = 0.5f) {
      cardOnFly.SendCardFromTo(cardname, fromloc, toloc, d);
    }
  }
}