using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.Rendering;
using UnityEngine.Rendering;
using System.IO;
using System.Collections.Generic;

namespace OccaSoftware.ToonKit2.Editor
{
    public class ToonKit2EditorGUI : ShaderGUI
    {
        bool showSurfaceOptions = true;
        bool showSurfaceInputs = true;
        bool showSpecularAndRim = true;
        bool showLightingOptions = true;
        bool showLighting = true;
        bool showAdvanced = true;

        public override void OnGUI(MaterialEditor e, MaterialProperty[] properties)
        {
            Material t = e.target as Material;
            Properties p = new Properties(properties);

            t.SetOverrideTag("RenderType", "");
            bool depthWrite = false;
            CoreUtils.SetKeyword(t, "_SURFACE_TYPE_TRANSPARENT", p._SurfaceOptions == Properties.SurfaceOptions.Transparent);

            if (p._SurfaceOptions == Properties.SurfaceOptions.Opaque)
            {
                t.SetOverrideTag("RenderType", "Opaque");

                SetSrcDestProperties(BlendMode.One, BlendMode.Zero);

                t.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                t.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");

                depthWrite = true;
            }
            else
            {
                t.SetOverrideTag("RenderType", "Transparent");

                t.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                t.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                t.DisableKeyword("_ALPHAMODULATE_ON");

                switch (p._BlendModeOptions)
                {
                    case Properties.BlendModeOptions.Alpha:
                        SetSrcDestProperties(BlendMode.SrcAlpha, BlendMode.OneMinusSrcAlpha);
                        break;
                    case Properties.BlendModeOptions.Premultiply:
                        SetSrcDestProperties(BlendMode.One, BlendMode.One);
                        t.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                        break;
                    case Properties.BlendModeOptions.Additive:
                        SetSrcDestProperties(BlendMode.SrcAlpha, BlendMode.One);
                        break;
                    case Properties.BlendModeOptions.Multiply:
                        SetSrcDestProperties(BlendMode.DstColor, BlendMode.Zero);
                        t.EnableKeyword("_ALPHAMODULATE_ON");
                        break;
                }

                depthWrite = false;
            }

            SetupDepthWriting(t, depthWrite);
            SetShadowCaster(t);

            void SetupDepthWriting(Material t, bool depthWrite)
            {
                if (p.DepthWriteControl.floatValue == 1.0f)
                {
                    depthWrite = true;
                }
                if (p.DepthWriteControl.floatValue == 2.0f)
                {
                    depthWrite = false;
                }
                t.SetFloat("_ZWrite", depthWrite ? 1.0f : 0.0f);
                t.SetShaderPassEnabled("DepthOnly", depthWrite);
            }

            void SetSrcDestProperties(BlendMode src, BlendMode dst)
            {
                t.SetFloat("_SrcBlend", (float)src);
                t.SetFloat("_DstBlend", (float)dst);
            }

            void SetShadowCaster(Material t)
            {
                t.SetShaderPassEnabled("ShadowCaster", p.CastShadows.floatValue == 1.0f);
            }

            showSurfaceOptions = EditorGUILayout.BeginFoldoutHeaderGroup(showSurfaceOptions, "Surface Options");
            if (showSurfaceOptions)
            {
                DrawEnumProperty(p._CullOptions, p.Cull, new GUIContent("Render Face"));
                EditorGUI.BeginChangeCheck();
                DrawEnumProperty(p._SurfaceOptions, p.Surface, new GUIContent("Surface"));
                if (EditorGUI.EndChangeCheck())
                {
                    if (p._SurfaceOptions == Properties.SurfaceOptions.Opaque)
                    {
                        if (p.AlphaClip.floatValue == 1.0f)
                        {
                            t.renderQueue = (int)RenderQueue.AlphaTest;
                        }
                        else
                        {
                            t.renderQueue = (int)RenderQueue.Geometry;
                        }
                    }
                    else
                    {
                        t.renderQueue = (int)RenderQueue.Transparent;
                    }
                }

                if ((Properties.SurfaceOptions)p.Surface.floatValue == Properties.SurfaceOptions.Transparent)
                {
                    EditorGUI.indentLevel++;
                    DrawEnumProperty(p._BlendModeOptions, p.Blend, new GUIContent("Blend Mode"));
                    EditorGUI.indentLevel--;
                }

                DrawToggleProperty(p.CastShadows, new GUIContent("Cast Shadows"));
                DrawToggleProperty(p.ReceiveShadows, new GUIContent("Receive Shadows"));
                EditorGUI.BeginChangeCheck();
                DrawToggleProperty(p.AlphaClip, new GUIContent("Alpha Clip"));
                if (EditorGUI.EndChangeCheck())
                {
                    if (p._SurfaceOptions == Properties.SurfaceOptions.Opaque)
                    {
                        if (p.AlphaClip.floatValue == 1.0f)
                        {
                            t.renderQueue = (int)RenderQueue.AlphaTest;
                        }
                        else
                        {
                            t.renderQueue = (int)RenderQueue.Geometry;
                        }
                    }
                }
                if (p.AlphaClip.floatValue == 1.0f)
                {
                    EditorGUI.indentLevel++;
                    e.ShaderProperty(p.AlphaClipThreshold, "Alpha Clip Threshold");
                    EditorGUI.indentLevel--;
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            EditorGUILayout.Space();
            showSurfaceInputs = EditorGUILayout.BeginFoldoutHeaderGroup(showSurfaceInputs, "Surface Inputs");
            if (showSurfaceInputs)
            {
                //e.ShaderProperty(p.MainColor, "Base Color");
                e.TexturePropertySingleLine(new GUIContent("Base Map"), p.MainTex, p.MainColor);

                e.TexturePropertySingleLine(new GUIContent("Normal Map"), p.NormalTex);
                t.SetFloat("_HasNormalMap", p.NormalTex.textureValue != null ? 1.0f : 0.0f);

                if (p.NormalTex.textureValue != null)
                {
                    EditorGUI.indentLevel++;
                    EditorGUI.indentLevel++;
                    e.ShaderProperty(p.NormalStrength, "Strength");
                    EditorGUI.indentLevel--;
                    EditorGUI.indentLevel--;
                }

                e.TexturePropertySingleLine(new GUIContent("Roughness Map"), p.RoughnessMap);

                if (p.RoughnessMap.textureValue != null)
                {
                    EditorGUI.indentLevel++;
                    EditorGUI.indentLevel++;
                    e.ShaderProperty(p.RoughnessAmount, "Strength");
                    EditorGUI.indentLevel--;
                    EditorGUI.indentLevel--;
                }

                e.ShaderProperty(p.Emissive, "Emissive");
                if (p.Emissive.floatValue == 1.0f)
                {
                    EditorGUI.indentLevel++;
                    e.TexturePropertySingleLine(new GUIContent("Emission Map"), p.EmissionMap, p.EmissionColor);
                    EditorGUI.indentLevel--;
                }

                EditorGUI.BeginChangeCheck();
                e.TextureScaleOffsetProperty(p.MainTex);
                if (EditorGUI.EndChangeCheck())
                {
                    p.NormalTex.textureScaleAndOffset = p.MainTex.textureScaleAndOffset;
                    p.EmissionMap.textureScaleAndOffset = p.MainTex.textureScaleAndOffset;
                    p.RoughnessMap.textureScaleAndOffset = p.MainTex.textureScaleAndOffset;
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            EditorGUILayout.Space();
            showLightingOptions = EditorGUILayout.BeginFoldoutHeaderGroup(showLightingOptions, "Lighting Options");
            if (showLightingOptions)
            {
                e.PopupShaderProperty(p.LightingBlendMode, new GUIContent("Lighting Mode"), Properties._LightingModes);
                e.ShaderProperty(p.Midpoint, "Midpoint");
                if (p.Midpoint.floatValue > 0)
                {
                    EditorGUI.indentLevel++;
                    e.ShaderProperty(p.MidpointStrength, "Midpoint Strength");
                    EditorGUI.indentLevel--;
                }
                e.ShaderProperty(p.ShadowTint, "Shadow Tint");
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            EditorGUILayout.Space();
            showSpecularAndRim = EditorGUILayout.BeginFoldoutHeaderGroup(showSpecularAndRim, "Specular and Rim Inputs");
            if (showSpecularAndRim)
            {
                e.ShaderProperty(p.SpecularHighlightsEnabled, "Specular Highlights");
                if (p.SpecularHighlightsEnabled.floatValue == 1)
                {
                    EditorGUI.indentLevel++;
                    DrawToggleProperty(p.SpecularColorAmount, new GUIContent("Override Light Color"));
                    if (p.SpecularColorAmount.floatValue == 1.0f)
                    {
                        EditorGUI.indentLevel++;
                        e.ShaderProperty(p.SpecularColor, "Color");
                        EditorGUI.indentLevel--;
                    }

                    DrawTextureProperty(p.SpecularDabTexture, new GUIContent("Dab Texture"));
                    if (p.SpecularDabTexture.textureValue != null)
                    {
                        EditorGUI.indentLevel++;
                        e.ShaderProperty(p.SpecularDabScale, "Scale");
                        e.ShaderProperty(p.SpecularDabRotation, "Rotation");

                        EditorGUI.indentLevel--;
                    }
                    EditorGUI.indentLevel--;
                    EditorGUILayout.Space();
                }

                e.ShaderProperty(p.RimLightingEnabled, "Rim Lighting");
                if (p.RimLightingEnabled.floatValue == 1)
                {
                    EditorGUI.indentLevel++;
                    e.ShaderProperty(p.RimThreshold, "Threshold");
                    e.ShaderProperty(p.RimColor, "Color");
                    EditorGUI.indentLevel--;
                }
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            EditorGUILayout.Space();
            showLighting = EditorGUILayout.BeginFoldoutHeaderGroup(showLighting, "Lighting Inputs");
            if (showLighting)
            {
                e.ShaderProperty(p.AmbientLightIntensity, "Ambient Lighting Strength");

                e.ShaderProperty(p.FogEnabled, "Receive Fog");

                e.ShaderProperty(p.AdditionalLightsEnabled, "Receive Additional Lights");
                if (p.AdditionalLightsEnabled.floatValue == 1)
                {
                    EditorGUI.indentLevel++;
                    e.ShaderProperty(p.AdditionalLightToonShadingEnabled, "Toon Shade");
                    if (p.AdditionalLightToonShadingEnabled.floatValue == 1)
                    {
                        EditorGUI.indentLevel++;
                        e.ShaderProperty(
                            p.AdditionalLightToonShadingThreshold,
                            new GUIContent("Threshold", "Light attenuation at which the lighting will be cut off. [Default: 0.1]")
                        );
                        EditorGUI.indentLevel--;
                    }
                    EditorGUI.indentLevel--;
                }

                e.ShaderProperty(p.AOEnabled, "Ambient Occlusion");
                if (p.AOEnabled.floatValue == 1)
                {
                    EditorGUI.indentLevel++;
                    e.ShaderProperty(p.AOStrength, "Strength");
                    e.ShaderProperty(p.AOToonShadingEnabled, "Toon Shade");
                    EditorGUI.indentLevel--;
                }
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            EditorGUILayout.Space();
            showAdvanced = EditorGUILayout.BeginFoldoutHeaderGroup(showAdvanced, "Advanced");
            if (showAdvanced)
            {
                e.PopupShaderProperty(p.ViewState, new GUIContent("View Options"), Properties._ViewOptions);
                e.IntPopupShaderProperty(p.DepthTest, "Depth Test", Properties._ZTestOptions, Properties._ZTestValues);
                e.PopupShaderProperty(p.DepthWriteControl, new GUIContent("Depth Write"), Properties._ZWriteOptions);

                EditorGUILayout.Space();
                e.EnableInstancingField();
                e.RenderQueueField();
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        public static void DrawToggleProperty(MaterialProperty p, GUIContent c)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = p.hasMixedValue;
            bool v = EditorGUILayout.Toggle(c, p.floatValue == 1.0f);
            if (EditorGUI.EndChangeCheck())
            {
                p.floatValue = v ? 1.0f : 0.0f;
            }
            EditorGUI.showMixedValue = false;
        }

        public static void DrawEnumProperty(Enum e, MaterialProperty p, GUIContent c)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = p.hasMixedValue;
            var v = EditorGUILayout.EnumPopup(c, e);
            if (EditorGUI.EndChangeCheck())
            {
                p.floatValue = Convert.ToInt32(v);
            }
            EditorGUI.showMixedValue = false;
        }

        public static void DrawTextureProperty(MaterialProperty p, GUIContent c)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = p.hasMixedValue;
            Texture2D t = (Texture2D)
                EditorGUILayout.ObjectField(c, p.textureValue, typeof(Texture2D), false, GUILayout.Height(EditorGUIUtility.singleLineHeight));
            if (EditorGUI.EndChangeCheck())
            {
                p.textureValue = t;
            }
            EditorGUI.showMixedValue = false;
        }

        public static void DrawOpacityProperty(MaterialProperty p, GUIContent c)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = p.hasMixedValue;
            float v = EditorGUILayout.Slider(c, p.colorValue.a, 0f, 1f);
            if (EditorGUI.EndChangeCheck())
            {
                p.colorValue = new Color(p.colorValue.r, p.colorValue.g, p.colorValue.b, v);
            }
            EditorGUI.showMixedValue = false;
        }

        private class Properties
        {
            public MaterialProperty MainColor;
            public MaterialProperty MainTex;

            public MaterialProperty Emissive;
            public MaterialProperty EmissionMap;
            public MaterialProperty EmissionColor;

            public MaterialProperty Opacity;
            public MaterialProperty AlphaClipThreshold;

            public MaterialProperty ShadowTint;
            public MaterialProperty Midpoint;
            public MaterialProperty MidpointStrength;

            public MaterialProperty LightingBlendMode;

            public MaterialProperty ViewState;

            public static readonly string[] _LightingModes = new string[] { "Constant", "Linear" };

            public static readonly string[] _ViewOptions = new string[]
            {
                "Default",
                "Normals",
                "Base Color",
                "Base Lighting",
                "Additional Lighting",
                "Ambient Occlusion",
                "Shadows",
                "Specular",
                "Rim",
                "Ambient Lighting",
                "Emission"
            };

            public SurfaceOptions _SurfaceOptions
            {
                get { return (SurfaceOptions)Surface.floatValue; }
                set { Surface.floatValue = (int)value; }
            }

            public enum SurfaceOptions
            {
                Opaque,
                Transparent
            }

            public BlendModeOptions _BlendModeOptions
            {
                get { return (BlendModeOptions)Blend.floatValue; }
                set { Blend.floatValue = (int)value; }
            }

            public enum BlendModeOptions
            {
                Alpha,
                Premultiply,
                Additive,
                Multiply
            }

            public static readonly string[] _ZWriteOptions = new string[] { "Auto", "Force Enabled", "Force Disabled" };

            public static readonly string[] _ZTestOptions = new string[]
            {
                "Never",
                "Less",
                "Equal",
                "LEqual",
                "Great",
                "Not Equal",
                "GEqual",
                "Always"
            };

            // ZTest 0 = Disabled, not a valid value
            public static readonly int[] _ZTestValues = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            public CullOptions _CullOptions
            {
                get { return (CullOptions)Cull.floatValue; }
                set { Cull.floatValue = (int)value; }
            }

            public enum CullOptions
            {
                Both,
                Back,
                Front
            }

            public MaterialProperty NormalTex;
            public MaterialProperty NormalStrength;

            public MaterialProperty ReceiveShadows;

            public MaterialProperty RoughnessMap;
            public MaterialProperty RoughnessAmount;

            public MaterialProperty SpecularHighlightsEnabled;

            #region Advanced Specular Settings
            public MaterialProperty SpecularColor;
            public MaterialProperty SpecularColorAmount;

            public MaterialProperty SpecularDabTexture;
            public MaterialProperty SpecularDabScale;
            public MaterialProperty SpecularDabRotation;
            #endregion

            public MaterialProperty RimLightingEnabled;
            public MaterialProperty RimThreshold;
            public MaterialProperty RimColor;

            #region Hide Under Advanced
            public MaterialProperty FogEnabled;
            public MaterialProperty AmbientLightIntensity;

            public MaterialProperty AOEnabled;
            public MaterialProperty AOStrength;
            public MaterialProperty AOToonShadingEnabled;

            public MaterialProperty AdditionalLightsEnabled;
            public MaterialProperty AdditionalLightToonShadingEnabled;
            public MaterialProperty AdditionalLightToonShadingThreshold;
            #endregion

            #region Shader Property Overrides
            public MaterialProperty Cull;

            public MaterialProperty Surface;
            public MaterialProperty Blend;

            public MaterialProperty AlphaClip;

            public MaterialProperty DepthWriteControl;
            public MaterialProperty DepthWrite;
            public MaterialProperty DepthTest;

            public MaterialProperty CastShadows;
            #endregion

            public Properties(MaterialProperty[] properties)
            {
                MainColor = FindProperty("_MainColor", properties, false);
                MainTex = FindProperty("_MainTex", properties, false);

                Emissive = FindProperty("_Emissive", properties, false);
                EmissionMap = FindProperty("_Emission_Map", properties, false);
                EmissionColor = FindProperty("_Emission_Color", properties, false);

                ReceiveShadows = FindProperty("_ReceiveShadows", properties, false);
                LightingBlendMode = FindProperty("_LightingMode", properties, false);
                Midpoint = FindProperty("_Midpoint", properties, false);
                MidpointStrength = FindProperty("_MidpointStrength", properties, false);
                ShadowTint = FindProperty("_ShadowTint", properties, false);

                NormalTex = FindProperty("_NormalMap", properties, false);
                NormalStrength = FindProperty("_NormalStrength", properties, false);

                RoughnessMap = FindProperty("_RoughnessMap", properties, false);
                RoughnessAmount = FindProperty("_RoughnessAmount", properties, false);

                SpecularHighlightsEnabled = FindProperty("_SpecularHighlightsEnabled", properties, false);
                SpecularColor = FindProperty("_SpecularColor", properties, false);
                SpecularColorAmount = FindProperty("_SpecularColorAmount", properties, false);
                SpecularDabTexture = FindProperty("_SpecularDabTexture", properties, false);
                SpecularDabScale = FindProperty("_SpecularDabScale", properties, false);
                SpecularDabRotation = FindProperty("_SpecularDabRotation", properties, false);

                RimLightingEnabled = FindProperty("_RimLightingEnabled", properties, false);
                RimThreshold = FindProperty("_RimThreshold", properties, false);
                RimColor = FindProperty("_RimColor", properties, false);

                AlphaClipThreshold = FindProperty("_AlphaClipThreshold", properties, false);

                AmbientLightIntensity = FindProperty("_AmbientLightStrength", properties, false);

                FogEnabled = FindProperty("_FogEnabled", properties, false);
                AOEnabled = FindProperty("_AOEnabled", properties, false);
                AOStrength = FindProperty("_AOStrength", properties, false);
                AOToonShadingEnabled = FindProperty("_AOToonShadingEnabled", properties, false);

                AdditionalLightToonShadingThreshold = FindProperty("_AdditionalLightToonShadingThreshold", properties, false);
                AdditionalLightToonShadingEnabled = FindProperty("_AdditionalLightToonShadingEnabled", properties, false);
                AdditionalLightsEnabled = FindProperty("_ReceiveAdditionalLights", properties, false);

                ViewState = FindProperty("_ViewState", properties, false);

                Cull = FindProperty("_Cull", properties, false);

                Surface = FindProperty("_Surface", properties, false);
                Blend = FindProperty("_Blend", properties, false);

                Opacity = FindProperty("_Opacity", properties, false);
                AlphaClip = FindProperty("_AlphaClip", properties, false);

                DepthWriteControl = FindProperty("_ZWriteControl", properties, false);
                DepthWrite = FindProperty("_ZWrite", properties, false);
                DepthTest = FindProperty("_ZTest", properties, false);

                CastShadows = FindProperty("_CastShadows", properties, false);
            }
        }
    }
}
