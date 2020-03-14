using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour
{

    public float speed;
    public Transform target;

    private Vector3 zAxis = new Vector3(0, 0, 1);

	void Start()
    {
		//renderCircle((transform.position - target.transform.position).magnitude, 4);
    }

    void FixedUpdate()
    {
        transform.RotateAround(target.position, zAxis, speed * Time.deltaTime);
        transform.Rotate(0, 0, -speed * Time.deltaTime);
    }

	public void renderCircle(float r, int size)
	{
		float theta_scale = 2 * Mathf.PI / size - 1;  // Circle resolution

		LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		//lineRenderer.SetColors(, c2);
		lineRenderer.startWidth = 0.2F;
		lineRenderer.positionCount = size;
		lineRenderer.loop = true;

		int i = 0;
		for (float theta = 0; theta < 2 * Mathf.PI; theta += theta_scale)
		{
			float x = r * Mathf.Cos(theta);
			float y = r * Mathf.Sin(theta);

			Vector3 pos = new Vector3(x, y, 0);
			lineRenderer.SetPosition(i, target.position + pos);
			i += 1;
		}
	}
}