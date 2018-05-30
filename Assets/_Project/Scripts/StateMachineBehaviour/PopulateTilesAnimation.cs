using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopulateTilesAnimation : StateMachineBehaviour {

	public float Delay = 1f;
	[SerializeField] private TileControllerList tileControllers;
	private IEnumerator coroutine;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
		foreach (TileController tileController in tileControllers.List) {
			tileController.GetComponent<Image>().enabled = false;
		}
		
		coroutine = PopulateTiles(animator);
	}

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		coroutine.MoveNext();
	}

	private IEnumerator PopulateTiles(Animator animator) {
		foreach (TileController tileController in tileControllers.List) {
			double endTime = Time.time + Delay;
			while (Time.time < endTime) {
				yield return null;
			}
			
			tileController.GetComponent<Image>().enabled = true;
		}
		
		animator.SetTrigger("PopulateTilesDone");
	}
}
