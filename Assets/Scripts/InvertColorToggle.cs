using Unity.VisualScripting;
using UnityEngine;

public class InvertColorToggle: MonoBehaviour
{
    [SerializeField] private Material invertMaterial;

    public void EnableInvert()
    {
       invertMaterial.SetFloat("_InvertEnabled", 1f);
    }

    public void DisableInvert()
    {
        invertMaterial.SetFloat("_InvertEnabled", 0f);
    }
}
