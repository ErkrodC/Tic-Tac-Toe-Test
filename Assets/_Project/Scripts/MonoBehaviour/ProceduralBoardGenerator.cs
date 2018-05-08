using UnityEngine;

public class ProceduralBoardGenerator : MonoBehaviour {

	public IntReference TilesPerSide;
	public GameObject TilePrefab;

	private void Start() {
		GenerateBoard();
	}

	private void GenerateBoard() {
		int totalNumberOfTiles = TilesPerSide * TilesPerSide;
		for (int i = 0; i < totalNumberOfTiles; i++) {
			Instantiate(TilePrefab, transform);
		}
	}
}
