using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class RewardView : MonoBehaviour
{
    private const string LastTimeDailyKey = "LastDailyRewardTime";
    private const string ActiveDailySlotKey = "ActiveDailySlot";

    private const string LastTimeWeeklyKey = "LastWeekRewardTime";
    private const string ActiveWeeklySlotKey = "ActiveWeeklySlot";

    #region Fields
    [Header("Time settings")]
    [SerializeField]
    public int DailyTimeCooldown = 86400;
    [SerializeField]
    public int DailyTimeDeadline = 172800;
    [SerializeField]
    public int WeeklyTimeCooldown;
    [SerializeField]
    public int WeeklyTimeDeadline;
    [Space]
    [Header("Reward Settings")]
    public List<Reward> DailyRewards;
    public List<Reward> WeeklyRewards;
    [Header("UI")]
    [SerializeField]
    public TMP_Text DailyRewardTimer;
    [SerializeField]
    public TMP_Text WeeklyRewardTimer;
    [SerializeField]
    public Transform SlotsParent;
    [SerializeField]
    public SlotRewardView SlotPrefab;
    [SerializeField]
    public Button ResetButton;
    [SerializeField]
    public Button CloseButton;
    [SerializeField]
    public Button GetDailyRewardButton;
    [SerializeField]
    public Button GetWeeklyRewardButton;
    [SerializeField]
    public Slider ProgressBarDaily;
    [SerializeField]
    public Slider ProgressBarWeekly;
    [Header("AnimSettings")]
    [SerializeField]
    private float _duration = 0.3f;

    private DailyRewardController _dailyRewardController;
    private WeeklyRewardController _weeklyRewardController;

    private bool _state;
    #endregion

    public int CurrentActiveDailySlot
    {
        get => PlayerPrefs.GetInt(ActiveDailySlotKey);
        set => PlayerPrefs.SetInt(ActiveDailySlotKey, value);
    }

    public int CurrentActiveWeeklySlot
    {
        get => PlayerPrefs.GetInt(ActiveWeeklySlotKey);
        set => PlayerPrefs.GetInt(ActiveWeeklySlotKey, value);
    }

    public DateTime? LastDailyRewardTime
    {
        get
        {
            var data = PlayerPrefs.GetString(LastTimeDailyKey);
            if (string.IsNullOrEmpty(data))
                return null;
            return DateTime.Parse(data);
        }
        set
        {
            if (value != null)
                PlayerPrefs.SetString(LastTimeDailyKey, value.ToString());
            else
                PlayerPrefs.DeleteKey(LastTimeDailyKey);
        }
    }

    public DateTime? LastWeeklyRewardTime
    {
        get
        {
            var data = PlayerPrefs.GetString(LastTimeWeeklyKey);
            if (string.IsNullOrEmpty(data))
                return null;
            return DateTime.Parse(data);
        }
        set
        {
            if (value != null)
                PlayerPrefs.SetString(LastTimeWeeklyKey, value.ToString());
            else
                PlayerPrefs.DeleteKey(LastTimeWeeklyKey);
        }
    }

    #region Animation
    public void Show()
    {
        gameObject.SetActive(true);
        _state = true;
        Animation(_state, _duration);
    }
    public void Hide()
    {
        _state = false;
        Animation(_state, _duration);
    }
    private void Animation(bool state, float value)
    {
        var sequence = DOTween.Sequence();

        if (state)
        {
            sequence.Insert(0.0f, transform.DOScale(Vector3.one, value));
            sequence.OnComplete(() =>
            {
                sequence = null;
            });
        }
        else
        {
            sequence.Insert(0.0f, transform.DOScale(Vector3.zero, value));
            sequence.OnComplete(() =>
            {
                sequence = null;
                gameObject.SetActive(false);
            });

        }
    }
    #endregion
    private void Start()
    {
        _dailyRewardController = new DailyRewardController(this);
        _weeklyRewardController = new WeeklyRewardController(this);

        CloseButton.onClick.AddListener(Hide);
    }
    private void OnDestroy()
    {
        GetDailyRewardButton.onClick.RemoveAllListeners();
        GetWeeklyRewardButton.onClick.RemoveAllListeners();
        ResetButton.onClick.RemoveAllListeners();
    }
}
