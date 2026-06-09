/*****************************************************************************
// File Name : DiceGrid.cs
// Author : Arcadia Koederitz
// Creation Date : 6/8/2026
// Last Modified : 6/8/2026
//
// Brief Description : Organizes all spawned dice into a grid.
*****************************************************************************/
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FoolsBrand
{
    public class DiceGrid : Manager
    {
        [SerializeField] private Camera diceGridCam;
        [SerializeField] private ObjectPool<DiceImposter> proxyPool;
        [SerializeField] private Grid parentGrid;
        [SerializeField] private Vector3 baseRotation;
        [SerializeField] private Vector3 spinSpeed;
        [SerializeField] private Vector3 baseScale = Vector3.one;
        [SerializeField] private Vector2Int maxGridSize = new Vector2Int(4, 4);

        private readonly List<DieBase> registeredDice = new();
        private readonly Dictionary<DieBase, DiceImposter> diceProxies = new();
        [SerializeField, ReadOnly] private readonly List<Transform> controlledTransforms = new();

        private Quaternion currentRotationQuat;
        private bool isSpinning;

        public List<DieBase> RegisteredDice => registeredDice;
        public Vector2Int MaxGridSize => maxGridSize;


        public void ToggleCamera(bool isEnabled)
        {
            diceGridCam.gameObject.SetActive(isEnabled);
        }

        /// <summary>
        /// Initialize the dice grid.
        /// </summary>
        /// <param name="gm"></param>
        /// <param name="parentManager"></param>
        public override void Init(GameManager gm, HierarchyManager parentManager)
        {
            isSpinning = true;
            currentRotationQuat = Quaternion.Euler(baseRotation);
            StartCoroutine(RotateDice());
        }

        public override void Deinit()
        {
            isSpinning = false;
        }

        #region Dice Management
        /// <summary>
        /// Registers this dice with a position on the grid.
        /// </summary>
        /// <param name="dice"></param>
        public void RegisterDice(DieBase dice)
        {
            
            if (!registeredDice.Contains(dice) && registeredDice.Count < maxGridSize.x * maxGridSize.y)
            {
                registeredDice.Add(dice);
                dice.transform.localScale = baseScale;
                ReturnDice(dice);
            }
        }

        /// <summary>
        /// Checks out a dice from the grid 
        /// </summary>
        /// <param name="dice"></param>
        public void CheckOutDice(DieBase dice)
        {
            if (registeredDice.Contains(dice))
            {
                int diceIndex = registeredDice.IndexOf(dice);
                
                // Create a dice proxy for this dice.
                DiceImposter proxy = proxyPool.GetObject();
                proxy.SetDice(dice);
                if (diceProxies.ContainsKey(dice))
                {
                    diceProxies[dice] = proxy;
                }
                else
                {
                    diceProxies.Add(dice, proxy);
                }

                GoToIndexPosition(proxy.transform, diceIndex);

                // Stop the dice spinning & start proxy spinning.
                controlledTransforms[diceIndex] = proxy.transform;
            }
        }

        /// <summary>
        /// Returns a dice to the dice grid.
        /// </summary>
        /// <param name="dice"></param>
        public void ReturnDice(DieBase dice)
        {
            if (registeredDice.Contains(dice))
            {
                int diceIndex = registeredDice.IndexOf(dice);
                // Snap the dice to the grid.
                GoToIndexPosition(dice.transform, diceIndex);
                dice.transform.localScale = baseScale;

                // Start the dice spinning.
                if (diceIndex < controlledTransforms.Count)
                {
                    controlledTransforms[diceIndex] = dice.transform;
                }
                else
                {
                    controlledTransforms.Insert(diceIndex, dice.transform);
                }    

                // Return any dice proxies used to replace the dice while it was checked out.
                if (diceProxies.ContainsKey(dice))
                {
                    DiceImposter proxy = diceProxies[dice];
                    proxyPool.ReturnObject(proxy);
                    diceProxies.Remove(dice);
                }
            }
        }

        /// <summary>
        /// Removes a dice from being managed by this grid.
        /// </summary>
        /// <param name="dice"></param>
        public void RemoveDice(DieBase dice)
        {
            ReturnDice(dice);
            int diceIndex = registeredDice.IndexOf(dice);
            registeredDice.Remove(dice);
            controlledTransforms.Remove(dice.transform);
            RefreshGrid();
        }
        #endregion

        private IEnumerator RotateDice()
        {
            while (isSpinning)
            {
                // Dice spinning should not scale with deltatime.
                currentRotationQuat = Quaternion.Normalize(QuaternionHelpers.RotateWorld(currentRotationQuat, spinSpeed * Time.unscaledDeltaTime));
                foreach (Transform transform in controlledTransforms)
                {
                    if (transform != null)
                    {
                        transform.rotation = currentRotationQuat;
                    }
                }
                
                yield return null;
            }
        }

        private void RefreshGrid()
        {
            for(int i = 0; i < controlledTransforms.Count; i++)
            {
                if (controlledTransforms[i] == null) { break; }
                GoToIndexPosition(controlledTransforms[i], i);
            }
        }

        private void GoToIndexPosition(Transform movedTransform, int index)
        {
            Vector3Int diceGridPos = IndexToGridPoint(index);
            Vector3 worldPos = GetWorldPos(diceGridPos);
            movedTransform.position = worldPos;
        }

        #region Position Calculating
        public Vector3Int IndexToGridPoint(int index)
        {
            return new Vector3Int(index % maxGridSize.x, 0, index / maxGridSize.x);
        }
        public int GridPointToIndex(Vector3Int gridPoint)
        {
            return gridPoint.z * maxGridSize.x + gridPoint.x;
        }

        private Vector3 GetWorldPos(Vector3Int gridPos)
        {
            gridPos.z *= -1;
            Vector3 worldPos = parentGrid.CellToWorld(gridPos)
                - new Vector3(maxGridSize.x * (parentGrid.cellSize.x + parentGrid.cellGap.x) / 2, 0, -maxGridSize.y * (parentGrid.cellSize.z + parentGrid.cellGap.z) / 2)
                + new Vector3((parentGrid.cellSize.x + parentGrid.cellGap.x) / 2, 0, -(parentGrid.cellSize.z + parentGrid.cellGap.z) / 2);
            return worldPos;
        }
        #endregion

        private void OnDrawGizmosSelected()
        {
            for(int i = 0; i < 16; i++)
            {
                Gizmos.DrawWireCube(GetWorldPos(IndexToGridPoint(i)), parentGrid.cellSize);
            }
        }
    }
}
