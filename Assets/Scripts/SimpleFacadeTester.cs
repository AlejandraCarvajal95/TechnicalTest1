using UnityEngine;

[ExecuteInEditMode]
public class SimpleFacadeTester : MonoBehaviour
{
  public SimpleFacadeGenerator facade; // Reference to the procedural facade generator asset

  [ContextMenu("Apply Facade")]
  public void ApplyFacade()
  {
    // Ensure the facade generator is assigned
    if (facade == null)
    {
      Debug.LogWarning("Assign a FacadeGenerator before applying.");
      return;
    }

    // Ensure this object has a renderer to receive the material
    var rend = GetComponent<Renderer>();
    if (rend == null)
    {
      Debug.LogWarning("This object has no Renderer component.");
      return;
    }

    // Generate and apply the procedural material
    rend.sharedMaterial = facade.GenerateMaterial();
    Debug.Log("Facade applied to " + name);
  }
}
