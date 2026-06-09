/*****************************************************************************
// File Name : DiceBagVisualizer.cs
// Author : Arcadia Koederitz
// Creation Date : 6/8/2026
// Last Modified : 6/8/2026
//
// Brief Description : Controls viewing dice information on the pause menu.
*****************************************************************************/
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace FoolsBrand.UI
{
    public class DiceBagVisualizer : MonoBehaviour, IPointerMoveHandler, IPointerExitHandler
    {
        [SerializeField] private DiceGridManager diceGrid;
        [SerializeField] private InfoBox infoBox;

        private RectTransform rTrans => transform as RectTransform;

        public void ToggleBag(bool canSee)
        {
            diceGrid.ToggleCamera(canSee);
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue() - (Vector2)rTrans.position;
            
            if(rTrans.rect.Contains(mousePos))
            {
                Vector2 normalizedPos = Rect.PointToNormalized(rTrans.rect, mousePos);
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

        /// <summary>
        /// Stop showing dice info when the mouse leaves the grid.
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerExit(PointerEventData eventData)
        {
            
            Debug.Log("Exit");
        }
    }
}
