using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class CoinBehaviour : MonoBehaviour {

	[HideInInspector] public bool StartsInBottomLeftCorner; // set by CoinSpewer.cs during coin pool initialization
	[HideInInspector] public static float ScreenXDistance, ScreenYDistance;
	private Vector3 startPosition;

	// initial transform.position is set by CoinSpewer.cs, either to bottomLeft or bottomRight corner of screen.
	private void Awake() {
		startPosition = transform.position;
	}

	// start animation when gameObject is enabled
	private void OnEnable() {
		transform.position = startPosition;
		StartCoroutine(LaunchCoin());
	}

	// reset position to prepare for next animation 
	private void OnDisable() {
		transform.position = startPosition;
		StopCoroutine(LaunchCoin());
	}

	// Main coin animation coroutine
	private IEnumerator LaunchCoin() {
		float startTime = Time.realtimeSinceStartup;
		float xVelocity = Random.Range(.001f * ScreenXDistance, .004f * ScreenXDistance);
		xVelocity *= StartsInBottomLeftCorner ? 1 : -1;

		// animation loop
		float timeSinceStart;
		do {
			timeSinceStart = Time.realtimeSinceStartup - startTime;
			// y value is simple distance formula (i.e. y = at^2 + vt)
			float yVelocity = -(.008f * ScreenYDistance) * Mathf.Pow(timeSinceStart, 2) + (Random.Range(.013f,.015f) * ScreenYDistance) * timeSinceStart;
			transform.Translate(xVelocity, yVelocity, 0);
			yield return null;
		} while (timeSinceStart < 3);

		gameObject.SetActive(false);
	}
}
