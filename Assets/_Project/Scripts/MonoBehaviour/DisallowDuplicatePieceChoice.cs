using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DisallowDuplicatePieceChoice : MonoBehaviour {

	[SerializeField] private List<Toggle> playerOnePieceChoices, playerTwoPieceChoices;

	private void Start() {
		OnValueChanged();
	}

	public void OnValueChanged() {
		// de-interactable player two option which player one has selected, and vice versa
		for (int i = 0; i < playerOnePieceChoices.Count; i++) {
			Toggle player1Toggle = playerOnePieceChoices[i];
			Toggle player2Toggle = playerTwoPieceChoices[i];

			if (player1Toggle.isOn) {
				player2Toggle.interactable = false;
			} else if (player2Toggle.isOn) {
				player1Toggle.interactable = false;
			} else {
				player1Toggle.interactable = true;
				player2Toggle.interactable = true;
			}
		}
	}
}
