using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OccaSoftware.Outlines.Runtime
{
  internal static class ShaderParams
  {
    public static readonly string BlurEnabled = "_BLUR_ON";
    public static readonly string BlurNoiseEnabled = "_BLUR_NOISE_ON";
    public static readonly string PaperEnabled = "_PAPER_ON";
    public static readonly string OffsetEnabled = "_OFFSET_ON";
    public static readonly string DepthEnabled = "_DEPTH_OUTLINE_ON";
    public static readonly string NormalEnabled = "_NORMAL_OUTLINE_ON";
    public static readonly string LumaEnabled = "_LUMA_OUTLINE_ON";
    public static readonly string ColorEnabled = "_COLOR_OUTLINE_ON";

    public static int ExcludeSkybox = Shader.PropertyToID("_OutlineExcludeSkybox");

    public static int BlurAmount = Shader.PropertyToID("_BlurAmount");

    public static int DepthToViewParams = Shader.PropertyToID("os_DepthToViewParams");
    public static int ScreenTexture = Shader.PropertyToID("_ScreenTexture");
    public static int OutlineTarget = Shader.PropertyToID("_OutlineTarget");
    public static int ScreenDimensions = Shader.PropertyToID("_ScreenSizePx");
    public static int MaximumOutlineThickness = Shader.PropertyToID("_MaximumOutlineThickness");

    public static int DepthThreshold = Shader.PropertyToID("_DepthThreshold");
    public static int DepthSoftness = Shader.PropertyToID("_DepthSoftness");
    public static int DepthThickness = Shader.PropertyToID("_DepthThickness01");

    public static int NormalThreshold = Shader.PropertyToID("_NormalThreshold");
    public static int NormalSoftness = Shader.PropertyToID("_NormalSoftness");
    public static int NormalThickness = Shader.PropertyToID("_NormalThickness01");

    public static int LumaThreshold = Shader.PropertyToID("_LumaThreshold");
    public static int LumaSoftness = Shader.PropertyToID("_LumaSoftness");
    public static int LumaThickness = Shader.PropertyToID("_LumaThickness01");

    public static int ColorThreshold = Shader.PropertyToID("_ColorThreshold");
    public static int ColorSoftness = Shader.PropertyToID("_ColorSoftness");
    public static int ColorThickness = Shader.PropertyToID("_ColorThickness01");

    public static int OutlineColor = Shader.PropertyToID("_OutlineColor");

    public static int NearThickness = Shader.PropertyToID("_NearThickness01");
    public static int FarThickness = Shader.PropertyToID("_FarThickness01");
    public static int NearAlpha = Shader.PropertyToID("_NearAlpha");
    public static int FarAlpha = Shader.PropertyToID("_FarAlpha");
    public static int DistanceStart = Shader.PropertyToID("_DistanceStart");
    public static int DistanceEnd = Shader.PropertyToID("_DistanceEnd");

    public static int GrazingAngleThreshold = Shader.PropertyToID("_GrazingAngleThreshold");
    public static int GrazingAngleOffset = Shader.PropertyToID("_GrazingAngleOffset");

    public static int EdgeThreshold = Shader.PropertyToID("_EdgeThreshold");

    public static int PreviewOutlines = Shader.PropertyToID("_PreviewOutlines");

    public static int OutlineTargetResource = Shader.PropertyToID("_OutlineTargetResource");
    public static int OutlineDrawTarget = Shader.PropertyToID("_OutlineDrawTarget");

    public static int FrameRate = Shader.PropertyToID("_FrameRate");
    public static int InverseFrameRate = Shader.PropertyToID("_InverseFrameRate");

    public static int OffsetTexture = Shader.PropertyToID("_OffsetTexture");
    public static int TextureSpeed = Shader.PropertyToID("_TextureSpeed");
    public static int TextureScale = Shader.PropertyToID("_TextureScale");
    public static int TextureOffset = Shader.PropertyToID("_TextureOffset");
    public static int OffsetExcludeSkybox = Shader.PropertyToID("_OffsetExcludeSkybox");

    public static int PaperTexture = Shader.PropertyToID("_PaperTexture");
    public static int PaperTextureSpeed = Shader.PropertyToID("_PaperTextureSpeed");
    public static int PaperTextureScale = Shader.PropertyToID("_PaperTextureScale");
    public static int PaperTextureStrength = Shader.PropertyToID("_PaperTextureStrength");
    public static int PaperExcludeSkybox = Shader.PropertyToID("_PaperExcludeSkybox");
  }
}
