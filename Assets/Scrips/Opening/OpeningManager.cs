using UnityEngine;

public class OpeningManager : MonoBehaviour
{
    [SerializeField] GameObject tapPanel;
    [SerializeField] GameObject startPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tapPanel.SetActive(true);
        startPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TapButton()
    {
        tapPanel.SetActive(false);
        startPanel.SetActive(true);
    }

    public void newStartButton()
    {
        ScenesManager.instance.SceneMove(1);
    }

    public void ContinueButton()
    {
        ScenesManager.instance.SceneMove(2);
    }
}
