/*****************************************************************************
// File Name : DiceMovement.cs
// Author : Arcadia Koederitz
// Creation Date : 6/12/2026
// Last Modified : 6/12/2026
//
// Brief Description : Aniamtes dice movements between designated points.
*****************************************************************************/
using CustomAttributes;
using System.Collections;
using UnityEngine;

namespace FoolsBrand
{
    public class DiceMovement : MonoBehaviour
    {
        [SerializeField] private AnimationCurve movementCurve;
        [SerializeField] private AnimationCurve scaleCurve;

        private IDiceInfo dice;

        private IDiceInfo Dice
        {
            get
            {
                if (dice == null)
                {
                    dice = GetComponent<IDiceInfo>();
                }
                return dice;
            }
        }

        private SingletonCoroutine moveRoutine;

        private void Awake()
        {
            moveRoutine = new SingletonCoroutine(SingletonCoroutine.InterruptMode.Cancel, this);
        }

        public void MoveImmediate(Transform targetTransform)
        {
            transform.position = targetTransform.position;
            transform.localScale = targetTransform.localScale;
        }
        
        /// <summary>
        /// Lerps the dice's position from it's current position to a given position.
        /// </summary>
        /// <param name="targetTransform"></param>
        /// <param name="moveTime"></param>
        public void MoveToPoint(Transform targetTransform, float moveTime)
        {
            moveRoutine.StartCoroutine(MoveRoutine(targetTransform, moveTime));
        }
        private IEnumerator MoveRoutine(Transform targetTransform, float moveTime)
        {
            Dice.IsClickable = false;
            Vector3 startPos = transform.position;
            Vector3 startScale = transform.localScale;
            float timer = 0;
            while(timer < moveTime)
            {
                float normalizedTime = timer / moveTime;

                transform.position = Vector3.LerpUnclamped(startPos, targetTransform.position, normalizedTime);
                transform.localScale = Vector3.LerpUnclamped(startScale, targetTransform.localScale, normalizedTime);

                timer += Time.deltaTime;
                yield return null;
            }

            Dice.IsClickable = true;
            MoveImmediate(targetTransform);
        }
    }
}
