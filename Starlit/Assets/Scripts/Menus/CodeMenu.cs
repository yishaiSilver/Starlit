using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using TMPro;

public class CodeMenu : MonoBehaviour {

    public HUDManager hudManager;

    private GameObject codePanel;
    public Text title;
    //public InGameCodeEditor.CodeEditor codeEditor;

    public Ship ship;
    private GameObject parentMenu;

    void Start () {
        codePanel = transform.Find("CodePanel").gameObject;
    }

    public void Enable(Ship ship, GameObject parent)
    {
        this.ship = ship;
        ship.playerController.disableControls();
        parentMenu = parent;

        title.text = ship.name;
        
        if (!ship.shipCode.Equals(""))
        {
            //codeEditor.Text = ship.shipCode;
        }
    }

    public void Disable()
    {
        ship.playerController.enableControls();
        //ship.shipCode = codeEditor.Text;
        hudManager.closeSubMenu(gameObject, parentMenu);
    }

    public string GetCode()
    {
        return "";// codeEditor.Text;
    }

    public void UpdateSource()
    {
        //bool success = ship.UpdateCode(codeEditor.InputField.text);

        //if (success)
        //{
        //    codePanel.transform.Find("PanelSuccess").gameObject.SetActive(true);
        //}
    }

    public void GiveFocus()
    {
        //codeEditor.InputField.ActivateInputField();
    }
}
