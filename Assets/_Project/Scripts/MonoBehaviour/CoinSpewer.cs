using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Canvas))]
public class CoinSpewer : MonoBehaviour {

	[SerializeField] private GameObject coinPrefab;
	private Canvas spewerCanvas;
	private List<GameObject> coinPool = new List<GameObject>();
	private float delay = 0.05f;
	private int initialPoolSize = 50;
	private Vector3 bottomLeftCorner, bottomRightCorner;

	
	private void Awake() {
		spewerCanvas = GetComponent<Canvas>();
		InitializeCornerPositions();
		InitializeCoinPool();
	}

	
	// called via GameEventListener component on this object
	public void OnGameWin() {
		StartCoroutine(SpewCoins());
	}

	// enables coin objects on at a time. Coin animation (rise and fall) is contained in CoinBehaviour.cs
	private IEnumerator SpewCoins() {
		WaitForSeconds waitOnDelay = new WaitForSeconds(delay);
		
		foreach (GameObject coin in coinPool) {
			coin.SetActive(true);

			yield return waitOnDelay;
		}
	}

	// initializes "coinPool" list
	private void InitializeCoinPool() {	
		for (int i = 0; i < initialPoolSize; i++) {
			GameObject clone = Instantiate(coinPrefab, i % 2 == 0 ? bottomLeftCorner : bottomRightCorner, Quaternion.identity, transform);
			clone.GetComponent<CoinBehaviour>().StartsInBottomLeftCorner = i % 2 == 0; 
			clone.SetActive(false);
			coinPool.Add(clone);
		}
	}

	// initializes Vector3's representing bottomLeft and bottomRight corners of the screen for the sake of placing coins in the correct start position.
	private void InitializeCornerPositions() {
		Vector3[] spewerCanvasCorners = new Vector3[4];
		(spewerCanvas.transform as RectTransform)?.GetWorldCorners(spewerCanvasCorners);

		bottomLeftCorner = spewerCanvasCorners[0];
		bottomRightCorner = spewerCanvasCorners[3];

		CoinBehaviour.ScreenXDistance = Mathf.Abs(bottomLeftCorner.x - bottomRightCorner.x);
		CoinBehaviour.ScreenYDistance = Mathf.Abs(bottomLeftCorner.y - spewerCanvasCorners[1].y);
	}
}
