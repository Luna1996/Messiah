namespace Messiah.Editor {
  using Messiah.UI;
  using UnityEditor;
  using UnityEngine;

  [CustomEditor(typeof(MessiahView))]
  public class MessiahViewInspector : UnityEditor.Editor {
    public override void OnInspectorGUI() {
      var script = (MessiahView)target;
      var trans = (RectTransform)script.transform;
      GUILayout.BeginHorizontal();
      if (GUILayout.Button("记录起点")) {
        script.ogpos = trans.anchoredPosition;
        script.ogscale = trans.localScale;
      }
      if (GUILayout.Button("回到起点")) {
        trans.anchoredPosition = script.ogpos;
        trans.localScale = script.ogscale;
      }
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();
      if (GUILayout.Button("记录终点")) {
        script.igpos = trans.anchoredPosition;
        script.igscale = trans.localScale;
      }
      if (GUILayout.Button("回到终点")) {
        trans.anchoredPosition = script.igpos;
        trans.localScale = script.igscale;
      }
      GUILayout.EndHorizontal();
      serializedObject.ApplyModifiedProperties();
    }
  }
}