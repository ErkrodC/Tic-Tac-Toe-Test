using System;
using UnityEngine;
using UnityEditor;
using MoveHistory = System.Collections.Generic.LinkedList<TicTacToeBoard>;

public class TestGameWindow : EditorWindow {

	private GameController gameController;
	private GameSettings settings;
	
	[MenuItem("Debug/Play Test TTT Game")]
	public static void OpenWindow() {
		TestGameWindow window = GetWindow<TestGameWindow>("Test TTT Game");
		window.minSize = new Vector2(600, 480);
	}

	// Init private variables.
	private void OnEnable() {
		string globalSettingsAssetPath = AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets($"t:{typeof(GameSettings).Name}")[0]); // global settings is a "singleton" SO
		settings = AssetDatabase.LoadAssetAtPath<GameSettings>(globalSettingsAssetPath);
	}

	private void OnInspectorUpdate() {
		Repaint();
	}

	private void OnGUI() {
		if (gameController == null) gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		
		if (Application.isPlaying && gameController.InGame) {
			MoveHistory generatedGame;
		
			GUILayout.Label("Select desired winning line.");
			
			for (int i = 0; i < settings.TilesPerSide; i++) {
				if (GUILayout.Button($"Row {i + 1}")) {
					TestGameConstraint constraint = new TestGameConstraint(){LineType = TestGameConstraint.WinLineType.Row, Index = i};
					generatedGame = GenerateFullGame(constraint);
					RunTestGame(generatedGame);
				}
			}

			for (int i = 0; i < settings.TilesPerSide; i++) {
				if (GUILayout.Button($"Column {i + 1}")) {
					TestGameConstraint constraint = new TestGameConstraint(){LineType = TestGameConstraint.WinLineType.Column, Index = i};
					generatedGame = GenerateFullGame(constraint);
					RunTestGame(generatedGame);
				}
			}

			for (int i = 0; i < 2; i++) {
				string buttonText = i == 0 ? "TopLeft-BottomRight Diagonal" : "TopRight-BottomLeft Diagonal";
				if (GUILayout.Button(buttonText)) {
					TestGameConstraint constraint = new TestGameConstraint(){LineType = TestGameConstraint.WinLineType.Diagonal, Index = i};
					generatedGame = GenerateFullGame(constraint);
					RunTestGame(generatedGame);
				}
			}
		} else {
			GUILayout.Label("Game must be started in order to test.");
		}
	}

	// Returns a procedurally generated linked list of boards that represents a TTT game from start to finish.
	private MoveHistory GenerateFullGame(TestGameConstraint constraint) {
		MoveHistory generatedGame = new MoveHistory();

		switch (constraint.LineType) {
			case TestGameConstraint.WinLineType.Row:
				break;
			case TestGameConstraint.WinLineType.Column:
				break;
			case TestGameConstraint.WinLineType.Diagonal:
				break;
		}

		return null;
	}

	private void RunTestGame(MoveHistory generatedGame) {
		
	}
}

internal class TestGameConstraint { 
	public enum WinLineType { Row, Column, Diagonal}

	public WinLineType LineType;
	public int Index;
}
