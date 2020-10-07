namespace Messiah.UI {
  using System;
  using UnityEngine;
  using DG.Tweening;
  using UnityEngine.UI;
  using UnityEngine.EventSystems;
  using XLua;
  using Logic;
  using Coffee.UIExtensions;

  public class CardView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    [NonSerialized]
    public HandView hands;
    [NonSerialized]
    public bool canPlay;

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
    }

    public async void Dissolve() {
      effect.enabled = true;
      effect.Play();
      await System.Threading.Tasks.Task.Delay((int)(effect.duration * 1000));
      Destroy(gameObject);
    }

    static Vector3 focusScal = new Vector3(1.5f, 1.5f, 1);
    public void OnBeginDrag(PointerEventData p) {
      if (!canPlay) return;
      hands.ReleaseFromHand(this);
      transform.DORotateQuaternion(Quaternion.identity, 0.2f);
      transform.DOScale(focusScal, 0.2f);
    }

    public void OnDrag(PointerEventData p) {
      if (!canPlay) return;
      transform.position = p.pointerCurrentRaycast.worldPosition;
    }

    public void OnEndDrag(PointerEventData p) {
      if (!canPlay) return;
      if (p.pointerCurrentRaycast.worldPosition.y > 0) {
        luacard.onPlay();
      } else
        hands.AddToHand(this);
    }

  }
}