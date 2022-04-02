using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare50 {
    public interface IVariableObserver<T> {
        void OnValueChanged(T newValue);
    }
}
