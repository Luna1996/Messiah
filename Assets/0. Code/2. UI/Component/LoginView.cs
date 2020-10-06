namespace Messiah.UI {
  using UnityEngine;
  using UnityEngine.UI;
  using DG.Tweening;
  using Messiah.Logic;
  using Messiah.Logic.GameCoreNS;
  using Coffee.UIExtensions;

  public class LoginView : MonoBehaviour {
    public InputField username;

    AppRoot appRoot;
    UITransitionEffect effect;
    void Start() {
      effect = GetComponent<UITransitionEffect>();
      effect.Show();
      appRoot = GameObject.Find("AppRoot").GetComponent<AppRoot>();
    }

    public async void NewUserLogin() {
      if (string.IsNullOrEmpty(username.text)) return;
      UserData.LocalLogin(username.text);
      effect.Hide();
      await System.Threading.Tasks.Task.Delay((int)(effect.duration * 1000));
      Destroy(gameObject);
      appRoot.viewManager.SwitchToOutGameView();
    }
  }
}