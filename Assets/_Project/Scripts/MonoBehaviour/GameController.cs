using System;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	[SerializeField] private GameEvent gameStartEvent;
	[SerializeField] private GameSettings settings;
	[SerializeField] private ToggleGroup player1PieceToggleGroup, player2PieceToggleGroup, gameModeToggleGroup;
	
	public void StartGame() {
		// Apply settings via active toggles
		settings.Player1Piece = player1PieceToggleGroup.ActiveToggles().First().GetComponent<TicTacToePieceBinding>().Piece;
		settings.Player2Piece = player2PieceToggleGroup.ActiveToggles().First().GetComponent<TicTacToePieceBinding>().Piece;
		settings.TilesPerSide.SetValue(gameModeToggleGroup.ActiveToggles().First().GetComponent<GameModeBinding>().TilesPerSide);
		
		gameStartEvent.Raise();
	}

	public void PauseGame() {
		
	}

	public void ChangeTurn() {
		
	}
	
	public void ExitGame() {
		
	}
}
