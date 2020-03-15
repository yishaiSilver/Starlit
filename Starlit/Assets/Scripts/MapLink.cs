using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLink : MonoBehaviour {
	GameObject lineObject;
	LineRenderer line;
	private float width = 0.05f;
	private float z = 0.05f;

	private Color defaultColor;
	private Color activeColor;

	private GameObject start;
	private GameObject end;

	public void newMapLink(GameObject parentNode, GameObject start, GameObject end, Color defaultColor, Color activeColor)
	{
		this.start = start;
		this.end = end;
		this.defaultColor = defaultColor;
		this.activeColor = activeColor;

		lineObject = new GameObject();
		lineObject.transform.SetParent(parentNode.transform);

		line = lineObject.AddComponent<LineRenderer>();
		line.startWidth = width;
		line.endWidth = width;
		line.material = new Material(Shader.Find("Particles/Additive"));

		line.startColor = defaultColor;
		line.endColor = defaultColor;
		
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

	public void setDefaultColor()
	{
		line.startColor = defaultColor;
		line.endColor = defaultColor;
	}

	public void setActiveColor()
	{
		line.startColor = activeColor;
		line.endColor = activeColor;
	}
}
