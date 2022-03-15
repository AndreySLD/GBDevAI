using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FightWindowView : MonoBehaviour
{
    [SerializeField]
    private RewardView _rewardView;
    #region Text
    [SerializeField]
    private TMP_Text _countMoneyText;

    [SerializeField]
    private TMP_Text _countHealthText;

    [SerializeField]
    private TMP_Text _countPowerText;

    [SerializeField]
    private TMP_Text _countPowerEnemyText;

    [SerializeField]
    private TMP_Text _countThreatLevelText;
    #endregion

    #region Buttons
    [SerializeField]
    private Button _addMoneyButton;

    [SerializeField]
    private Button _minusMoneyButton;


    [SerializeField]
    private Button _addHealthButton;

    [SerializeField]
    private Button _minusHealthButton;


    [SerializeField]
    private Button _addPowerButton;

    [SerializeField]
    private Button _minusPowerButton;


    [SerializeField]
    private Button _addThreatLevelButton;

    [SerializeField]
    private Button _minusThreatLevelButton;

    [SerializeField]
    private Button _fightButton;

    [SerializeField]
    private Button _evadeButton;

    [SerializeField]
    private Button _buttonOpenShop;
    #endregion

    private Enemy _enemy;

    private Money _money;
    private Health _health;
    private Power _power;
    private Threat _threat;

    private int _allCountMoneyPlayer;
    private int _allCountHealthPlayer;
    private int _allCountPowerPlayer;
    private int _allCountThreatLevel; //int only!

    private void Start()
    {
        _enemy = new Enemy("Flappy");

        _money = new Money(nameof(Money));
        _money.Attach(_enemy);

        _health = new Health(nameof(Health));
        _health.Attach(_enemy);

        _power = new Power(nameof(Power));
        _power.Attach(_enemy);

        _threat = new Threat(nameof(Threat));
        _threat.Attach(_enemy);

        _addMoneyButton.onClick.AddListener(() => ChangeMoney(true));
        _minusMoneyButton.onClick.AddListener(() => ChangeMoney(false));

        _addHealthButton.onClick.AddListener(() => ChangeHealth(true));
        _minusHealthButton.onClick.AddListener(() => ChangeHealth(false));

        _addPowerButton.onClick.AddListener(() => ChangePower(true));
        _minusPowerButton.onClick.AddListener(() => ChangePower(false));

        _addThreatLevelButton.onClick.AddListener(() => ChangeThreatLevel(true));
        _minusThreatLevelButton.onClick.AddListener(() => ChangeThreatLevel(false));

        _fightButton.onClick.AddListener(Fight);
        _evadeButton.onClick.AddListener(TryEvade);

        _buttonOpenShop.onClick.AddListener(_rewardView.Show);
    }

    private void OnDestroy()
    {
        _addMoneyButton.onClick.RemoveAllListeners();
        _minusMoneyButton.onClick.RemoveAllListeners();

        _addHealthButton.onClick.RemoveAllListeners();
        _minusHealthButton.onClick.RemoveAllListeners();

        _addPowerButton.onClick.RemoveAllListeners();
        _minusPowerButton.onClick.RemoveAllListeners();

        _addThreatLevelButton.onClick.RemoveAllListeners();
        _minusThreatLevelButton.onClick.RemoveAllListeners();

        _fightButton.onClick.RemoveAllListeners();
        _evadeButton.onClick.RemoveAllListeners();

        _buttonOpenShop.onClick.RemoveAllListeners();

        _money.Detach(_enemy);
        _health.Detach(_enemy);
        _power.Detach(_enemy);
        _threat.Detach(_enemy);
    }

    private void Fight()
    {
        Debug.Log(_allCountPowerPlayer >= _enemy.Power ? "Win" : "Lose");
    }

    private void TryEvade()
    {
        var result = _allCountPowerPlayer >= _enemy.Power ? "Victory" : "Defeat";
        
        if (_allCountThreatLevel < 3)
        {
            Debug.Log("Successful evasion.");
        }
        else if(_allCountThreatLevel < 6 && _allCountThreatLevel > 2)
        {
            int chance = Random.Range(0, 5);
            if (chance <= 1) //25%
            {
                Debug.Log("Successful evasion.");
            }
            else
            {
                Debug.Log($"Threat level too high, failed to evade. {result}");

            }
        }
        else
        {           
            Debug.Log($"Threat level too high, failed to evade. {result}");
        }
    }

    private void ChangePower(bool isAddCount)
    {
        if (isAddCount)
            _allCountPowerPlayer++;
        else
            _allCountPowerPlayer--;

        ChangeDataWindow(_allCountPowerPlayer, DataType.Power);
    }

    private void ChangeHealth(bool isAddCount)
    {
        if (isAddCount)
            _allCountHealthPlayer++;
        else
            _allCountHealthPlayer--;

        ChangeDataWindow(_allCountHealthPlayer, DataType.Health);
    }

    private void ChangeMoney(bool isAddCount)
    {
        if (isAddCount)
            _allCountMoneyPlayer++;
        else
            _allCountMoneyPlayer--;

        ChangeDataWindow(_allCountMoneyPlayer, DataType.Money);
    }

    private void ChangeThreatLevel(bool isAddCount)
    {
        if (isAddCount)
            _allCountThreatLevel++;
        else
            _allCountThreatLevel--;

        ChangeDataWindow(_allCountThreatLevel, DataType.Threat);
    }

    private void ChangeDataWindow(int countChangeData, DataType dataType)
    {
        switch (dataType)
        {
            case DataType.Money:
                _countMoneyText.text = $"Player money: {countChangeData}";
                _money.CountMoney = countChangeData;
                break;

            case DataType.Health:
                _countHealthText.text = $"Player health: {countChangeData}";
                _health.CountHealth = countChangeData;
                break;

            case DataType.Power:
                _countPowerText.text = $"Player power: {countChangeData}";
                _power.CountPower = countChangeData;
                break;

            case DataType.Threat:
                _countThreatLevelText.text = $"Threat level: {countChangeData}";
                _threat.CountThreatLevel = countChangeData;
                break;
        }

        _countPowerEnemyText.text = $"Enemy power: {_enemy.Power}";
    }
}
