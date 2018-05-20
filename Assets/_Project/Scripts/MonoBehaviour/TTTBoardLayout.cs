using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class TTTBoardLayout : GridLayoutGroup {

	[SerializeField] private GameSettings settings;
    
	// used to auto size tiles in grid layout group
	public override void CalculateLayoutInputHorizontal() {
		base.CalculateLayoutInputHorizontal();

		float fHeight = (rectTransform.rect.height - ((settings.TilesPerSide - 1) * (spacing.y))) - ((padding.top + padding.bottom));
		float fWidth = (rectTransform.rect.width - ((settings.TilesPerSide - 1) * (spacing.x))) - ( (padding.right + padding.left));
		Vector2 vSize = new Vector2(fWidth / settings.TilesPerSide, (fHeight) / settings.TilesPerSide);
		
		cellSize = vSize;
	}
}