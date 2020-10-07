namespace Messiah.UI {
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;
  using DG.Tweening;
  using UnityEngine.AddressableAssets;
  using System.Threading.Tasks;
  using Messiah.Logic;
  using XLua;

  public class HandView : MonoBehaviour {
    [SerializeField]
    public float m_curvature = 0;

    [SerializeField]
    public float m_widthRatio = 1;

    [SerializeField]
    public FillBar[] fillBar;

    [NonSerialized]
    public List<CardView> hands;

    GameData gameData;

    void Reset() {
      UpdateArcData();
    }

    void Start() {
      Logic.GameManager.handView = this;
      UpdateArcData();
      hands = new List<CardView>();
      gameData = Logic.GameCoreNS.GameCore.userData.currentGameData;
    }

    public async Task RemoveHands() {
      while (hands.Count > 0) {
        RemoveCard(0);
        await Task.Delay(100);
      }
    }

    public void AddCard(string card, CardLocation from = CardLocation.DrawPile) {
      Logic.GameCoreNS.GameCore.userData.currentGameData.hands.Add(card);
      var clone = Logic.PrefabManager.Instanciate("Card", transform);
      Vector3 pos, scale;
      Quaternion quat;
      GameManager.cardOnFly.GetLoc(from, out pos, out scale, out quat);
      clone.transform.position = pos;
      clone.transform.rotation = quat;
      clone.name = hands.Count.ToString();
      var cardui = clone.GetComponent<CardView>();
      cardui.canPlay = false;
      cardui.hands = this;
      cardui.SetLuaCard(card);
      hands.Add(cardui);
      RestoreCardPosition();
    }

    public void RemoveCard(int i) {
      if (hands.Count == 0) return;

      var cardui = hands[i];
      hands.RemoveAt(i);
      Logic.GameCoreNS.GameCore.userData.currentGameData.hands.RemoveAt(i);
      if (hands.Count > 0) RestoreCardPosition();
    }

    public void RemoveCard(Card card) {
      int i = 0;
      while (i < hands.Count) {
        if (hands[i] == card.cardView) {
          RemoveCard(i);
          return;
        }
        i++;
      }
    }

    public void RestoreCardPosition(float duration = 0.5f) {
      var dir = arcData.from;
      var rotateStep = Quaternion.AngleAxis(arcData.degree / (hands.Count + 1), -transform.forward);
      int index = 0;
      foreach (var cardui in hands) {
        index++;
        dir = rotateStep * dir;
        if (cardui == null) continue;
        cardui.transform.SetSiblingIndex(index - 1);
        cardui.gameObject.name = $"{index - 1}";
        var pos = arcData.center + dir * arcData.radius;
        var rot = Quaternion.FromToRotation(transform.up, dir);
        cardui.transform.DOMove(pos, duration);
        cardui.transform.DORotateQuaternion(rot, duration);
        cardui.transform.DOScale(Vector3.one, duration).OnComplete(() => { cardui.canPlay = true; });
      }
    }

    public void ReleaseFromHand(CardView ui) {
      hands.Remove(ui);
      RestoreCardPosition(0.2f);
      ui.transform.SetAsLastSibling();
    }

    public void AddToHand(CardView ui) {
      int index;
      int.TryParse(ui.gameObject.name, out index);
      hands.Insert(index, ui);
      RestoreCardPosition(0.2f);
    }

    [NonSerialized]
    public (Vector3[] rect, Vector3 cardgen, float halfWidth) transData =
    (new Vector3[4], Vector3.zero, 0);
    [NonSerialized]
    public (Vector3 center, Vector3 from, float degree, float radius) arcData =
    (Vector3.zero, Vector3.zero, 0, 0);
    public void UpdateArcData() {
      var trans = transform as RectTransform;
      trans.GetWorldCorners(transData.rect);
      transData.cardgen = (transData.rect[1] + transData.rect[2]) / 2 - transform.forward;
      transData.halfWidth = (transData.rect[0] - transData.rect[3]).magnitude / 2 * m_widthRatio;
      trans.hasChanged = false;

      if (m_curvature == 0) return;
      var radius = transData.halfWidth / m_curvature;
      var centerPos = trans.position - trans.up * radius;
      var halfArc = Mathf.Min(radius, transData.halfWidth);
      var halfRadian = Mathf.Asin(halfArc / radius);
      var startDir =
        trans.up * Mathf.Sqrt(radius * radius - halfArc * halfArc)
        - trans.right * halfArc;
      var degree = Mathf.Rad2Deg * halfRadian * 2;

      arcData.center = centerPos;
      arcData.from = startDir / startDir.magnitude;
      arcData.degree = degree;
      arcData.radius = radius;
    }

    void OnDestroy() {
    }

    #region 调试函数
    [SerializeField]
    public int m_handSize;

    public void SetFakeHands(int num) {
      m_handSize = num;
      if (num < hands.Count)
        while (num != hands.Count)
          RemoveCard(hands.Count - 1);
      else if (num > hands.Count)
        while (num != hands.Count)
          AddCard(null);
    }

    public IEnumerator DoRandomEfect() {
      var t = UnityEngine.Random.Range(0, 2);
      var r = UnityEngine.Random.Range(1, 3);
      switch (t) {
        case 0:
          for (int j = 0; j < r; j++) {
            AddCard(null);
            yield return new WaitForSeconds(0.2f);
          }
          break;
        case 1:
          for (int j = 0; j < r; j++) {
            RemoveCard(UnityEngine.Random.Range(0, hands.Count));
            yield return new WaitForSeconds(0.2f);
          }
          break;
      }
      t = UnityEngine.Random.Range(0, 2);
      // switch (t) {
      //   case 0:
      //     r = UnityEngine.Random.Range(0, 3);
      //     fillBar[r].percent += 0.1f;
      //     if (fillBar[r].percent > 1) fillBar[r].percent = 1;
      //     fillBar[r].UpdatePercent();
      //     break;
      //   case 1:
      //     r = UnityEngine.Random.Range(0, 3);
      //     fillBar[r].percent -= 0.1f;
      //     if (fillBar[r].percent < 0) fillBar[r].percent = 0;
      //     fillBar[r].UpdatePercent();
      //     break;
      // }
    }

    public void DrawFiveCard() {
      StartCoroutine(InitHands(0));
    }

    IEnumerator InitHands(float wait = 1) {
      yield return new WaitForSeconds(wait);
      for (int i = 0; i < 5; i++) {
        AddCard(null);
        yield return new WaitForSeconds(0.1f);
      }
    }
    #endregion
  }
}