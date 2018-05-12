using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupFader : MonoBehaviour {

	private CanvasGroup canvasGroup;

	private void OnEnable() {
		canvasGroup = GetComponent<CanvasGroup>();
	}

	public void FadeIn() {
		StartCoroutine(FadeInCoroutine());
	}
	
	public void FadeOut() {
		StartCoroutine(FadeOutCoroutine());
	}

	private IEnumerator FadeInCoroutine() {
		StartCoroutine(AlphaFaderCoroutine(0, 1));
		
		yield return new WaitUntil(() => Math.Abs(canvasGroup.alpha - 1) < 0.01f);
		StopCoroutine(AlphaFaderCoroutine(0, 1));

		canvasGroup.interactable = true;
		canvasGroup.blocksRaycasts = true;
		StopAllCoroutines();
	}

	private IEnumerator FadeOutCoroutine() {
		StartCoroutine(AlphaFaderCoroutine(1, 0));
		
		yield return new WaitUntil(() => Math.Abs(canvasGroup.alpha) < 0.01f);
		StopCoroutine(AlphaFaderCoroutine(1, 0));

		canvasGroup.interactable = false;
		canvasGroup.blocksRaycasts = false;
		StopAllCoroutines();
	}

	private IEnumerator AlphaFaderCoroutine(float fromValue, float toValue) {
		float step = 0f;
		while (true) {
			canvasGroup.alpha = Mathf.Lerp(fromValue, toValue, step);
			step += .01f;
			yield return null;
		}
	}
	
	
}
