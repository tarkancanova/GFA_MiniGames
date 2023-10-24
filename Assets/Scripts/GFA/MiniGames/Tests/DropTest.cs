using UnityEngine;
using UnityEngine.EventSystems;

namespace GFA.MiniGames.Tests
{
    public class DropTest : MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            print("Dropped!");
            var dragged = eventData.pointerDrag;
            if (dragged.TryGetComponent<DraggableTest>(out var draggableTest))
            {
                draggableTest.OnDropped();
                dragged.transform.SetParent(transform);
            }
        }
    }
}
