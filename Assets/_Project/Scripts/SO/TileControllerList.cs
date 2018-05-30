using UnityEngine;
using System.Collections.Generic;

public class TileControllerList : ScriptableObject {
	[HideInInspector] public List<TileController> List;

	private void OnEnable() {
		List = new List<TileController>();
	}
}
