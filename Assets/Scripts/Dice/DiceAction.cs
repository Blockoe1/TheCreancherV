using FoolsBrand;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Stores types of actions
/// </summary>
public abstract class DiceAction : ScriptableObject
{
    private const string IMPACT_ANIM_EVENT_NAME = "Impact";

    [SerializeField] private string animationName;
    [SerializeField] private ActionVFX actionVFX;

    public abstract int PriorityValue { get; }

    /// <summary>
    /// Gets the time signature of the Impact event.
    /// </summary>
    /// <param name="clip"></param>
    /// <returns></returns>
    protected static float GetImpactTime(AnimationClip clip)
    {
        AnimationEvent impactEvent = Array.Find(clip.events, x => x.functionName == IMPACT_ANIM_EVENT_NAME);
        if (impactEvent != null)
        {
            return impactEvent.time;
        }
        return 0;
    }

    /// <summary>
    /// Plays the action with handling for playing the animation and timing the effect.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="source"></param>
    /// <param name="user"></param>
    /// <param name="value"></param>
    /// <param name="sourceFace"></param>
    /// <returns></returns>
    public IEnumerator PlayAction(ITargetable target, IActionSource source, Combatant user, int value, DieFace sourceFace)
    {
        if (target.IsDead) { yield break; }
        // Play the animation.
        AnimationClip clip = source.PlayAnimation(animationName);
        // Get the animation, impact, and vfx preload time.
        float animationTime = 0, impactTime = 0, effectPreloadTime = 0;

        if (clip != null)
        {
            animationTime = clip.length;
            impactTime = GetImpactTime(clip);
        }
        if (actionVFX != null)
        {
            effectPreloadTime = actionVFX.PreloadTime;
        }

        // Wait the corresponding times.
        yield return new WaitForSeconds(Mathf.Max(impactTime - effectPreloadTime, effectPreloadTime));
        // Play visual effects.
        if (actionVFX != null)
        {
            PlayVFX(target, source, user, actionVFX.EffectObj);
        }
        yield return new WaitForSeconds(effectPreloadTime);
        // Perform the actual action.
        yield return PerformAction(target, source, user, value, sourceFace);
        // Wait for the animation to finish.
        yield return new WaitForSeconds(animationTime - impactTime);
    }

    public abstract IEnumerator PerformAction(ITargetable target, IActionSource source, Combatant user, int value, DieFace sourceFace);

    /// <summary>
    /// By default, effects play at the target.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="source"></param>
    /// <param name="user"></param>
    /// <param name="effectPrefab"></param>
    protected virtual void PlayVFX(ITargetable target, IActionSource source, Combatant user, GameObject effectPrefab)
    {
        if (target is MonoBehaviour mb)
        {
            GameObject.Instantiate(effectPrefab, target.GetEffectTransform().position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Target was invalid, cannot play effects.");
        }
    }
}
