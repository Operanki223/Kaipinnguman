using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Stage
{
    [SerializeField] GameObject stagePanel;
    [SerializeField] string stageName;
    Vector3 panelPos;
    float x, y, z;
    bool view = false;
    float moveSpeed = 1f;

    public void SetPanelPos(float x, float y = 0, float z = 0)
    {
        panelPos = new Vector3(this.x = x, this.y = y, this.z = z);
        stagePanel.transform.position = panelPos;
    }

    public void SetMoveSpped(float speed)
    {
        this.moveSpeed = speed;
    }

    public void SetView(bool b)
    {
        this.view = b;
    }

    public Vector3 GetVector3()
    {
        return this.panelPos;
    }

    public bool GetView()
    {
        return this.view;
    }

    public string GetStageName()
    {
        return this.stageName;
    }

    public void AddPos(float x, float y = 0, float z = 0)
    {
        this.panelPos += new Vector3(x, y, z);
    }

    public void UpdateMove()
    {
        if (stagePanel == null) return;

        // 現在位置を目標位置に向かって徐々に移動
        stagePanel.transform.position = Vector3.Lerp(
            stagePanel.transform.position,
            panelPos,
            Time.deltaTime * moveSpeed
        );
    }
}

public class StageSelectManager : MonoBehaviour
{
    [SerializeField] public List<Stage> stages = new List<Stage>();
    [SerializeField] GameObject leftButton;
    [SerializeField] GameObject rightButton;
    [SerializeField] TextMeshProUGUI stageNameText = null;
    [Header("パネルごとの間隔"), SerializeField] float width = 10;
    [SerializeField] float speed = 2.0f;
    bool panelMove = false;
    int stageCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StagePanelSetting();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var stage in stages)
        {
            stage.UpdateMove(); // ← 毎フレーム位置を補間
        }
        StagePanelMove();
    }

    /// <summary>
    /// パネルのセッティング（位置など）
    /// </summary> 
    void StagePanelSetting()
    {
        stageNameText.text = stages[0].GetStageName();
        stages[0].SetView(true);
        leftButton.SetActive(false);
        for (float i = 0; i < stages.Count; i++)
        {
            stages[(int)i].SetPanelPos(i * width, 0, 0);
            stages[(int)i].SetMoveSpped(speed);
        }
    }

    public void LeftButton()
    {
        panelMove = true;
        stageCount--;
        foreach (var stage in stages)
        {
            stage.AddPos(width, 0, 0);
        }
    }

    public void RightButton()
    {
        panelMove = true;
        stageCount++;
        foreach (var stage in stages)
        {
            stage.AddPos(-width, 0, 0);
        }
    }

    void StagePanelMove()
    {
        if (panelMove)
        {
            if (stageCount <= 0)
            {
                leftButton.SetActive(false);
                rightButton.SetActive(true);
            }
            else if (stageCount >= stages.Count - 1)
            {
                leftButton.SetActive(true);
                rightButton.SetActive(false);
            }
            else
            {
                leftButton.SetActive(true);
                rightButton.SetActive(true);
            }
            for (int i = 0; i < stages.Count; i++)
            {
                if (i == stageCount)
                {
                    stageNameText.text = stages[i].GetStageName();
                }
            }
        }
        panelMove = false;
    }
}
