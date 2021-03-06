﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode : MonoBehaviour {

	[SerializeField] private MapNode[] neighbors;
	public StarSystem thisStar;
	private Map map;
	private int index;
	private MapLink[] links;

	public void setMap(Map map)
	{
		this.map = map;
	}

	public MapNode[] getNeighbors()
	{
		return neighbors;
	}

	public MapLink[] getLinks()
	{
		return links;
	}

	public void initializeLinks()
	{
		if (links == null)
		{
			links = new MapLink[neighbors.Length];
		}
	}

	public StarSystem getStar()
	{
		return thisStar;
	}

	public void setNodeIndex(int i)
	{
		index = i;
		if (thisStar != null)
		{
			thisStar.setStarSystemIndex(i);
		}
	}

	public int getNodeIndex()
	{
		return index;
	}

	public MapLink makeNewMapLink(GameObject mapPanel, int n, Color defaultColor, Color activeColor)
	{
		links[n] = gameObject.AddComponent<MapLink>();
		links[n].newMapLink(mapPanel, gameObject, gameObject, neighbors[n].gameObject, defaultColor, activeColor);
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

	public void setLinkActive(MapNode to)
	{
		for (int i = 0; i < neighbors.Length; i++)
		{
			if (neighbors[i] == to)
			{
				links[i].setActiveColor();
				break;
			}
		}
	}

	public void setLinkDefault(MapNode to)
	{
		for (int i = 0; i < neighbors.Length; i++)
		{
			if (neighbors[i] == to)
			{
				links[i].setDefaultColor();
				break;
			}
		}
	}

	public void onButtonPress()
	{
		map.getStackTo(this);
	}
}
