using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetBubble {

	private Texture2D texture;
	private float height;
	private float xCenter;
	private float yCenter;

	private float width;

	private float xSpeed;

	private Color bubbleColor;

	public PlanetBubble(Texture2D texture, float xCenter, float yCenter, float width, float height, float xSpeed, Color bubbleColor)
	{
		this.texture = texture;
		this.xCenter = xCenter;
		this.yCenter = yCenter;
		this.height = height;
		this.width = (width < height) ? height : width;

		this.xSpeed = xSpeed;
		this.bubbleColor = bubbleColor;
	}
	
	public void draw()
	{
		float leftPointX = xCenter - (width/ 2f) + (height / 2f);
		float rightPointX = xCenter + (width / 2f) - (height / 2f);

		//Debug.Log("(" + xCenter + ", " + yCenter + ")");

		xCenter += xSpeed * Time.deltaTime;

		//drawRect(new Color(1, 0, 0, 0.3f), (int)(xCenter - (width / 2f)), (int)(xCenter + (width / 2f)));
		drawCircle(bubbleColor, height / 2f, leftPointX, yCenter, true);
		drawCircle(bubbleColor, height / 2f, rightPointX, yCenter, false);

		drawRect(bubbleColor, (int)leftPointX, (int)rightPointX);

		xCenter += texture.width;
		yCenter += texture.height;
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

	public bool isOverlapping(PlanetBubble bubble)
	{ // change xCenter, yCenter by factors of -1, 0, 1

		int lx = bubble.getLeftX();
		int rx = bubble.getRightX();
		int ty = bubble.getTopY();
		int by = bubble.getBottomY();

		float tlx = getLeftX() - texture.width;
		float trx = getRightX() - texture.width;

		for (int x = 0; x < 2; x++)
		{
			float tby = yCenter - (height / 2f) - texture.height;
			float tty = yCenter + (height / 2f) - texture.height;

			for (int y = 0; y < 2; y++)
			{
				bool xContainsLeft = (tlx < lx && lx < trx);
				bool xContainsRight = (tlx < rx && rx < trx);

				bool yContainsTop = (tby < ty && ty < tty);
				bool yContainsBottom = (tby < by && by < tty);

				bool xBounded = xContainsLeft || xContainsRight;
				bool yBounded = yContainsTop || yContainsBottom;

				if (xBounded )
				{
					return true;
				}

				tby += texture.height;
				tty += texture.height;
			}

			tlx += texture.width;
			trx += texture.width;
		}

		return false;
	}

	public bool isXBounded(PlanetBubble bubble)
	{

		return true;
	}

	public void drawCornerStatus(bool isValid, int x, int y)
	{
		if (isValid)
		{
			drawCircle(new Color(0, 1, 0), 5, x, y, true);
			drawCircle(new Color(0, 1, 0), 5, x, y, false);
		}
		else
		{
			drawCircle(new Color(1, 0, 0), 5, x, y, true);
			drawCircle(new Color(1, 0, 0), 5, x, y, false);
		}
	}

	public int getLeftX()
	{
		return (int)(xCenter - ((width) / 2f));
	}

	public int getRightX()
	{
		return (int)(xCenter + ((width) / 2f));
	}

	public int getTopY()
	{
		return (int)(yCenter + (height / 2f));
	}

	public int getBottomY()
	{
		return (int)(yCenter - (height / 2f));
	}
}
