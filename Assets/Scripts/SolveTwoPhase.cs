using Kociemba;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SolveTwoPhase : MonoBehaviour
{
     ReadSurface ReadSurface;
     Automate automate;

    public bool solved=false;
    void Start()
    {
        ReadSurface = FindObjectOfType<ReadSurface>();
        automate = FindObjectOfType<Automate>();
    }
    public void Solver()
    {
        if (!solved)
            StartCoroutine(SolveCoroutine());
    }

    IEnumerator SolveCoroutine() //用协程来保证完整执行完毕
    {
        if(!automate.shuffled)
        {
            solved= true;
            ReadSurface.ReadState();

            // 获取魔方状态的字符串表示
            string moveString = ReadSurface.GetStateString();

            // 求解魔方
            string info = "";
            string solution = Search.solution(moveString, out info);

            // 将解决的步骤从字符串转换为列表
            List<string> solutionList = StringToList(solution);

            // 自动执行步骤列表
            automate.moveList = solutionList;

            print(info);
        }
        yield return new WaitForSeconds(6f);
        solved = false;

    }

    List<string> StringToList(string solution)
    {
        //List<string> solutionList = new List<string>(solution.Split(new string[] {""},System.StringSplitOptions.RemoveEmptyEntries));
        List<string> solutionList = new List<string>(solution.Split(' ', System.StringSplitOptions.RemoveEmptyEntries));
        return solutionList;
    }
}
