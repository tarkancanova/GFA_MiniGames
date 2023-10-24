using UnityEngine;

namespace GFA.MiniGames.Games.Match3.BlockTypes
{
	[CreateAssetMenu(menuName = "Game Data/Match3/Special Block")]
	public class SpecialBlockType : BlockType
	{
		public override bool UseGravity => true;

		public override void ExecuteClickInteraction(BlockInstance blockInstance)
		{
			Debug.Log("YOU INTERACTED WITH A SPECIAL BLOCK");
			//blockInstance.LevelData.Swap(blockInstance.Position,
			//	new Vector2Int(Random.Range(0, 5), Random.Range(0, 5)));

			var blocks = blockInstance.LevelData.GetInRange(blockInstance.Position, 4);
			foreach (var block in blocks)
			{
				blockInstance.LevelData.RemoveBlock(block.Position);
			}
			blockInstance.LevelData.ApplyGravity();
		}

		public override void OnAssigned(BlockInstance blockInstance)
		{
		}
	}
}