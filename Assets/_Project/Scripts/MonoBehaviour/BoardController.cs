using UnityEngine;

public class BoardController : MonoBehaviour {

	[SerializeField] private TicTacToeBoard runningBoard;
	[SerializeField] private GameSettings settings;
	[SerializeField] private GameObject tilePrefab;

	public void GenerateBoard() {
		int totalNumberOfTiles = settings.TilesPerSide * settings.TilesPerSide;
		
		// Instantiate tiles
		for (int i = 0; i < totalNumberOfTiles; i++) {
			Instantiate(tilePrefab, transform);
		}

		// update tileInfos according to tile's sibling index
		foreach (TileController controller in GetComponentsInChildren<TileController>()) {
			int sibIndex = controller.transform.GetSiblingIndex();
			TileInfo tileInfo = ScriptableObject.CreateInstance<TileInfo>();
			tileInfo.Row = SibIndexToRow(sibIndex);
			tileInfo.Column = SibIndexToColumn(sibIndex);
			controller.TileInfo = tileInfo;
		}
	}

	private int SibIndexToRow(int index) {
		return index / runningBoard.TilesPerSide;
	}

	private int SibIndexToColumn(int index) {
		return index % runningBoard.TilesPerSide;
	}
}
