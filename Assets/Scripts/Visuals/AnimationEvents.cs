using UnityEngine;

namespace FoolsBrand
{
    public class AnimationEvents : MonoBehaviour
    {
        private void ChangeMaterialColorOfAllChildren(Color color)
        {
            //MeshRenderer[] mr = GetComponentsInChildren<MeshRenderer>();
            //GetComponentInChildren<MeshRenderer>().material.SetColor("_Region1", color);
            Debug.Log("r");
        }

        public void ChangeMaterialColorOfAllChildrenToRed()
        {
            ChangeMaterialColorOfAllChildren(Color.red);
        }

    }
}
