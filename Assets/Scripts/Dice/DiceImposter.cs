/*****************************************************************************
// File Name : DiceProxy.cs
// Author : Arcadia Koederitz
// Creation Date : 6/8/2026
// Last Modified : 6/8/2026
//
// Brief Description : Copies all visuals of a dice object.
*****************************************************************************/
using CustomAttributes;
using TMPro;
using UnityEngine;

namespace FoolsBrand
{
    public class DiceImposter : MonoBehaviour, IDiceInfo
    {
        [SerializeField, ShowIfNull] private MeshRenderer diceRenderer;
        [SerializeField, ShowIfNull] private MeshRenderer borderRenderer;
        [SerializeField] private ParticleSystem corruptedParticles;
        [SerializeField] private TMP_Text[] faceTexts;

        private DieBase referenceDie;

        public string DieName => referenceDie != null ? referenceDie.DieName : "Dice Proxy";

        public string DieDescription => referenceDie != null ? referenceDie.DieDescription : "ERROR: No Dice Set.";

        public bool IsReserved => referenceDie != null ? referenceDie.IsReserved : false;

        private void Awake()
        {
            DieBase.DiceCorruptedEvent += CheckBecomeCorrupted;
        }

        private void OnDestroy()
        {
            DieBase.DiceCorruptedEvent -= CheckBecomeCorrupted;
        }

        private void CheckBecomeCorrupted(DieBase affectedDice, bool isCorrupt)
        {
            if (affectedDice == referenceDie)
            {
                UpdateCorruptParticles();
            }
        }

        private void UpdateCorruptParticles()
        {
            if (corruptedParticles != null)
            {
                if (referenceDie.Corrupted)
                {
                    corruptedParticles.Play();
                }
                else
                {
                    corruptedParticles.Stop();
                }
            }
        }

        public void SetDice(DieBase referenceDie)
        {
            this.referenceDie = referenceDie;

            // Set the proper materials.
            diceRenderer.sharedMaterial = referenceDie.DieMaterial;
            borderRenderer.sharedMaterial = referenceDie.BorderMaterial;

            UpdateCorruptParticles();

            // Set the text displayed on the dice.
            for(int i = 0; i < faceTexts.Length; i++)
            {
                faceTexts[i].text = referenceDie.Faces[i].GetFaceText();
                faceTexts[i].color = referenceDie.Faces[i].FaceColor;
            }
        }
    }
}
