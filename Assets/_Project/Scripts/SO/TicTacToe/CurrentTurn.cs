using UnityEngine;

public class CurrentTurn : ScriptableObject {

	public Player Player = Player.One;

	public void Toggle() {
		Player = Player == Player.One ? Player.Two : Player.One;
	}
}
