using UnityEngine;

public class GameSettings : ScriptableObject {
	
	private enum Player { PlayerOne, PlayerTwo }
	
	public IntVariable TilesPerSide;
	public TicTacToePiece Player1Piece, Player2Piece;

	public void SetTilesPerSide(int tilesPerSide) {
		TilesPerSide.SetValue(tilesPerSide);
	}

	// this method structure is necessary to allow methods to be called via OnValueChanged calls
	public void ChangePlayerOnePiece(TicTacToePiece piece) {
		SetPlayerPiece(Player.PlayerOne, piece);
	}

	public void ChangePlayerTwoPiece(TicTacToePiece piece) {
		SetPlayerPiece(Player.PlayerTwo, piece);
	}

	private void SetPlayerPiece(Player player, TicTacToePiece piece) {
		switch (player) {
			case Player.PlayerOne:
				Player1Piece = piece;
				break;
			case Player.PlayerTwo:
				Player2Piece = piece;
				break;
		}
	}
}
