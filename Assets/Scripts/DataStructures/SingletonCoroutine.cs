/*****************************************************************************
// File Name : SingletonCoroutine.cs
// Author : Arcadia Koederitz
// Creation Date : 6/12/2026
// Last Modified : 6/12/2026
//
// Brief Description : Manager class for having a coroutine that can only run 1 instance.
*****************************************************************************/
using System.Collections;
using UnityEngine;

namespace FoolsBrand
{
    public class SingletonCoroutine
    {
        private readonly InterruptMode interruptMode;
        private readonly MonoBehaviour source;

        private Coroutine singletonRoutine;

        public enum InterruptMode
        {
            Cancel,
            Ignore
        }

        public SingletonCoroutine(InterruptMode interruptMode, MonoBehaviour source)
        {
            this.interruptMode = interruptMode;
            this.source = source;
        }

        /// <summary>
        /// Starts a coroutine, following the singleton rule.
        /// </summary>
        /// <param name="coroutine"></param>
        public void StartCoroutine(IEnumerator coroutine)
        {
            switch (interruptMode)
            {
                case InterruptMode.Cancel:
                    if (singletonRoutine != null)
                    {
                        source.StopCoroutine(singletonRoutine);
                        singletonRoutine = null;
                    }
                    singletonRoutine = source.StartCoroutine(CoroutineWrapper(coroutine));
                    break;
                case InterruptMode.Ignore:
                    // Only start the coroutine if the singleton routine ref is null.
                    if (singletonRoutine == null)
                    {
                        singletonRoutine = source.StartCoroutine(CoroutineWrapper(coroutine));
                    }
                    break;
            }

        }

        private IEnumerator CoroutineWrapper(IEnumerator coroutine)
        {
            yield return coroutine;
            singletonRoutine = null;
        }

        /// <summary>
        /// Stops a currently ongoing singleton coroutine.
        /// </summary>
        public void StopCoroutine()
        {
            if (singletonRoutine != null)
            {
                source.StopCoroutine(singletonRoutine);
                singletonRoutine = null;
            }
        }
    }
}
