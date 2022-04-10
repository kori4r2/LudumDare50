using System.Collections.Generic;
using UnityEngine;

namespace LudumDare50 {
    public abstract class GenericEvent<T> : ScriptableObject {
        private List<IGenericEventListener<T>> listeners = new List<IGenericEventListener<T>>();

        public void AddListener(IGenericEventListener<T> newListener) {
            if (!listeners.Contains(newListener)) {
                listeners.Add(newListener);
            }
        }

        public void RemoveListener(IGenericEventListener<T> newListener) {
            if (listeners.Contains(newListener)) {
                listeners.Remove(newListener);
            }
        }

        public void Raise(T value) {
            foreach (IGenericEventListener<T> listener in listeners) {
                if (listener != null)
                    listener.OnEventRaised(value);
            }
        }
    }
}
