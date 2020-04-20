using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise3D : MonoBehaviour {

	public float width;
	public float height;
	public float yScale = 1;
	public float xScale = 1;

	public float rotSpeed = 1;
	private float offset = 0;

	public float greyThreshold;

	public bool is3d;

	public float radius;
	public float xCenter;
	public float yCenter;

	public bool isLeft;

	public float bubbleWidth;
	public float bubbleHeight;

	void Start()
	{
		radius = width / (2 * Mathf.PI);
	}

	void Update () {
		Renderer rend = GetComponent<Renderer>();
		rend.material.mainTexture = GenerateTexture();

		offset = (offset + rotSpeed * Time.deltaTime) % (2 * Mathf.PI);
	}
	
	private Texture2D GenerateTexture()
	{
		Texture2D texture = new Texture2D((int)width, (int)height);

		drawCircle(texture, radius, xCenter, yCenter, isLeft);

		PlanetBubble planetBubble = new PlanetBubble(texture, xCenter, yCenter, bubbleWidth, bubbleHeight);
		planetBubble.draw(Color.yellow);

		/*for(int x = 0; x < width; x++)
		{
			for(int y = 0; y < height; y++)
			{
				Color color;
				if (!is3d)
				{
					color = CalculateColor(x, y);
				}
				else
				{
					color = CalculateColor3D(x, y);
				}
				texture.SetPixel(x, y, color);
			}
		}*/

		texture.Apply();

		return texture;
	}

	private Color CalculateColor(float x, float y)
	{
		x = x / width * xScale;
		y = y / width * yScale;

		float grey = 1 - (((int)(Mathf.PerlinNoise(x, y) * 6f)) / 5f);
		
		if (grey < 0.4f)
		{
			return Color.blue;
		}
		else if (grey < 0.6f)
		{
			return Color.cyan;
		}
		else if (grey < 0.8f)
		{
			return Color.yellow;
		}
		else if (grey <= 1)
		{
			return Color.green;
		}
		else
		{
			return new Color(grey, grey, grey); ;
		}
		
	}

	private Color CalculateColor3D(float x, float y)
	{
		float px = x;
		float z = y;

		float theta = ((x * 2 * Mathf.PI) / width + offset) % (2 * Mathf.PI);

		x = Mathf.Cos(theta) * radius;
		y = Mathf.Sin(theta) * radius;

		/*
		if (z <= 5)
		{
			float i = theta / (2 * Mathf.PI);
			return new Color(i, i, i);
		}
		else if(z <= 10)
		{
			float i = ((x / radius) + 1) / (2);
			return new Color(i, i, i);
		}
		else if (z <= 15)
		{ 
			float i = ((y / radius) + 1) / (2);
			return new Color(i, i, i);
		}*/

		px *= xScale;

		theta = ((px * 2 * Mathf.PI) / width + offset) % (2 * Mathf.PI);

		x = Mathf.Cos(theta) * radius;
		y = Mathf.Sin(theta) * radius;

		z = z * yScale;
		float grey = Perlin3D(x, y, z);

		return new Color(grey, grey, grey);
	}

	private float Perlin3D(float x, float y, float z)
	{
		float ab = Mathf.PerlinNoise(x, y);
		float bc = Mathf.PerlinNoise(y, z);
		float ac = Mathf.PerlinNoise(x, z);

		float ba = Mathf.PerlinNoise(y, x);
		float cb = Mathf.PerlinNoise(z, y);
		float ca = Mathf.PerlinNoise(z, x);

		float abc = ab + bc + ac + ba + cb + ca;
		return abc / 6f;
	}

	public void drawCircle(Texture2D texture, float radius, float xCenter, float yCenter, bool isLeftSide)
	{
		for (int x = (int)(xCenter - radius); x <= (int)(xCenter + radius); x++)
		{
			if(x < xCenter)
				continue;

			for (int y = (int)(yCenter - radius); y <= (int)(yCenter + radius); y++)
			{
				Color color;

				bool isValid = Mathf.Pow(x - xCenter, 2) + Mathf.Pow(y - yCenter, 2) < radius * radius;

				if (isValid)
				{
					color = Color.black;
					texture.SetPixel(x, y, color);
				}

			}
		}
		
	}

	public void makeRound(Texture2D texture, float radius, float xCenter, float yCenter, bool isLeftSide)
	{
		for (int x = 0; x <= (int)(xCenter + radius); x++)
		{
			if (x < xCenter)
				continue;

			for (int y = (int)(yCenter - radius); y <= (int)(yCenter + radius); y++)
			{
				Color color;

				bool isValid = Mathf.Pow(x - xCenter, 2) + Mathf.Pow(y - yCenter, 2) < radius * radius;

				if (isValid)
				{
					color = Color.black;
					texture.SetPixel(x, y, color);
				}

			}
		}

	}
}


/*if (theta <= 0.01 * 2 * Mathf.PI)
{
	return Color.green;
}
else if(theta >= 0.99 * 2 * Mathf.PI)
{
	return Color.red;
}
/*else if(theta % 10 == 0 || theta % 10 == 1 || theta % 10 == 3)
{
	return Color.red;
}
else if(grey > 0.5f)
{
	return Color.black;
}
else
{
	return Color.white;
}*/
