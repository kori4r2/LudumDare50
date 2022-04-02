using UnityEngine;
using UnityEngine.Events;

namespace LudumDare50 {
    [System.Serializable]
    public class EventListener : IEventListener {
        [SerializeField] private EventSO eventListened = null;
        [SerializeField] private UnityEvent eventCallback = new UnityEvent();

        public void OnEventRaised() {
            eventCallback?.Invoke();
        }

        public void StartListeningEvent() {
            eventListened?.AddListener(this);
        }

        public void StopListeningEvent() {
            eventListened?.RemoveListener(this);
        }
    }
}
