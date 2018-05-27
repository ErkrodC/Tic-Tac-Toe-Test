using System.Collections.Generic;
using UnityEngine;

public class TicTacToeBoard : ScriptableObject {
	// TODO refactor to accomodate public Settings variable (i.e. modify SnapShotBoard and InitializeBoard to handle that variable and others more effectively
	
	public int TilesPerSide;
	public List<List<TicTacToePiece>> Matrix;
	public GameSettings Settings;

	// Returns a copy of the passed board
	public static TicTacToeBoard SnapshotBoard(TicTacToeBoard boardToCopy) {
		TicTacToeBoard board = CreateInstance<TicTacToeBoard>();
		board.InitializeBoard(boardToCopy.TilesPerSide);
		board.TilesPerSide = boardToCopy.TilesPerSide;
		board.Settings = boardToCopy.Settings;
		
		for (int i = 0; i < board.TilesPerSide; i++) {
			for (int j = 0; j < board.TilesPerSide; j++) {
				board.Matrix[i][j] = boardToCopy.Matrix[i][j];
			}
		}

		return board;
	}

	// Initializes Matrix with null values at each coordinate
	public void InitializeBoard(int tilesPerSide) {
		Matrix = new List<List<TicTacToePiece>>();
		TilesPerSide = tilesPerSide;

		for (int i = 0; i < TilesPerSide; i++) {
			Matrix.Add(new List<TicTacToePiece>());
			
			for (int j = 0; j < TilesPerSide; j++) {
				Matrix[i].Add(null);
			}
		}
	}

	public void SetTile(int row, int column, TicTacToePiece piece) {
		Matrix[row][column] = piece;
	}

	// Sets each coordinate in Matrix to null. Note that no new lists are created in this method
	public void ResetBoard() {
		for (int i = 0; i < TilesPerSide; i++) {
			for (int j = 0; j < TilesPerSide; j++) {
				Matrix[i][j] = null;
			}
		}
	}

	// Checks if the passed board meets a tic tac toe end condiition.
	public bool MeetsEndCondition(out GameOverState gameOverState) {
		if (FullRowColumnOrDiagonalExists(out gameOverState)) {
			return true;
		}

		if (BoardIsFull()) {
			gameOverState = GameOverState.Tie;
			return true;
		}

		gameOverState = default(GameOverState);
		return false;
	}

	// Checks if a full row, column or diagonal is found using the passed coordinates as a starting point
	private bool FullRowColumnOrDiagonalExists(out GameOverState gameOverState) {
		// Check rows
		for (int row = 0; row < TilesPerSide; row++) {
			TicTacToePiece pieceToMatch = Matrix[row][0];

			for (int column = 0; column < TilesPerSide; column++) {
				if (Matrix[row][column] == null || Matrix[row][column] != pieceToMatch) { // empty tile or mixed pieces on row, break loop to go to next row
					break;
				}
				
				if (column == TilesPerSide - 1) { // full row found
					gameOverState = pieceToMatch == Settings.Player1Piece ? GameOverState.Player1Wins : GameOverState.Player2Wins;
					return true;
				}
			}
		}
		
		// Check columns
		for (int column = 0; column < TilesPerSide; column++) {
			TicTacToePiece pieceToMatch = Matrix[0][column];

			for (int row = 0; row < TilesPerSide; row++) {
				if (Matrix[row][column] == null || Matrix[row][column] != pieceToMatch) { //empty tile or mixed pieces on row, break loop to go to next row
					break;
				}
				
				if (row == TilesPerSide - 1) { // full column found
					gameOverState = pieceToMatch == Settings.Player1Piece ? GameOverState.Player1Wins : GameOverState.Player2Wins;
					return true;
				}
			}
		}
		
		// Check diagonals
		{
			// top-left to bottom-right diagonal
			{
				TicTacToePiece pieceToMatch = Matrix[0][0];

				int row = 0, column = 0;
				for (int i = 0; i < TilesPerSide; i++) {
					if (Matrix[row][column] == null || Matrix[row][column] != pieceToMatch) { //empty tile or mixed pieces on row, break loop to go to next row
						break;
					}
				
					if (i == TilesPerSide - 1) { // full column found
						gameOverState = pieceToMatch == Settings.Player1Piece ? GameOverState.Player1Wins : GameOverState.Player2Wins;
						return true;
					}

					row++;
					column++;
				}
			}
			
			// top-right to bottom-left diagonal
			{
				TicTacToePiece pieceToMatch = Matrix[0][TilesPerSide - 1];

				int row = 0, column = TilesPerSide - 1;
				for (int i = 0; i < TilesPerSide; i++) {
					if (Matrix[row][column] == null || Matrix[row][column] != pieceToMatch) { //empty tile or mixed pieces on row, break loop to go to next row
						break;
					}
				
					if (i == TilesPerSide - 1) { // full column found
						gameOverState = pieceToMatch == Settings.Player1Piece ? GameOverState.Player1Wins : GameOverState.Player2Wins;
						return true;
					}

					row++;
					column--;
				}
			}
		}

		gameOverState = default(GameOverState);
		return false;
	}

	private bool BoardIsFull() {
		for (int row = 0; row < TilesPerSide; row++) {
			for (int column = 0; column < TilesPerSide; column++) {
				if (Matrix[row][column] == null) return false;
			}
		}

		return true;
	}
}
