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

    IEnumerator SolveCoroutine() //��Э������֤����ִ�����
    {
        if(!automate.shuffled)
        {
            solved= true;
            ReadSurface.ReadState();

            // ��ȡħ��״̬���ַ�����ʾ
            string moveString = ReadSurface.GetStateString();

            // ���ħ��
            string info = "";
            string solution = Search.solution(moveString, out info);

            // ������Ĳ�����ַ���ת��Ϊ�б�
            List<string> solutionList = StringToList(solution);

            // �Զ�ִ�в����б�
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
