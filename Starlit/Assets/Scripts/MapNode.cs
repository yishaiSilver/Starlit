using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode : MonoBehaviour {

	[SerializeField] private MapNode[] neighbors;
	public StarSystem thisStar;
	private Map map;
	private int index;
	private MapLink[] links;

	void Start()
	{
		map = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>();
		links = new MapLink[neighbors.Length];
	}

	public MapNode[] getNeighbors()
	{
		return neighbors;
	}

	public MapLink[] getLinks()
	{
		return links;
	}

	public StarSystem getStar()
	{
		return thisStar;
	}

	public void setNodeIndex(int i)
	{
		index = i;
	}

	public int getNodeIndex()
	{
		return index;
	}

	public MapLink makeNewMapLink(GameObject mapLine, int n, Color color)
	{
		links[n] = gameObject.AddComponent<MapLink>();
		links[n].newMapLink(gameObject, gameObject, neighbors[n].gameObject, color);
		return links[n];
	}

	public void setNewMapLink(MapNode from, MapLink link)
	{
		for (int i = 0; i < neighbors.Length; i++)
		{
			if (neighbors[i] == from)
			{
				links[i] = link;
				break;
			}
		}
	}
}
