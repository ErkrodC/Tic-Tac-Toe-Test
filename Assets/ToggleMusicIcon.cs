#pragma warning disable 0649
// icons set via inspector

using UnityEngine;
using UnityEngine.UI;

public class ToggleMusicIcon : MonoBehaviour {
	[SerializeField] private Image buttonImage;
	[SerializeField] private Sprite unmutedIcon, mutedIcon;

	public void ToggleIcon() {
		buttonImage.sprite = buttonImage.sprite == unmutedIcon ? mutedIcon : unmutedIcon;
	}
}
