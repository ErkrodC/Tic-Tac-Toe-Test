using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CoinSpewer : MonoBehaviour {

	[SerializeField] private GameObject coinPrefab;
	private List<GameObject> coinPool = new List<GameObject>();
	private float delay = 0.1f;
	private int initialPoolSize = 15;

	private void Awake() {
		for (int i = 0; i < initialPoolSize; i++) {
			GameObject clone = Instantiate(coinPrefab, transform);
			clone.SetActive(false);
			coinPool.Add(clone);
		}
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
}
