namespace Messiah.UI {
  using UnityEngine;
  using Logic;

  public class TextFlyer : MonoBehaviour {
    public bool dir;

    public void FlyText(string str) {
      var flytext = PrefabManager.Instanciate("FlyText", transform).GetComponent<FlyText>();
      flytext.Fly(str, dir);
    }
  }
}