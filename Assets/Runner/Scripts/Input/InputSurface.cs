using UnityEngine;
using UnityEngine.EventSystems;

namespace Runner.Input
{
    public class InputSurface : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        public delegate void DragHandler(Vector2 position);

        public delegate void PointerDownHandler(Vector2 position);

        private int? _pointerId;

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (_pointerId != eventData.pointerId) return;
            Drag?.Invoke(eventData.position);
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (_pointerId.HasValue) return;
            _pointerId = eventData.pointerId;
            PointerDown?.Invoke(eventData.position);
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            if (_pointerId != eventData.pointerId) return;
            _pointerId = null;
        }

        public event PointerDownHandler PointerDown;
        public event DragHandler Drag;
    }
}