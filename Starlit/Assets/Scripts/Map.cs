using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

	private MapNode[] allNodes;
	[SerializeField] private Color defaultColor;
	[SerializeField] private Color activeColor;

	private Ship defaultShip;

	public GameObject mapPanel;

	// Use this for initialization
	void Start()
	{
		mapPanel.SetActive(true);

		GameObject[] starObjects = GameObject.FindGameObjectsWithTag("MapNode");
		allNodes = new MapNode[starObjects.Length];
		for (int i = 0; i < starObjects.Length; i++)
		{
			allNodes[i] = starObjects[i].GetComponent<MapNode>();
			allNodes[i].setNodeIndex(i);
			allNodes[i].initializeLinks();
			allNodes[i].setMap(this);
			Debug.Log(i + ": " + allNodes[i].name);
		}

		drawAllLinks();

		mapPanel.SetActive(false);
		//getStackTo(allNodes[14], allNodes[0]);
	}

	public void setDefaultShip(Ship ship)
	{
		defaultShip = ship;
	}

	public void drawAllLinks()
	{
		Queue<MapNode> q = new Queue<MapNode>();
		q.Enqueue(allNodes[0]);

		bool[] haveVisited = new bool[allNodes.Length];

		haveVisited[allNodes[0].getNodeIndex()] = true;

		while (q.Count != 0)
		{
			MapNode currentNode = q.Dequeue();
			MapNode[] neighbors = currentNode.getNeighbors();
			MapLink[] links = currentNode.getLinks();

			for(int i = 0; i < neighbors.Length; i ++)
			{
				if (links[i] == null)
				{
					MapLink link = currentNode.makeNewMapLink(i, defaultColor, activeColor);
					neighbors[i].setNewMapLink(currentNode, link);
					if (!haveVisited[neighbors[i].getNodeIndex()])
					{
						q.Enqueue(neighbors[i]);
						haveVisited[neighbors[i].getNodeIndex()] = true;
					}
				}
			}
		}
	}

	public void getStackTo(MapNode target)
	{
		getStackTo(defaultShip, target);
	}

	public void getStackTo(Ship callingShip, MapNode to)
    {
		//Debug.Log("Getting path from " + from.name + " to " + to.name + ".\n");

		MapNode from = allNodes[callingShip.getStarSystem().getStarSystemIndex()];
		callingShip.setTargetOnMap(to);

		if(from == to)
		{
			callingShip.setJumpPath(null);
		}

		if (callingShip == defaultShip)
		{
			int[] bfsReturn = BFSShortestReach(from, true);
			callingShip.setJumpPath(retraceBFSPath(from, to, bfsReturn, true));
		}
		else
		{
			int[] bfsReturn = BFSShortestReach(from, false);
			callingShip.setJumpPath(retraceBFSPath(from, to, bfsReturn, false));
		}
    }

	public int[] BFSShortestReach(MapNode from, bool shouldDraw)
	{
		Queue<MapNode> q = new Queue<MapNode>();
		q.Enqueue(from);

		int[] previousSystem = new int[allNodes.Length];
		bool[] haveVisited = new bool[allNodes.Length];

		haveVisited[from.getNodeIndex()] = true;
		previousSystem[from.getNodeIndex()] = from.getNodeIndex();

		while (q.Count != 0)
		{
			MapNode currentNode = q.Dequeue();
			MapNode[] neighbors = currentNode.getNeighbors();

			for(int i = 0; i < neighbors.Length; i ++)
			{
				if (!haveVisited[neighbors[i].getNodeIndex()]) // issue
				{
					previousSystem[neighbors[i].getNodeIndex()] = currentNode.getNodeIndex();
					haveVisited[neighbors[i].getNodeIndex()] = true;

					if (shouldDraw) { 
						currentNode.setLinkDefault(neighbors[i]);
					}
					
					q.Enqueue(neighbors[i]);
				}
			}
		}

		return previousSystem;
	}

	public Stack<StarSystem> retraceBFSPath(MapNode from, MapNode to, int[] BFSOutput, bool shouldDraw)
	{
		Stack<StarSystem> directions = new Stack<StarSystem>();
		
		for (int at = to.getNodeIndex(); at != BFSOutput[at]; at = BFSOutput[at])
		{
			//Debug.Log(allNodes[at] + " -> " + allNodes[BFSOutput[at]]);
			directions.Push(allNodes[at].thisStar);

			if (shouldDraw)
			{
				allNodes[at].setLinkActive(allNodes[BFSOutput[at]]);
			}
		}

		if(directions.Peek() == from)
		{
			return null;
		}

		return directions;
	}

	public void setDefualtShip(Ship ship)
	{
		defaultShip = ship;
	}

	public void loadDirections()
	{
		loadDirections(defaultShip);
	}

	public void loadDirections(Ship callingShip)
	{
		MapNode from = allNodes[callingShip.getStarSystem().getStarSystemIndex()];
		MapNode to = callingShip.getTargetOnMap();

		int[] bfsReturn = BFSShortestReach(from, true);

		if (from != to && to != null)
		{
			callingShip.setJumpPath(retraceBFSPath(from, to, bfsReturn, true));
		}
	}
}
