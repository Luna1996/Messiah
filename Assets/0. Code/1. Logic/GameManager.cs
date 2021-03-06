#pragma warning disable 4014
namespace Messiah.Logic {
  using GameCoreNS;
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using UI;
  using Utility;
  using System.Collections.ObjectModel;

  public enum ResourceType {
    Mine = 0,
    Food = 1,
    Wood = 2,
    Iron = 3,
    Tech = 4,
    Faith = 5,
  }

  public enum DeckType {
    Build,
    OriginalDeckAndDrawPile,
    DrawPile,
    DiscardPile,
    BuildingDeck,
    EventDeck,
  }

  public static class GameManager {
    public static AppRoot appRoot;
    public static ViewManager viewManager;
    public static InGameView inGameView;
    public static MessiahView messiahView;
    public static HandView handView;
    public static GameData gameData;
    public static CardOnFly cardOnFly;

    public static int[] turneventpool = new int[]{
      0,
      14,1,11,-1,9,15,11,2,10,-1,12,7,-1,3,11,8,16,12,6,
      11,-1,12,-1,11,12,-1,12,6,-1,12,
      4,-1,12,6,12,-1,-1,12,11,-1,-1,-1,
      5,-1,-1,12,6,-1,12,-1,-1,-1,-1,
      13
    };
    public static string[][] eventpool = new string[][] {

       new string[]{"Event0001"},
       
      new string[]{"EventBomb",},new string[]{"EventChurch"},
      new string[]{"EventCurse1"},new string[]{"EventCurse2",},new string[]{"EventCurse3"},
      new string[]{"EventDelCard",},new string[]{"EventDetective"},new string[]{"EventDoc"},
      new string[]{"EventGirl"},new string[]{"EventPirate"},new string[]{"EventPoolBuild"},
      new string[]{"EventPoolTrash1","EventPoolTrash2","EventPoolTrash3","EventPoolTrash4"},
       new string[]{"EndingEvent"},
      new string[]{"EventPopula1"},
      new string[]{"EventPopula2"},
      new string[]{"EventPopula3"},
     
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
        handView.ReleaseFromHand(card.cardView);
        gameData.buildingDeck.Add(card.getCardFullName());
        card.cardView.DissolveInCenter();
      }
      if (!gameData.buildingAcquired.Contains(bname))
        gameData.buildingAcquired.Add(bname);
      messiahView.Build(bname);
    }


    public static void Discard(Card card) {
      handView.ReleaseFromHand(card.cardView);
      gameData.discardPile.Add(card.getCardFullName());
      card.cardView.playsound.Play();
      SendCardTo(card.cardView, CardLocation.DiscardPile, 0.2f);
    }

    public static void Exile(Card card) {
      handView.ReleaseFromHand(card.cardView);
      gameData.exilePile.Add(card.getCardFullName());
      card.cardView.disolvesound.Play();
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
      foreach (var card in cardOnFly) {
        gameData.drawPile.Add(card);
      }
      gameData.discardPile.Clear();

      GameData.Shuffle(gameData.drawPile);
      appRoot.shufflesound.Play();
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

    public static async void AddCard(DeckType deckType, string[] card) {
      switch (deckType) {
        case DeckType.OriginalDeckAndDrawPile:
          AddCardTo(gameData.build, card);
          goto case DeckType.DrawPile;
        case DeckType.DrawPile:
          AddCardTo(gameData.drawPile, card);
          foreach (var c in card) {
            SendCardFromTo(c, CardLocation.Center, CardLocation.DrawPile);
            await Task.Delay(100);
          }
          break;
        case DeckType.DiscardPile:
          AddCardTo(gameData.discardPile, card);
          foreach (var c in card) {
            SendCardFromTo(c, CardLocation.Center, CardLocation.DiscardPile);
            await Task.Delay(100);
          }
          break;
        case DeckType.BuildingDeck:
          AddCardTo(gameData.buildingDeck, card);
          break;
        case DeckType.EventDeck:
          AddCardTo(gameData.eventDeck, card);
          break;
      }
    }

    static void AddCardTo(ObservableCollection<string> list, string[] card) {
      foreach (var c in card) list.Add(c);
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

    static void ReplaceCardTo(IList<string> list, string src, string dst) {
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

    static void RemoveCardTo(IList<string> list, string[] card) {
      foreach (var c in card) {
        for (int i = 0; i < list.Count; i++)
          if (list[i].StartsWith(c)) list.RemoveAt(i);
      }
    }

    public static async Task AddCardsToHand(string[] cards) {
      foreach (var card in cards) {
        SendCardFromTo(card, CardLocation.Center, CardLocation.Hand);
        await Task.Delay(100);
      }
    }

    public static void AddCardToHand(string card) {
      SendCardFromTo(card, CardLocation.Center, CardLocation.Hand);
    }

    public static async Task DisplayCard(string card, float d = 1) {
      await cardOnFly.SendCardFromTo(card, CardLocation.Center, CardLocation.Center, d);
    }

    public static async Task SendCardTo(CardView cardView, CardLocation loc, float d = 0.5f) {
      await cardOnFly.SendCardTo(cardView, loc, d);
    }

    public static async Task SendCardFromTo(string cardname, CardLocation fromloc, CardLocation toloc, float d = 0.5f) {
      await cardOnFly.SendCardFromTo(cardname, fromloc, toloc, d);
    }

    public static void SelectCards(IList<string> cards, string title, int max, XLua.LuaFunction cb, XLua.LuaFunction canchoose = null) {
      CardSelectionView.ToggleView(viewManager.transform, cards, title, max, cb, canchoose);
    }

    public static async Task SelectCards(DeckType dtype, string title, int max, XLua.LuaFunction cb, XLua.LuaFunction canchoose = null) {
      ObservableCollection<string> cards;
      switch (dtype) {
        case DeckType.Build:
          cards = gameData.build;
          break;
        case DeckType.DrawPile:
          cards = new ObservableCollection<string>(gameData.drawPile);
          GameData.Shuffle(cards);
          break;
        case DeckType.DiscardPile:
          cards = gameData.discardPile;
          break;
        default:
          cards = null;
          break;
      }
      await CardSelectionView.ToggleView(viewManager.transform, cards, title, max, cb, canchoose);
    }

    public static async Task Do(System.Func<Task>[] tasks) {
      foreach (var task in tasks) {
        var t = task();
        if (t != null) await t;
      }
    }

    public static string[] GetHeroes() {
      var max = inGameView.charpanel.transform.childCount;
      List<string> names = new List<string>();
      for (int i = 0; i < max; i++)
        names.Add(inGameView.charpanel.transform.GetChild(i).gameObject.name);
      return names.ToArray();
    }
  }
}