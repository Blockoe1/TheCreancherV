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
        [SerializeField] private CrossAnimation crossAnimation;
        [SerializeField] private FadeAnimation fadeAnimation;

        private static TransitionManager instance;
        private bool isTransitioning;

        public static event Action OnTransitionFinish;

        #region Nested
        [System.Serializable]
        private abstract class TransitionAnimation
        {
            [SerializeField] private CanvasGroup transitionGroup;
            [SerializeField] private float transitionTime;
            [SerializeField] private AnimationCurve curve;

            internal float TransitionTime => transitionTime;

            internal void Animate(float startVal, float endVal, float normalizedTime)
            {
                SetValue(Mathf.Lerp(startVal, endVal, curve.Evaluate(normalizedTime)));
            }

            internal abstract void SetValue(float value);

            internal abstract void SetColor(Color col);

            internal void ToggleTransition(bool isTransitioning)
            {
                transitionGroup.alpha = isTransitioning ? 1 : 0;
                transitionGroup.blocksRaycasts = isTransitioning;
            }
        }

        [System.Serializable]
        private class CrossAnimation : TransitionAnimation
        {
            [SerializeField] private Image transitionImageL;
            [SerializeField] private Image transitionImageR;

            internal override void SetColor(Color col)
            {
                transitionImageL.color = col;
                transitionImageR.color = col;
            }

            internal override void SetValue(float value)
            {
                Vector2 lAnchor = transitionImageL.rectTransform.anchorMax;
                Vector2 rAnchor = transitionImageR.rectTransform.anchorMin;

                value = Mathf.Clamp01(value);

                lAnchor.x = Mathf.Lerp(0, 0.505f, value);
                rAnchor.x = Mathf.Lerp(1, 0.495f, value);

                transitionImageL.rectTransform.anchorMax = lAnchor;
                transitionImageR.rectTransform.anchorMin = rAnchor;
            }
        }

        [System.Serializable]
        private class FadeAnimation : TransitionAnimation
        {
            [SerializeField] private Image fadeImage;
            internal override void SetColor(Color col)
            {
                fadeImage.color = col;
            }

            internal override void SetValue(float value)
            {
                Color col = fadeImage.color;
                col.a = value;
                fadeImage.color = col;
            }
        }

        #endregion

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
        }

        public static void LoadScene(string sceneName, TransitionType transitionType = TransitionType.Cross)
        {
            LoadScene(sceneName, Color.black, transitionType);
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
                instance.PlayTransition(sceneName, color, transitionType);
            }
            else
            {
                // Load the scene with no transition if the transition manager isntance doesnt exist.
                SceneManager.LoadScene(sceneName);
                OnTransitionFinish?.Invoke();
            }
        }

        private void PlayTransition(string sceneName, Color color, TransitionType transitionType)
        {
            TransitionAnimation animation = null;
            switch (transitionType)
            {
                case TransitionType.Fade:
                    animation = fadeAnimation;
                    break;
                case TransitionType.Cross:
                    animation = crossAnimation;
                    break;
            }

            animation.SetColor(color);
            StartCoroutine(TransitionRoutine(sceneName, animation));
        }

        private IEnumerator TransitionRoutine(string sceneName, TransitionAnimation anim)
        {
            isTransitioning = true;
            anim.ToggleTransition(true);

            yield return AnimateValue(0, 1, anim);

            // Load the scene asyncronously and wait until it's loaded.
            AsyncOperation loadingOp = SceneManager.LoadSceneAsync(sceneName);
            // Wait until we've loaded the scene.
            yield return new WaitWhile(() => !loadingOp.isDone);

            yield return AnimateValue(1, 0, anim);

            anim.ToggleTransition(false);
            isTransitioning = false;

            OnTransitionFinish?.Invoke();
        }

        private IEnumerator AnimateValue(float startVal, float endVal, TransitionAnimation animation)
        {
            animation.SetValue(startVal);

            float timer = 0;
            while (timer < animation.TransitionTime)
            {
                float normalizedTime = timer / animation.TransitionTime;

                animation.Animate(startVal, endVal, normalizedTime);

                timer += Time.unscaledDeltaTime;
                yield return null;
            }
        }
    }
}
