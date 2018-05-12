using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : ScriptableObject {
	
	private enum Player { PlayerOne, PlayerTwo }
	
	public IntVariable TilesPerSide;
	public TicTacToePiece Player1Piece, Player2Piece;
}
