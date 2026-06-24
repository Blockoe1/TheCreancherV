/*****************************************************************************
// File Name : DiceUIManager.cs
// Author : Arcadia Koederitz
// Creation Date : 5/29/2026
// Last Modified : 5/29/2026
//
// Brief Description : UI amanger for dice reserving and rolling.
*****************************************************************************/
using NaughtyAttributes;
using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FoolsBrand
{
    public class DiceUIManager : Manager
    {
        [SerializeField] private CanvasGroup rollButton;
        [SerializeField, Range(0, 1)] private float disabledAlpha;

        [SerializeField] private Camera overlayCamera;
        [SerializeField] private LayerMask UI;

        [ShowNonSerializedField, ReadOnly] private GameObject _hoveredObject;
        [Header("InfoBox")]
        [SerializeField] private InfoBox _infoBox;

        private InputAction click;
        private bool canReserve;

        public override void Init(GameManager gm, HierarchyManager parentManager)
        {
            click = InputSystem.actions.FindAction("ClickInput");
            click.started += Click_started;
        }


        private void OnDestroy()
        {
            click.started -= Click_started;
        }

        private void Click_started(InputAction.CallbackContext obj)
        {
            if (_hoveredObject == null || _hoveredObject.GetComponent<DieBase>().IsReserved)
            {
                return;
            }

            OnReservePressed(DiceManager.Instance.DiceInPlay.IndexOf(_hoveredObject));
        }

        public void SetCanReserve(bool canReserve)
        {
            this.canReserve = canReserve;
        }

        public void ToggleRollButton(bool isVisible)
        {
            ToggleGroup(rollButton, isVisible);
        }

        private void ToggleGroup(CanvasGroup group, bool isVisible)
        {
            group.alpha = isVisible ? 1 : disabledAlpha;
            group.interactable = isVisible;
            group.blocksRaycasts = isVisible;
        }

        /// <summary>
        /// Switch this to a different manager this is temporary.
        /// When the roll button gets pressed, call the invoke
        /// </summary>
        public void OnRollPressed()
        {
            PlayerInputManager.RollPressed();
        }

        public void OnReservePressed(int index)
        {
            if (canReserve)
            {
                PlayerInputManager.ReservePressed(index);
            }
        }

        /// <summary>
        /// Do some mousecasting
        /// </summary>
        private void FixedUpdate()
        {
            Ray ray = overlayCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if(Physics.Raycast(ray, out RaycastHit hit, 5, LayerMask.GetMask("UI")) && !PauseMenu.IsGamePaused)
            {
                GameObject hitObj = hit.collider.gameObject;
                IDiceInfo diceInfo = hit.collider.GetComponent<IDiceInfo>();

                if (hitObj != _hoveredObject && diceInfo.IsClickable)
                {
                    _hoveredObject = hitObj;

                    //InvertColorEffect invertColorEffect = UnityEngine.Object.FindFirstObjectByType<InvertColorEffect>();
                    //if (!(invertColorEffect != null && invertColorEffect.isInverted))
                    //{

                    //    _infoBox.SetDisplayDice(diceInfo, (canReserve && !diceInfo.IsReserved && diceInfo is DieBase ? "\n\n<i>Click to reserve.</i>" : ""));
                    //}
                    _infoBox.SetDisplayDice(diceInfo, (canReserve && !diceInfo.IsReserved && diceInfo is DieBase ? "\n\n<i>Click to reserve.</i>" : ""));
                }
                
            }
            else if (_hoveredObject != null)
            {
                _infoBox.SetDisplayDice(null);

                _hoveredObject = null;
            }
        }
    }
}
