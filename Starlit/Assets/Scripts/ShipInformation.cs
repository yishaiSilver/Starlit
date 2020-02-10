using UnityEngine;

public static class ShipInformation {

    public static float rotation = 0;
    public static bool jumpped = false;
    public static Vector2 velocity;
    public static float standardScale = 0.2664156f;

    public static bool hasTurret = false;

    public static Transform ship;
    public static AsteroidSpawner spawner;

    public static bool hitAsteroid;

    public static void setRotation(float rot)
    {
        rotation = rot;
    }

    public static float getRotation()
    {
        return rotation;
    }
}
