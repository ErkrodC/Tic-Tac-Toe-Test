using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class AutoSquareGridLayout : GridLayoutGroup {

	public IntReference ItemsPerSide;
    
	public override void CalculateLayoutInputHorizontal() {
		base.CalculateLayoutInputHorizontal();

		float fHeight = (rectTransform.rect.height - ((ItemsPerSide - 1) * (spacing.y))) - ((padding.top + padding.bottom));
		float fWidth = (rectTransform.rect.width - ((ItemsPerSide - 1) * (spacing.x))) - ( (padding.right + padding.left));
		Vector2 vSize = new Vector2(fWidth / ItemsPerSide, (fHeight) / ItemsPerSide);
		
		cellSize = vSize;
	}
}