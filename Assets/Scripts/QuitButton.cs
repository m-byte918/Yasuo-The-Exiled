using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : MonoBehaviour
{
    public Color startColor;
    public Color mouseOverColor;

    private void OnMouseOver()
    {
        this.GetComponent<MeshRenderer>().material.SetColor("_Color", mouseOverColor);

        if (Input.GetMouseButtonUp(0))
        {
            Application.Quit();
        }
    }

    private void OnMouseExit()
    {
        this.GetComponent<MeshRenderer>().material.SetColor("_Color", startColor);
    }
}
