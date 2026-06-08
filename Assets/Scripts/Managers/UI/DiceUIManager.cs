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
        [SerializeField] private GameObject _infoBox;
        [SerializeField] private TMP_Text _dieNameText;
        [SerializeField] private TMP_Text _dieDescText;

        private InputAction click;
        private bool canReserve;

        public override void Init(GameManager gm, HierarchyManager parentManager)
        {
            Debug.Log("Run");
            click = InputSystem.actions.FindAction("ClickInput");
            Debug.Log(click);
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
            PlayerInputManager.OnRollPressed();
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
            if(Physics.Raycast(ray, out RaycastHit hit, 999, LayerMask.GetMask("UI")))
            {
                _hoveredObject = hit.collider.gameObject;
                _infoBox.SetActive(true);
                DieBase die = hit.collider.GetComponent<DieBase>();
                _dieNameText.text = die.DieName;
                _dieDescText.text = die.DieDescription + 
                    (canReserve && !die.IsReserved ? "\n\n<i>Click to reserve.</i>" : "");
            }
            else
            {
                _hoveredObject = null;
                _infoBox.SetActive(false);
            }
        }
    }
}
