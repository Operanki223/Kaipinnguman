using System.Collections;
using UnityEngine;

public class SAMPLE : MonoBehaviour
{
    public int id;
    public Vector2 startPos;
    public Vector2 finishPos;
    EnemyFlow enemyFlow;
    EnemyAction enemyAction = EnemyAction.MOVE;
    EnemyPos3x3 enemyPos3X3 = EnemyPos3x3.UP1;

    public float moveSpeed = 1.0f;
    bool isMoving = false;
    bool isInitialized = false; // ← 初期化完了フラグ

    public void Init(int id)
    {
        this.id = id;

        // GameManager.instanceがnullでないか確認
        if (GameManager.instance == null)
        {
            Debug.LogError("GameManager.instance が null です！");
            return;
        }

        // EnemiesFlowリストが存在するか確認
        if (GameManager.instance.EnemiesFlow == null || GameManager.instance.EnemiesFlow.Count <= id)
        {
            Debug.LogError($"EnemiesFlowが存在しない、またはID {id} が範囲外です");
            return;
        }

        enemyFlow = GameManager.instance.EnemiesFlow[id];
        enemyAction = enemyFlow.GetEnemyAction();
        enemyPos3X3 = enemyFlow.GetEnemyPos3X3();
        moveSpeed = enemyFlow.GetSpeed();
        startPos = transform.position;

        if (enemyAction == EnemyAction.MOVE)
        {
            switch (enemyPos3X3)
            {
                case EnemyPos3x3.UP1:
                    finishPos = enemyFlow.SetPos3x3(EnemyPos3x3.BOTTOM5);
                    break;
                case EnemyPos3x3.UP2:
                    finishPos = enemyFlow.SetPos3x3(EnemyPos3x3.BOTTOM4);
                    break;
                case EnemyPos3x3.UP3:
                    finishPos = enemyFlow.SetPos3x3(EnemyPos3x3.BOTTOM3);
                    break;
                default:
                    finishPos = transform.position;
                    break;
            }
        }

        isInitialized = true; // ← 初期化完了！
    }

    void Update()
    {
        if (!isInitialized) return; // ← まだ初期化されてなければスキップ

        if (!isMoving && enemyAction == EnemyAction.MOVE)
        {
            isMoving = true;
            StartCoroutine(MoveEnemyTo(finishPos));
        }
    }

    IEnumerator MoveEnemyTo(Vector2 targetPos)
    {
        Vector2 startPos = this.startPos;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            transform.position = Vector2.Lerp(startPos, targetPos, t);
            yield return null;
        }

        Destroy(gameObject);
    }

    public IEnumerator MoveAndWait()
    {
        // 移動開始
        yield return StartCoroutine(MoveEnemyTo(finishPos));
    }

}
