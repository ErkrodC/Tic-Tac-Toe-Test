using UnityEngine;

public class GameSettings : ScriptableObject {
	
	private enum Player { PlayerOne, PlayerTwo }
	
	public IntVariable TilesPerSide;
	public TicTacToePiece Player1Piece, Player2Piece;
}
