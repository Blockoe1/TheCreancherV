using FoolsBrand;
using NaughtyAttributes;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

/// <summary>
/// The base class for all dice. Can also be used as the basic die variant
/// </summary>
public class DieBase : MonoBehaviour, IDiceInfo
{
    private const string CORRUPTION_DESCRIPTION = "\n\nCorrupt <sprite name=\"Corruption\">.\nIncreases the value of all faces by <b>2</b>.\nIf more than half of your dice are corrupt, you die.";
    private const int CORRUPTION_AMOUNT = 2;

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
    [SerializeField, TextArea] private string _dieDescription = "To be revealed in a future milestone...";
    [SerializeField] private bool rewardSelectable = true;
    [SerializeField] private MeshRenderer dieRenderer;
    [SerializeField] private MeshRenderer borderRenderer;
    [Header("Rolling Animation")]
    [SerializeField] private Vector3 localRotSpeed;
    [SerializeField] private Vector3 worldRotSpeed;
    [SerializeField] private float slerpTime;
    [SerializeField] private AnimationCurve slerpCurve;
    [Header("Faces")]
    [SerializeField, Tooltip("DO NOT CHANGE THE NUMBER OF FACES. The effects of each face")] private DieFace[] dieFaces = new DieFace[6];

    private int dieIndex = 0;
    private bool isRolling;
    private bool isReserved = false;
    [SerializeField] private bool corrupted = false;
    [SerializeField] private ParticleSystem corruptedParticles;

    [SerializeField] private Material outlineMaterial;
    [SerializeField] private MeshRenderer[] outlinedMeshes;

    private bool isClickable = true;

    public bool RewardSelectable => rewardSelectable;
    public string DieName { get => _dieName; }
    public string DieDescription
    { 
        get
        {
            string facesString = "";
            for(int i = 0; i < dieFaces.Length; i++)
            {
                facesString += dieFaces[i].FaceValue + (i == dieFaces.Length - 1 ? "" : ", ");
            }
            return _dieDescription.Replace("#faces", facesString) + (corrupted ? CORRUPTION_DESCRIPTION : "");
        }
    }

    public Material DieMaterial => dieRenderer.sharedMaterial;
    public Material BorderMaterial => borderRenderer.sharedMaterial;

    public ReadOnlyArray<DieFace> Faces => dieFaces;

    public bool IsReserved { get => isReserved; set => isReserved = value; }
    public bool IsClickable
    {
        get
        {
            return !IsReserved && isClickable;
        }
        set
        {
            isClickable = value;
        }
    }
    public bool Corrupted { get => corrupted; }

    public static event Action<DieBase, bool> DiceCorruptedEvent;
    public static event Action<DieBase> DiceRolledEvent;

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

    private Quaternion ApplyRollRotation(Quaternion rotation, float strength = 1)
    {
        rotation = QuaternionHelpers.RotateLocal(rotation, strength * Time.deltaTime * localRotSpeed);
        //rotation *= Quaternion.Euler(strength * Time.deltaTime * localRotSpeed);

        // Apply world rotation.
        rotation = QuaternionHelpers.RotateWorld(rotation, strength * Time.deltaTime * worldRotSpeed);
        //rotation *= Quaternion.Inverse(rotation) * Quaternion.Euler(strength * Time.deltaTime * worldRotSpeed) * rotation;
        return rotation;
    }

    private IEnumerator RollingAnimation()
    {
        isRolling = true;

        // Set a random starting rotation.
        transform.eulerAngles = new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360));

        while(dieIndex < 0)
        {

            //transform.Rotate(localRotSpeed * Time.deltaTime, Space.Self);
            //transform.Rotate(worldRotSpeed * Time.deltaTime, Space.World);

            transform.rotation = ApplyRollRotation(transform.rotation);

            yield return null;
        }

        Quaternion rollRot = transform.rotation;
        Quaternion targetRot = Quaternion.LookRotation(FACE_AXES[dieIndex % 6], Vector3.up);

        float timer = 0;
        while (timer < slerpTime)
        {
            float normalizedTime = timer / slerpTime;
            // Continue the rolling animation, and slerp between that and the target.
            rollRot = Quaternion.Normalize(ApplyRollRotation(rollRot, 1 - normalizedTime));
            transform.rotation = Quaternion.Slerp(rollRot, targetRot, slerpCurve.Evaluate(normalizedTime));

            timer += Time.deltaTime;
            yield return null;
        }
        transform.rotation = targetRot;
        isRolling = false;
    }

    private void OnDisable()
    {
        isRolling = false;
        isReserved = false;
    }

    /// <summary>
    /// The actual rolling of this die
    /// </summary>
    public DiceActionInfo[] RollDie()
    {
        //Don't tell anyone that I'm not going to make the game break if there are more or less faces. Don't do it...maybe
        dieIndex = UnityEngine.Random.Range(0, dieFaces.Length);
        if (!dieFaces[dieIndex].IsInitialized)
        {
            dieFaces[dieIndex].Initialize(this);
        }

        // Stop the rolling animation.

        DiceRolledEvent?.Invoke(this);
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

    public void SetCorruption(bool isCorrupt)
    {
        if (corrupted == isCorrupt) { return; }
        corrupted = isCorrupt;
        DiceCorruptedEvent?.Invoke(this, corrupted);
        if (isCorrupt)
        {
            corruptedParticles.Play();
        }
        else
        {
            corruptedParticles.Stop();
        }
        foreach(DieFace face in dieFaces)
        {
            face.AddValue(isCorrupt ? CORRUPTION_AMOUNT : -CORRUPTION_AMOUNT);
        }
    }

    public void ShowHoverOutline()
    {
        MaterialChange.AddOverlayMaterial(outlinedMeshes, outlineMaterial);
    }
    public void HideHoverOutline()
    {
        MaterialChange.RemoveOverlayMaterial(outlinedMeshes);
    }
}
