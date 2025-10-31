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

        // 行動ごとの設定
        switch (enemyAction)
        {
            case EnemyAction.MOVE:
                // 移動アクションの場合、行先を設定
                switch (enemyPos3X3)
                {
                    // 🔽 上段 → 下段
                    case EnemyPos3x3.UP1: finishPos = enemyFlow.SetPos3x3(EnemyPos3x3.BOTTOM5); break;
                    case EnemyPos3x3.UP2: finishPos = enemyFlow.SetPos3x3(EnemyPos3x3.BOTTOM4); break;
                    case EnemyPos3x3.UP3: finishPos = enemyFlow.SetPos3x3(EnemyPos3x3.BOTTOM3); break;
                    case EnemyPos3x3.UP4: finishPos = enemyFlow.SetPos3x3(EnemyPos3x3.BOTTOM2); break;
                    case EnemyPos3x3.UP5: finishPos = enemyFlow.SetPos3x3(EnemyPos3x3.BOTTOM1); break;

                    // 🔼 下段 → 上段
                    case EnemyPos3x3.BOTTOM1: finishPos = enemyFlow.SetPos3x3(EnemyPos3x3.UP5); break;
                    case EnemyPos3x3.BOTTOM2: finishPos = enemyFlow.SetPos3x3(EnemyPos3x3.UP4); break;
                    case EnemyPos3x3.BOTTOM3: finishPos = enemyFlow.SetPos3x3(EnemyPos3x3.UP3); break;
                    case EnemyPos3x3.BOTTOM4: finishPos = enemyFlow.SetPos3x3(EnemyPos3x3.UP2); break;
                    case EnemyPos3x3.BOTTOM5: finishPos = enemyFlow.SetPos3x3(EnemyPos3x3.UP1); break;

                    // ➡️ 左 → 右
                    case EnemyPos3x3.LEFT1: finishPos = enemyFlow.SetPos3x3(EnemyPos3x3.RIGHT5); break;
                    case EnemyPos3x3.LEFT2: finishPos = enemyFlow.SetPos3x3(EnemyPos3x3.RIGHT4); break;
                    case EnemyPos3x3.LEFT3: finishPos = enemyFlow.SetPos3x3(EnemyPos3x3.RIGHT3); break;
                    case EnemyPos3x3.LEFT4: finishPos = enemyFlow.SetPos3x3(EnemyPos3x3.RIGHT2); break;
                    case EnemyPos3x3.LEFT5: finishPos = enemyFlow.SetPos3x3(EnemyPos3x3.RIGHT1); break;

                    // ⬅️ 右 → 左
                    case EnemyPos3x3.RIGHT1: finishPos = enemyFlow.SetPos3x3(EnemyPos3x3.LEFT5); break;
                    case EnemyPos3x3.RIGHT2: finishPos = enemyFlow.SetPos3x3(EnemyPos3x3.LEFT4); break;
                    case EnemyPos3x3.RIGHT3: finishPos = enemyFlow.SetPos3x3(EnemyPos3x3.LEFT3); break;
                    case EnemyPos3x3.RIGHT4: finishPos = enemyFlow.SetPos3x3(EnemyPos3x3.LEFT2); break;
                    case EnemyPos3x3.RIGHT5: finishPos = enemyFlow.SetPos3x3(EnemyPos3x3.LEFT1); break;

                    default:
                        finishPos = transform.position; // 安全策
                        break;
                }
                break;

            case EnemyAction.STOP:
                // 停止アクション → 位置固定
                finishPos = startPos;
                break;

            case EnemyAction.BEAM:
                // 攻撃アクション → 位置固定 or エフェクト再生準備など
                finishPos = startPos;
                // ここでビーム演出準備可能
                break;

            case EnemyAction.EXPLOTION:
                // 爆発アクション → 位置固定
                finishPos = startPos;
                break;
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
