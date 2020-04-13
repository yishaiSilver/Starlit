using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise3D : MonoBehaviour {

	public float spriteWidth;
	public float spriteHeight;

	public float width;
	public float height;
	public float yScale = 1;
	public float xScale = 1;

	public float radius = 10;

	public float rotSpeed = 1;
	private float offset = 0;

	public bool is3d;

	void Start()
	{
		radius = width / (2 * Mathf.PI);
	}

	void Update () {
		SpriteRenderer rend = GetComponent<SpriteRenderer>();
		//rend.material.mainTexture = GenerateTexture();
		rend.sprite = Sprite.Create(GenerateTexture(), new Rect(0.0f, 0.0f, spriteWidth /  2, spriteHeight), new Vector2(0.5f, 0.5f), 100.0f);

		offset = (offset + rotSpeed * Time.deltaTime) % (2 * Mathf.PI);
	}
	
	private Texture2D GenerateTexture()
	{
		Texture2D texture = new Texture2D((int)width, (int)height);
		
		
		for(int x = 0; x < width; x++)
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
		}

		texture.Apply();

		return texture;
	}

	private Color CalculateColor(float x, float y)
	{
		x = x / width * xScale;
		y = y / width * yScale;

		float grey = Mathf.PerlinNoise(x, y);

		return new Color(grey, grey, grey);
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
