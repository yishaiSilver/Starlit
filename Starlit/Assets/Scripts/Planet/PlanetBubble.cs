using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetBubble {

	private Texture2D texture;
	private float height;
	private float xCenter;
	private float yCenter;

	private float width;

	public PlanetBubble(Texture2D texture, float xCenter, float yCenter, float width, float height)
	{
		this.texture = texture;
		this.xCenter = xCenter;
		this.yCenter = yCenter;
		this.height = height;
		this.width = (width < height) ? height : width;
	}
	
	public void draw(Color color)
	{
		float leftPointX = xCenter - (width/ 2f) + (height / 2f);
		float rightPointX = xCenter + (width / 2f) - (height / 2f);

		drawCircle(color, height / 2f, leftPointX, yCenter, true);
		drawCircle(color, height / 2f, rightPointX, yCenter, false);

		drawRect(color, (int)leftPointX, (int)rightPointX);
	}

	public void drawRect(Color color, int startX, int endX)
	{
		int startY = (int)(yCenter - (height / 2f));
		int endY = (int)(yCenter + (height / 2f));

		for (int x = startX; x <= endX && x >= startX; x++)
		{
			for (int y = startY; y <= endY && y >= startY; y++)
			{
				texture.SetPixel(x, y, color);
			}
		}
	}

	public void drawCircle(Color color, float radius, float xCenter, float yCenter, bool isLeftSide)
	{
		for (int x = (int)(xCenter - radius); x <= (int)(xCenter + radius); x++)
		{
			if (isLeftSide && x >= xCenter)
				continue;
			else if (!isLeftSide && x <= xCenter)
				continue;

			for (int y = (int)(yCenter - radius); y <= (int)(yCenter + radius); y++)
			{
				bool isValid = Mathf.Pow(x - xCenter, 2) + Mathf.Pow(y - yCenter, 2) < radius * radius;

				if (isValid)
				{
					texture.SetPixel(x, y, color);
				}

			}
		}

	}

	public bool containsPoint(int x, int y)
	{
		float lx = (xCenter - (width / 2f)) % width;
		float rx = (xCenter + (width / 2f)) % width;

		float by = (yCenter - (height / 2f)) % height;
		float ty = (yCenter + (height / 2f)) % height;

		if (lx < 0)
			lx *= -1;
		if (rx < 0)
			rx *= -1;
		if (by < 0)
			by *= -1;
		if (ty < 0)
			ty *= -1;

		bool xBounded = x > lx && x < rx;
		bool yBounded = y > by && y < ty;

		Debug.Log(xBounded && yBounded);
		return xBounded && yBounded;
	}
}
