using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeMap : MonoBehaviour
{

    public Transform up;
    public Transform down;
    public Transform left;
    public Transform right;
    public Transform front;
    public Transform back;
    private ReadSurface cubeState;

    public void SetColor()
    {
        cubeState = FindObjectOfType<ReadSurface>();

        UpdateMap(cubeState.upState,up);
        UpdateMap(cubeState.downState, down);
        UpdateMap(cubeState.leftState, left);
        UpdateMap(cubeState.rightState, right);
        UpdateMap(cubeState.frontState, front);
        UpdateMap(cubeState.backState, back);
    } 

    private void UpdateMap(List<GameObject> cuebState, Transform maps)
    {
        int i = 0;
        foreach (Transform map in maps)
        {
            if (cuebState[i].name[0] == 'f')
            {
                map.GetComponent<Image>().color = Color.red;
            }
            else if (cuebState[i].name[0] == 'b')
            {
                map.GetComponent<Image>().color = new Color(241, 255, 0);
            }
            else if (cuebState[i].name[0] == 'l')
            {
                map.GetComponent<Image>().color = Color.white;
            }
            else if (cuebState[i].name[0] == 'r')
            {
                map.GetComponent<Image>().color = Color.green;
            }
            else if (cuebState[i].name[0] == 'u')
            {
                map.GetComponent<Image>().color = new Color(0,41,255);
            }
            else if (cuebState[i].name[0] == 'd')
            {
                map.GetComponent<Image>().color = new Color(255, 0, 235);
            }
            i++;
        }
    }
}
