using UnityEngine;

public class ProceduralBoardGenerator : MonoBehaviour {

	[SerializeField] private GameSettings settings;
	[SerializeField] private GameObject tilePrefab;

	public void GenerateBoard() {
		int totalNumberOfTiles = settings.TilesPerSide * settings.TilesPerSide;
		for (int i = 0; i < totalNumberOfTiles; i++) {
			Instantiate(tilePrefab, transform);
		}
	}
}
