using UnityEngine;

public class TileInfo : ScriptableObject {

	[HideInInspector] public int Row, Column;
	[HideInInspector] public bool IsOccupied;
	[HideInInspector] public TicTacToePiece OccupyingPiece;
}
