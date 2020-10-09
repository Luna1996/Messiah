#pragma warning disable 4014
namespace Messiah.UI {
  using System;
  using UnityEngine;
  using DG.Tweening;
  using UnityEngine.UI;
  using UnityEngine.EventSystems;
  using XLua;
  using Logic;
  using Coffee.UIExtensions;
  using Utility;
  using Logic.GameCoreNS;

  public class CardView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler {
    [NonSerialized]
    public HandView hands;
    [NonSerialized]
    public bool canPlay;
    [NonSerialized]
    public bool inPanel;
    [NonSerialized]
    public bool canSelect;

    public RawImage image;
    public RawImage frame;
    public Text mainCost;
    public Text subCost;
    public Text cardName;
    public Text ruleText;
    public Text subType;

    [NonSerialized]
    public Card luacard;

    UIDissolve effect;
    void Start() {
      effect = GetComponent<UIDissolve>();
    }

    public void SetLuaCard(string card) {
      luacard = LuaManager.CreateLuaObject(card).Cast<Card>();
      luacard.cardView = this;
      image.texture = AtlasManager.GetTextrue(luacard.image);
      frame.texture = AtlasManager.GetTextrue(luacard.frame);
      cardName.text = luacard.name;
      ruleText.text = luacard.desc;
      luacard.setCardView();
      EventService.Listen(GameEvent.IG_OnCostModifiterChanged, luacard.setCardView);
    }

    void SetCardView() {
      if (luacard.cost != -1) { 
      }
      //     if self.cost then
      //   Debug.Log(self.cardView.mainCost)
      //   -- self.cardView.mainCost:SetActive(true)
      //   -- local color = "white"
      //   -- if CostModifiter < 0 then
      //   --   color = "green"
      //   -- elseif CostModifiter > 0 then
      //   --   color = "red"
      //   -- end
      //   -- local cost = self.cost + CostModifiter
      //   -- if cost < 0 then cost = 0 end
      //   -- self.cardView.mainCost.text = "<color="..color..">"..cost.."</color>"
      // end
    }

    public async void Dissolve() {
      effect.enabled = true;
      effect.Play();
      await System.Threading.Tasks.Task.Delay((int)(effect.duration * 1000));
      Destroy(gameObject);
    }

    static Vector3 focusScal = new Vector3(2f, 2f, 1);
    public void OnBeginDrag(PointerEventData p) {
      if (!canPlay || inPanel) {
        return;
      }
      hands.ReleaseFromHand(this);
      transform.DORotateQuaternion(Quaternion.identity, 0.2f);
      transform.DOScale(focusScal, 0.2f);
    }

    public void OnDrag(PointerEventData p) {
      if (!canPlay || inPanel) {
        return;
      }
      transform.position = p.pointerCurrentRaycast.worldPosition;
    }

    public void OnEndDrag(PointerEventData p) {
      if (!canPlay || inPanel) {
        return;
      }
      if (p.pointerCurrentRaycast.worldPosition.y > -0.05) {
        luacard.onPlay();
      } else
        hands.AddToHand(this);
    }

    public void OnPointerClick(PointerEventData p) {
      if (!inPanel) return;
      if (mask) CloseMask();
      else if (canSelect) OpenMask();
    }

    [NonSerialized]
    public GameObject mask;
    public void OpenMask() {
      mask = PrefabManager.Instanciate("CardMask", transform);
      EventService.NotifyWithArg(GameEvent.IG_OnCardSelectionChanged, this);
    }

    public void CloseMask() {
      Destroy(mask);
      mask = null;
      EventService.NotifyWithArg(GameEvent.IG_OnCardSelectionChanged, this);
    }

    void OnDestroy() {
      if (luacard != null)
        EventService.Ignore(GameEvent.IG_OnCostModifiterChanged, luacard.setCardView);
    }


  }
}