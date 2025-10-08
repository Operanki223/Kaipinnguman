using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] TextMeshProUGUI[] words;
    [SerializeField] List<GameObject> wordsObj;
    TextMeshProUGUI[] subwords;
    [SerializeField] string[] wordArray;
    public TMP_InputField inputField;

    int[,] isPlayer3x3 = new int[3, 3];
    Vector2[,] playerPos3x3 = new Vector2[3, 3];
    string[,] wordPos3x3 = new string[3, 3];
    string inputValue;
    bool playmove = false;

    void Start()
    {
        Reset();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (inputField != null && inputField.isFocused)
            {
                OnEnterInputField();
            }
        }
    }

    void Reset()
    {
        SetArrys3x3(3, 3);
        SetPlayerPos3x3();
        RandomSetWordArray(); // 初回だけ設定
    }

    void SetArrys3x3(int x = 3, int y = 3, string width = "全部")
    {
        for (int n = 0; n < 3; n++)
            for (int m = 0; m < 3; m++)
                isPlayer3x3[m, n] = 2;

        if (x == 3 && y == 3)
            isPlayer3x3[1, 1] = 1;
        else if (x == 1 && y == 3 && width == "左")
            isPlayer3x3[0, 1] = 1;
        else if (x == 3 && y == 1 && width == "上")
            isPlayer3x3[1, 0] = 1;
    }

    void SetPlayerPos3x3(int x = 0, int y = 0)
    {
        int count = 0;
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                playerPos3x3[row, col] = wordsObj[count++].transform.position;
            }
        }
    }

    public void OnEnterInputField()
    {
        inputValue = inputField.text;
        Debug.Log("InputFieldの値: " + inputValue);

        PlayerMove();

        inputField.text = "";
        inputField.ActivateInputField();

        if (playmove)
        {
            RandomSetWordArray();
            playmove = false;
        }
    }

    void PlayerMove(float x = 0, float y = 0)
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (wordPos3x3[row, col] == inputValue)
                {
                    player.transform.position = playerPos3x3[row, col];
                    playmove = true;
                }
            }
        }
    }

    void RandomSetWordArray()
    {
        if (words == null || words.Length != 9 || wordArray == null || wordArray.Length < 9)
        {
            Debug.LogError("wordsまたはwordArrayが正しく設定されていません");
            return;
        }

        bool[] used = new bool[wordArray.Length];
        int rnd;
        int count = 0;

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                do
                {
                    rnd = Random.Range(0, wordArray.Length);
                } while (used[rnd]);

                used[rnd] = true;
                words[count++].text = wordArray[rnd];
                wordPos3x3[row, col] = wordArray[rnd];
            }
        }
    }
}
