using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] GameObject titleText;

    [Header("要素順に敵が出現"), SerializeField] public List<EnemyFlow> EnemiesFlow = new List<EnemyFlow>();
    [Header("敵を入れる"), SerializeField] List<EnemyList> enemyList = new List<EnemyList>();
    [SerializeField] Transform enemtParent;
    [SerializeField] GameObject player;
    [SerializeField] float moveSpeed = 1.0f;
    [SerializeField] TextMeshProUGUI[] words;
    [SerializeField] public List<GameObject> wordsObj;
    [SerializeField] string[] wordArray;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] GameObject backGround;
    public Transform backGroundParent;
    public float backGround_spownposX = 0.0f;
    public float backGround_spownposY = 0.0f;
    public float backGround_spownTime = 1.0f;
    public float BackGround_destroyTime = 10f;

    int[,] isPlayer3x3 = new int[3, 3];//プレーヤーの位置の記録
    Vector2[,] playerPos3x3 = new Vector2[3, 3];
    string[,] wordPos3x3 = new string[3, 3];
    string inputValue;
    bool playmove = false;
    bool game_play = true;
    Vector3 secondPos;

    public Vector3 GetSecondPos()
    {
        return secondPos;
    }

    void Start()
    {
        titleText.SetActive(true);
        instance = this;
        Reset();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) && game_play)
        {
            if (inputField != null && inputField.isFocused)
            {
                OnEnterInputField();
            }
        }
        if (game_play)
        {
            game_play = false;
            StartCoroutine(PlayGame());     // ← PlayGameを開始
        }
    }

    void Reset()
    {
        game_play = true;
        SetArrys3x3(3, 3);
        SetPlayerPos3x3();
        RandomSetWordArray(); // 初回だけ設定
    }

    void SetArrys3x3(int x = 3, int y = 3, string width = "全部")
    {
        for (int n = 0; n < 3; n++)
        {
            for (int m = 0; m < 3; m++)
            {
                isPlayer3x3[m, n] = 2;
            }
        }

        if (x == 3 && y == 3)
            isPlayer3x3[1, 1] = 1;
        else if (x == 1 && y == 3 && width == "左")
            isPlayer3x3[0, 1] = 1;
        else if (x == 3 && y == 1 && width == "上")
            isPlayer3x3[1, 0] = 1;
    }


    /// <summary>
    /// 座標を入れる
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
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
        int count = 0;

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (wordPos3x3[row, col] == inputValue)
                {
                    Vector2 targetPos = playerPos3x3[row, col];
                    WordsSetActive();

                    // 移動先の文字を非表示にする
                    words[count].gameObject.SetActive(false);

                    StopAllCoroutines(); // 前回の移動を止める
                    StartCoroutine(MovePlayerTo(targetPos));
                    playmove = true;
                    return;
                }
                count++;
            }
        }
    }

    void WordsSetActive(bool b = true, string width = null)
    {
        foreach (var word in words)
        {
            word.gameObject.SetActive(b);
        }

        if (width == "3x3") words[4].gameObject.SetActive(false);
    }


    // プレイヤーをなめらかに目的地へ移動
    IEnumerator MovePlayerTo(Vector2 targetPos)
    {
        Vector2 startPos = player.transform.position;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            player.transform.position = Vector2.Lerp(startPos, targetPos, t);
            yield return null;
        }

        player.transform.position = targetPos; // 最後に正確な位置へ
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

    public IEnumerator PlayGame()
    {
        // タイトルアニメーションが終わるまで待つ
        yield return StartCoroutine(TitleAnimation());
        WordsSetActive(true, "3x3");

        for (int i = 0; i < EnemiesFlow.Count; i++)
        {
            EnemyFlow nowEnemy = EnemiesFlow[i];
            EnemyFlow nextEnemy;
            if (i + 1 < EnemiesFlow.Count)
            {
                // 次の敵情報が存在する場合 
                nextEnemy = EnemiesFlow[i + 1];
            }
            else
            {
                // 最後の要素の場合（次がない） 
                nextEnemy = null;
            }

            nowEnemy.SetPos3x3(nowEnemy.GetEnemyPos3X3());

            foreach (var enemy in enemyList)
            {
                if (enemy.GetEnemyName().Equals(nowEnemy.GetEnemyName()))
                {
                    // 敵オブジェクト生成
                    GameObject newEnemy = Instantiate(enemy.GetEnemyObj(), nowEnemy.GetPos(), Quaternion.identity, enemtParent);
                    Debug.Log($"{enemy.GetEnemyName()}を召喚 (ID: {i})");

                    // SAMPLEスクリプトを取得して初期化
                    SAMPLE sample = newEnemy.GetComponent<SAMPLE>();
                    if (sample != null)
                    {
                        sample.Init(i);

                        // ★次の敵が存在して、SpawnTime が異なる場合のみ移動完了を待つ
                        if (nextEnemy == null || nowEnemy.GetSpawnTime() != nextEnemy.GetSpawnTime())
                        {
                            yield return StartCoroutine(sample.MoveAndWait());
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"{enemy.GetEnemyName()} に SAMPLE スクリプトが付いていません");
                    }
                }
            }
        }

        Debug.Log("ゲーム終了");
    }


    public float WaiteTiem(Vector2 startPos, Vector2 targetPos, float speed)
    {
        if (speed <= 0f)
        {
            Debug.LogWarning("速度が0以下です。移動時間を計算できません。");
            return 0f;
        }

        // 2点間の距離を計算
        float distance = Vector2.Distance(startPos, targetPos);

        // 移動にかかる時間 = 距離 ÷ 速度
        float waiteTime = distance / speed;
        Debug.Log($"{startPos}{targetPos}{distance}{waiteTime}");
        return waiteTime;
    }

    /// <summary>
    /// ゲーム開始時のアニメーション
    /// </summary>
    IEnumerator TitleAnimation()
    {
        WordsSetActive(false);

        RectTransform rect = titleText.GetComponent<RectTransform>();

        // 画面幅を取得（CanvasがScreen Space Overlayの場合）
        float screenWidth = Screen.width;

        // スタート位置（画面右外）と終了位置（画面左外）
        Vector2 startPos = new Vector2(screenWidth + 400f, rect.anchoredPosition.y);
        Vector2 centerPos = new Vector2(0f, rect.anchoredPosition.y);
        Vector2 endPos = new Vector2(-screenWidth - 400f, rect.anchoredPosition.y);

        // 初期位置を右外に設定
        rect.anchoredPosition = startPos;
        rect.localScale = Vector3.one * 0.8f;

        // 透明度用CanvasGroupを設定（なければ追加）
        CanvasGroup cg = titleText.GetComponent<CanvasGroup>();
        if (cg == null) cg = titleText.AddComponent<CanvasGroup>();
        cg.alpha = 0f;

        // DOTweenシーケンス作成
        Sequence seq = DOTween.Sequence();

        // ✅ 1. フェードインしながら右 → 中央へ移動
        seq.Append(cg.DOFade(1f, 0.8f)); // フェードイン
        seq.Join(rect.DOAnchorPosX(centerPos.x, 2.5f).SetEase(Ease.OutQuad));

        // ✅ 2. 中央で1秒停止
        seq.AppendInterval(1f);

        // ✅ 3. 左へ移動しながらフェードアウト
        seq.Append(rect.DOAnchorPosX(endPos.x, 2.5f).SetEase(Ease.InQuad));
        seq.Join(cg.DOFade(0f, 1.0f));

        // ✅ 4. 完了待機
        yield return seq.WaitForCompletion();

        Debug.Log("タイトルアニメーション完了！");
    }

}
