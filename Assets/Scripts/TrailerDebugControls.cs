/*****************************************************************************
// File Name : TrailerDebugControls.cs
// Author : Arcadia Koederitz
// Creation Date : 6/23/2026
// Last Modified : 6/23/2026
//
// Brief Description : Set of input controls for manipulating the game without UI.
*****************************************************************************/
using FoolsBrand.Audio;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FoolsBrand
{
    public class TrailerDebugControls : MonoBehaviour
    {
        private InputAction targetAction;
        private InputAction reserveAction;
        private InputAction rollAction;

        private void Awake()
        {
            if (TryGetComponent(out PlayerInput pi))
            {
                targetAction = pi.currentActionMap.FindAction("TargetLimb");
                reserveAction = pi.currentActionMap.FindAction("Reserve");
                rollAction = pi.currentActionMap.FindAction("Roll");

                targetAction.performed += TargetAction_performed;
                reserveAction.performed += ReserveAction_performed;
                rollAction.performed += RollAction_performed;
            }
        }

        private void OnDestroy()
        {
            targetAction.performed -= TargetAction_performed;
            reserveAction.performed -= ReserveAction_performed;
            rollAction.performed -= RollAction_performed;
        }

        private void TargetAction_performed(InputAction.CallbackContext obj)
        {
            Debug.Log(targetAction.GetBindingIndexForControl(obj.control));
            PlayerInputManager.LimbSelected(targetAction.GetBindingIndexForControl(obj.control));
        }
        private void RollAction_performed(InputAction.CallbackContext obj)
        {
            AudioManager.Instance.PlayOneShot("RollDice");
            PlayerInputManager.RollPressed();
        }

        private void ReserveAction_performed(InputAction.CallbackContext obj)
        {
            Debug.Log(reserveAction.GetBindingIndexForControl(obj.control));
            PlayerInputManager.ReservePressed(reserveAction.GetBindingIndexForControl(obj.control));
        }

    }
}
