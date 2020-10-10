namespace Messiah.UI {
  public class EventPhaseView : UIMask {
    new async void Start() {
      duration = 0.5f;
      await base.Start();
      await Close();
    }
  }
}