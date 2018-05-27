using UnityEngine;
using System.Collections.Generic;

public class BoardController : MonoBehaviour {

	public readonly List<TileController> TileControllerList = new List<TileController>();
	
	[SerializeField] private TicTacToeBoard runningBoard;
	[SerializeField] private GameSettings settings;
	[SerializeField] private GameObject tilePrefab;

	public void GenerateBoard() {
		ResetBoard();
		ValidateBoardSize();
		SetTileIndices();
	}

	// used to ensure the number of tiles on the UI board corresponds with the game settings
	public void ValidateBoardSize() {
		int requiredSize = settings.TilesPerSide;
		int actualSize = Mathf.FloorToInt(Mathf.Sqrt(TileControllerList.Count));
		
		if (requiredSize != actualSize) {
			int difference = Mathf.Abs((requiredSize * requiredSize) - (TileControllerList.Count));
			
			if (requiredSize > actualSize) {
				for (int i = 0; i < difference; i++) {
					GameObject tile = Instantiate(tilePrefab, transform); // generate needed UI tile game objects
					TileControllerList.Add(tile.GetComponent<TileController>());
				}
			} else {
				for (int i = TileControllerList.Count - 1; i >= (actualSize * actualSize) - difference; i--) { // remove extra UI tile game objects
					Destroy(TileControllerList[i].gameObject);
					TileControllerList.RemoveAt(i);
				}
			}
		}
	}

	public void ResetBoard() {
		foreach (TileController tileController in TileControllerList) {
			tileController.ResetTile();
		}
	}

	// used to give UI tile game objects their correct coordinates according to their sibling index 
	private void SetTileIndices() {
		foreach (TileController tileController in TileControllerList) {
			int sibIndex = tileController.transform.GetSiblingIndex();
			tileController.Row = SiblingIndexToRow(sibIndex);
			tileController.Column = SiblingIndexToColumn(sibIndex);
		}
	}

	private int SiblingIndexToRow(int index) {
		return index / runningBoard.TilesPerSide;
	}

	private int SiblingIndexToColumn(int index) {
		return index % runningBoard.TilesPerSide;
	}
}
