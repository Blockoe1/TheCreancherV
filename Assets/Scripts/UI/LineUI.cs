/*****************************************************************************
// File Name : LimbUI.cs
// Author : Arcadia Koederitz
// Creation Date : 5/29/2026
// Last Modified : 5/29/2026
//
// Brief Description : Mimicks functionality of a line renderer by stretching and rotating an image.
*****************************************************************************/
using UnityEditor.Search;
using UnityEngine;

namespace FoolsBrand
{
    public class LineUI : MonoBehaviour
    {
        [SerializeField] private Vector2 referenceScreenSize = new Vector2(1920, 1080);
        private RectTransform rectTransform => transform as RectTransform;

        public void SetPoints(Vector2 startPoint, Vector2 endPoint)
        {
            Vector2 mid = (startPoint + endPoint) / 2;

            rectTransform.position = mid;

            Vector2 scaleFactor = new Vector2(referenceScreenSize.x / Screen.width, referenceScreenSize.y / Screen.height);
            startPoint = new Vector2(startPoint.x * scaleFactor.x, startPoint.y * scaleFactor.y);
            endPoint = new Vector2(endPoint.x * scaleFactor.x, endPoint.y * scaleFactor.y);

            Vector2 dirVector = startPoint - endPoint;
            rectTransform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dirVector.y, dirVector.x) * Mathf.Rad2Deg);
            Debug.Log(dirVector.magnitude);
            rectTransform.sizeDelta = new Vector2(dirVector.magnitude, rectTransform.sizeDelta.y);
        }
    }
}
