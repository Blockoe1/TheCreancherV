using NaughtyAttributes;
using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

/// <summary>
/// The base class for all dice. Can also be used as the basic die variant
/// </summary>
public class DieBase : MonoBehaviour
{
    private static readonly Vector3[] FACE_AXES = 
    { 
        Vector3.up, 
        Vector3.forward, 
        Vector3.left, 
        Vector3.right, 
        Vector3.back, 
        Vector3.down
    };

    [SerializeField] private string _dieName = "Basic Die 1-6";
    [Header("Rolling Animation")]
    [SerializeField] private float rollingSpeed;
    [SerializeField] private float rollingVariance;
    [SerializeField] private float snapToResultTime;
    [Header("Faces")]
    [SerializeField, Tooltip("DO NOT CHANGE THE NUMBER OF FACES. The effects of each face")] private DieFace[] dieFaces = new DieFace[6];

    private int dieIndex = 0;
    private bool isRolling;

    public string DieName { get => _dieName; }

    public ReadOnlyArray<DieFace> Faces => dieFaces;

    /// <summary>
    /// Starts the dice's rolling animation.
    /// </summary>
    public void StartRolling()
    {
        if (!isRolling)
        {
            dieIndex = -1;
            StartCoroutine(RollingAnimation());
        }
    }
    
    private static Vector3 GetRandomRollingVector(float speed)
    {
        return new Vector3(Random.Range(-1f, 1f) * speed, Random.Range(-1f, 1f) * speed, Random.Range(-1f, 1f) * speed);
    }

    private IEnumerator RollingAnimation()
    {
        isRolling = true;
        float elapsedTime = 0;
        Vector3 speedVector = GetRandomRollingVector(rollingSpeed);
        Debug.Log(speedVector);
        Vector3 varianceVector = new Vector3(rollingVariance, rollingVariance, rollingVariance);
        while (dieIndex < 0)
        {
            elapsedTime += Time.deltaTime;
            // Calculate the target rotation with integrals.
            //Vector3 rotation = (-2 * ((maxRollingSpeed - minRollingSpeed) / 2) * Mathf.Cos(Mathf.PI / 2 * elapsedTime) / 2) + 
            //    ((maxRollingSpeed + minRollingSpeed) / 2) * elapsedTime;
            //transform.eulerAngles = rotation;

            Vector3 rotation = (-2 * Mathf.Cos(Mathf.PI / 2 * elapsedTime) * varianceVector / Mathf.PI) +
                speedVector * elapsedTime;
            transform.eulerAngles = rotation;
            //transform.Rotate(rollingSpeed * Time.deltaTime);

            yield return null;
        }

        // Quickly snap to the target face.
        Quaternion startRot = transform.rotation;
        Quaternion targetRot = Quaternion.LookRotation(FACE_AXES[dieIndex], Vector3.up);

        float timer = 0;
        while(timer < snapToResultTime)
        {
            float normalizedTime = timer / snapToResultTime;
            transform.rotation = Quaternion.Slerp(startRot, targetRot, normalizedTime);

            timer += Time.deltaTime;
            yield return null;
        }
        transform.rotation = targetRot;
        isRolling = false;
    }

    /// <summary>
    /// The actual rolling of this die
    /// </summary>
    public DiceAction[] RollDie()
    {
        //Don't tell anyone that I'm not going to make the game break if there are more or less faces. Don't do it...maybe
        dieIndex = Random.Range(0, dieFaces.Length);
        if (!dieFaces[dieIndex].IsInitialized)
        {
            dieFaces[dieIndex].Initialize(this);
        }

        // Stop the rolling animation.

        return dieFaces[dieIndex].GetActions();
    }

    /// <summary>
    /// Refreshes face text since subclasses can't trigger OnValidate.
    /// </summary>
    [Button]
    public void RefreshText()
    {
        foreach(var face in dieFaces)
        {
            face.RefreshText();
        }
    }
}
