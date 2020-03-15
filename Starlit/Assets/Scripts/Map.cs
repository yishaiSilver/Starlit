using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

	private MapNode[] allNodes;
	[SerializeField] private Color defaultColor;
	[SerializeField] private Color activeColor;

	// Use this for initialization
	void Start()
	{
		GameObject[] starObjects = GameObject.FindGameObjectsWithTag("MapNode");
		allNodes = new MapNode[starObjects.Length];
		for (int i = 0; i < starObjects.Length; i++)
		{
			allNodes[i] = starObjects[i].GetComponent<MapNode>();
			allNodes[i].setNodeIndex(i);
			allNodes[i].initializeLinks();
			Debug.Log(i + ": " + allNodes[i].name);
		}

		drawAllLinks();
		getStackTo(allNodes[6], allNodes[3]);
	}

	/*public Stack<StarSystem> getStackTo(StarSystem from, StarSystem to)
	{
		int[] bfsReturn = BFSShortestReach(from);
		return retraceBFSPath(from, to, bfsReturn);
	}

	public int[] BFSShortestReach(StarSystem from)
	{
		Queue<StarSystem> q = new Queue<StarSystem>();
		q.Enqueue(from);

		int[] previousSystem = new int[allNodes.Length];
		bool[] haveVisited = new bool[allNodes.Length];

		haveVisited[from.getStarSystemIndex()] = true;
		previousSystem[from.getStarSystemIndex()] = from.getStarSystemIndex();

		while (q.Count != 0)
		{
			StarSystem currentNode = q.Dequeue();
			StarSystem[] neighbors = currentNode.getNeighboringStars();

			for (int i = 0; i < neighbors.Length; i++)
			{
				if (!haveVisited[neighbors[i].getStarSystemIndex()]) // issue
				{
					previousSystem[neighbors[i].getStarSystemIndex()] = currentNode.getStarSystemIndex();
					haveVisited[neighbors[i].getStarSystemIndex()] = true;
					q.Enqueue(neighbors[i]);
				}
			}
		}

		return previousSystem;
	}

	public Stack<StarSystem> retraceBFSPath(StarSystem from, StarSystem to, int[] BFSOutput)
	{
		Stack<StarSystem> directions = new Stack<StarSystem>();


		for (int at = to.getStarSystemIndex(); at != BFSOutput[at]; at = BFSOutput[at])
		{
			Debug.Log("Pushing: " + allNodes[at]);
			directions.Push(allNodes[at].getStar());
		}

		if (directions.Peek() == from)
		{
			return null;
		}

		return directions;
	} */

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

	public Stack<MapNode> getStackTo(MapNode from, MapNode to)
    {
		Debug.Log("Getting path from " + from.name + " to " + to.name + ".\n");

		int[] bfsReturn = BFSShortestReach(from);
		return retraceBFSPath(from, to, bfsReturn);
    }

	public int[] BFSShortestReach(MapNode from)
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
				if(!haveVisited[neighbors[i].getNodeIndex()]) // issue
				{
					previousSystem[neighbors[i].getNodeIndex()] = currentNode.getNodeIndex();
					haveVisited[neighbors[i].getNodeIndex()] = true;
					q.Enqueue(neighbors[i]);
				}
			}
		}

		return previousSystem;
	}

	public Stack<MapNode> retraceBFSPath(MapNode from, MapNode to, int[] BFSOutput)
	{
		Stack<MapNode> directions = new Stack<MapNode>();

		
		for (int at = to.getNodeIndex(); at != BFSOutput[at]; at = BFSOutput[at])
		{
			Debug.Log(allNodes[at] + " -> " + allNodes[BFSOutput[at]]);
			directions.Push(allNodes[at]);
			allNodes[at].setLinkActive(allNodes[BFSOutput[at]]);
		}

		if(directions.Peek() == from)
		{
			return null;
		}

		return directions;
	}
}
