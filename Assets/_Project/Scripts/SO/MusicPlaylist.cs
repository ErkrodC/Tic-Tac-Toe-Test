using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Playlist")]
public class MusicPlaylist : ScriptableObject {
	public List<AudioClip> Songs;
}
