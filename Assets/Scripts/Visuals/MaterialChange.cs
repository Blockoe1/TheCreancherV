using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FoolsBrand
{
    public class MaterialChange
    {
        public static void AddOverlayMaterial(MeshRenderer[] mrs, Material mat)
        {
            foreach (MeshRenderer mr in mrs)
                AddOverlayMaterial(mr, mat);
        }

        public static void AddOverlayMaterial(MeshRenderer mr, Material mat)
        {
            List<Material> materials = mr.materials.ToList();
            materials.Add(mat);
            mr.materials = materials.ToArray();
        }

        public static void RemoveOverlayMaterial(MeshRenderer[] mrs, int index = 1)
        {
            foreach (MeshRenderer mr in mrs)
                RemoveOverlayMaterial(mr, index);
        }

        public static void RemoveOverlayMaterial(MeshRenderer mr, int index = 1)
        {
            if (index >= mr.materials.Length)
            {
                Debug.LogError("Could not remove overlay material. Index out of bounds.");
                return;
            }

            List<Material> materials = mr.materials.ToList();
            materials.RemoveAt(index);
            mr.materials = materials.ToArray();
        }
    }
}
