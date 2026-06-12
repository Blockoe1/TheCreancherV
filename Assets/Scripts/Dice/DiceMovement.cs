/*****************************************************************************
// File Name : DiceMovement.cs
// Author : Arcadia Koederitz
// Creation Date : 6/12/2026
// Last Modified : 6/12/2026
//
// Brief Description : Aniamtes dice movements between designated points.
*****************************************************************************/
using System.Collections;
using UnityEngine;

namespace FoolsBrand
{
    public class DiceMovement : MonoBehaviour
    {
        private SingletonCoroutine moveRoutine;

        private void Awake()
        {
            moveRoutine = new SingletonCoroutine(SingletonCoroutine.InterruptMode.Cancel, this);
        }

        public void MoveImmediate(Vector3 position)
        {
            transform.position = position;
        }
        
        /// <summary>
        /// Lerps the dice's position from it's current position to a given position.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="moveTime"></param>
        public void MoveToPoint(Vector3 position, float moveTime)
        {
            moveRoutine.StartCoroutine(MoveRoutine(position, moveTime));
        }
        private IEnumerator MoveRoutine(Vector3 targetPosition, float moveTime)
        {
            yield return null;
        }
    }
}
