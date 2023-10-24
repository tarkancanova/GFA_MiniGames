using UnityEngine;

namespace GFA.MiniGames.Games.Match3.BlockTypes
{
	public class EmptyBlockType: BlockType
	{
		public override bool UseGravity => true;

		public override GameObject CreateGraphics()
		{
			return null;
		}

		public override void ExecuteClickInteraction(BlockInstance blockInstance)
		{
		}

		public override void OnAssigned(BlockInstance blockInstance)
		{
		}
	}
}