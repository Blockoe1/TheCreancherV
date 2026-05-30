using NaughtyAttributes;
using System.Collections;
using System.Runtime.CompilerServices;
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
    private static readonly Vector3[] ROTATIONS =
    {
        new Vector3(90, 0, 0),
        new Vector3(-90, 0, 0),
        new Vector3(0, 90, 0),
        new Vector3(0, -90, 0),
        new Vector3(0, 0, 90),
        new Vector3(0, 0, -90)
    };

    [SerializeField] private string _dieName = "Basic Die 1-6";
    [Header("Rolling Animation")]
    [SerializeField] private Vector3 localRotSpeed;
    [SerializeField] private Vector3 worldRotSpeed;
    [SerializeField] private float slerpTime;
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
        return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * speed;
    }

    private IEnumerator RollingAnimation()
    {
        isRolling = true;

        // Set a random starting rotation.
        transform.eulerAngles = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        IEnumerator Rolling()
        {
            //Quaternion rotQuat = Quaternion.Inverse(transform.rotation) * Quaternion.Euler(rotationSpeed);
            //transform.rotation = transform.rotation * rotQuat;

            transform.Rotate(localRotSpeed * Time.deltaTime, Space.Self);
            //transform.Rotate(worldRotSpeed * Time.deltaTime, Space.World);

            transform.rotation = ApplyRollRotation(transform.rotation);

            yield return null;
        }

        Quaternion ApplyRollRotation(Quaternion startRotation)
        {
            // Apply local rotation
            Quaternion rotation = startRotation;

            // Apply world rotation.
            rotation *= Quaternion.Euler(worldRotSpeed * Time.deltaTime);
            return rotation;
        }

        IEnumerator SnapToTarget()
        {
            // Quickly snap to the target face.
            Quaternion startRot = transform.rotation;
            Quaternion targetRot = Quaternion.LookRotation(FACE_AXES[dieIndex], Vector3.up);

            float timer = 0;
            while (timer < slerpTime)
            {
                float normalizedTime = timer / slerpTime;
                transform.rotation = Quaternion.Slerp(startRot, targetRot, normalizedTime);

                timer += Time.deltaTime;
                yield return null;
            }
            transform.rotation = targetRot;
            isRolling = false;
        }

        while(dieIndex < 0)
        {
            yield return Rolling();
        }
        yield return SnapToTarget();
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
