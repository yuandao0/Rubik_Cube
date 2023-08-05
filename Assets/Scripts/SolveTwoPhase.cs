using Kociemba;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolveTwoPhase : MonoBehaviour
{
     ReadSurface ReadSurface;
     Automate automate;
     public bool doOnce = true;

    void Start()
    {
        ReadSurface = FindObjectOfType<ReadSurface>();
        automate = FindObjectOfType<Automate>();
    }

    void Update()
    {
        if(ReadSurface.started && doOnce )
        {
            doOnce = false;
            Solver();
        }
    }

    public void Solver()
    {
        ReadSurface.ReadState();

        //get the state of the cubr as a string
        string moveString = ReadSurface.GetStateString();

        //solve the cube
        string info = "";
        //first time build the tables
        //string solution = SearchRunTime.solution(moveString, out info, buildTables: true);

        //every other time
        string solution = Search.solution(moveString, out info);

        //convert the solved moves from a string to a list
        List<string > solutionList = StringToList(solution);

        //Automate the list
        automate.moveList = solutionList;
        print("Solution: " + solution);
        print("Moves Count: " + solutionList.Count);
        print(info);
    }

    List<string> StringToList(string solution)
    {
        List<string> solutionList = new List<string>(solution.Split(new string[] {""},System.StringSplitOptions.RemoveEmptyEntries));

        return solutionList;
    }
}
