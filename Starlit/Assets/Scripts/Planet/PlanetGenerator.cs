using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour {

	public float radius;
	public float height;
	public float width;

	public float xCenter;
	public float yCenter;
	public float bubbleWidth;
	public float bubbleHeight;

	private SpriteRenderer rend;
	private Sprite sprite;

	private float  xOffset = 0;
	public float xSpeed;

	private ArrayList bubbles;


	Texture2D texture;

	void Start()
	{
		rend = GetComponent<SpriteRenderer>();
		width = height * Mathf.PI;

		texture = new Texture2D((int)width, (int)height);

		GameObject planetMask = GameObject.Find("PlanetMask");
		SpriteMask spriteMask = planetMask.GetComponent<SpriteMask>();

		Texture2D maskTexture = new Texture2D((int)height, (int)height);
		makeClearCircle(maskTexture);
		maskTexture.Apply();

		Sprite maskSprite = Sprite.Create(maskTexture, new Rect(0.0f, 0.0f, maskTexture.width, maskTexture.height), new Vector2(0.5f, 0.5f), 100.0f);

		spriteMask.sprite = maskSprite;

		bubbles = new ArrayList();
		for (int i = 0; i < 7; i++)
		{
			spawnBubble(texture, 40, 50, false);
		}
	}

	void Update()
	{
		xOffset = (xOffset + xSpeed * Time.deltaTime) % width;
		Texture2D texture = GenerateTexture();
		sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
		rend.sprite = sprite;
	}

	private Texture2D GenerateTexture()
	{

		//drawCircle(texture, radius, xCenter, yCenter, isLeft);
		//clearSprite(texture);

		PlanetBubble planetBubble = new PlanetBubble(texture, xCenter + xOffset, yCenter, bubbleWidth, bubbleHeight);
		foreach (PlanetBubble bubble in bubbles)
		{
			bubble.draw(new Color(0, 255, 0, 0.5f));
		}

		texture.Apply();

		return texture;
	}

	private void clearSprite(Texture2D texture)
	{
		for(int x = 0; x < texture.width; x++)
		{
			for(int y = 0; y < texture.height; y++)
			{
				texture.SetPixel(x, y, new Color(0, 0, 0, 0));
			}
		}
	}

	private void makeClearCircle(Texture2D texture)
	{
		float maskCenter = texture.width / 2f;
		float maskRadius = texture.width / 2f;

		for (int x = 0; x < texture.width; x++)
		{
			for (int y = 0; y < texture.height; y++)
			{
				bool isValid = Mathf.Pow(x - maskCenter, 2) + Mathf.Pow(y - maskCenter, 2) > maskRadius * maskRadius;

				if (isValid)
				{
					texture.SetPixel(x, y, new Color(0, 0, 0, 0));
				}

			}
		}
	}

	private void spawnBubble(Texture2D texture, float minHeight, float maxHeight, bool allowOverlap)
	{
		int numTries = 0;

		while (numTries < 5)
		{
			float xCenter = Random.Range(0, width);
			float yCenter = Random.Range(0, height);

			float bubbleHeight = Random.Range(minHeight, maxHeight);
			//float bubbleWidth = 0;
			float bubbleWidth = Random.Range(bubbleHeight * 1.5f, bubbleHeight * 3f);

			if (!allowOverlap)
			{
				Debug.Log("new iteration");
				bool shouldRedo = false;

				int tlx = (int)(xCenter - bubbleWidth / 2f);
				int tly = (int)(yCenter + bubbleHeight / 2f);

				int blx = (int)(xCenter - bubbleWidth / 2f);
				int bly = (int)(yCenter - bubbleHeight / 2f);

				int trx = (int)(xCenter + bubbleWidth / 2f);
				int ytr = (int)(yCenter + bubbleHeight / 2f); // try

				int brx = (int)(xCenter + bubbleWidth / 2f);
				int bry = (int)(yCenter - bubbleHeight / 2f);

				foreach (PlanetBubble bubble in bubbles)
				{
					if(bubble.containsPoint(tlx, tly) || bubble.containsPoint(blx, bly) || bubble.containsPoint(trx, ytr) || bubble.containsPoint(brx, bry)){
						shouldRedo = true;
						break;
					}
				}
				
				if (shouldRedo)
				{
					numTries++;
					Debug.Log("Overlap detected");
				}
				else
				{
					PlanetBubble planetBubble = new PlanetBubble(texture, xCenter, yCenter, bubbleWidth, bubbleHeight);
					bubbles.Add(planetBubble);
					Debug.Log("Added bubble");
					break;
				}
			}
			else
			{
				PlanetBubble planetBubble = new PlanetBubble(texture, xCenter, yCenter, bubbleWidth, bubbleHeight);
				bubbles.Add(planetBubble);
				break;
			}
		}
	}
}
