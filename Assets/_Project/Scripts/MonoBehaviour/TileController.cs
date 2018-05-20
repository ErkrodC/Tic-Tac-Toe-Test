using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class TileController : MonoBehaviour {

	[HideInInspector] public TileInfo TileInfo;

	[SerializeField] private TicTacToeBoard runningBoard;
	[SerializeField] private GameSettings settings;
	[SerializeField] private CurrentTurn currentTurn;
	[SerializeField] private Image pieceImageComponent;
	[SerializeField] private GameEvent changeTurnRequest;
	
	public void OnClick() {
		if (!TileInfo.IsOccupied) {
			switch (currentTurn.Player) {
				case Player.One:
					TileInfo.OccupyingPiece = settings.Player1Piece;
					SetPieceImage(settings.Player1Piece.Image);
					runningBoard.SetTile(TileInfo.Row, TileInfo.Column, settings.Player1Piece);
					break;
				case Player.Two:
					TileInfo.OccupyingPiece = settings.Player2Piece;
					SetPieceImage(settings.Player2Piece.Image);
					runningBoard.SetTile(TileInfo.Row, TileInfo.Column, settings.Player2Piece);
					break;
			}
			
			TileInfo.IsOccupied = true;
			changeTurnRequest.Raise();
		}
	}

	public void ResetTile() {
		pieceImageComponent.enabled = false;
		pieceImageComponent.sprite = null;
		TileInfo.OccupyingPiece = null;
		TileInfo.IsOccupied = false;
	}

	private void SetPieceImage(Sprite sprite) {
		pieceImageComponent.sprite = sprite;
		pieceImageComponent.enabled = true;
	}

}
