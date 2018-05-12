using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomPropertyDrawer(typeof(ScriptableEnum), true)]
public class ScriptableEnumDrawer : PropertyDrawer {
	private readonly List<string> displayNames = new List<string>();
	private readonly List<string> enumAssetPaths = new List<string>();

	private int selectedIndex;

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		BuildPathList(property);

		// Check if property does not currently reference a SO, and if so set it to "None" in the inspector.
		int currentIndex;
		if (property.objectReferenceValue == null) {
			if (!displayNames.Contains("None")) {
				displayNames.Insert(0, "None");
				enumAssetPaths.Insert(0, "None");
			}

			currentIndex = 0;
		} else {
			currentIndex = displayNames.IndexOf(property.objectReferenceValue.name);
			displayNames.Remove("None");
			enumAssetPaths.Remove("None");
		}

		// Get the selected index from the inspector and use it to set the corresponding SO instance.
		selectedIndex = EditorGUI.Popup(position, label.text, currentIndex, displayNames.ToArray(), EditorStyles.popup);

		if (enumAssetPaths[selectedIndex] != "None") {
			string typeString = property.type.Remove(0, 6).Split('>')[0];
			Type type = AppDomain.CurrentDomain.GetAssemblies().ToList().Single(assembly => assembly.FullName.Split(',')[0] == "Assembly-CSharp").GetType(typeString, true);
			Object loadedAsset = AssetDatabase.LoadAssetAtPath(enumAssetPaths[selectedIndex], type);
			property.objectReferenceValue = loadedAsset;
		}

		property.serializedObject.ApplyModifiedProperties();
	}

	// Build lists of paths to SOs that represent the indices of the enum, as well as the prettified names.
	private void BuildPathList(SerializedProperty property) {
		string propertyType = property.type.Remove(0, 6).Split('>')[0];
		string[] guids = AssetDatabase.FindAssets($"t:{propertyType}");

		foreach (string guid in guids) {
			string assetPath = AssetDatabase.GUIDToAssetPath(guid);
			string displayName = assetPath.Split('/')[assetPath.Split('/').Length - 1].Split('.')[0];
			
			enumAssetPaths.Add(assetPath);
			displayNames.Add(displayName);
		}
	}
}