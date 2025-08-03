using System;
using System.Collections;
using MalbersAnimations;
using MalbersAnimations.InputSystem;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Nice, easy to understand enum-based game manager. For larger and more complex games, look into
/// state machines. But this will serve just fine for most games.
/// </summary>
public class GameManager : StaticInstance<GameManager>
{
    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;

    public GameState State { get; private set; }

    // Kick the game off with the first state
    void Start() => ChangeState(GameState.Starting);

    public void ChangeState(GameState newState)
    {
        OnBeforeStateChanged?.Invoke(newState);

        State = newState;
        switch (newState)
        {
            case GameState.Starting:
                HandleStarting();
                break;
            case GameState.BeginCycles:
                HandleBeginCycles();
                break;
            case GameState.SpawningEnemies:
                HandleSpawningEnemies();
                break;
            case GameState.HeroTurn:
                HandleHeroTurn();
                break;
            case GameState.EnemyTurn:
                break;
            case GameState.Win:
                break;
            case GameState.Lose:
                HandleLose();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnAfterStateChanged?.Invoke(newState);

        Debug.Log($"New state: {newState}");
    }

    public void RestartGame()
    {
        cam.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (_robot != null)
        {
            Rigidbody rb = _robot.GetComponent<Rigidbody>();

            // 1. Gèle temporairement le Rigidbody
            rb.isKinematic = true;

            // 2. Replace à la position de départ
            _robot.transform.position = new Vector3(-40f, 2f, 1.1f);
            _robot.transform.rotation = Quaternion.Euler(0f, 11f, 0f);

            // 3. (optionnel) Réinitialise l’échelle si tu l’avais modifiée
            _robot.transform.localScale = Vector3.one;

            // 4. Réactive le Rigidbody
            rb.isKinematic = false;

            // 5. Remet les vitesses à zéro (par sécurité)
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            // _robot.transform.position = new Vector3(-40f, 2f, 1.1f);
            // _robot.transform.rotation = Quaternion.Euler(0f, 11f, 0f);

            var input = _robot.GetComponent<MInputLink>();
            if (input != null) input.SetEnable(true);

            if (_burnParticlesRoot != null)
                _burnParticlesRoot.SetActive(false);
        }

        _restartButton.gameObject.SetActive(false);

        ChangeState(GameState.Starting);
    }

    [SerializeField] private Button _restartButton;
    [SerializeField] private GameObject cam;

    private void HandleStarting()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ChangeState(GameState.BeginCycles);
    }

    private void HandleBeginCycles()
    {

        CyclesManager.Instance.BeginCycles();

        // ChangeState(GameState.SpawningEnemies);
    }

    private void HandleSpawningEnemies()
    {

        // Spawn enemies

        ChangeState(GameState.HeroTurn);
    }

    private void HandleHeroTurn()
    {
        // If you're making a turn based game, this could show the turn menu, highlight available units etc

        // Keep track of how many units need to make a move, once they've all finished, change the state. This could
        // be monitored in the unit manager or the units themselves.
    }

    [SerializeField] GameObject _robot;
    [SerializeField] GameObject _burnParticlesRoot;
    private void HandleLose()
    {
        cam.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        var input = _robot.GetComponent<MInputLink>();
        if (input != null) input.SetEnable(false);

        if (_burnParticlesRoot == null) return;
        StartCoroutine(PlayParticlesCoroutine());
    }

    private IEnumerator PlayParticlesCoroutine()
    {
        yield return new WaitForSeconds(.3f);

        _burnParticlesRoot.SetActive(true);

        foreach (var ps in _burnParticlesRoot.GetComponentsInChildren<ParticleSystem>())
        {
            ps.Play();
        }

        yield return new WaitForSeconds(2f);
        _restartButton.gameObject.SetActive(true);
    }
}

/// <summary>
/// This is obviously an example and I have no idea what kind of game you're making.
/// You can use a similar manager for controlling your menu states or dynamic-cinematics, etc
/// </summary>
[Serializable]
public enum GameState
{
    Starting = 0,
    BeginCycles = 1,
    SpawningEnemies = 2,
    HeroTurn = 3,
    EnemyTurn = 4,
    Win = 5,
    Lose = 6,
}