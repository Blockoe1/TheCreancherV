/*****************************************************************************
// File Name : LimbUI.cs
// Author : Arcadia Koederitz
// Creation Date : 5/29/2026
// Last Modified : 5/29/2026
//
// Brief Description : Mimicks functionality of a line renderer by stretching and rotating an image.
*****************************************************************************/
using System;
using Unity.VisualScripting;
using UnityEngine;

namespace FoolsBrand
{
    public class LineUI : MonoBehaviour
    {
        private RectTransform rectTransform => transform as RectTransform;

        public void SetPoints(Vector2 startPoint, Vector2 endPoint)
        {
            Vector2 mid = (startPoint + endPoint) / 2;

            rectTransform.position = mid;

            Vector2 dirVector = startPoint - endPoint;
            rectTransform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dirVector.y, dirVector.x) * Mathf.Rad2Deg);
            rectTransform.sizeDelta = new Vector2(dirVector.magnitude, rectTransform.sizeDelta.y);
        }
    }
}
