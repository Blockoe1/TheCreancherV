/*****************************************************************************
// File Name : TutorialManager.cs
// Author : Arcadia Koederitz
// Creation Date : 6/4/2026
// Last Modified : 6/4/2026
//
// Brief Description : Manages what tutorial popus to show on the player's screen.
*****************************************************************************/
using NaughtyAttributes;
using System;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FoolsBrand.UI
{
    public class TutorialUIManager : Manager
    {
        [SerializeField] private TMP_Text tutorialText;
        [SerializeField] private CanvasGroup tutorialTextGroup;
        [SerializeField] private InputAction advanceTextAction;
        [SerializeField] private TutorialSequence[] tutorialSequences;

        private TutorialSequence encounterTutorialData;
        private int currentTutorialIndex;


        public override void Init(GameManager gm, HierarchyManager parentManager)
        {
            if (RunManager.CurrentEncounterNum < tutorialSequences.Length && tutorialSequences[RunManager.CurrentEncounterNum] != null)
            {
                encounterTutorialData = tutorialSequences[RunManager.CurrentEncounterNum];

                advanceTextAction.Enable();
                advanceTextAction.performed += HandleClickInput;

                PlayerInputManager.OnReserveInput += HandleReserveInput;
                PlayerInputManager.OnRollButtonPressed += HandleRollInput;
                PlayerInputManager.OnLimbSelectedInput += HandleLimbInput;

                RefreshTutorial();
            }
            else
            {
                tutorialTextGroup.alpha = 0;
            }
            
        }

        public override void Deinit()
        {
            advanceTextAction.performed -= HandleClickInput;

            PlayerInputManager.OnReserveInput -= HandleReserveInput;
            PlayerInputManager.OnRollButtonPressed -= HandleRollInput;
            PlayerInputManager.OnLimbSelectedInput -= HandleLimbInput;
        }

        #region Input Handling
        private void HandleClickInput(InputAction.CallbackContext obj)
        {
            HandleInput(AdvanceCondition.Click);
        }

        private void HandleReserveInput(int obj)
        {
            HandleInput(AdvanceCondition.ReservePressed);
        }

        private void HandleRollInput()
        {
            HandleInput(AdvanceCondition.RollPressed);
        }

        private void HandleLimbInput(int obj)
        {
            HandleInput(AdvanceCondition.LimbSelected);
        }
        #endregion

        private void HandleInput(AdvanceCondition inputType)
        {
            if (encounterTutorialData.Tutorials[currentTutorialIndex].AdvanceCondition == inputType)
            {
                currentTutorialIndex++;
            }
            RefreshTutorial();
        }

        private void RefreshTutorial()
        {
            if (currentTutorialIndex < encounterTutorialData.Tutorials.Length)
            {
                tutorialText.text = ParseTutorialText(encounterTutorialData.Tutorials[currentTutorialIndex]);
            }
            else
            {
                tutorialTextGroup.alpha = 0;
            }
        }

        private string ParseTutorialText(EncounterTutorial tutorial)
        {
            return tutorial.TutorialText + (tutorial.AdvanceCondition == AdvanceCondition.Click ? "\n\n<i>Click to continue.</i>" : "");
        }
    }
}
