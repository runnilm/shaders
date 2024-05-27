using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace OccaSoftware.Outlines.Runtime
{
  [System.Serializable, VolumeComponentMenu("OccaSoftware/Outlines")]
  public class OutlineVolumeSettings : VolumeComponent, IPostProcessComponent
  {
    [Header("Basic Options")]
    [Tooltip("The color of the outline")]
    public ColorParameter outlineColor = new ColorParameter(Color.black);

    [Tooltip("The maximum thickness of the outline")]
    public FloatParameter maximumOutlineThickness = new MinFloatParameter(0, 0);

    [Tooltip("The threshold for edge detection for the combined outline effect")]
    public ClampedFloatParameter edgeThreshold = new ClampedFloatParameter(0, 0, 1);

    [Tooltip("Whether to exclude the skybox from outlines")]
    public BoolParameter excludeSkybox = new BoolParameter(true);

    [Tooltip(
      "The resolution baseline for the outline effect. If enabled, the outline thickness will dynamically adjust at different screen resolutions relative to the baseline."
    )]
    public ReferenceResolutionParameter resolutionScaling = new ReferenceResolutionParameter(
      ReferenceResolution._1080p_FULL_HD
    );

    [Tooltip("The frame rate at which the offset texture and paper texture are moved")]
    public FloatParameter frameRate = new MinFloatParameter(6, 0);

    [Header("Depth")]
    [Tooltip("The thickness of the depth-based outline")]
    public ClampedFloatParameter depthThickness01 = new ClampedFloatParameter(1, 0, 1);

    [Tooltip("The depth threshold for the outline")]
    public FloatParameter depthThreshold = new MinFloatParameter(1.0f, 0);

    [Tooltip("The softness of the depth-based outline")]
    public FloatParameter depthSoftness = new MinFloatParameter(0.1f, 0);

    [Header("Normals")]
    [Tooltip("The thickness of the normal-based outline")]
    public ClampedFloatParameter normalThickness01 = new ClampedFloatParameter(1, 0, 1);

    [Tooltip("The normal threshold for the outline")]
    public FloatParameter normalThreshold = new MinFloatParameter(0.5f, 0);

    [Tooltip("The softness of the normal-based outline")]
    public FloatParameter normalSoftness = new MinFloatParameter(0.5f, 0);

    [Header("Luma")]
    [Tooltip("The thickness of the luma-based outline")]
    public ClampedFloatParameter lumaThickness01 = new ClampedFloatParameter(1, 0, 1);

    [Tooltip("The luma threshold for the outline")]
    public FloatParameter lumaThreshold = new MinFloatParameter(0.2f, 0);

    [Tooltip("The softness of the luma-based outline")]
    public FloatParameter lumaSoftness = new MinFloatParameter(0.1f, 0);

    [Header("Color")]
    [Tooltip("The thickness of the color-based outline")]
    public ClampedFloatParameter colorThickness01 = new ClampedFloatParameter(1, 0, 1);

    [Tooltip("The color threshold for the outline")]
    public FloatParameter colorThreshold = new MinFloatParameter(0.2f, 0);

    [Tooltip("The softness of the color-based outline")]
    public FloatParameter colorSoftness = new MinFloatParameter(0.1f, 0);

    [Header("Distance Scaling")]
    [Tooltip("The start distance for distance-based scaling")]
    public FloatParameter distanceStart = new MinFloatParameter(0, 0);

    [Tooltip("The end distance for distance-based scaling")]
    public FloatParameter distanceEnd = new MinFloatParameter(100f, 0);

    [Tooltip("The thickness of the outline at the near distance")]
    public ClampedFloatParameter nearThickness01 = new ClampedFloatParameter(1, 0, 1);

    [Tooltip("The thickness of the outline at the far distance")]
    public ClampedFloatParameter farThickness01 = new ClampedFloatParameter(1, 0, 1);

    [Tooltip("The alpha value of the outline at the near distance")]
    public ClampedFloatParameter nearAlpha = new ClampedFloatParameter(1, 0, 1);

    [Tooltip("The alpha value of the outline at the far distance")]
    public ClampedFloatParameter farAlpha = new ClampedFloatParameter(1, 0, 1);

    [Header("Grazing Angles")]
    [Tooltip("The threshold at which the grazing angle takes effect")]
    public FloatParameter grazingAngleThreshold = new MinFloatParameter(0.2f, 0);

    [Tooltip("The strength of the grazing angle effect")]
    public FloatParameter grazingAngleOffset = new MinFloatParameter(3f, 0);

    [Header("Offset Texture")]
    [Tooltip("The offset texture that will be applied to the outline position")]
    public TextureParameter offsetTexture = new TextureParameter(null);

    [Tooltip("The strength of the offset texture")]
    public FloatParameter offsetTextureStrength = new MinFloatParameter(0, 0);

    [Tooltip("The scale of the offset texture")]
    public FloatParameter offsetTextureScale = new MinFloatParameter(3, 0);

    [Tooltip("The rate at which the offset texture is moved over time")]
    public FloatParameter offsetTextureSpeed = new FloatParameter(10);

    [Tooltip("Whether to exclude the skybox from the offset texture")]
    public BoolParameter offsetExcludeSkybox = new BoolParameter(true);

    [Header("Paper Texture")]
    [Tooltip("The paper texture that will be applied to the outline and surfaces in the scene")]
    public TextureParameter paperTexture = new TextureParameter(null);

    [Tooltip("The strength of the paper texture")]
    public ClampedFloatParameter paperTextureStrength = new ClampedFloatParameter(0, 0, 1);

    [Tooltip("The scale of the paper texture")]
    public FloatParameter paperTextureScale = new MinFloatParameter(10, 0);

    [Tooltip("The rate at which the paper texture is moved over time")]
    public FloatParameter paperTextureSpeed = new FloatParameter(0);

    [Tooltip("Whether to exclude the skybox from the paper texture")]
    public BoolParameter paperExcludeSkybox = new BoolParameter(true);

    [Header("Blur")]
    [Tooltip("The amount of blur applied to the outline")]
    public MinFloatParameter blurAmount = new MinFloatParameter(0f, 0f);

    [Tooltip(
      "When enabled, applies Interleaved Gradient Noise to the blur offset distance to reduce banding"
    )]
    public BoolParameter blurNoiseEnabled = new BoolParameter(false);

    [Header("Preview")]
    [Tooltip("Whether to preview the outlines without compositing the scene color")]
    public BoolParameter previewOutlines = new BoolParameter(false);

    public bool IsActive() => maximumOutlineThickness.value > 0 && outlineColor.value.a > 0;

    public bool IsTileCompatible() => false;
  }

  [System.Serializable]
  public class ReferenceResolutionParameter : VolumeParameter<ReferenceResolution>
  {
    public ReferenceResolutionParameter(ReferenceResolution value, bool overrideState = false)
      : base(value, overrideState) { }
  }

  // Enum for resolution scaling
  public enum ReferenceResolution
  {
    Disabled = 0,
    _720p_HD = 720,
    _1080p_FULL_HD = 1080,
    _1440p_QHD = 1440,
    _2160p_UHD = 2160
  }

  [System.Serializable]
  public class TextureSpaceParameter : VolumeParameter<TextureSpace>
  {
    public TextureSpaceParameter(TextureSpace value, bool overrideState = false)
      : base(value, overrideState) { }
  }

  // Enum for resolution scaling
  public enum TextureSpace
  {
    Screen = 0,
    World = 1,
  }
}
