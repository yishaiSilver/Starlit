using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSystemManager : MonoBehaviour {

	public StarSystemList systemList;

	public void TransferSystems(GameObject jumpingObject)
    {
        SetLayerOfObject(jumpingObject.transform, LayerMask.NameToLayer("Proxima"));
    }

    public void SetLayerOfObject(Transform obj, int layer)
    {
        obj.gameObject.layer = layer;

        for(int i = 0; i < obj.childCount; i++)
        {
            SetLayerOfObject(obj.GetChild(i), layer);
        }
    }
}
