namespace Messiah.Editor {
  using UnityEditor;
  using UnityEngine;
  using Messiah.UI;

  [CustomEditor(typeof(FillBar))]
  public class FillBarInspector : UnityEditor.Editor {
    new FillBar target;
    SerializedProperty s_percent;
    static GUIContent percentLable = new GUIContent("百分比");

    void OnEnable() {
      target = base.target as FillBar;
      s_percent = serializedObject.FindProperty(nameof(target.percent));
    }

    public override void OnInspectorGUI() {
      EditorGUILayout.PropertyField(s_percent, percentLable);
      if (serializedObject.hasModifiedProperties) {
        serializedObject.ApplyModifiedProperties();
        target.UpdatePercent();
      }
    }
  }
}
