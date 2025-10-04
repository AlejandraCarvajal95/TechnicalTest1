using UnityEngine;

[ExecuteInEditMode]
public class CityGenerator : MonoBehaviour
{
    [Header("Facades (Procedural Textures)")]
    public SimpleFacadeGenerator facadeGenerator; // Assign your procedural facade asset here

    [Header("City Layout (in blocks)")]
    public int blocksX = 15;
    public int blocksY = 15;

    [Header("Block Settings")]
    public int buildingsPerBlock = 3;  // 3x3 buildings per block
    public float buildingSpacing = 2f; // Distance between buildings within a block
    public float streetWidth = 4f;     // Space between blocks

    [Header("Building Settings")]
    public GameObject buildingPrefab;  // Optional prefab (defaults to cube if null)
    public Vector2 buildingHeightRange = new Vector2(2f, 8f);
    public Vector2 buildingFootprintRange = new Vector2(0.8f, 1.8f);

    [Header("Generation Options")]
    public bool clearBeforeGenerate = true;
    public int seed = 0; // 0 = random seed

    [ContextMenu("Generate City")]
    public void Generate()
    {
        if (clearBeforeGenerate) ClearCity();
        if (seed != 0) Random.InitState(seed);

        float blockExtent = buildingsPerBlock * buildingSpacing;
        float totalWidth = blocksX * blockExtent + (blocksX - 1) * streetWidth;
        float totalDepth = blocksY * blockExtent + (blocksY - 1) * streetWidth;

        // Center the city at world origin (0,0,0)
        float xOffset = -totalWidth / 2f + buildingSpacing / 2f;
        float zOffset = -totalDepth / 2f + buildingSpacing / 2f;

        // Generate each block and its buildings
        for (int bx = 0; bx < blocksX; bx++)
        {
            for (int by = 0; by < blocksY; by++)
            {
                float blockOriginX = bx * (blockExtent + streetWidth);
                float blockOriginZ = by * (blockExtent + streetWidth);

                for (int ix = 0; ix < buildingsPerBlock; ix++)
                {
                    for (int iz = 0; iz < buildingsPerBlock; iz++)
                    {
                        float x = xOffset + blockOriginX + ix * buildingSpacing;
                        float z = zOffset + blockOriginZ + iz * buildingSpacing;
                        float h = Random.Range(buildingHeightRange.x, buildingHeightRange.y);
                        float foot = Random.Range(buildingFootprintRange.x, buildingFootprintRange.y);

                        // Create building object
                        GameObject b;
                        if (buildingPrefab != null)
                            b = Instantiate(buildingPrefab, this.transform);
                        else
                        {
                            b = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            b.transform.parent = this.transform;
                        }

                        b.name = $"B_{bx}_{by}_{ix}_{iz}";
                        b.transform.localScale = new Vector3(foot, h, foot);
                        b.transform.position = new Vector3(x, h / 2f, z);

                        // Apply procedural facade material if available
                        var rend = b.GetComponent<Renderer>();
                        if (rend != null)
                        {
                            if (facadeGenerator != null)
                                rend.sharedMaterial = facadeGenerator.GenerateMaterial();
                            else
                            {
                                // Fallback to simple gray material
                                var mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                                mat.SetColor("_BaseColor", Color.gray);
                                rend.sharedMaterial = mat;
                            }
                        }

                        // Mark building as static (for editor performance)
                        b.isStatic = true;
                    }
                }
            }
        }

        Debug.Log($"City generated: {blocksX}x{blocksY} blocks.");
    }

    [ContextMenu("Clear City")]
    public void ClearCity()
    {
        // Remove all previously generated buildings
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
#if UNITY_EDITOR
            DestroyImmediate(transform.GetChild(i).gameObject);
#else
            Destroy(transform.GetChild(i).gameObject);
#endif
        }
    }
}
