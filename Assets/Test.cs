using TMPro;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] TextMeshProUGUI[] words;
    [SerializeField] string[] wordArray;
    Vector2[,] playerPos3x3 = new Vector2[3, 3];
    public TMP_InputField inputField;
    string inputValue;
    string[,] wordPos3x3 = new string[3, 3];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RandomSetWordArray();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnEnterInputField()
    {
        inputValue = inputField.text;
        Debug.Log("InputFieldの値: " + inputValue);

        PlayerMove();

        inputField.text = "";
        inputField.ActivateInputField();
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

        int count = 0;

        bool[] used = new bool[wordArray.Length];
        int rnd;

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                do
                {
                    rnd = Random.Range(0, wordArray.Length);
                } while (used[rnd]);

                used[rnd] = true;
                words[count].text = wordArray[rnd];
                count++;
            }
        }
    }
}
