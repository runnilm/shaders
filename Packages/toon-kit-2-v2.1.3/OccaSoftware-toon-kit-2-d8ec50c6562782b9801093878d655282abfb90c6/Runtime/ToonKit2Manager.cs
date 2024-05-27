using UnityEngine;

namespace OccaSoftware.ToonKit2.Runtime
{
    [ExecuteAlways]
    public class ToonKit2Manager : MonoBehaviour
    {
        [SerializeField, ColorUsage(false, true)]
        Color ambientLightColor = Color.white;

        [SerializeField, Range(0, 1)]
        float shadowStrength = 1f;

        void Update()
        {
            Shader.SetGlobalColor(ShaderParams.AmbientLightColor.Id, ambientLightColor);
            Shader.SetGlobalFloat(ShaderParams.ShadowStrength.Id, shadowStrength);
        }

        private static class ShaderParams
        {
            public class ShaderParam
            {
                string name;
                public string Name
                {
                    get { return name; }
                }
                int id;
                public int Id
                {
                    get { return id; }
                }

                public ShaderParam(string name)
                {
                    this.name = name;
                    id = Shader.PropertyToID(name);
                }
            }

            public static ShaderParam AmbientLightColor = new ShaderParam("_AmbientLightColorTK2");
            public static ShaderParam ShadowStrength = new ShaderParam("_ShadowStrengthTK2");
        }
    }
}
