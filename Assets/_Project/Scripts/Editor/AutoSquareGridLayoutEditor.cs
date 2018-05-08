using UnityEditor;

[CustomEditor(typeof(AutoSquareGridLayout),false), CanEditMultipleObjects]
public class AutoSquareGridLayoutEditor : Editor {
	private SerializedProperty itemsPerSide;
	private SerializedProperty padding;
	private SerializedProperty spacing;
	private SerializedProperty startCorner;
	private SerializedProperty startAxis;
	private SerializedProperty childAlignment;

	private void OnEnable() {
		itemsPerSide = serializedObject.FindProperty("ItemsPerSide");
		padding = serializedObject.FindProperty("m_Padding");
		spacing = serializedObject.FindProperty("m_Spacing");
		startCorner = serializedObject.FindProperty("m_StartCorner");
		startAxis = serializedObject.FindProperty("m_StartAxis");
		childAlignment = serializedObject.FindProperty("m_ChildAlignment");
	}

	public override void OnInspectorGUI() {
		serializedObject.Update();
        
		EditorGUILayout.PropertyField(itemsPerSide, true);
		EditorGUILayout.PropertyField(padding, true);
		EditorGUILayout.PropertyField(spacing, true);
		EditorGUILayout.PropertyField(startCorner, true);
		EditorGUILayout.PropertyField(startAxis, true);
		EditorGUILayout.PropertyField(childAlignment, true);
		
		serializedObject.ApplyModifiedProperties();
	}
}