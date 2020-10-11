#pragma warning disable 4014
namespace Messiah.UI {
  using System;
  using UnityEngine;
  using DG.Tweening;
  using UnityEngine.UI;
  using UnityEngine.EventSystems;
  using Logic;
  using Coffee.UIExtensions;
  using Utility;
  using Logic.GameCoreNS;
  using System.Threading.Tasks;

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

    public AudioSource drawsound;
    public AudioSource playsound;
    public AudioSource disolvesound;

    [NonSerialized]
    public Card luacard;

    public async Task Appear(float d = 0.5f) {
      var cg = GetComponent<CanvasGroup>();
      cg.alpha = 0;
      await cg.DOFade(1, d).AsyncWaitForCompletion();
    }

    public async Task Disappear(float d = 0.5f) {
      await GetComponent<CanvasGroup>().DOFade(0, d).AsyncWaitForCompletion();
      if (this != null)
        Destroy(gameObject);
    }

    public void SetLuaCard(string card) {
      luacard = LuaManager.CreateLuaObject(card).Cast<Card>();
      luacard.cardView = this;
      image.texture = AtlasManager.GetTexture(luacard.image);
      frame.texture = AtlasManager.GetTexture(luacard.frame);
      cardName.text = luacard.name;
      ruleText.text = luacard.desc;
      luacard.setCardView();
      EventService.Listen(GameEvent.IG_OnCostModifiterChanged, luacard.setCardView);
    }

    public async void Dissolve() {
      var effect = GetComponent<UIDissolve>();
      effect.enabled = true;
      effect.Play(true);
      await System.Threading.Tasks.Task.Delay((int)(effect.duration * 1000));
      if (this != null)
        Destroy(gameObject);
    }

    static Vector3 cscale = new Vector3(0.6f, 0.6f, 1);
    static Vector3 center = new Vector3(0, 0, -9);
    public async void DissolveInCenter() {
      transform.DOScale(cscale, 0.5f);
      await transform.DOMove(center, 0.5f).AsyncWaitForCompletion();
      Dissolve();
    }

    static Vector3 focusScal = new Vector3(2f, 2f, 1);
    public void OnBeginDrag(PointerEventData p) {
      if (!canPlay || inPanel || !GameManager.messiahView.canMove) {
        return;
      }
      hands.ReleaseFromHand(this);
      transform.DORotateQuaternion(Quaternion.identity, 0.2f);
      transform.DOScale(focusScal, 0.2f);
    }

    public void OnDrag(PointerEventData p) {
      if (!canPlay || inPanel || !GameManager.messiahView.canMove) {
        return;
      }
      transform.position = p.pointerCurrentRaycast.worldPosition;
    }

    public void OnEndDrag(PointerEventData p) {
      if (!canPlay || inPanel || !GameManager.messiahView.canMove) {
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