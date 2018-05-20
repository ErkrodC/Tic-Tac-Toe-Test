using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicController : MonoBehaviour {


	[SerializeField] private MusicPlaylist playlist;
	[SerializeField] private AudioSource source;
	private int counter = 0;

	private void Start() {
		source.clip = playlist.Songs[0]; // kinda just like the first song :P
		source.Play();
		StartCoroutine(PlayMusic());
	}

	public void ToggleMusic() {
		source.mute = !source.mute;
	}

	public void NextTrack() {
		source.Stop(); // stopping music triggers PlayMusic coroutine to go to next track
		source.mute = false;
	}

	private IEnumerator PlayMusic() {
		while (true) {
			counter++;
			
			yield return new WaitWhile(() => source.isPlaying);
			
			counter %= playlist.Songs.Count;
			source.clip = playlist.Songs[counter];
			source.Play();
		}
	}
}
