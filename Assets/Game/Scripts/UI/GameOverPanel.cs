using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverPanel : MonoBehaviour
{
    [Header("UI")]
    public GameObject root;
    public TMP_Text titleText;
    public TMP_Text hintText;

    private bool isShowing = false;

    private void Awake()
    {
        Hide();
    }

    private void Update()
    {
        if (!isShowing) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void Show()
    {
        if (root == null)
        {
            Debug.LogError("GameOverPanel: root 没有绑定。");
            return;
        }

        isShowing = true;
        root.SetActive(true);

        if (titleText != null)
        {
            titleText.text = "Game Over";
        }

        if (hintText != null)
        {
            hintText.text = "按 R 重新开始";
        }

        Time.timeScale = 0f;
    }

    public void Hide()
    {
        isShowing = false;

        if (root != null)
        {
            root.SetActive(false);
        }

        Time.timeScale = 1f;
    }
}