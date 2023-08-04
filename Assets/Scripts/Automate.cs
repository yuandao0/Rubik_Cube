using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditorInternal.VersionControl.ListControl;

public class Automate : MonoBehaviour
{
    public List<string> moveList = new List<string>() ;
    private readonly List<string> allMoves = new List<string>()
    {
        "U","D","L","R","F","B",
        "U2","D2","L2","R2","F2","B2",
        "U'","D'","L'","R'","F'","B'"
    };

    private ReadSurface cubeState;

    void Start()
    {
        cubeState = FindObjectOfType<ReadSurface>();
        moveList = new List<string>();
        ReadSurface.autoRotating = false;
        ReadSurface.started = false;
    }

    void Update()
    {
        if (moveList.Count > 0 && !ReadSurface.autoRotating && ReadSurface.started)
        {
            //do the move at the first index
            DoMove(moveList[0]);
            //remove the move at the index
            //moveList.Remove(moveList[0]);
            moveList.RemoveAt(0);
        }    
    }

    public void Shuffle()
    {
        List<string> moves= new List<string>();
        int shuffleLength = Random.Range(5, 10);
        for(int i = 0;i<shuffleLength;i++)
        {
            int randomMove = Random.Range(0, allMoves.Count);
            moves.Add(allMoves[randomMove]);
        }
        moveList = moves;
    }

    void DoMove(string move)
    {
        cubeState.ReadState();
        ReadSurface.autoRotating = true;
        if (move == "U")
        {
            RotateSide(cubeState.upState, -90);           
        }
        if (move == "U'")
        {
            RotateSide(cubeState.upState, 90);
        }
        if (move == "U2")
        {
            RotateSide(cubeState.upState, -180);
        }
        if (move == "D")
        {
            RotateSide(cubeState.downState, -90);           
        }
        if (move == "D'")
        {
            RotateSide(cubeState.downState, 90);
        }
        if (move == "D2")
        {
            RotateSide(cubeState.downState, -180);
        }
        if (move == "L")
        {
            RotateSide(cubeState.leftState, -90);
        }
        if (move == "L'")
        {
            RotateSide(cubeState.leftState, 90);
        }
        if (move == "L2")
        {
            RotateSide(cubeState.leftState, -180);
        }
        if (move == "R")
        {
            RotateSide(cubeState.rightState, -90);
        }
        if (move == "R'")
        {
            RotateSide(cubeState.rightState, 90);
        }
        if (move == "R2")
        {
            RotateSide(cubeState.rightState, -180);
        }
        if (move == "F")
        {
            RotateSide(cubeState.frontState, -90);
        }
        if (move == "F'")
        {
            RotateSide(cubeState.frontState, 90);
        }
        if (move == "F2")
        {
            RotateSide(cubeState.frontState, -180);
        }
        if (move == "B")
        {
            RotateSide(cubeState.backState, -90);
        }
        if (move == "B'")
        {
            RotateSide(cubeState.backState, 90);
        }
        if (move == "B2")
        {
            RotateSide(cubeState.backState, -180);
        }
    }

    void RotateSide(List<GameObject> side, float angle)
    {
        //automatically rotate the side by angle
        PivotRotation pr = side[4].transform.parent.GetComponent<PivotRotation>();
        pr.StartAutoRotate(side, angle);
    }
}


