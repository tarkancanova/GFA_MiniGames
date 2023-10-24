using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GFA.MiniGames.Games.Match3.BlockTypes
{
    
    [CreateAssetMenu(menuName = "Game Data/Match3/Simple Block")]
    public class SimpleBlockType : BlockType
    {
        public override bool UseGravity => true;

        public override void ExecuteClickInteraction(BlockInstance blockInstance)
        {
            var matching = blockInstance.LevelData.GetAllMatchingNeighbors(blockInstance.Position);
            if (matching.Length >= 3)
            {
                foreach (var block in matching)
                {
                    blockInstance.LevelData.RemoveBlock(block.Position);
                }
                blockInstance.LevelData.ApplyGravity();
            }
        }

        public override void OnAssigned(BlockInstance blockInstance)
        {
        }
    }
}
