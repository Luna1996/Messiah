namespace Messiah.UI {
  using UnityEngine;
  using DG.Tweening;
  using Messiah.Logic.GameCoreNS;

  public class MessiahView : MonoBehaviour {
    [SerializeField]
    public Vector2 ogpos;
    [SerializeField]
    public Vector3 ogscale;
    [SerializeField]
    public Vector2 igpos;
    [SerializeField]
    public Vector3 igscale;

    public void SwitchState(GameState gs) {
      var trans = (RectTransform)transform;
      switch (gs) {
        case GameState.OutGameState:
          trans.DOAnchorPos(ogpos, 2);
          trans.DOScale(ogscale, 2);
          break;
        case GameState.InGameState:
          trans.DOAnchorPos(igpos, 2);
          trans.DOScale(igscale, 2);
          break;
      }
    }
  }
}