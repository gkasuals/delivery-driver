using UnityEngine;

public class Operator : MonoBehaviour
{
    [SerializeField]
    public void Start()
    {
        ex2();
        int level = 5;
        bool hasSpecialItem = true;
        bool isInBattle = true;
        if (level > 5)
        {
            if (hasSpecialItem == true && isInBattle == true)
                Debug.Log("아이템 사용 가능");
        }
        else Debug.Log("아이템 사용 불가");
    }

    private static void ex2()
    {
        int mathscore = 85;
        int englishscore = 95;

        if ((englishscore > 60) && (mathscore > 60))
        {
            int average = (englishscore + mathscore) / 2;
            if (average > 90)
                Debug.Log("우수합격");
            else
                Debug.Log("일반합격");
        }
        else
        {
            Debug.Log("불합격");
        }
    }
}