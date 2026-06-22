/*****************************************************************************
// File Name : TransitionManager.cs
// Author : Arcadia Koederitz
// Creation Date : 6/22/2026
// Last Modified : 6/22/2026
//
// Brief Description : Controls screen transitions between scenes.
*****************************************************************************/
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FoolsBrand
{
    public enum TransitionType
    { 
        Fade,
        Cross
    }
    public class TransitionManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup transitionGroup;
        [SerializeField] private RectTransform transitionImageL;
        [SerializeField] private RectTransform transitionImageR;
        [SerializeField] private float transitionTime;

        private static TransitionManager instance;
        private bool isTransitioning;

        private Image[] images;

        public static event Action OnTransitionFinish;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                Debug.Log("Destroyed duplicate TransitionManager.");
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }

            images = GetComponentsInChildren<Image>();
        }

        public static void LoadScene(string sceneName, TransitionType transitionType = TransitionType.Cross)
        {
            LoadScene(sceneName, Color.white, transitionType);
        }
        /// <summary>
        /// Loads a scene with a specific scene transition.
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="color"></param>
        /// <param name="transitionType"></param>
        public static void LoadScene(string sceneName, Color color, TransitionType transitionType = TransitionType.Cross)
        {
            if (instance != null)
            {
                if (instance.isTransitioning) { return; }
                Action<float> animatedAction = null;
                switch (transitionType)
                {
                    case TransitionType.Fade:
                        animatedAction = instance.SetAlpha;
                        break;
                    case TransitionType.Cross:
                        animatedAction = instance.SetCrossValue;
                        break;
                }

                instance.StartCoroutine(instance.TransitionRoutine(sceneName, color, 
                    instance.AnimateValue(0, 1, animatedAction), instance.AnimateValue(1, 0, animatedAction)));
            }
            else
            {
                // Load the scene with no transition if the transition manager isntance doesnt exist.
                SceneManager.LoadScene(sceneName);
            }
        }

        private IEnumerator TransitionRoutine(string sceneName, Color transitionColor, IEnumerator transitionToRoutine, IEnumerator transitionFromRoutine)
        {
            isTransitioning = true;
            ToggleTransition(true);
            SetColor(transitionColor);

            yield return transitionToRoutine;

            // Load the scene asyncronously and wait until it's loaded.
            AsyncOperation loadingOp = SceneManager.LoadSceneAsync(sceneName);
            // Wait until we've loaded the scene.
            yield return new WaitWhile(() => !loadingOp.isDone);

            yield return transitionFromRoutine;

            ToggleTransition(false);
            isTransitioning = false;
        }

        private IEnumerator AnimateValue(float startVal, float endVal, Action<float> setter)
        {
            setter(startVal);

            float timer = 0;
            while (timer < transitionTime)
            {
                float normalizedTime = timer / transitionTime;

                setter(Mathf.Lerp(startVal, endVal, normalizedTime));

                timer += Time.unscaledDeltaTime;
                yield return null;
            }
        }

        private void SetCrossValue(float value)
        {
            Vector2 lAnchor = transitionImageL.anchorMax;
            Vector2 rAnchor = transitionImageR.anchorMin;

            value = Mathf.Clamp01(value);

            lAnchor.x = Mathf.Lerp(0, 0.5f, value);
            rAnchor.x = Mathf.Lerp(1, 0.5f, value);

            transitionImageL.anchorMax = lAnchor;
            transitionImageR.anchorMin = rAnchor;
        }

        private void SetAlpha(float alpha)
        {
            transitionGroup.alpha = alpha;
        }

        private void SetColor(Color col)
        {
            foreach(Image img in images)
            {
                img.color = col;
            }
        }

        private void ToggleTransition(bool isTransitioning)
        {
            transitionGroup.alpha = isTransitioning ? 1 : 0;
            transitionGroup.blocksRaycasts = isTransitioning;
        }
    }
}
