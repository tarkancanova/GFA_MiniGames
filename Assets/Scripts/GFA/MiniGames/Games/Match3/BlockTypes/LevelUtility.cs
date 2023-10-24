using UnityEngine;

namespace GFA.MiniGames.Games.Match3.BlockTypes
{
	public static class LevelUtility
	{
		public static int GetIndexOfPosition(Vector2Int position, int width)
		{
			return position.x + position.y * width;
		}
		
		public static Vector2Int GetPositionOfIndex(int index, int width)
		{
			return new Vector2Int(index % width, index / width);
		}
	}
}