using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	[HideInInspector] public bool InGame = false;
	
	public TicTacToeBoard RunningBoard;
	[SerializeField] private GameEvent gameStartedEvent, turnChangedEvent, displayGameOverPanelRequest, resetRunningGameUIRequest, gameWonEvent;
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
		RunningBoard.InitializeBoard(settings.TilesPerSide);
		boardsLinkedList.AddFirst(TicTacToeBoard.SnapshotBoard(RunningBoard));
		
		//raise event for other scripts to respond
		gameStartedEvent.Raise();

		InGame = true;
	}

	// adds current board to history, and changes players' turns
	// checks current board in case the game is over;
	public void ChangeTurn() {
		boardsLinkedList.AddLast(TicTacToeBoard.SnapshotBoard(RunningBoard));
		currentTurn.Toggle();
		turnCount++;
		
		GameOverState gameOverState;
		if (RunningBoard.MeetsEndCondition(out gameOverState)) EndGame(gameOverState); 
		else turnChangedEvent.Raise();
	}

	// handles displaying game over information to screen 
	public void EndGame(GameOverState gameOverState) {
		switch (gameOverState) {
			case GameOverState.Player1Wins:
				gameOverStateText.text = "Player One Wins!";
				gameWonEvent.Raise();
				break;
			case GameOverState.Player2Wins:
				gameOverStateText.text = "Player Two Wins!";
				gameWonEvent.Raise();
				break;
			case GameOverState.Tie:
				gameOverStateText.text = "Tie";
				break;
		}
		
		// game over panel listens for this event to know when to activate
		InGame = false;
		displayGameOverPanelRequest.Raise();
	}

	// reset game, calling methods that try to minimize "new" uses
	public void RestartGame() {
		turnCount = 0;
		currentTurn.Player = Player.One;
		
		RunningBoard.ResetBoard();
		
		boardsLinkedList.Clear();
		boardsLinkedList.AddFirst(TicTacToeBoard.SnapshotBoard(RunningBoard));
		
		InGame = true;
		resetRunningGameUIRequest.Raise();
	}

	public void ExitGame() {
		Application.Quit();
	}
}
