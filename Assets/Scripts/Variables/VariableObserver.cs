using UnityEngine;
using UnityEngine.Events;

namespace LudumDare50
{
	[System.Serializable]
	public class VariableObserver<T> : IVariableObserver<T> {
        [SerializeField] private GenericVariable<T> observedVariable;
        public GenericVariable<T> ObservedVariable => observedVariable;
        [SerializeField] private UnityEvent<T> callbackOnChange = new UnityEvent<T>();

        public VariableObserver(GenericVariable<T> variable, UnityAction<T> response) {
            observedVariable = variable;
            callbackOnChange = new UnityEvent<T>();
            callbackOnChange.AddListener(response);
        }

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
