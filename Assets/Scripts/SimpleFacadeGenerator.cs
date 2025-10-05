using UnityEngine;

[CreateAssetMenu(fileName = "SimpleFacade", menuName = "City/Simple Facade")]
public class SimpleFacadeGenerator : ScriptableObject
{
    [Header("Texture Size")]
    public int textureWidth = 128;
    public int textureHeight = 256;

    [Header("Windows (size and spacing)")]
    public int windowSize = 20;
    public int windowSpacing = 30;

    [Header("Colors and Probabilities")]
    public Color[] litColors = {
        new Color(1f, 0.9f, 0.6f), // warm yellow
        new Color(1f, 0.8f, 0.4f), // orange
        new Color(0.4f, 0.6f, 1f), // blue
        new Color(1f, 0.3f, 0.3f)  // red
    };
    [Range(0f, 1f)] public float litProbability = 0.7f;  // chance that a window is lit
    [Range(0f, 0.5f)] public float colorVariation = 0.12f;  // small color brightness variation
    public Color offColor = new Color(0.02f, 0.02f, 0.02f); // dark window (off)
    public Color backgroundColor = new Color(0.02f, 0.02f, 0.04f); // wall color

    [Header("Emission (for bloom effect)")]
    public bool enableEmission = true;
    public float emissionMultiplier = 1.2f;

    [Header("Reproducibility (optional)")]
    public int seed = 0; // 0 = random every time; any other number fixes the seed

    // 🔹 Generates a material with a procedural building facade texture
    public Material GenerateMaterial()
    {
        if (seed != 0) Random.InitState(seed);

        // Create and initialize texture
        Texture2D tex = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Point;
        tex.wrapMode = TextureWrapMode.Repeat;

        // Paint entire background first
        for (int y = 0; y < textureHeight; y++)
            for (int x = 0; x < textureWidth; x++)
                tex.SetPixel(x, y, backgroundColor);

        // Draw window grid across the texture
        for (int y = 0; y < textureHeight; y += windowSize + windowSpacing)
        {
            for (int x = 0; x < textureWidth; x += windowSize + windowSpacing)
            {
                bool lit = Random.value < litProbability; // decide if window is lit
                Color color = lit ? litColors[Random.Range(0, litColors.Length)] : offColor;

                // Fill a small square area for the window
                for (int wy = 0; wy < windowSize; wy++)
                {
                    for (int wx = 0; wx < windowSize; wx++)
                    {
                        int px = x + wx;
                        int py = y + wy;
                        if (px < textureWidth && py < textureHeight)
                            tex.SetPixel(px, py, color);
                    }
                }
            }
        }

        tex.Apply();

        // Create material and assign the generated texture
        //Shader shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
        Shader shader = Shader.Find("Standard");

        Material mat = new Material(shader);

        if (mat.HasProperty("_BaseMap")) mat.SetTexture("_BaseMap", tex);
        else mat.mainTexture = tex;

        // Enable emission for glowing windows
        if (enableEmission && mat.HasProperty("_EmissionColor"))
        {
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", Color.black * emissionMultiplier);
            if (mat.HasProperty("_EmissionMap")) mat.SetTexture("_EmissionMap", tex);
        }

        return mat;
    }

}
