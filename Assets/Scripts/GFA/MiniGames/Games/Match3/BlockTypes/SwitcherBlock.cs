using UnityEngine;

namespace GFA.MiniGames.Games.Match3.BlockTypes
{
	[CreateAssetMenu(menuName = "Game Data/Match3/Switcher Block")]
    public class SwitcherBlock : BlockType
    {
        public override bool UseGravity => true;
        
        [SerializeField]
        private BlockType _targetBlockType;

        public override GameObject CreateGraphics()
        {
            var graphics = new GameObject("ContainerGraphics");
            var selfGraphics = base.CreateGraphics();
            var targetGraphics = _targetBlockType.CreateGraphics();
            
            targetGraphics.transform.SetParent(graphics.transform, false);
            selfGraphics.transform.SetParent(graphics.transform, false);
            selfGraphics.GetComponent<SpriteRenderer>().sortingOrder = 1;

            return graphics;
        }

        public override void ExecuteClickInteraction(BlockInstance blockInstance)
        {

        }

        public override void OnAssigned(BlockInstance blockInstance)
        {
            blockInstance.LevelData.RegisterNeighborDestructionListener(new LevelData.NeighborDestructionListener(
                blockInstance,
                () => OnNeighborDestroyed(blockInstance)
            ));
        }

        private void OnNeighborDestroyed(BlockInstance blockInstance)
        {
            blockInstance.BlockType = _targetBlockType;
        }
    }
}
