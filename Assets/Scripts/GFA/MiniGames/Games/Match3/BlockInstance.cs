using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GFA.MiniGames.Games.Match3
{
    public class BlockInstance : MonoBehaviour
    {
        public Vector2Int Position { get; set; }
        public LevelData LevelData { get; set; }

        public float VerticalVelocity;
        
        public Vector3 DisplayPosition { get; set; }


        private GameObject _graphics;
        
        private BlockType _blockType;
        public BlockType BlockType
        {
            get => _blockType;
            set
            {

                if (_graphics)
                {
                    Destroy(_graphics);
                }
                
                _blockType = value;
                _graphics = _blockType.CreateGraphics();

                if (_graphics)
                {
                    _graphics.transform.SetParent(transform, false);

                    //graphicsTransform.offsetMax = Vector2.zero;
                    //graphicsTransform.offsetMin = Vector2.zero;
                }
            }
        }
        
        public void OnClicked()
        {
            BlockType.ExecuteClickInteraction(this);
        }

        public void OnRemoved()
        {
            
        }


        public void OnMoved()
        {
        }
    }
}
