/*****************************************************************************
// File Name : DiceGrid.cs
// Author : Arcadia Koederitz
// Creation Date : 6/8/2026
// Last Modified : 6/8/2026
//
// Brief Description : Organizes all spawned dice into a grid.
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FoolsBrand
{
    public class DiceGridManager : Manager
    {
        [SerializeField] private ObjectPool<DiceProxy> proxyPool;
        [SerializeField] private Grid parentGrid;
        [SerializeField] private Vector3 baseRotation;
        [SerializeField] private Vector3 spinSpeed;
        [SerializeField] private Vector3 baseScale = Vector3.one;
        [SerializeField] private Vector2Int maxGridSize = new Vector2Int(4, 4);

        private readonly List<DieBase> registeredDice = new();
        private readonly Dictionary<DieBase, DiceProxy> diceProxies = new();
        private readonly List<Transform> rotatedObjects = new();

        private Quaternion currentRotationQuat;
        private bool isSpinning;

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
            if(!registeredDice.Contains(dice))
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
                // Stop the dice spinning.
                rotatedObjects.Remove(dice.transform);

                // Create a dice proxy for this dice.
                DiceProxy proxy = proxyPool.GetObject();
                proxy.SetDice(dice);

                GoToIndexPosition(proxy.transform, registeredDice.IndexOf(dice));

                // Start the proxy spinning.
                rotatedObjects.Add(proxy.transform);
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
                // Snap the dice to the grid.
                GoToIndexPosition(dice.transform, registeredDice.IndexOf(dice));
                dice.transform.localScale = baseScale;

                // Start the dice spinning.
                rotatedObjects.Add(dice.transform);

                // Return any dice proxies used to replace the dice while it was checked out.
                if (diceProxies.ContainsKey(dice))
                {
                    DiceProxy proxy = diceProxies[dice];
                    proxyPool.ReturnObject(proxy);
                    rotatedObjects.Remove(proxy.transform);
                }
            }
        }

        /// <summary>
        /// Removes a dice from being managed by this grid.
        /// </summary>
        /// <param name="dice"></param>
        public void RemoveDice(DieBase dice)
        {
            registeredDice.Remove(dice);
            RefreshGrid();
        }
        #endregion

        private IEnumerator RotateDice()
        {
            while (isSpinning)
            {
                Debug.Log(currentRotationQuat);
                // Dice spinning should not scale with deltatime.
                currentRotationQuat = QuaternionHelpers.RotateWorld(currentRotationQuat, spinSpeed * Time.unscaledDeltaTime);
                foreach(Transform transform in rotatedObjects)
                {
                    transform.rotation = currentRotationQuat;
                }
                
                yield return null;
            }
        }

        private void RefreshGrid()
        {
            for(int i = 0; i < registeredDice.Count; i++)
            {
                GoToIndexPosition(registeredDice[i].transform, i);
            }
        }

        private void GoToIndexPosition(Transform movedTransform, int index)
        {
            Vector3Int diceGridPos = IndexToGridPoint(index);
            Vector3 worldPos = GetWorldPos(diceGridPos);
            movedTransform.position = worldPos;
        }

        #region Position Calculating
        private Vector3Int IndexToGridPoint(int index)
        {
            return new Vector3Int(index % maxGridSize.x, index / maxGridSize.x);
        }

        private Vector3 GetWorldPos(Vector3Int gridPos)
        {
            (gridPos.y, gridPos.z) = (gridPos.z, -gridPos.y);
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
