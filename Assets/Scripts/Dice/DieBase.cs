using UnityEngine;

/// <summary>
/// The base class for all dice. Can also be used as the basic die variant
/// </summary>
public class DieBase : MonoBehaviour
{
    [SerializeField] private string _dieName = "Basic Die 1-6";
    [SerializeField, Tooltip("DO NOT CHANGE THE NUMBER OF FACES. The effects of each face")] private DieFace[] dieFaces = new DieFace[6];

    private int dieIndex = 0;

    public string DieName { get => _dieName; }

    private void Start()
    {
        RollDie();
        ApplyEffect();
    }

    /// <summary>
    /// The actual rolling of this die
    /// </summary>
    public void RollDie()
    {
        //Don't tell anyone that I'm not going to make the game break if there are more or less faces. Don't do it.
        dieIndex = Random.Range(0, dieFaces.Length);
    }

    /// <summary>
    /// Applying the effect of the die to the enemy's limb
    /// </summary>
    public void ApplyEffect(/*Target goes in here*/)
    {
        //target = target;
        dieFaces[dieIndex].RollDie.Invoke();
    }

    /// <summary>
    /// How much damage the die deals
    /// </summary>
    /// <param name="damage"></param>
    public void FaceDamage(int damage)
    {
        Debug.Log("Dealt " + damage.ToString() + " damage");
    }

    /// <summary>
    /// How much poison the die deals
    /// </summary>
    /// <param name="poison"></param>
    public void FacePoison(int poison)
    {
        Debug.Log("Dealt " + poison.ToString() + " poison");
    }

    /// <summary>
    /// How much healing the die deals
    /// </summary>
    /// <param name="healing"></param>
    public void FaceSelfHeal(int healing)
    {
        Debug.Log("Dealt " + healing.ToString() + " healing");
    }

    /// <summary>
    /// Increase corruption on the die
    /// </summary>
    public void FaceCorruption()
    {
        Debug.Log("Corrupted Die");
    }
}
