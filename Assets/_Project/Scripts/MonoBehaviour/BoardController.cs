using UnityEngine;
using System.Collections.Generic;

public class BoardController : MonoBehaviour {

	[SerializeField] private TicTacToeBoard runningBoard;
	[SerializeField] private GameSettings settings;
	[SerializeField] private GameObject tilePrefab;
	private readonly List<TileController> tileControllerList = new List<TileController>();

	public void GenerateBoard() {
		ResetBoard();
		ValidateBoardSize();
		SetTileIndices();
	}

	public void ValidateBoardSize() {
		int requiredSize = settings.TilesPerSide;
		int actualSize = Mathf.FloorToInt(Mathf.Sqrt(tileControllerList.Count));
		
		if (requiredSize != actualSize) {
			int difference = Mathf.Abs((requiredSize * requiredSize) - (tileControllerList.Count));
			
			if (requiredSize > actualSize) {
				for (int i = 0; i < difference; i++) {
					GameObject tile = Instantiate(tilePrefab, transform);
					tileControllerList.Add(tile.GetComponent<TileController>());
				}
			} else {
				for (int i = tileControllerList.Count - 1; i >= (actualSize * actualSize) - difference; i--) {
					Destroy(tileControllerList[i].gameObject);
					tileControllerList.RemoveAt(i);
				}
			}
		}
	}

	public void ResetBoard() {
		foreach (TileController tileController in tileControllerList) {
			tileController.ResetTile();
		}
	}

	private void SetTileIndices() {
		foreach (TileController tileController in tileControllerList) {
			int sibIndex = tileController.transform.GetSiblingIndex();
			TileInfo tileInfo = ScriptableObject.CreateInstance<TileInfo>();
			tileInfo.Row = SibIndexToRow(sibIndex);
			tileInfo.Column = SibIndexToColumn(sibIndex);
			tileController.TileInfo = tileInfo;
		}
	}

	private int SibIndexToRow(int index) {
		return index / runningBoard.TilesPerSide;
	}

	private int SibIndexToColumn(int index) {
		return index % runningBoard.TilesPerSide;
	}
}
