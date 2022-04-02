using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LudumDare50
{
	[System.Serializable]
	public class VariableObserver<T> : IVariableObserver<T> {
        [SerializeField] private GenericVariable<T> observedVariable;
        [SerializeField] private UnityEvent<T> callbackOnChange;

		public void OnValueChanged(T newValue) {
            callbackOnChange?.Invoke(newValue);
		}

        public void StartWatching() {
            observedVariable?.AddObserver(this);
        }

        public void StopWatching() {
            observedVariable?.RemoveObserver(this);
        }
	}
}
