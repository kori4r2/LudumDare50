using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LudumDare50 {
    public class InputActivator : MonoBehaviour {
        [SerializeField] private InputActionAsset inputActions;
        private void OnEnable() {
            inputActions.Enable();
        }

        private void OnDisable() {
            inputActions.Disable();
        }
    }
}
