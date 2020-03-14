using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLink : MonoBehaviour {
	GameObject lineObject;
	LineRenderer line;
	private float width = 0.05f;
	private float z = 0.05f;

	private GameObject start;
	private GameObject end;

	void Start()
	{

	}

	public void newMapLink(GameObject lineObject, GameObject start, GameObject end, Color color)
	{
		this.start = start;
		this.end = end;

		GameObject asdf = new GameObject();
		line = asdf.AddComponent<LineRenderer>();
		line.startWidth = width;
		line.endWidth = width;
		line.material = new Material(Shader.Find("Particles/Additive"));
		line.startColor = color;
		line.endColor = color;
		line.sortingLayerName = "UI";
		line.positionCount = 2;

		line.SetPosition(0, new Vector3(start.transform.position.x, start.transform.position.y, z));
		line.SetPosition(1, new Vector3(end.transform.position.x, end.transform.position.y, z));
	}

	void LateUpdate()
	{
		line.SetPosition(0, new Vector3(start.transform.position.x, start.transform.position.y, z));
		line.SetPosition(1, new Vector3(end.transform.position.x, end.transform.position.y, z));
	}
}
