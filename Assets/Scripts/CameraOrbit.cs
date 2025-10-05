using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
  public Transform target;
  public float speed = 6f;       // Orbit rotation speed (degrees per second)
  public float height = 12f;     // Camera height above the target
  public float distance = 25f;   // Orbit radius (distance from target)
  public float startAngle = 45f; // Starting angle (so it doesn't begin flat)

  private float angle;

  void Start()
  {
    // Automatically find the CityGenerator if not manually assigned
    if (target == null)
    {
      var cg = GameObject.FindObjectOfType<CityGenerator>();
      if (cg != null) target = cg.transform;
    }

    // Initialize the orbit angle
    angle = startAngle;

    // Immediately position the camera correctly at startup
    UpdateCameraPosition();
  }

  void Update()
  {
    if (target == null) return;

    angle += speed * Time.deltaTime;
    UpdateCameraPosition();
  }

  void UpdateCameraPosition()
  {
    float rad = angle * Mathf.Deg2Rad;
    Vector3 pos = target.position + new Vector3(Mathf.Sin(rad) * distance, height, Mathf.Cos(rad) * distance);
    transform.position = pos;
    transform.LookAt(target.position + Vector3.up * (height * 0.2f)); // look slightly downward
  }
}
