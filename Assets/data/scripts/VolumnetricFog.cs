// Create a new C# script called "VolumetricFog.cs"
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[ExecuteAlways]
public class VolumetricFog : MonoBehaviour {
	public enum FogMode {
		Uniform,
		Exponential,
		HeightBased
	}

	[System.Serializable]
	public class FogSettings {
		public FogMode fogMode = FogMode.HeightBased;
		public Color fogColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);
		[Range(0, 1)] public float fogDensity = 0.2f;
		public float fogHeightFalloff = 0.5f;
		public float fogHeight = 10f;
		[Range(0, 1)] public float fogScatteringIntensity = 0.5f;
		[Range(0, 100)] public float maxDistance = 50f;
	}

	public FogSettings settings = new FogSettings();
	public Material fogMaterial;

	// References to the scene lighting
	public Light mainLight;

	private void OnEnable() {
		if (mainLight == null)
			mainLight = FindObjectOfType<Light>();

		// Find the renderer feature if using URP custom renderer
		var rendererData = GetURPRendererData();
		if (rendererData != null) {
			UpdateRendererFeature(rendererData);
		}
	}

	private void Update() {
		if (fogMaterial != null) {
			// Update fog material properties
			fogMaterial.SetColor("_FogColor", settings.fogColor);
			fogMaterial.SetFloat("_FogDensity", settings.fogDensity);
			fogMaterial.SetFloat("_FogHeightFalloff", settings.fogHeightFalloff);
			fogMaterial.SetFloat("_FogHeight", settings.fogHeight);
			fogMaterial.SetFloat("_ScatteringIntensity", settings.fogScatteringIntensity);
			fogMaterial.SetFloat("_MaxDistance", settings.maxDistance);

			// Update light direction and color
			if (mainLight != null) {
				fogMaterial.SetVector("_LightDir", -mainLight.transform.forward);
				fogMaterial.SetColor("_LightColor", mainLight.color * mainLight.intensity);
			}

			// Update fog mode
			fogMaterial.SetFloat("_FogMode", (int)settings.fogMode);
		}
	}

	private ScriptableRendererData GetURPRendererData() {
		// Try to get the renderer data from the current pipeline asset
		var universalRenderPipelineAsset = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
		if (universalRenderPipelineAsset != null) {
			System.Reflection.FieldInfo propertyInfo = universalRenderPipelineAsset.GetType().GetField("m_RendererDataList",
				System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

			if (propertyInfo != null) {
				return (propertyInfo.GetValue(universalRenderPipelineAsset) as ScriptableRendererData[])?[0];
			}
		}
		return null;
	}

	private void UpdateRendererFeature(ScriptableRendererData rendererData) {
		// Update or add renderer feature as needed
		// This is a placeholder - in a real implementation you'd find or add your
		// custom fog renderer feature to the renderer data
	}
}
