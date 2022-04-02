using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare50 {
    public abstract class GenericVariable<T> : ScriptableObject {
        [SerializeField] private T value;

        public T Value {
            get => value;
            set {
                this.value = value;
                NotifyObservers();
            }
        }

        private List<IVariableObserver<T>> observers = new List<IVariableObserver<T>>();

        public void AddObserver(IVariableObserver<T> newObserver) {
            if(!observers.Contains(newObserver)) {
                observers.Add(newObserver);
            }
        }

        public void RemoveObserver(IVariableObserver<T> newObserver) {
            if(observers.Contains(newObserver)) {
                observers.Remove(newObserver);
            }
        }

        private void NotifyObservers() {
            foreach(IVariableObserver<T> observer in observers) {
                observer?.OnValueChanged(Value);
            }
        }
    }
}
