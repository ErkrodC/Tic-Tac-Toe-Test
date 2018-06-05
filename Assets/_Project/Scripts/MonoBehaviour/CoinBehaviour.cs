using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class CoinBehaviour : MonoBehaviour {

	public bool StartsInBottomLeftCorner; // set by CoinSpewer.cs during coin pool initialization
	private Vector3 startPosition;

	// initial transform.position is set by CoinSpewer.cs, either to bottomLeft or bottomRight corner of screen.
	private void Awake() {
		startPosition = transform.position;
	}
	
	// start animation when gameObject is enabled
	private void OnEnable() {
		StartCoroutine(LaunchCoin());
	}

	// reset position to prepare for next animation 
	private void OnDisable() {
		transform.position = startPosition;
		StopCoroutine(LaunchCoin());
	}

	// Main coin animation coroutine
	private IEnumerator LaunchCoin() {
		float startTime = Time.time;
		float xVelocity = Random.Range(0.001f, 0.004f);
		xVelocity *= StartsInBottomLeftCorner ? 1 : -1; 

		// animation loop
		float timeSinceStart;
		do {
			timeSinceStart = Time.time - startTime;
			// y value is simple distance formula (i.e. y = at^2 + vt)
			transform.Translate(xVelocity, -.005f * Mathf.Pow(timeSinceStart, 2) + .01f * timeSinceStart, 0);
			yield return null;
		} while (timeSinceStart < 3);

		gameObject.SetActive(false);
	}
}
