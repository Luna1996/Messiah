namespace Messiah.Editor {
  using Messiah.UI;
  using UnityEditor;
  using UnityEngine;

  [CustomEditor(typeof(MoveEffect))]
  public class MoveEffectInspector : UnityEditor.Editor {
    public override void OnInspectorGUI() {
      var script = (MoveEffect)target;
      var trans = (RectTransform)script.transform;
      GUILayout.BeginHorizontal();
      if (GUILayout.Button("记录起点")) {
        script.spos = trans.position;
      }
      if (GUILayout.Button("回到起点")) {
        trans.position = script.spos;
      }
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();
      if (GUILayout.Button("记录终点")) {
        script.epos = trans.position;
      }
      if (GUILayout.Button("回到终点")) {
        trans.position = script.epos;
      }
      GUILayout.EndHorizontal();
    }
  }
}