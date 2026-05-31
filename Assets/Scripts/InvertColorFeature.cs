using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

public class InvertColorFeature : ScriptableRendererFeature
{
    public static InvertColorFeature Instance;

    [System.Serializable]
    public class Settings
    {
        public Material invertMaterial;
    }

    public Settings settings = new Settings();

    class InvertPass : ScriptableRenderPass
    {
        public bool IsActive;
        private Material material;
        private RTHandle tempRT;

        public InvertPass(Material mat)
        {
            material = mat;
            renderPassEvent = RenderPassEvent.AfterRendering;
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            var desc = renderingData.cameraData.cameraTargetDescriptor;
            RenderingUtils.ReAllocateIfNeeded(ref tempRT, desc, name: "_TempInvertTex");
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (!IsActive || material == null)
                return;

            var cmd = CommandBufferPool.Get("Invert Colors");

            var source = renderingData.cameraData.renderer.cameraColorTargetHandle;

            Blitter.BlitCameraTexture(cmd, source, tempRT, material, 0);
            Blitter.BlitCameraTexture(cmd, tempRT, source);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    private InvertPass pass;

    public override void Create()
    {
        Instance = this;
        pass = new InvertPass(settings.invertMaterial);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(pass);
    }


    public void EnableInvert() => pass.IsActive = true;
    public void DisableInvert() => pass.IsActive = false;
}
