using System;
using UnityEngine;
using UnityEditor;
using MoveHistory = System.Collections.Generic.LinkedList<TicTacToeBoard>;

public class TestGameWindow : EditorWindow {

	private GameSettings settings;
	private TestGameConstraint selectedConstraint;
	
	[MenuItem("Debug/Play Test TTT Game")]
	public static void OpenWindow() {
		TestGameWindow window = GetWindow<TestGameWindow>("Test TTT Game");
		window.minSize = new Vector2(600, 480);
	}

	// Init private variable "settings".
	private void OnValidate() {
		string globalSettingsAssetPath = AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets($"t:{typeof(GameSettings).Name}")[0]); // global settings is a "singleton" SO
		settings = AssetDatabase.LoadAssetAtPath<GameSettings>(globalSettingsAssetPath);
	}

	private void OnGUI() {

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
}

internal class TestGameConstraint { 
	public enum WinLineType { Row, Column, Diagonal}

	public WinLineType LineType;
	public int Index;
}
