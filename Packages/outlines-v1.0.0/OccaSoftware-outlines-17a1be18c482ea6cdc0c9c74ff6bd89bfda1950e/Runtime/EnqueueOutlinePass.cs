using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace OccaSoftware.Outlines.Runtime
{
  [ExecuteAlways]
  [AddComponentMenu("OccaSoftware/Outlines/Enqueue Outline Pass")]
  public class EnqueueOutlinePass : MonoBehaviour
  {
    private OutlineRenderPass outlineRenderPass;
    public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingTransparents;
    public int renderPassEventOffset;
    public bool renderInSceneView = true;

    private void OnEnable()
    {
      outlineRenderPass = new OutlineRenderPass();
      RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
    }

    private void OnDisable()
    {
      RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
      outlineRenderPass.Dispose();
    }

    private void OnBeginCameraRendering(ScriptableRenderContext context, Camera cam)
    {
      // Exclude preview and reflection cameras
      if (cam.cameraType == CameraType.Preview || cam.cameraType == CameraType.Reflection)
        return;

      if (!renderInSceneView && cam.cameraType == CameraType.SceneView)
      {
        return;
      }

      outlineRenderPass.renderPassEvent = renderPassEvent + renderPassEventOffset;
      outlineRenderPass.Setup();
      cam.GetUniversalAdditionalCameraData().scriptableRenderer.EnqueuePass(outlineRenderPass);
    }
  }
}
