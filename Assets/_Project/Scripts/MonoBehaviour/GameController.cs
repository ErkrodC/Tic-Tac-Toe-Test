using System;
using UnityEngine;

public class GameController : MonoBehaviour {

	private enum GameState { InMainMenu, AskDesiredPiece, GamePaused, GameOver, Restart}

	private GameState state;
	[SerializeField] private GameEvent gameStartEvent, gamePauseEvent, turnChangeEvent;

	private void Start() {
		// NOTE class unfinished, this is used for testing
		gameStartEvent.Raise();
	}

}
