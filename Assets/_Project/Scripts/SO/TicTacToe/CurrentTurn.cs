using UnityEngine;

public class CurrentTurn : ScriptableObject {

	public Player Turn = Player.One;

	public void Toggle() {
		Turn = Turn == Player.One ? Player.Two : Player.One;
	}
}
