using System.Collections;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;
using MoveHistory = System.Collections.Generic.LinkedList<TicTacToeBoard>;
using Random = UnityEngine.Random;
using static UnityEditor.AssetDatabase;

public class TestGameWindow : EditorWindow {

	private bool runningTestGame;
	private const float MoveDelayThreshold = 0.1f;
	private GameController gameController;
	private GameEvent changeTurnRequest;
	private GameSettings settings;
	private IEnumerator coroutine;
	private TicTacToeBoard runningBoard, drawnGameBoard;
	private TileControllerList tileControllers;
	private BoardController boardController;

	[MenuItem("Debug/Game and Animations")]
	public static void OpenWindow() {
		TestGameWindow window = GetWindow<TestGameWindow>("Test TTT Game");
		window.minSize = new Vector2(600, 480);
	}

	private void OnEnable() {
		runningBoard = LoadAssetAtPath<TicTacToeBoard>(GUIDToAssetPath(FindAssets("RunningBoard")[0]));
		settings = LoadAssetAtPath<GameSettings>(GUIDToAssetPath(FindAssets("GlobalGameSettings")[0]));
		changeTurnRequest = LoadAssetAtPath<GameEvent>(GUIDToAssetPath(FindAssets("ChangeTurnRequest")[0]));
		tileControllers = LoadAssetAtPath<TileControllerList>(GUIDToAssetPath(FindAssets($"t:{typeof(TileControllerList).Name}")[0]));
	}

	private void Update() {
		Repaint();
		
		if (runningTestGame) {
			coroutine.MoveNext();
		}
	}

	private void OnGUI() {
		// NOTE Hard reference to GameController is desirable in this case since no game controller means no possibility to test a game.
		if (gameController == null) {
			gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		}
		
		if (!Application.isPlaying) {
			GUILayout.Label("Editor must be in play mode in order to test.");
		} else if (!gameController.InGame){
			GUILayout.Label("Game must be started in order to test.");
		} else if (runningTestGame) {
			GUILayout.Label("Test game running...");
		} else {
			EditorGUILayout.LabelField("Game Testing", EditorStyles.boldLabel);
			GUILayout.Label("Select desired game-ending state.");

			DrawGameTestingButtons();
			
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			
			EditorGUILayout.LabelField("Animation Testing", EditorStyles.boldLabel);
			GUILayout.Label("Select desired animation.");

			DrawAnimationTestingButtons();
		}
	}

	private void DrawAnimationTestingButtons() {
		if (boardController == null) {
			boardController = FindObjectOfType<BoardController>();
		}
		
		if (GUILayout.Button("Tile Population")) {
			boardController.GetComponent<Animator>().SetTrigger("PopulateTiles");
		}
	}

