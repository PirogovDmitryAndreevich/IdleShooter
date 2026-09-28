using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameStaticData))]
public class GameStaticDataEditor : Editor
{
    private const string ObstaclesDataPath = "StaticData/Obstacles";
    private const string WeaponsDataPath = "StaticData/WeaponsData";
    private const string WeaponViewsPath = "StaticData/WeaponView/WeaponViews";
    private const string EnemiesStaticDataPath = "StaticData/Enemies";
    private const string LevelsViewPath = "Levels";

    private Vector2 _levelTableScroll;

    // --- Параметры формулы прокачки ---
    private int _baseXp = 100;          // XP на 2-м уровне
    private float _growthStart = 1.15f; // коэффициент роста на старте
    private float _growthEnd = 1.08f;   // коэффициент роста в конце

    private Vector2 _simulationTableScroll;
    private BalanceSimulationResult[] _simulationResults;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GameStaticData gameData = (GameStaticData)target;

        if (GUILayout.Button("Build"))
        {
            Build(gameData);
            EditorUtility.SetDirty(target);
        }

        DrawSimulation(gameData);
    }

    private void Build(GameStaticData gameData)
    {
        LobbySpawnPoint lobbySpawn = FindObjectOfType<LobbySpawnPoint>();
        if (lobbySpawn != null)
            gameData.LobbySpawnPosition = lobbySpawn.transform.position;

        gameData.LevelViews = Resources.LoadAll<LevelView>(LevelsViewPath);
        gameData.Obstacles = Resources.LoadAll<ObstacleStaticData>(ObstaclesDataPath);
        gameData.WeaponsStaticData = Resources.LoadAll<WeaponStaticData>(WeaponsDataPath);
        gameData.WeaponsView = Resources.Load<WeaponViewsData>(WeaponViewsPath);
        gameData.EnemiesStaticData = Resources.LoadAll<EnemyStaticData>(EnemiesStaticDataPath);
    }

    // =========================================================
    //  LEVEL GENERATOR (формула)
    // =========================================================

    private void DrawLevelGenerator(GameStaticData gameData)
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Level Generator", EditorStyles.boldLabel);

        _baseXp = EditorGUILayout.IntField("Base XP (level 2)", _baseXp);
        _growthStart = EditorGUILayout.FloatField("Growth Start", _growthStart);
        _growthEnd = EditorGUILayout.FloatField("Growth End", _growthEnd);

        EditorGUILayout.HelpBox(
            "XP(n) = XP(n-1) * Lerp(GrowthStart, GrowthEnd, n / MaxLevel)\n" +
            "XP(1) = 0, XP(2) = Base XP",
            MessageType.Info);

        if (GUILayout.Button("Fill Levels"))
        {
            FillLevels(gameData, gameData.MaxLevel);
            EditorUtility.SetDirty(target);
        }

        EditorGUILayout.EndVertical();
    }

    private void FillLevels(GameStaticData gameData, int maxLevel)
    {
        if (maxLevel < 1) maxLevel = 1;

        var list = new List<PlayerLevelsStaticData>(maxLevel);

        list.Add(new PlayerLevelsStaticData
        {
            Level = 1,
            TargetScore = 0
        });

        float prev = 0f;

        for (int level = 2; level <= maxLevel; level++)
        {
            // t — доля прогресса 0..1
            float t = maxLevel > 1 ? (level - 1) / (float)(maxLevel - 1) : 0f;

            // коэффициент роста плавно падает от GrowthStart к GrowthEnd
            float growth = Mathf.Lerp(_growthStart, _growthEnd, t);

            float xp;
            if (level == 2)
                xp = _baseXp;
            else
                xp = prev * growth;

            // Ограничение: не больше int.MaxValue
            if (xp > int.MaxValue) xp = int.MaxValue;

            int targetScore = Mathf.RoundToInt(xp);

            list.Add(new PlayerLevelsStaticData
            {
                Level = level,
                TargetScore = targetScore
            });

            prev = xp;
        }

        gameData.PlayerLevelsData = list.ToArray();
    }

    // =========================================================
    //  PLAYER LEVELS TABLE
    // =========================================================

    private void DrawPlayerLevelTable(GameStaticData gameData)
    {
        if (gameData.PlayerLevelsData == null || gameData.PlayerLevelsData.Length == 0)
            return;

        EditorGUILayout.Space(4);
        EditorGUILayout.LabelField("Levels Table", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        GUILayout.Label("Level", EditorStyles.toolbarButton, GUILayout.Width(60));
        GUILayout.Label("Target Score", EditorStyles.toolbarButton, GUILayout.Width(120));
        EditorGUILayout.EndHorizontal();

        _levelTableScroll = EditorGUILayout.BeginScrollView(
            _levelTableScroll,
            GUILayout.Height(300));

        for (int i = 0; i < gameData.PlayerLevelsData.Length; i++)
        {
            var lvl = gameData.PlayerLevelsData[i];

            EditorGUILayout.BeginHorizontal("box");
            GUILayout.Label(lvl.Level.ToString(), GUILayout.Width(60));
            GUILayout.Label(NumberFormatter.NumberAsString(lvl.TargetScore), GUILayout.Width(120));
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();
    }

    private void DrawSimulation(GameStaticData gameData)
    {
        EditorGUILayout.Space(10);

        EditorGUILayout.BeginVertical("box");

        EditorGUILayout.LabelField(
            "Balance Simulation",
            EditorStyles.boldLabel);

        if (GUILayout.Button("Simulate"))
        {
            BalanceSimulator simulator =
                new BalanceSimulator();

            _simulationResults =
                simulator
                    .Simulate(gameData)
                    .ToArray();

            UpdateRecommendedLevels(gameData);
        }

        if (_simulationResults == null ||
            _simulationResults.Length == 0)
        {
            EditorGUILayout.EndVertical();
            return;
        }

        EditorGUILayout.Space(5);

        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

        GUILayout.Label(
            "Run",
            EditorStyles.toolbarButton,
            GUILayout.Width(45));

        GUILayout.Label(
            "Level",
            EditorStyles.toolbarButton,
            GUILayout.Width(45));

        GUILayout.Label(
            "Weapon",
            EditorStyles.toolbarButton,
            GUILayout.Width(90));

        GUILayout.Label(
            "Player Level Start",
            EditorStyles.toolbarButton,
            GUILayout.Width(100));

        GUILayout.Label(
            "Player Level End",
            EditorStyles.toolbarButton,
            GUILayout.Width(100));

        GUILayout.Label(
            "Damage Start",
            EditorStyles.toolbarButton,
            GUILayout.Width(100));

        GUILayout.Label(
            "Damage End",
            EditorStyles.toolbarButton,
            GUILayout.Width(100));

        GUILayout.Label(
            "Shots",
            EditorStyles.toolbarButton,
            GUILayout.Width(55));

        GUILayout.Label(
            "Score",
            EditorStyles.toolbarButton,
            GUILayout.Width(100));

        GUILayout.Label(
            "Wins +",
            EditorStyles.toolbarButton,
            GUILayout.Width(55));

        GUILayout.Label(
            "Wins",
            EditorStyles.toolbarButton,
            GUILayout.Width(55));

        GUILayout.Label(
            "Bought",
            EditorStyles.toolbarButton,
            GUILayout.Width(90));

        EditorGUILayout.EndHorizontal();

        _simulationTableScroll =
            EditorGUILayout.BeginScrollView(
                _simulationTableScroll,
                GUILayout.Height(400));

        for (int i = 0;
             i < _simulationResults.Length;
             i++)
        {
            BalanceSimulationResult result =
                _simulationResults[i];          

            EditorGUILayout.BeginHorizontal("box");

            GUILayout.Label(
                result.Run.ToString(),
                GUILayout.Width(45));

            GUILayout.Label(
                result.GameLevel.ToString(),
                GUILayout.Width(45));

            GUILayout.Label(
                result.Weapon.ToString(),
                GUILayout.Width(90));

            GUILayout.Label(
                result.PlayerLevelStart.ToString(),
                GUILayout.Width(100));

            GUILayout.Label(
                result.PlayerLevelEnd.ToString(),
                GUILayout.Width(100));

            GUILayout.Label(
                result.DamageStart.ToString(),
                GUILayout.Width(100));

            GUILayout.Label(
                result.DamageEnd.ToString(),
                GUILayout.Width(100));

            GUILayout.Label(
                result.Shots.ToString(),
                GUILayout.Width(55));

            GUILayout.Label(
                result.Score.ToString(),
                GUILayout.Width(100));

            GUILayout.Label(
                result.WinsReceived.ToString(),
                GUILayout.Width(55));

            GUILayout.Label(
                result.WinsBalance.ToString(),
                GUILayout.Width(55));

            GUILayout.Label(
                result.BoughtWeapon?.ToString() ?? "-",
                GUILayout.Width(90));

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();

        EditorGUILayout.EndVertical();
    }

    private void UpdateRecommendedLevels(GameStaticData gameData)
    {
        if (_simulationResults == null ||
            _simulationResults.Length == 0)
            return;

        HashSet<int> processedLevels = new HashSet<int>();

        foreach (BalanceSimulationResult result in _simulationResults)
        {
            if (!processedLevels.Add(result.GameLevel))
                continue;

            int index = result.GameLevel;

            if (index <= 0 ||
                index >= gameData.LevelsData.Length)
                continue;

            LevelsStaticData level =
                gameData.LevelsData[index];

            level.Recommended =
                result.PlayerLevelStart;
        }

        EditorUtility.SetDirty(gameData);
    }
}

public class BalanceSimulator
{
    private GameStaticData _gameData;

    private float _score;
    private int _currentPlayerLevel;

    private int _damage;
    private int _wins;

    private int _currentMaxLevelIndex;

    private int _currentWeaponIndex;
    private WeaponStaticData _currentWeapon;

    private int _currentRun;

    private List<LevelsStaticData> _sortedLevels;
    private List<WeaponStaticData> _sortedWeapons;

    private readonly List<BalanceSimulationResult> _results =
        new List<BalanceSimulationResult>();

    public IReadOnlyList<BalanceSimulationResult> Simulate(
        GameStaticData gameData)
    {
        _gameData = gameData;

        _score = 0;
        _currentPlayerLevel = 1;

        _damage = 1;
        _wins = 0;

        _currentMaxLevelIndex = 2;

        _currentWeaponIndex = 0;

        _currentRun = 0;

        _results.Clear();

        _sortedLevels =
            _gameData.LevelsData
                .OrderBy(level => level.Type)
                .ToList();

        _sortedWeapons =
            _gameData.WeaponsStaticData
                .OrderBy(weapon => weapon.Type)
                .ToList();

        var cannon = _sortedWeapons.FirstOrDefault(x => x.Type == WeaponType.Cannon);

        _sortedWeapons.Remove(cannon);

        if (_sortedWeapons.Count == 0)
            return _results;

        _currentWeapon =
            _sortedWeapons[_currentWeaponIndex];

        StartSimulate();

        return _results;
    }

    private void StartSimulate()
    {
        while (_currentMaxLevelIndex <= _sortedLevels.Count)
        {
            _currentRun++;

            BalanceSimulationResult lastResult =
                SimulateRun();

            // Последний игровой уровень пройден.
            // Симуляция полностью завершена.
            if (IsLastLevelReached())
                break;

            // Возвращаемся в Lobby и проверяем покупку.
            if (TryBuyNextWeapon(lastResult))
            {
                _currentMaxLevelIndex++;
                continue;
            }

            if (!HasNextWeapon())
                _currentMaxLevelIndex++;

            // Оружие ещё не купили.
            // Следующий Run будет снова проходить
            // те же доступные уровни.
        }
    }

    private BalanceSimulationResult SimulateRun()
    {
        LevelsStaticData lastLevel = null;
        BalanceSimulationResult lastResult = null;

        for (int i = 1; i < _currentMaxLevelIndex; i++)
        {
            lastLevel = _sortedLevels[i];

            int startDamage = _damage;
            int startPlayerLevel = _currentPlayerLevel;
            int startScore = (int)_score;

            EnteringLevel(lastLevel);

            int shots =
                _damage - startDamage;

            int score =
                (int)_score - startScore;

            lastResult = new BalanceSimulationResult
            {
                Run = _currentRun,
                GameLevel = i,

                Weapon = _currentWeapon.Type,

                DamageStart = startDamage,
                DamageEnd = _damage,

                PlayerLevelStart = startPlayerLevel,
                PlayerLevelEnd = _currentPlayerLevel,

                Shots = shots,
                Score = score,

                WinsReceived = 0,
                WinsBalance = _wins
            };

            _results.Add(lastResult);
        }

        if (lastLevel == null)
            return null;

        // Награда только за самый дальний
        // пройденный уровень.
        _wins += lastLevel.Wins;

        lastResult.WinsReceived =
            lastLevel.Wins;

        lastResult.WinsBalance =
            _wins;

        return lastResult;
    }

    private void EnteringLevel(LevelsStaticData level)
    {
        for (int i = 0; i < level.EnemiesOnLevel.Length; i++)
        {
            EnemyType enemyType =
                level.EnemiesOnLevel[i];

            EnemyStaticData enemy =
                _gameData.EnemiesStaticData
                    .FirstOrDefault(
                        x => x.Type == enemyType);

            if (enemy == null)
                continue;

            SimulateTarget(enemy.HealthPoints);
        }

        for (int i = 0; i < level.ObstaclesOnLevel.Length; i++)
        {
            ObstacleType obstacleType =
                level.ObstaclesOnLevel[i];

            ObstacleStaticData obstacle =
                _gameData.Obstacles
                    .FirstOrDefault(
                        x => x.Type == obstacleType);

            if (obstacle == null)
                continue;

            SimulateTarget(obstacle.HealthPoints);
        }
    }

    private void SimulateTarget(int health)
    {
        while (health > 0)
        {
            health -= _damage;

            _damage++;
            _score += _currentWeapon.Score;

            CheckPlayerLevelUp();
        }
    }

    private void CheckPlayerLevelUp()
    {
        PlayerLevelsStaticData[] playerLevels =
            _gameData.PlayerLevelsData;

        for (int i = 0; i < playerLevels.Length; i++)
        {
            if (playerLevels[i].Level <= _currentPlayerLevel)
                continue;

            if (playerLevels[i].TargetScore <= _score)
            {
                _currentPlayerLevel =
                    playerLevels[i].Level;
            }
        }
    }

    private bool TryBuyNextWeapon(
    BalanceSimulationResult result)
    {
        int nextWeaponIndex =
            _currentWeaponIndex + 1;

        if (nextWeaponIndex >= _sortedWeapons.Count)
            return false;

        WeaponStaticData nextWeapon =
            _sortedWeapons[nextWeaponIndex];

        if (_wins < nextWeapon.Price)
            return false;

        _wins -= nextWeapon.Price;

        _currentWeaponIndex =
            nextWeaponIndex;

        _currentWeapon =
            nextWeapon;

        result.BoughtWeapon =
            _currentWeapon.Type;

        result.WinsBalance =
            _wins;

        return true;
    }

    private bool IsLastLevelReached()
    {
        return _currentMaxLevelIndex >= _sortedLevels.Count;
    }

    private bool HasNextWeapon()
    {
        int nextWeaponIndex = _currentWeaponIndex + 1;

        return nextWeaponIndex < _sortedWeapons.Count;
    }
}

public class BalanceSimulationResult
{
    public int Run;
    public int GameLevel;

    public WeaponType Weapon;

    public int PlayerLevelStart;
    public int PlayerLevelEnd;

    public int DamageStart;
    public int DamageEnd;

    public int Shots;
    public int Score;

    public int WinsReceived;
    public int WinsBalance;

    public WeaponType? BoughtWeapon;
}