using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{   
    public RectTransform panel;
    public Vector2 sizeHorizontal = new Vector2(988, 274);
    public Vector2 sizeVertical = new Vector2(274, 988);
    
    public Vector2 posHorizontal = new Vector2(0, 0);
    public Vector2 posVertical = new Vector2(0, 0);
    private bool isAtSide = false;

    public void TogglePosition()
    {
        if (panel == null) return;

        if (!isAtSide)
        {
            panel.anchoredPosition = posVertical;
            panel.localRotation = Quaternion.Euler(0, 0, 90);
            panel.sizeDelta = sizeVertical;

            isAtSide = true;
        }
        else
        {
            panel.anchoredPosition = posHorizontal;
            panel.localRotation = Quaternion.Euler(0, 0, 0);
            panel.sizeDelta = sizeHorizontal;

            isAtSide = false;
        }
    }
}

