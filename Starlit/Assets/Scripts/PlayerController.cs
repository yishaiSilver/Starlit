using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public Ship personalShip;

    public StarSystemManager starSystemManager;

    private GameObject canvas;
    private HUDManager hudManager;

    public CameraController cam;
    public Map map;

    private bool controlsEnabled = true;

    public void Start()
    {
        personalShip.setPlayerController(this);
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        hudManager = canvas.GetComponent<HUDManager>();
        map.setDefaultShip(personalShip);
    }

    public void JumpToSystem(int layerInt)
    {
        cam.setCameraLayer(layerInt);
    }

    public void FixedUpdate()
    {
        transform.position = personalShip.transform.position;
        if (controlsEnabled)
        {
            if (!personalShip.Manual())
            {
                personalShip.Automatic();
            }
            else
            {
                personalShip.StopAutomatic();
                SetTarget("");
            }
        }
        else
        {
            personalShip.MoveTowards();
        }
    }

    public void notifyOfTakeoff()
    {
        hudManager.Reset();
    }

    public void notifyOfLand(GameObject ship, LandableObject target)
    {
        hudManager.Land(ship, target);
    }

    public void SetTarget(GameObject target)
    {
        GameObject targetBox = canvas.transform.Find("HUD").Find("Target").gameObject;
        Text text = targetBox.GetComponent<Text>();
        text.text = target.name.ToUpper();
    }

    public void SetTarget(string str)
    {
        GameObject targetBox = canvas.transform.Find("HUD").Find("Target").gameObject;
        Text text = targetBox.GetComponent<Text>();
        text.text = str.ToUpper();
    }

    public void disableControls()
    {
        controlsEnabled = false;
    }

    public void enableControls()
    {
        controlsEnabled = true;
    }

    public void toggleMap()
    {
        map.loadDirections();
        hudManager.toggleMap(personalShip);
    }
}