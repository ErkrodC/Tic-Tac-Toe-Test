using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	[SerializeField] private TicTacToeBoard runningBoard;
	[SerializeField] private GameEvent gameStartedEvent, turnChangedEvent;
	[SerializeField] private GameSettings settings;
	[SerializeField] private ToggleGroup player1PieceToggleGroup, player2PieceToggleGroup, gameModeToggleGroup;
	[SerializeField] private CurrentTurn currentTurn;

	private int turnCount;
	private readonly LinkedList<TicTacToeBoard> boardsLinkedList = new LinkedList<TicTacToeBoard>();

	public void StartGame() {
		turnCount = 0;
		currentTurn.Turn = Player.One;
		
		// Gather game settings UI's active toggles
		settings.Player1Piece = player1PieceToggleGroup.ActiveToggles().First().GetComponent<TicTacToePieceBinding>().Piece;
		settings.Player2Piece = player2PieceToggleGroup.ActiveToggles().First().GetComponent<TicTacToePieceBinding>().Piece;
		settings.TilesPerSide.SetValue(gameModeToggleGroup.ActiveToggles().First().GetComponent<GameModeBinding>().TilesPerSide);
		
		// Initialize the starting board, and add its snapshot to the history of boards
		runningBoard.NewEmptyBoard(settings.TilesPerSide);
		boardsLinkedList.AddFirst(TicTacToeBoard.SnapshotBoard(runningBoard));
		
		//raise event for other scripts to respond
		gameStartedEvent.Raise();
	}

	public void ChangeTurn() {
		boardsLinkedList.AddLast(TicTacToeBoard.SnapshotBoard(runningBoard));
		currentTurn.Toggle();
		turnCount++;
		
		GameOverState gameOverState;
		if (runningBoard.MeetsEndCondition(out gameOverState)) EndGame(gameOverState); 
		else turnChangedEvent.Raise();
	}

	public void EndGame(GameOverState gameOverState) {
		switch (gameOverState) {
			case GameOverState.Player1Wins:
				break;
			case GameOverState.Player2Wins:
				break;
			case GameOverState.Tie:
				break;
		}
	}
}
