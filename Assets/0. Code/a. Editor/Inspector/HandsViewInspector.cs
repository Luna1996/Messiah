namespace Messiah.Editor {
  using UnityEditor;
  using UnityEngine;
  using Messiah.UI;

  [CustomEditor(typeof(HandView))]
  public class HandViewInspector : UnityEditor.Editor {

    new HandView target;
    SerializedProperty s_curvature;
    SerializedProperty s_widthRatio;
    SerializedProperty s_handSize;

    void OnEnable() {
      target = base.target as HandView;
      s_curvature = serializedObject.FindProperty(nameof(target.m_curvature));
      s_widthRatio = serializedObject.FindProperty(nameof(target.m_widthRatio));
    }

    public override void OnInspectorGUI() {
      EditorGUILayout.Slider(s_curvature, 0, 1, "曲率");
      EditorGUILayout.Slider(s_widthRatio, 0, 1, "宽度");
      if (serializedObject.hasModifiedProperties) {
        serializedObject.ApplyModifiedProperties();
        target.UpdateArcData();
      }
      EditorGUILayout.IntSlider(s_handSize, 0, 20, "手牌");
      if (serializedObject.hasModifiedProperties) {
        serializedObject.ApplyModifiedProperties();
      }
    }

    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected, typeof(HandView))]
    static void DrawTest(HandView self, GizmoType gizmoType) {
      var trans = self.transform as RectTransform;
      if (trans.hasChanged)
        self.UpdateArcData();

      Handles.color = Color.cyan;
      if (self.m_curvature == 0)
        Handles.DrawLine(
          trans.position - self.transData.halfWidth * trans.right,
          trans.position + self.transData.halfWidth * trans.right);
      else
        Handles.DrawWireArc(
          self.arcData.center,
          -trans.forward,
          self.arcData.from,
          self.arcData.degree,
          self.arcData.radius);
    }
  }
}