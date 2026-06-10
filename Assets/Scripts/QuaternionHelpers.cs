/*****************************************************************************
// File Name : QuaternionHelpers.cs
// Author : Arcadia Koederitz
// Creation Date : 6/8/2026
// Last Modified : 6/8/2026
//
// Brief Description : Set of centralized helper functions for managing quaternions.
*****************************************************************************/
using UnityEngine;

namespace FoolsBrand
{
    public static class QuaternionHelpers
    {
        /// <summary>
        /// Rotates the quaternion locally by the given eulers.
        /// </summary>
        /// <param name="toRotate">The quaternion to rotate.</param>
        /// <param name="eulers">The euler angles to rotate the quaternion by.</param>
        /// <returns>The rotated quaternion.</returns>
        public static Quaternion RotateLocal(Quaternion toRotate, Vector3 eulers)
        {
            return toRotate * Quaternion.Euler(eulers);
        }

        /// <summary>
        /// Rotates a quaternion by the given degrees in world space.
        /// </summary>
        /// <param name="toRotate">The quaternion to rotate.</param>
        /// <param name="eulers">The euler angles to rotate the quaternion by.</param>
        /// <returns>The rotated quaternion.</returns>
        public static Quaternion RotateWorld(Quaternion toRotate, Vector3 eulers)
        {
            return toRotate * Quaternion.Inverse(toRotate) * Quaternion.Euler(eulers) * toRotate;
        }
    }
}
