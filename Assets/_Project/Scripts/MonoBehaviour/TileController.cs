using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class TileController : MonoBehaviour {

	// Tile info
	[HideInInspector] public int Row, Column;
	[HideInInspector] public bool IsOccupied;

	[SerializeField] private TicTacToeBoard runningBoard;
	[SerializeField] private GameSettings settings;
	[SerializeField] private CurrentTurn currentTurn;
	[SerializeField] private Image pieceImageComponent;
	[SerializeField] private GameEvent changeTurnRequest;
	
	public void OnClick() {
		if (!IsOccupied) {
			
			// overlay tile image with appropriate piece sprite, and set the corresponding tile in board data structure to the appropriate piece SO
			switch (currentTurn.Player) {
				case Player.One:
					SetPieceImage(settings.Player1Piece.Image);
					runningBoard.SetTile(Row, Column, settings.Player1Piece);
					break;
				case Player.Two:
					SetPieceImage(settings.Player2Piece.Image);
					runningBoard.SetTile(Row, Column, settings.Player2Piece);
					break;
			}
			
			IsOccupied = true;
			changeTurnRequest.Raise();
		}
	}

	public void ResetTile() {
		pieceImageComponent.enabled = false;
		pieceImageComponent.color = Color.white;
		pieceImageComponent.sprite = null;
		IsOccupied = false;
	}

	public void SetPieceImage(Sprite sprite) {
		pieceImageComponent.sprite = sprite;
		pieceImageComponent.enabled = true;
	}

	public void BlackoutTilePiece() {
		pieceImageComponent.color = Color.black;
	}
}
