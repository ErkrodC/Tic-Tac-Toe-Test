using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;
using MoveHistory = System.Collections.Generic.LinkedList<TicTacToeBoard>;

public class TestGameWindow : EditorWindow {
	
	private enum GameEndType { Row, Column, Diagonal}
	
	private struct DesiredEndGame { 
		public readonly GameEndType GameEndType;
		public readonly int Index;	// row or column index of desired winning row/column. For diagonals: topLeft-bottomRight is 0, topRight-bottomLeft is 1. 

		public DesiredEndGame(GameEndType gameEndType, int index) {
			GameEndType = gameEndType;
			Index = index;
		}
	}

	[SerializeField] private GameSettings settings;
	[SerializeField] private TicTacToeBoard runningBoard;
	[SerializeField] private GameEvent changeTurnRequest;
	private GameController gameController;

	[MenuItem("Debug/Play Test TTT Game")]
	public static void OpenWindow() {
		TestGameWindow window = GetWindow<TestGameWindow>("Test TTT Game");
		window.minSize = new Vector2(600, 480);
	}

	// Allows window to update even when outside of focus.
	private void OnInspectorUpdate() {
		Repaint();
	}

	private void OnGUI() {
		// NOTE Hard reference to GameController is desirable in this case since no game controller means no possibility to test a game.
		if (gameController == null) {
			gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
			runningBoard = AssetDatabase.LoadAssetAtPath<TicTacToeBoard>(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("RunningBoard")[0]));
			settings = AssetDatabase.LoadAssetAtPath<GameSettings>(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("GlobalGameSettings")[0]));
			changeTurnRequest = AssetDatabase.LoadAssetAtPath<GameEvent>(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets($"ChangeTurnRequest")[0]));
		}
		
		if (Application.isPlaying && gameController.InGame) {
			MoveHistory generatedGame;
		
			GUILayout.Label("Select desired winning line.");
			
			// TODO use beginHorizontal for the different game end types
			// Generate row buttons.
			for (int i = 0; i < settings.TilesPerSide; i++) {
				if (GUILayout.Button($"Row {i + 1}")) {
					DesiredEndGame desiredEndGame = new DesiredEndGame(GameEndType.Row, i);
					generatedGame = GenerateFullGame(desiredEndGame);
					RunTestGame(generatedGame);
				}
			}

			// Generate column buttons.
			for (int i = 0; i < settings.TilesPerSide; i++) {
				if (GUILayout.Button($"Column {i + 1}")) {
					DesiredEndGame desiredEndGame = new DesiredEndGame(GameEndType.Column, i);
					generatedGame = GenerateFullGame(desiredEndGame);
					RunTestGame(generatedGame);
				}
			}

			// Generate diagonal buttons.
			for (int i = 0; i < 2; i++) {
				string buttonText = i == 0 ? "\\ Diagonal" : "/ Diagonal";
				if (GUILayout.Button(buttonText)) {
					DesiredEndGame desiredEndGame = new DesiredEndGame(GameEndType.Diagonal, i);
					generatedGame = GenerateFullGame(desiredEndGame);
					RunTestGame(generatedGame);
				}
			}
		} else {
			GUILayout.Label("Game must be started in order to test.");
		}
	}

	// Returns a procedurally generated linked list of boards that represents a TTT game from start to finish.
	private MoveHistory GenerateFullGame(DesiredEndGame desiredEndGame) {
		CurrentTurn currentTurn = CreateInstance<CurrentTurn>();
		MoveHistory generatedGame = new MoveHistory();
		TicTacToeBoard initialBoard = CreateInstance<TicTacToeBoard>();

		currentTurn.Player = Player.One;
		initialBoard.InitializeBoard(settings.TilesPerSide);
		initialBoard.Settings = settings;
		generatedGame.AddLast(initialBoard);

		bool gameIsFinished = false;
		while (!gameIsFinished) {
			generatedGame.AddLast(GenerateRandomProceedingBoard(generatedGame.Last.Value, desiredEndGame, currentTurn));
			currentTurn.Toggle();
			
			GameOverState gameOverState;
			gameIsFinished = generatedGame.Last.Value.MeetsEndCondition(out gameOverState);
		}

		return generatedGame;
	}

	// Generates a random proceeding board that is one move ahead of the passed board.
	// Takes in a DesiredEndGame to handle playing on tiles reserved for win condition.
	private TicTacToeBoard GenerateRandomProceedingBoard(TicTacToeBoard board, DesiredEndGame desiredEndGame, CurrentTurn currentTurn) {
		TicTacToeBoard proceedingBoard = TicTacToeBoard.SnapshotBoard(board);

		// Winning player turn.
		if (currentTurn.Player == Player.One) {
			switch (desiredEndGame.GameEndType) {
				case GameEndType.Row:
					// gather available empty tiles
					List<int> emptyTileColumnIndices = new List<int>();
					for (int column = 0; column < settings.TilesPerSide; column++) {
						if (board.Matrix[desiredEndGame.Index][column] == null) emptyTileColumnIndices.Add(column);
					}

					// set a random square in desired row to winning player piece
					int randomColumn = emptyTileColumnIndices[Random.Range(0, emptyTileColumnIndices.Count)];
					proceedingBoard.SetTile(desiredEndGame.Index, randomColumn, settings.Player1Piece);
					break;
				case GameEndType.Column:
					// gather available empty tiles
					List<int> emptyTileRowIndices = new List<int>();
					for (int row = 0; row < settings.TilesPerSide; row++) {
						if (board.Matrix[row][desiredEndGame.Index] == null) emptyTileRowIndices.Add(row);
					}

					// set a random square in desired column to winning player piece
					int randomRow = emptyTileRowIndices[Random.Range(0, emptyTileRowIndices.Count)];
					proceedingBoard.SetTile(desiredEndGame.Index, randomRow, settings.Player1Piece);
					break;
				case GameEndType.Diagonal:
					int rowIndex, columnIndex;
					bool topLeftBottomRightDiagChosen = desiredEndGame.Index == 0;
					List<int> emptyTileDiagonalIndices = new List<int>();
					
					// gather available empty tiles 
					for (int diagIndex = 0; diagIndex < settings.TilesPerSide; diagIndex++) {
						rowIndex = diagIndex;
						columnIndex = topLeftBottomRightDiagChosen ? diagIndex : (settings.TilesPerSide - 1) - diagIndex; // handle both diagonals

						if (board.Matrix[rowIndex][columnIndex] == null) emptyTileDiagonalIndices.Add(diagIndex);
					}
					
					// set a random square in desired diagonal to winning player piece
					int randomDiagIndex = emptyTileDiagonalIndices[Random.Range(0, emptyTileDiagonalIndices.Count)];

					rowIndex = randomDiagIndex;
					columnIndex = topLeftBottomRightDiagChosen ? randomDiagIndex : (settings.TilesPerSide - 1) - randomDiagIndex; // handle both diagonals
					proceedingBoard.SetTile(rowIndex, columnIndex, settings.Player1Piece);
					break;
			}
		} else {	// Losing player turn.
			List<int> availableRowIndices, availableColumnIndices;
			List<TileCoord> availableTiles;
			
			switch (desiredEndGame.GameEndType) {
				case GameEndType.Row:
					availableRowIndices = Enumerable.Range(0, settings.TilesPerSide).Where(index => index != desiredEndGame.Index).ToList();
					availableColumnIndices = Enumerable.Range(0, settings.TilesPerSide).ToList();

					availableTiles = new List<TileCoord>();
					foreach (int availableRowIndex in availableRowIndices) {
						foreach (int availableColumnIndex in availableColumnIndices) {
							if (board.Matrix[availableRowIndex][availableColumnIndex] == null) {
								availableTiles.Add(new TileCoord(availableRowIndex, availableColumnIndex));
							}
						}
					}

					TileCoord randomTile = availableTiles[Random.Range(0, availableTiles.Count)];
					proceedingBoard.SetTile(randomTile.Row, randomTile.Column, settings.Player2Piece);

					break;
				case GameEndType.Column:
					availableRowIndices = Enumerable.Range(0, settings.TilesPerSide).ToList();
					availableColumnIndices = Enumerable.Range(0, settings.TilesPerSide).Where(index => index != desiredEndGame.Index).ToList();
					
					availableTiles = new List<TileCoord>();
					foreach (int availableRowIndex in availableRowIndices) {
						foreach (int availableColumnIndex in availableColumnIndices) {
							if (board.Matrix[availableRowIndex][availableColumnIndex] == null) {
								availableTiles.Add(new TileCoord(availableRowIndex, availableColumnIndex));
							}
						}
					}
					
					randomTile = availableTiles[Random.Range(0, availableTiles.Count)];
					proceedingBoard.SetTile(randomTile.Row, randomTile.Column, settings.Player2Piece);
					
					break;
				case GameEndType.Diagonal:
					availableTiles = new List<TileCoord>();

					for (int row = 0; row < settings.TilesPerSide; row++) {
						for (int column = 0; column < settings.TilesPerSide; column++) {
							bool tileCoordIsOnDiagonal = desiredEndGame.Index == 0 ? column == row : column == (settings.TilesPerSide - 1) - row;
							
							if (proceedingBoard.Matrix[row][column] == null && !tileCoordIsOnDiagonal) {
								availableTiles.Add(new TileCoord(row, column));
							}
						}
					}
					
					randomTile = availableTiles[Random.Range(0, availableTiles.Count)];
					proceedingBoard.SetTile(randomTile.Row, randomTile.Column, settings.Player2Piece);
					
					break;
			}
		}

		return proceedingBoard;
	}

	// TODO use Move struct to make code more efficient
	private void RunTestGame(MoveHistory generatedGame) {
		BoardController boardController = FindObjectOfType<BoardController>();
		List<TileController> tiles = boardController.TileControllerList;
		
		LinkedListNode<TicTacToeBoard> currentNode = generatedGame.First;
		while (currentNode != null) {
			runningBoard.Matrix = currentNode.Value.Matrix;
			
			// clear visual board, then redraw pieces
			for (int row = 0; row < settings.TilesPerSide; row++) {
				for (int column = 0; column < settings.TilesPerSide; column++) {
					if (currentNode.Value.Matrix[row][column] != null) {
						TileController correspondingTile = tiles.Find(tile => tile.Row == row && tile.Column == column);
						correspondingTile.SetPieceImage(currentNode.Value.Matrix[row][column].Image);
					}
				}
			}
			
			changeTurnRequest.Raise();
			currentNode = currentNode.Next;
		}
	}
}
