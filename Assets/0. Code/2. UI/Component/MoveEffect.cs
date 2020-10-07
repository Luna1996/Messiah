namespace Messiah.UI {
  using UnityEngine;
  using DG.Tweening;
  public class MoveEffect : MonoBehaviour {
    public Vector3 spos;
    public Vector3 epos;

    Tween move;
    bool flag;
    void Start() {
      flag = true;
      DoLoop();
    }

    public async void DoLoop() {
      while (flag) {
        var t = Random.Range(2f, 5f);
        move = transform.DOMove(epos, t);
        await move.AsyncWaitForCompletion();
        if (!flag) break;
        t = Random.Range(2f, 5f);
        move = transform.DOMove(spos, t);
        await move.AsyncWaitForCompletion();
      }
    }

    void OnDestroy() {
      flag = false;
      move.Kill();
    }
  }
}