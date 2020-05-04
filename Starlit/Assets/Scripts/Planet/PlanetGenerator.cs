using System.Collections;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour
{

	public float radius;
	public float height;
	public float width;

	public float xCenter;
	public float yCenter;
	public float bubbleWidth;
	public float bubbleHeight;

	private SpriteRenderer rend;
	private Sprite sprite;

	private float xOffset = 0;
	public float xSpeed;

	private ArrayList bubbles;


	private Texture2D texture;

	private Texture2D textureBase;

	private Texture2D textureOne;
	private Texture2D textureTwo;
	private Texture2D textureThree;


	public bool isPreloaded;

	public Color baseColor;
	public Color layerOneColor;
	public Color layerTwoColor;
	public Color layerThreeColor;

	public int layerOneSpeed;
	public int layerTwoSpeed;
	public int layerThreeSpeed;

	[SerializeField]
	public PlanetBubble[] bubblesLayerOne;

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

		for (int i = 0; i < 7; i ++)
		{
			spawnBubble(texture, 40, 80, true, 1);
		}
		for (int i = 0; i < 7; i++)
		{
			spawnBubble(texture, 40, 80, true, 2);
		}
		for (int i = 0; i < 7; i++)
		{
			spawnBubble(texture, 40, 80, true, 3);
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

		//PlanetBubble planetBubble = new PlanetBubble(texture, xCenter + xOffset, yCenter, bubbleWidth, bubbleHeight);


		makeBase(texture);

		foreach (PlanetBubble bubble in bubbles)
		{
			bubble.draw();
		}

		texture.Apply();

		return texture;
	}

	private void clearSprite(Texture2D texture)
	{
		for (int x = 0; x < texture.width; x++)
		{
			for (int y = 0; y < texture.height; y++)
			{
				texture.SetPixel(x, y, new Color(0, 0, 0, 0));
			}
		}
	}


	private void makeBase(Texture2D texture)
	{
		for (int x = 0; x < texture.width; x++)
		{
			for (int y = 0; y < texture.height; y++)
			{
				texture.SetPixel(x, y, baseColor);
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

	private void spawnBubble(Texture2D texture, float minHeight, float maxHeight, bool allowOverlap, int layer)
	{
		int numTries = 0;

		while (true)
		{
			bool shouldRedo = false;

			float xCenter = Random.Range(0, width);
			float yCenter = Random.Range(0, height);

			float bubbleHeight = Random.Range(minHeight, maxHeight);
			//float bubbleWidth = 0;
			float bubbleWidth = Random.Range(bubbleHeight * 1.5f, bubbleHeight * 3f);
		
			PlanetBubble planetBubble = new PlanetBubble(texture, xCenter, yCenter, bubbleWidth, bubbleHeight, layerOneSpeed, layerOneColor);

			switch (layer) {
				case 1:
					planetBubble = new PlanetBubble(texture, xCenter, yCenter, bubbleWidth, bubbleHeight, layerOneSpeed, layerOneColor);
					break;
				case 2:
					planetBubble = new PlanetBubble(texture, xCenter, yCenter, bubbleWidth, bubbleHeight, layerTwoSpeed, layerTwoColor);
					break;
				case 3:
					planetBubble = new PlanetBubble(texture, xCenter, yCenter, bubbleWidth, bubbleHeight, layerThreeSpeed, layerThreeColor);
					break;
				default:
					break;
			}

			if (!allowOverlap)
			{
				foreach (PlanetBubble bubble in bubbles)
				{
					if (bubble.isOverlapping(planetBubble) || planetBubble.isOverlapping(bubble))
					{
						shouldRedo = true;
						break;
					}
				}

				if (shouldRedo)
				{
					planetBubble.draw();
					numTries++;
					Debug.Log("Overlap detected");
					continue;
				}
				else
				{
					bubbles.Add(planetBubble);
					break;
				}
			}
			else
			{
				bubbles.Add(planetBubble);
				break;
			}
		}
	}
}
