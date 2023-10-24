using UnityEngine;

namespace GFA.MiniGames.Games.Match3
{
	[CreateAssetMenu(menuName = "Game Data/Match3/Level Template")]
	public class LevelTemplate: ScriptableObject
	{
		[SerializeField, Min(3)]
		private int _gridSizeX;
		
		[SerializeField, Min(3)]
		private int _gridSizeY;
		
		[SerializeField]
		private BlockType[] _blockTypes;
		public BlockType[] BlockTypes => _blockTypes;

		[SerializeField]
		private BlockType[] _randomBlocks;
		public BlockType[] RandomBlock => _randomBlocks;

		[SerializeField]
		private bool _fillNullFromRandom;
		public bool FillNullFromRandom => _fillNullFromRandom;
		
		public Vector2Int GridSize => new Vector2Int(_gridSizeX, _gridSizeY);
	}
}