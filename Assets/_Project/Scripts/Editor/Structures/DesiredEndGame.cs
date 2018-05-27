public struct DesiredEndGame {
	public readonly GameEndType GameEndType;
	public readonly int Index;	// row or column index of desired winning row/column. For diagonals: topLeft-bottomRight is 0, topRight-bottomLeft is 1. 

	public DesiredEndGame(GameEndType gameEndType, int index) {
		GameEndType = gameEndType;
		Index = index;
	}
}
