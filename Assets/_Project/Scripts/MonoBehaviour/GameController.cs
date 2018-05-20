using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	[SerializeField] private TicTacToeBoard runningBoard;
	[SerializeField] private GameEvent gameStartedEvent, turnChangedEvent, displayGameOverPanelRequest, resetRunningGameUIRequest;
	[SerializeField] private GameSettings settings;
	[SerializeField] private ToggleGroup player1PieceToggleGroup, player2PieceToggleGroup, gameModeToggleGroup;
	[SerializeField] private CurrentTurn currentTurn;
	[SerializeField] private TextMeshProUGUI gameOverStateText;
	

	private int turnCount;
	private readonly LinkedList<TicTacToeBoard> boardsLinkedList = new LinkedList<TicTacToeBoard>();

	public void StartGame() {
		turnCount = 0;
		currentTurn.Player = Player.One;
		
		// Gather game settings UI's active toggles
		settings.Player1Piece = player1PieceToggleGroup.ActiveToggles().First().GetComponent<TicTacToePieceBinding>().Piece;
		settings.Player2Piece = player2PieceToggleGroup.ActiveToggles().First().GetComponent<TicTacToePieceBinding>().Piece;
		settings.TilesPerSide.SetValue(gameModeToggleGroup.ActiveToggles().First().GetComponent<GameModeBinding>().TilesPerSide);
		
		// Initialize the starting board, and add its snapshot to the history of boards
		runningBoard.InitializeBoard(settings.TilesPerSide);
		boardsLinkedList.AddFirst(TicTacToeBoard.SnapshotBoard(runningBoard));
		
		//raise event for other scripts to respond
		gameStartedEvent.Raise();
	}

	// adds current board to history, and changes players' turns
	// checks current board in case the game is over;
	public void ChangeTurn() {
		boardsLinkedList.AddLast(TicTacToeBoard.SnapshotBoard(runningBoard));
		currentTurn.Toggle();
		turnCount++;
		
		GameOverState gameOverState;
		if (runningBoard.MeetsEndCondition(out gameOverState)) EndGame(gameOverState); 
		else turnChangedEvent.Raise();
	}

	// handles displaying game over information to screen 
	public void EndGame(GameOverState gameOverState) {
		switch (gameOverState) {
			case GameOverState.Player1Wins:
				gameOverStateText.text = "Player One Wins!";
				break;
			case GameOverState.Player2Wins:
				gameOverStateText.text = "Player Two Wins!";
				break;
			case GameOverState.Tie:
				gameOverStateText.text = "Tie";
				break;
		}
		
		// game over panel listens for this event to know when to activate
		displayGameOverPanelRequest.Raise();
	}

	// reset game, calling methods that try to minimize "new" uses
	public void RestartGame() {
		turnCount = 0;
		currentTurn.Player = Player.One;
		
		runningBoard.ResetBoard();
		
		boardsLinkedList.Clear();
		boardsLinkedList.AddFirst(TicTacToeBoard.SnapshotBoard(runningBoard));
		
		resetRunningGameUIRequest.Raise();
	}

	public void ExitGame() {
		Application.Quit();
	}
}
