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
    private SolveTwoPhase solveTwoPhase;

    void Start()
    {
        cubeState = FindObjectOfType<ReadSurface>();
        solveTwoPhase = FindObjectOfType<SolveTwoPhase>();
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
        switch (move)
        {
            case "U":
                RotateSide(cubeState.upState, -90);
                break;
            case "U'":
                RotateSide(cubeState.upState, 90);
                break;
            case "U2":
                RotateSide(cubeState.upState, -180);
                break;
            case "D":
                RotateSide(cubeState.downState, -90);
                break;
            case "D'":
                RotateSide(cubeState.downState, 90);
                break;
            case "D2":
                RotateSide(cubeState.downState, -180);
                break;
            case "L":
                RotateSide(cubeState.leftState, -90);
                break;
            case "L'":
                RotateSide(cubeState.leftState, 90);
                break;
            case "L2":
                RotateSide(cubeState.leftState, -180);
                break;
            case "R":
                RotateSide(cubeState.rightState, -90);
                break;
            case "R'":
                RotateSide(cubeState.rightState, 90);
                break;
            case "R2":
                RotateSide(cubeState.rightState, -180);
                break;
            case "F":
                RotateSide(cubeState.frontState, -90);
                break;
            case "F'":
                RotateSide(cubeState.frontState, 90);
                break;
            case "F2":
                RotateSide(cubeState.frontState, -180);
                break;
            case "B":
                RotateSide(cubeState.backState, -90);
                break;
            case "B'":
                RotateSide(cubeState.backState, 90);
                break;
            case "B2":
                RotateSide(cubeState.backState, -180);
                break;
        }
    }

    void RotateSide(List<GameObject> side, float angle)
    {
        //automatically rotate the side by angle
        PivotRotation pr = side[4].transform.parent.GetComponent<PivotRotation>();
        pr.StartAutoRotate(side, angle);
    }
}


