/*****************************************************************************
// File Name : DiceBagVisualizer.cs
// Author : Arcadia Koederitz
// Creation Date : 6/8/2026
// Last Modified : 6/8/2026
//
// Brief Description : Controls viewing dice information on the pause menu.
*****************************************************************************/
using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace FoolsBrand.UI
{
    public class DiceBagVisualizer : MonoBehaviour, IPointerMoveHandler
    {
        [SerializeField] private DiceGrid diceGrid;
        [SerializeField] private InfoBox infoBox;

        private RectTransform rTrans => transform as RectTransform;

        public void HandlePauseToggled(bool isPaused)
        {
            diceGrid.ToggleCamera(isPaused);
            if (isPaused)
            {

            }
            else
            {
                infoBox.SetDisplayDice(null);
            }
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            Vector2 mousePos;
            mousePos = Mouse.current.position.ReadValue();
            //if (offsetCanvas != null)
            //{
            //    Vector2 gridOffset = rTrans.position - offsetCanvas.transform.position;
            //    Vector2 scaledGridOffset = new Vector2(gridOffset.x / offsetCanvas.transform.localScale.x, gridOffset.y / offsetCanvas.transform.localScale.y);
            //    // Calculate the screen position for the rect since it's always centered at 0,0,0
            //    mousePos = mousePos - scaledGridOffset - new Vector2(offsetCanvas.pixelRect.width / 2, offsetCanvas.pixelRect.height / 2);
            //}
            //else
            //{
            //    mousePos = mousePos - (Vector2)rTrans.position;
            //}
            if (RectTransformUtility.RectangleContainsScreenPoint(rTrans, mousePos))
            {
                Vector2 localPoint = rTrans.InverseTransformPoint(mousePos);
                Debug.Log(localPoint);
                Vector2 normalizedPos = Rect.PointToNormalized(rTrans.rect, localPoint);
                //Debug.Log(normalizedPos);
                Vector3Int coordinates = new Vector3Int((int)(normalizedPos.x * diceGrid.MaxGridSize.x), 0, (int)((1 - normalizedPos.y) * diceGrid.MaxGridSize.y));
                int index = diceGrid.GridPointToIndex(coordinates);
                if (index < diceGrid.RegisteredDice.Count)
                {
                    infoBox.SetDisplayDice(diceGrid.RegisteredDice[index]);
                }
                else
                {
                    infoBox.SetDisplayDice(null);
                }
                
            }
            else
            {
                infoBox.SetDisplayDice(null);
            }
        }
    }
}
