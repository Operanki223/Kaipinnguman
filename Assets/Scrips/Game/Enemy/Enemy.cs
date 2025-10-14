using UnityEngine;

[System.Serializable]
public class EnemyFlow
{
    [SerializeField] EnemyName name;
    [SerializeField] EnemyPos3x3 startPos;
    Vector2 pos;//最初の座標を入れる
    [SerializeField] EnemyAction enemyAction;
    [SerializeField] float spawnTime;
    [SerializeField] float speed;
    [SerializeField] int HP;
    float animatioTime = 1.0f;
    Vector3 up = new Vector2(0f, 1.75f);
    Vector3 right = new Vector2(2.5f, 0f);
    Vector3 bottom = new Vector2(0f, -1.75f);
    Vector3 left = new Vector2(-2.5f, 0f);


    /// <summary>
    /// 指定した場所の座標も受け取れる
    /// </summary>
    /// <param name="startPos"></param>
    /// <returns></returns>
    public Vector3 SetPos3x3(EnemyPos3x3 startPos)
    {
        Vector2 sub_pos;
        switch (startPos)
        {
            case 0:
                pos = up + GameManager.instance.wordsObj[0].gameObject.transform.position;
                break;
            case (EnemyPos3x3)1:
                pos = up + (GameManager.instance.wordsObj[1].gameObject.transform.position + GameManager.instance.wordsObj[0].gameObject.transform.position) / 2;
                break;
            case (EnemyPos3x3)2:
                pos = up + GameManager.instance.wordsObj[1].gameObject.transform.position;
                break;
            case (EnemyPos3x3)3:
                pos = up + (GameManager.instance.wordsObj[2].gameObject.transform.position + GameManager.instance.wordsObj[1].gameObject.transform.position) / 2;
                break;
            case (EnemyPos3x3)4:
                pos = up + GameManager.instance.wordsObj[2].gameObject.transform.position;
                break;
            case (EnemyPos3x3)5:
                pos = right + GameManager.instance.wordsObj[2].gameObject.transform.position;
                break;
            case (EnemyPos3x3)6:
                pos = right + (GameManager.instance.wordsObj[2].gameObject.transform.position + GameManager.instance.wordsObj[5].gameObject.transform.position) / 2;
                break;
            case (EnemyPos3x3)7:
                pos = right + GameManager.instance.wordsObj[5].gameObject.transform.position;
                break;
            case (EnemyPos3x3)8:
                pos = right + (GameManager.instance.wordsObj[5].gameObject.transform.position + GameManager.instance.wordsObj[8].gameObject.transform.position) / 2;
                break;
            case (EnemyPos3x3)9:
                pos = right + GameManager.instance.wordsObj[8].gameObject.transform.position;
                break;
            case (EnemyPos3x3)10:
                pos = bottom + GameManager.instance.wordsObj[8].gameObject.transform.position;
                break;
            case (EnemyPos3x3)11:
                pos = bottom + (GameManager.instance.wordsObj[8].gameObject.transform.position + GameManager.instance.wordsObj[7].gameObject.transform.position) / 2;
                break;
            case (EnemyPos3x3)12:
                pos = bottom + GameManager.instance.wordsObj[7].gameObject.transform.position;
                break;
            case (EnemyPos3x3)13:
                pos = bottom + (GameManager.instance.wordsObj[7].gameObject.transform.position + GameManager.instance.wordsObj[6].gameObject.transform.position) / 2;
                break;
            case (EnemyPos3x3)14:
                pos = bottom + GameManager.instance.wordsObj[6].gameObject.transform.position;
                break;
            case (EnemyPos3x3)15:
                pos = left + GameManager.instance.wordsObj[6].gameObject.transform.position;
                break;
            case (EnemyPos3x3)16:
                pos = left + (GameManager.instance.wordsObj[3].gameObject.transform.position + GameManager.instance.wordsObj[6].gameObject.transform.position) / 2;
                break;
            case (EnemyPos3x3)17:
                pos = left + GameManager.instance.wordsObj[3].gameObject.transform.position;
                break;
            case (EnemyPos3x3)18:
                pos = left + (GameManager.instance.wordsObj[0].gameObject.transform.position + GameManager.instance.wordsObj[3].gameObject.transform.position) / 2;
                break;
            case (EnemyPos3x3)19:
                pos = left + GameManager.instance.wordsObj[0].gameObject.transform.position;
                break;
            default:
                break;
        }

        return sub_pos = pos;
    }

    public EnemyName GetEnemyName()
    {
        return this.name;
    }

    public Vector2 GetPos()
    {
        return this.pos;
    }
    public EnemyPos3x3 GetEnemyPos3X3()
    {
        return this.startPos;
    }

    public float GetSpawnTime()
    {
        return this.spawnTime;
    }

    public EnemyAction GetEnemyAction()
    {
        return this.enemyAction;
    }

    public float GetAnimationTime()
    {
        return this.animatioTime;
    }

    public float GetSpeed()
    {
        return this.speed;
    }
}

[System.Serializable]
public class EnemyList
{
    public EnemyName name;
    public GameObject enemyObj;

    public EnemyName GetEnemyName()
    {
        return this.name;
    }

    public GameObject GetEnemyObj()
    {
        return this.enemyObj;
    }
}

public enum EnemyAction
{
    MOVE,
    BEAM,
    STOP,
    EXPLOTION,
}
public enum EnemyPos3x3
{
    UP1 = 0,
    UP2 = 1,
    UP3 = 2,
    UP4 = 3,
    UP5 = 4,
    RIGHT1 = 5,
    RIGHT2 = 6,
    RIGHT3 = 7,
    RIGHT4 = 8,
    RIGHT5 = 9,
    BOTTOM1 = 10,
    BOTTOM2 = 11,
    BOTTOM3 = 12,
    BOTTOM4 = 13,
    BOTTOM5 = 14,
    LEFT1 = 15,
    LEFT2 = 16,
    LEFT3 = 17,
    LEFT4 = 18,
    LEFT5 = 19,
}

[SerializeField]
public enum EnemyName
{
    SAMPLE,
    KAMOME,
    TONNBO,
    CLOUD,
}
