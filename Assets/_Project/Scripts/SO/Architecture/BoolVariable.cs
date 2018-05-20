using System;
using System.Xml.Schema;
using UnityEngine;

[CreateAssetMenu(menuName = "Variable/Bool"), Serializable]
public class BoolVariable : ScriptableObject {
#if UNITY_EDITOR
	[Multiline] public string DeveloperDescription = "";
#endif

	public bool Value;

	public void SetValue(bool value) {
		Value = value;
	}

	public void SetValue(BoolVariable value) {
		Value = value.Value;
	}

	public void Toggle() {
		Value = !Value;
	}

	public static implicit operator bool(BoolVariable variable) {
		return variable.Value;
	}
}