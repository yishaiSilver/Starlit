using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneInformation: MonoBehaviour
{
    public SystemInformation systemInformation;

    public TargetIndicator hiddenTarget;
    public TargetIndicator shownTarget;

    public GameObject surface;
    public bool surfaceEnabled = false;

    public Transform jumpTarget;

    private void Start()
    {
        StoryController.hiddenTarget = this.hiddenTarget;
        if(SceneManager.GetActiveScene().name == "5SaturnScene")
            surface.SetActive(false);
        if(gameObject.name == "MFalcon")
        {
            StoryController.hiddenTarget = this.hiddenTarget;
            surface.SetActive(true);
        }
    }

    public void hideAllInidicators()
    {
        hiddenTarget.setHidden(true);
        shownTarget.setHidden(true);
    }

    public void toggleSurface()
    {
        surfaceEnabled = !surfaceEnabled;
        surface.SetActive(surfaceEnabled);
    }
}
