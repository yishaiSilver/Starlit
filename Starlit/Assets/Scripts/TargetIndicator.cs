using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIndicator : MonoBehaviour {

    public Transform target;

    public float hideDistance;
    public float maxDistance;
    public float minDistance;

    public float size;

    public bool hidden = false;

	// Use this for initialization
	void Start () {
        setChildrenActive(hidden);
	}

    // Update is called once per frame
    void Update()
    {
        try
        {
            var dir = target.position - transform.position;

            if (dir.magnitude < hideDistance || hidden)
            {
                setChildrenActive(false);
            }
            else
            {
                setChildrenActive(true);

                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }

            if (dir.magnitude > maxDistance && dir.magnitude < minDistance)
            {
                foreach (Transform child in transform)
                {
                    child.localScale = new Vector3((maxDistance) / dir.magnitude * size, (maxDistance) / dir.magnitude * size);
                }

            }
            else if (dir.magnitude > minDistance)
            {
                foreach (Transform child in transform)
                {
                    child.localScale = new Vector3(maxDistance / minDistance * size, maxDistance / minDistance * size);
                }
            }
        }
        catch
        {

        }
    }

    // Hide when too close
    void setChildrenActive(bool val)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(val);
        }
    }

    public void setHidden(bool val)
    {
        hidden = val;
        Debug.Log("hidden - update");
    }
}
