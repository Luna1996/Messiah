#pragma warning disable 4014
namespace Messiah.Logic {
  using GameCoreNS;
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using UI;
  using UnityEngine;
  using Utility;

  public enum ResourceType {
    Energy = 0,
    Food = 1,
    Ston = 2,
    Iron = 3,
    Tech = 4,
    Faith = 5,
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
      card.cardView.Dissolve();
      gameData.exilePile.Add(card.getCardFullName());
      SendCardTo(card.cardView, CardLocation.ExilePile);
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

    static void SendCardTo(CardView cardView, CardLocation loc, float d = 0.5f) {
      cardOnFly.SendCardTo(cardView, loc, d);
    }

    static void SendCardFromTo(string cardname, CardLocation fromloc, CardLocation toloc, float d = 0.5f) {
      cardOnFly.SendCardFromTo(cardname, fromloc, toloc, d);
    }
  }
}