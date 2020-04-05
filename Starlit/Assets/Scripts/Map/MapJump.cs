using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapJump {

	private StarSystem targetStar;
	private float angle;

	public MapJump(StarSystem targetStar, float angle)
	{
		this.targetStar = targetStar;
		this.angle = angle;
	}

	public StarSystem getTargetStar()
	{
		return targetStar;
	}

	public float getAngle()
	{
		return angle;
	}
}
