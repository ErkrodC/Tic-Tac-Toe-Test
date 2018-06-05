using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Canvas))]
public class CoinSpewer : MonoBehaviour {

	[SerializeField] private GameObject coinPrefab;
	private Canvas spewerCanvas;
	private List<GameObject> coinPool = new List<GameObject>();
	private float delay = 0.1f;
	private int initialPoolSize = 25;
	private Vector3 bottomLeftCorner, bottomRightCorner;

	private void Awake() {
		spewerCanvas = GetComponent<Canvas>();

		InitializeCornerPositions();
		InitializeCoinPool();
		
		
	}

	public void OnGameWin() {
		StartCoroutine(SpewCoins());
	}

	private IEnumerator SpewCoins() {
		WaitForSeconds waitOnDelay = new WaitForSeconds(delay);
		
		foreach (GameObject coin in coinPool) {
			coin.SetActive(true);

			yield return waitOnDelay;
		}
	}

	private void InitializeCoinPool() {	
		for (int i = 0; i < initialPoolSize; i++) {
			GameObject clone = Instantiate(coinPrefab, i % 2 == 0 ? bottomLeftCorner : bottomRightCorner, Quaternion.identity, transform);
			clone.GetComponent<CoinBehaviour>().StartsInBottomLeftCorner = i % 2 == 0; 
			clone.SetActive(false);
			coinPool.Add(clone);
		}
	}

	private void InitializeCornerPositions() {
		Vector3[] spewerCanvasCorners = new Vector3[4];
		(spewerCanvas.transform as RectTransform)?.GetWorldCorners(spewerCanvasCorners);

		bottomLeftCorner = spewerCanvasCorners[0];
		bottomRightCorner = spewerCanvasCorners[3];
	}
}
