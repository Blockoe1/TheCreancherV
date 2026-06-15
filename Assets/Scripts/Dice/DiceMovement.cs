/*****************************************************************************
// File Name : DiceMovement.cs
// Author : Arcadia Koederitz
// Creation Date : 6/12/2026
// Last Modified : 6/12/2026
//
// Brief Description : Aniamtes dice movements between designated points.
*****************************************************************************/
using System.Collections;
using UnityEditor.Experimental;
using UnityEngine;

namespace FoolsBrand
{
    public class DiceMovement : MonoBehaviour
    {
        [SerializeField] private AnimationCurve movementCurve;
        [SerializeField] private AnimationCurve scaleCurve;

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

            MoveImmediate(targetTransform);
        }
    }
}
