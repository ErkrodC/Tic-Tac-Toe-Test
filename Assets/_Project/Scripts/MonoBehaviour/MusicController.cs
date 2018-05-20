using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicController : MonoBehaviour {


	[SerializeField] private MusicPlaylist playlist;
	[SerializeField] private AudioSource source;
	private int counter;

	private void Start() {
		counter = Random.Range(0, playlist.Songs.Count);
		StartCoroutine(PlayMusic());
	}

	public void ToggleMusic() {
		source.mute = !source.mute;
	}

	public void NextTrack() {
		source.Stop();
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
