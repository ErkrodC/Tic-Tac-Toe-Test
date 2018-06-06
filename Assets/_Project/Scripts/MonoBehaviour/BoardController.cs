using UnityEngine;
using System.Collections.Generic;

public class BoardController : MonoBehaviour {

	[SerializeField] private TileControllerList tileControllers;
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
		int actualSize = Mathf.FloorToInt(Mathf.Sqrt(tileControllers.List.Count));
		
		if (requiredSize != actualSize) {
			int difference = Mathf.Abs((requiredSize * requiredSize) - (tileControllers.List.Count));
			
			if (requiredSize > actualSize) {
				for (int i = 0; i < difference; i++) {
					GameObject tile = Instantiate(tilePrefab, transform); // generate needed UI tile game objects
					tileControllers.List.Add(tile.GetComponent<TileController>());
				}
			} else {
				for (int i = tileControllers.List.Count - 1; i >= (actualSize * actualSize) - difference; i--) { // remove extra UI tile game objects
					Destroy(tileControllers.List[i].gameObject);
					tileControllers.List.RemoveAt(i);
				}
			}
		}
	}

	public void ResetBoard() {
		foreach (TileController tileController in tileControllers.List) {
			tileController.ResetTile();
		}
	}

	public void HighlightWinningTiles() {
		// gather tile controllers for tiles that are not involved in win
		List<TileController> nonWinningTiles = tileControllers.List.FindAll(tileController => 
			!runningBoard.WinningTiles.Exists(tileCoord => 
				tileCoord.Row == tileController.Row && tileCoord.Column == tileController.Column));

		// black out those uninvolved tiles
		foreach (TileController tileController in nonWinningTiles) {
			tileController.BlackoutTilePiece();
		}
	}

	// used to give UI tile game objects their correct coordinates according to their sibling index 
	private void SetTileIndices() {
		foreach (TileController tileController in tileControllers.List) {
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
