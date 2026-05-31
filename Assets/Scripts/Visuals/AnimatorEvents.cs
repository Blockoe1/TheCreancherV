using NaughtyAttributes;
using System;
using System.Collections;
using UnityEngine;

namespace FoolsBrand
{
    public class AnimatorEvents : MonoBehaviour
    {
        [SerializeField, MinValue(0)] float _tick = 0.01f;

        protected MeshColorRegions[] mcs = new MeshColorRegions[6];
        protected void Start()
        {
            MeshRenderer[] mrs = GetComponentsInChildren<MeshRenderer>();
            Material curMat;
            for (int i = 0; i < mrs.Length; i++)
            {
                curMat = mrs[i].material;

                if (!curMat.HasColor(ColorRegions.Region0VarName)) continue;

                mcs[i] = new MeshColorRegions(mrs[i], new ColorRegions(curMat.GetColor(ColorRegions.Region0VarName), curMat.GetColor(ColorRegions.Region1VarName), curMat.GetColor(ColorRegions.Region2VarName), curMat.GetColor(ColorRegions.Region3VarName), curMat.GetColor(ColorRegions.Region4VarName), curMat.GetColor(ColorRegions.Region5VarName)));
            }
        }

        private void LerpMatColor(MeshRenderer mr, string varName, Color sColor, Color eColor, float duration, ref float elapsed)
        {
            elapsed += _tick;
            float percent = elapsed / duration;

            mr.material.SetColor(varName, Color.Lerp(sColor, eColor, percent));
        }

        protected IEnumerator BeginLerpMatColor(MeshRenderer mr, string varName, Color sColor, Color eColor, float duration)
        {
            float elapsed = 0;

            while (elapsed < duration)
            {
                yield return new WaitForSeconds(_tick);
                LerpMatColor(mr, varName, sColor, eColor, duration, ref elapsed);
            }
        }

        protected void ColorChangeAllRegions(Color eColor, float duration)
        {
            foreach (MeshColorRegions mc in mcs)
            {
                StartCoroutine(BeginLerpMatColor(mc.Renderer, ColorRegions.Region0VarName, mc.Renderer.material.GetColor(ColorRegions.Region0VarName), eColor, duration));
                StartCoroutine(BeginLerpMatColor(mc.Renderer, ColorRegions.Region1VarName, mc.Renderer.material.GetColor(ColorRegions.Region1VarName), eColor, duration));
                StartCoroutine(BeginLerpMatColor(mc.Renderer, ColorRegions.Region2VarName, mc.Renderer.material.GetColor(ColorRegions.Region2VarName), eColor, duration));
                StartCoroutine(BeginLerpMatColor(mc.Renderer, ColorRegions.Region3VarName, mc.Renderer.material.GetColor(ColorRegions.Region3VarName), eColor, duration));
                StartCoroutine(BeginLerpMatColor(mc.Renderer, ColorRegions.Region4VarName, mc.Renderer.material.GetColor(ColorRegions.Region4VarName), eColor, duration));
                StartCoroutine(BeginLerpMatColor(mc.Renderer, ColorRegions.Region5VarName, mc.Renderer.material.GetColor(ColorRegions.Region5VarName), eColor, duration));
            }
        }

        protected void ColorReturnAllRegions(float duration)
        {
            try
            {
                foreach (MeshColorRegions mc in mcs)
                {
                    StartCoroutine(BeginLerpMatColor(mc.Renderer, ColorRegions.Region0VarName, mc.Renderer.material.GetColor(ColorRegions.Region0VarName), mc.Regions.R0, duration));
                    StartCoroutine(BeginLerpMatColor(mc.Renderer, ColorRegions.Region1VarName, mc.Renderer.material.GetColor(ColorRegions.Region1VarName), mc.Regions.R1, duration));
                    StartCoroutine(BeginLerpMatColor(mc.Renderer, ColorRegions.Region2VarName, mc.Renderer.material.GetColor(ColorRegions.Region2VarName), mc.Regions.R2, duration));
                    StartCoroutine(BeginLerpMatColor(mc.Renderer, ColorRegions.Region3VarName, mc.Renderer.material.GetColor(ColorRegions.Region3VarName), mc.Regions.R3, duration));
                    StartCoroutine(BeginLerpMatColor(mc.Renderer, ColorRegions.Region4VarName, mc.Renderer.material.GetColor(ColorRegions.Region4VarName), mc.Regions.R4, duration));
                    StartCoroutine(BeginLerpMatColor(mc.Renderer, ColorRegions.Region5VarName, mc.Renderer.material.GetColor(ColorRegions.Region5VarName), mc.Regions.R5, duration));
                }
            }
            catch (NullReferenceException) {}
        }
    }
}
