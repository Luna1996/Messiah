namespace Messiah.UI {
  using System.Collections;
  using UnityEngine;
  using DG.Tweening;
  using UnityEngine.UI;
  using UnityEngine.EventSystems;
  using XLua;
  using Utility;

  public class CardView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    static int dissolveRate_id = 0;
    public HandView hands;
    public bool canPlay;

    public LuaTable luacard;

    Material material;
    void Start() {
      if (dissolveRate_id == 0) {
        dissolveRate_id = Shader.PropertyToID("_DissolveRate");
      }
      var rawImage = GetComponent<RawImage>();
      var originMaterial = rawImage.material;
      material = Instantiate(originMaterial);
      material.CopyPropertiesFromMaterial(originMaterial);
      rawImage.material = material;
    }

    public void SetLuaCard(string card) {

    }

    public void Burn() {
      canPlay = false;
      transform.SetAsLastSibling();
      DOTween.To((dissolveRate) => {
        material.SetFloat(dissolveRate_id, dissolveRate);
      }, 1, 0, 1).OnComplete(() => Destroy(gameObject));
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
        StartCoroutine(hands.DoRandomEfect());
        Burn();
      } else
        hands.AddToHand(this);
    }

  }
}