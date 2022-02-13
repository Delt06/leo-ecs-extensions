using DELTation.LeoEcsExtensions.Components;
using UnityEngine;
using UnityEngine.UI;

namespace DELTation.LeoEcsExtensions.Views.Ui
{
    public abstract class ButtonClickEventProvider<TEvent> : MonoBehaviour where TEvent : struct
    {
        [SerializeField] private Button _button;

        private IEntityView _entityView;

        public void Construct(IEntityView entityView)
        {
            _entityView = entityView;
        }

        protected virtual void OnEnable()
        {
            _button.onClick.AddListener(Button_OnClick);
        }

        protected virtual void OnDisable()
        {
            _button.onClick.RemoveListener(Button_OnClick);
        }

        private void Button_OnClick()
        {
            if (_entityView.TryGetEntity(out var entity))
            {
                ref var @event = ref entity.GetOrAdd<TEvent>();
                ConfigureEvent(ref @event);
            }
        }

        protected virtual void ConfigureEvent(ref TEvent @event) { }
    }
}