using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
  public Transform target;
  public float speed = 6f;      // Orbit rotation speed (degrees per second)
  public float height = 6f;     // Camera height above the target
  public float distance = 30f;  // Orbit radius (distance from target)

  private float angle = 0f;

  void Start()
  {
    // Automatically find the CityGenerator if no target is manually assigned
    if (target == null)
    {
      var cg = GameObject.FindObjectOfType<CityGenerator>();
      if (cg != null) target = cg.transform;
    }
  }

  void Update()
  {
    if (target == null) return;

    // Calculate the camera's new position along the orbit path
    angle += speed * Time.deltaTime;
    float rad = angle * Mathf.Deg2Rad;
    Vector3 pos = target.position + new Vector3(Mathf.Sin(rad) * distance, height, Mathf.Cos(rad) * distance);

    transform.position = pos;

    // Always look at the center (target) while orbiting
    transform.LookAt(target.position);
  }
}
