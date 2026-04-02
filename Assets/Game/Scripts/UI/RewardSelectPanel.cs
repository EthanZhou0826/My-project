using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardSelectPanel : MonoBehaviour
{
    public enum RewardType
    {
        DamageUp,
        FireRateUp,
        ProjectileSpeedUp,
        MaxHpUp,
        Heal,
        PierceUp
    }

    [System.Serializable]
    public class RewardOption
    {
        public RewardType type;
        public string title;
        public string description;
    }

    [System.Serializable]
    public class RewardButton
    {
        public Button button;
        public TMP_Text titleText;
        public TMP_Text descText;
    }

    [Header("UI")]
    public GameObject root;
    public TMP_Text panelTitleText;
    public RewardButton[] buttons;

    private RewardOption[] currentOptions;
    private Action<RewardOption> onSelected;

    private void Awake()
    {
        Hide();
    }

    public void Show(RewardOption[] options, Action<RewardOption> selectedCallback)
    {
        currentOptions = options;
        onSelected = selectedCallback;

        if (panelTitleText != null)
        {
            panelTitleText.text = "选择一项强化";
        }

        root.SetActive(true);
        Time.timeScale = 0f;

        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;

            if (i >= options.Length)
            {
                buttons[i].button.gameObject.SetActive(false);
                continue;
            }

            buttons[i].button.gameObject.SetActive(true);
            buttons[i].titleText.text = options[i].title;
            buttons[i].descText.text = options[i].description;

            buttons[i].button.onClick.RemoveAllListeners();
            buttons[i].button.onClick.AddListener(() => Select(index));
        }
    }

    public void Hide()
    {
        if (root != null)
        {
            root.SetActive(false);
        }

        Time.timeScale = 1f;
    }

    private void Select(int index)
    {
        if (currentOptions == null || index < 0 || index >= currentOptions.Length) return;

        RewardOption option = currentOptions[index];
        Hide();
        onSelected?.Invoke(option);
    }
}