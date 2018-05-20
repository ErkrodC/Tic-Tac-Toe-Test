using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicController : MonoBehaviour {


	[SerializeField] private MusicPlaylist playlist;
	[SerializeField] private AudioSource source;
	private int counter;
	private bool amMuted;

	private void Start() {
		counter = Random.Range(0, playlist.Songs.Count);
		StartCoroutine(PlayPlaylist());
	}

	public void ToggleMusic() {
		if (amMuted) {
			StartCoroutine(PlayPlaylist());
		} else {
			StopCoroutine(PlayPlaylist());
			source.Stop();
		}

		amMuted = !amMuted;
	}

	private IEnumerator PlayPlaylist() {
		while (true) {
			if (counter == playlist.Songs.Count) counter = 0;

			if (!amMuted) {
				source.clip = playlist.Songs[counter];
				source.Play();
				counter++;
			}
			
			yield return new WaitWhile(() => source.isPlaying);
		}
	}
}