	private void DrawGameTestingButtons() {
		EditorGUILayout.BeginHorizontal();
		{
			// Generate row buttons.
			for (int i = 0; i < settings.TilesPerSide; i++) {
				if (GUILayout.Button($"Row {i + 1}")) {
					runningTestGame = true;
					coroutine = RunTestGame(GenerateFullGame(new DesiredEndGame(GameEndType.Row, i)));
				}
			}
		} EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		{
			// Generate column buttons.
			for (int i = 0; i < settings.TilesPerSide; i++) {
				if (GUILayout.Button($"Column {i + 1}")) {
					runningTestGame = true;
					coroutine = RunTestGame(GenerateFullGame(new DesiredEndGame(GameEndType.Column, i)));
				}
			}
		} EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		{
			// Generate diagonal buttons.
			for (int i = 0; i < 2; i++) {
				string buttonText = i == 0 ? "\\ Diagonal" : "/ Diagonal";
				if (GUILayout.Button(buttonText)) {
					runningTestGame = true;
					coroutine = RunTestGame(GenerateFullGame(new DesiredEndGame(GameEndType.Diagonal, i)));
				}
			}
		} EditorGUILayout.EndHorizontal();

		if (GUILayout.Button("Draw")) {
			runningTestGame = true;
			coroutine = RunTestGame(GenerateFullGame(new DesiredEndGame(GameEndType.Draw, -1)));
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
		List<TileCoord> availableTiles = new List<TileCoord>();

		// Winning player turn.
		if (currentTurn.Player == Player.One) {
			switch (desiredEndGame.GameEndType) {
				case GameEndType.Row:
					// gather available empty row tiles
					for (int column = 0; column < settings.TilesPerSide; column++) {
						if (board.Matrix[desiredEndGame.Index][column] == null) availableTiles.Add(new TileCoord(desiredEndGame.Index, column));
					}
					break;
				case GameEndType.Column:
					// gather available empty column tiles
					for (int row = 0; row < settings.TilesPerSide; row++) {
						if (board.Matrix[row][desiredEndGame.Index] == null) availableTiles.Add(new TileCoord(row, desiredEndGame.Index));
					}
					break;
				case GameEndType.Diagonal:
					// gather available empty diagonal tiles 
					for (int diagIndex = 0; diagIndex < settings.TilesPerSide; diagIndex++) {
						int rowIndex = diagIndex;
						int columnIndex = desiredEndGame.Index == 0 ? diagIndex : (settings.TilesPerSide - 1) - diagIndex; // handle both diagonals
						if (board.Matrix[rowIndex][columnIndex] == null) availableTiles.Add(new TileCoord(rowIndex, columnIndex));
					}
					break;
				case GameEndType.Draw:
					if (drawnGameBoard == null) InitializeDrawnGameBoard();

					for (int row = 0; row < settings.TilesPerSide; row++) {
						for (int column = 0; column < settings.TilesPerSide; column++) {
							if (drawnGameBoard.Matrix[row][column] == settings.Player1Piece && board.Matrix[row][column] == null) availableTiles.Add(new TileCoord(row, column));
						}
					}
					break;
			}
		} else {	// Losing player turn.
			switch (desiredEndGame.GameEndType) {
				case GameEndType.Row:
				case GameEndType.Column:
					// Losing player cannot play on row/column that is reserved for game win, therefore substract its index from available rows or columns depending on desired GameEndType.
					List<int> availableRowIndices = desiredEndGame.GameEndType == GameEndType.Row ?
						                                Enumerable.Range(0, settings.TilesPerSide).Where(index => index != desiredEndGame.Index).ToList() :
						                                Enumerable.Range(0, settings.TilesPerSide).ToList();
					List<int> availableColumnIndices = desiredEndGame.GameEndType == GameEndType.Row ?
						                                   Enumerable.Range(0, settings.TilesPerSide).ToList() :
						                                   Enumerable.Range(0, settings.TilesPerSide).Where(index => index != desiredEndGame.Index).ToList();
					
					foreach (int availableRowIndex in availableRowIndices) {
						foreach (int availableColumnIndex in availableColumnIndices) {
							if (board.Matrix[availableRowIndex][availableColumnIndex] == null) {
								availableTiles.Add(new TileCoord(availableRowIndex, availableColumnIndex));
							}
						}
					}
					break;
				case GameEndType.Diagonal:
					// Gather empty tiles that are not on winning diagonal
					for (int row = 0; row < settings.TilesPerSide; row++) {
						for (int column = 0; column < settings.TilesPerSide; column++) {
							bool tileCoordIsOnDiagonal = desiredEndGame.Index == 0 ? column == row : column == (settings.TilesPerSide - 1) - row;
							
							if (board.Matrix[row][column] == null && !tileCoordIsOnDiagonal) {
								availableTiles.Add(new TileCoord(row, column));
							}
						}
					}
					break;
				case GameEndType.Draw:
					if (drawnGameBoard == null) InitializeDrawnGameBoard();
					for (int row = 0; row < settings.TilesPerSide; row++) {
						for (int column = 0; column < settings.TilesPerSide; column++) {
							if (drawnGameBoard.Matrix[row][column] == settings.Player2Piece && board.Matrix[row][column] == null) availableTiles.Add(new TileCoord(row, column));
						}
					}
					break;
			}
		}

		// set a random (and appropriate) tile to currentTurn's player piece
		TileCoord randomTile = availableTiles[Random.Range(0, availableTiles.Count)];
		
		TicTacToeBoard proceedingBoard = TicTacToeBoard.SnapshotBoard(board);
		proceedingBoard.SetTile(randomTile.Row, randomTile.Column, currentTurn.Player == Player.One ? settings.Player1Piece : settings.Player2Piece);
		return proceedingBoard;
	}

	// Sets drawnGameBoard variable to a finished board that is a tied game.
	private void InitializeDrawnGameBoard() {
		drawnGameBoard = CreateInstance<TicTacToeBoard>();
		drawnGameBoard.InitializeBoard(settings.TilesPerSide);
		drawnGameBoard.Settings = settings;

		for (int i = 0; i < settings.TilesPerSide * settings.TilesPerSide; i++) {
			int row = i / settings.TilesPerSide,
			    column = i % runningBoard.TilesPerSide;

			drawnGameBoard.Matrix[row][column] = i % 2 == 0 ? settings.Player1Piece : settings.Player2Piece;
		}

		int randomRowIndex;
		if (settings.TilesPerSide % 2 == 0) { // e.g. 4x4
			// rotate right a single random row by 1 
			randomRowIndex = Random.Range(0, settings.TilesPerSide);
			List<TicTacToePiece> randomRow = drawnGameBoard.Matrix[randomRowIndex];
			drawnGameBoard.Matrix[randomRowIndex] = randomRow.Skip(1).Concat(randomRow.Take(1)).ToList();

		} else { // e.g. 3x3
			// swap two adjacent rows
			randomRowIndex = Random.Range(0, settings.TilesPerSide - 1);
			List<TicTacToePiece> tempRow = drawnGameBoard.Matrix[randomRowIndex];

			drawnGameBoard.Matrix[randomRowIndex] = drawnGameBoard.Matrix[randomRowIndex + 1];
			drawnGameBoard.Matrix[randomRowIndex + 1] = tempRow;
		}
	}

	// Handles drawing board and raising changeTurnRequest to progress game flow.
	private IEnumerator RunTestGame(MoveHistory generatedGame) {
		BoardController boardController = FindObjectOfType<BoardController>();
		
		boardController.ResetBoard();
		
		LinkedListNode<TicTacToeBoard> currentNode = generatedGame.First;
		while (currentNode != null) {
			runningBoard.Matrix = currentNode.Value.Matrix;
			
			// clear visual board, then redraw pieces
			for (int row = 0; row < settings.TilesPerSide; row++) {
				for (int column = 0; column < settings.TilesPerSide; column++) {
					if (currentNode.Value.Matrix[row][column] != null) {
						TileController correspondingTile = tileControllers.List.Find(tile => tile.Row == row && tile.Column == column);
						correspondingTile.SetPieceImage(currentNode.Value.Matrix[row][column].Image);
					}
				}
			}
			
			changeTurnRequest.Raise();
			currentNode = currentNode.Next;

			// NOTE yield waitforsecs doesn't seem to work correctly
			double endTime = EditorApplication.timeSinceStartup + MoveDelayThreshold;
			while (EditorApplication.timeSinceStartup < endTime) {
				yield return null;
			}
		}

		drawnGameBoard = null;
		runningTestGame = false;
	}
}
