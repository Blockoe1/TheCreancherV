/*****************************************************************************
// File Name : DiceUIManager.cs
// Author : Arcadia Koederitz
// Creation Date : 5/29/2026
// Last Modified : 5/29/2026
//
// Brief Description : UI amanger for dice reserving and rolling.
*****************************************************************************/
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FoolsBrand
{
    public class DiceUIManager : Manager
    {
        [SerializeField] private CanvasGroup reserveButtons;
        [SerializeField] private CanvasGroup rollButton;
        [SerializeField, Range(0, 1)] private float disabledAlpha;

        [SerializeField] private Camera overlayCamera;
        [SerializeField] private LayerMask UI;

        [SerializeField] private GameObject _hoveredObject;
        [Header("InfoBox")]
        [SerializeField] private GameObject _infoBox;
        [SerializeField] private TMP_Text _dieNameText;
        [SerializeField] private TMP_Text _dieDescText;

        public void ToggleReserveButtons(bool isVisible)
        {
            ToggleGroup(reserveButtons, isVisible);
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
            PlayerInputManager.ReservePressed(index);
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
                _dieNameText.text = hit.collider.GetComponent<DieBase>().DieName;
                _dieDescText.text = hit.collider.GetComponent<DieBase>().DieDescription;
            }
            else
            {
                _hoveredObject = null;
                _infoBox.SetActive(false);
            }
        }
    }
}
