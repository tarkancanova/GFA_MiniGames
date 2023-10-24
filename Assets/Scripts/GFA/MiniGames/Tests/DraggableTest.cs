using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GFA.MiniGames.Tests
{
    public class DraggableTest : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private Vector2 _dragOffset;

        private Transform _dragGraphics;
        
        public void OnDrag(PointerEventData eventData)
        {
            _dragGraphics.position = eventData.position + _dragOffset;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            print("Drag Started");
            _dragOffset = (Vector2)transform.position - eventData.position;
            var image = Instantiate(GetComponentInChildren<Image>(), transform.parent);
            image.raycastTarget = false;
            _dragGraphics = image.transform;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            print("Drag Ended");
            _dragGraphics.DOMove(transform.position, 0.2f).OnComplete(() => Destroy(_dragGraphics.gameObject));
        }

        public void OnDropped()
        {
            Destroy(_dragGraphics.gameObject);
        }
    }
}
