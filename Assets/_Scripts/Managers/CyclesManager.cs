using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An example of a scene-specific manager grabbing resources from the resource system
/// Scene-specific managers are things like grid managers, unit managers, environment managers etc
/// </summary>
public class CyclesManager : StaticInstance<CyclesManager>
{
    [SerializeField] Transform _tiles;
    public Transform Tiles => _tiles; Dictionary<string, List<GameObject>> _tilePools = new Dictionary<string, List<GameObject>>();

    [Header("Gameplay")]
    [SerializeField] float _degreesPerSecondSpeed = 10f;
    [SerializeField] int _nbCycles = 3;

    public int NbCycles => _nbCycles;
    public void SetNbCycles(int v) => _nbCycles = v;
    public float NeedleAngleDeg { get; private set; } = 0f;
    public int CurrentSegmentIndex { get; private set; } = -1;
    public float DegreesPerCycle => 360f / _nbCycles;

    int _previousSegmentIndex = -1;

    void Update()
    {
        if (GameManager.Instance.State != GameState.BeginCycles)
            return;

        NeedleAngleDeg += Time.deltaTime * _degreesPerSecondSpeed;
        NeedleAngleDeg %= 360f;

        CurrentSegmentIndex = Mathf.FloorToInt(NeedleAngleDeg / DegreesPerCycle);

        if (CurrentSegmentIndex != _previousSegmentIndex)
        {
            _previousSegmentIndex = CurrentSegmentIndex;
            // Debug.Log($"Current Segment : {CurrentSegmentIndex} ({NeedleAngleDeg:0.00}°)");

            switch (CurrentSegmentIndex)
            {
                case 0:
                    DisableTiles("Red");
                    break;
                case 1:
                    DisableTiles("Blue");
                    break;
                case 2:
                    DisableTiles("Yellow");
                    break;
                case 3:
                    DisableTiles("Green");
                    break;
            }
        }
    }

    public void BeginCycles()
    {
        // SpawnUnit();
        _tilePools["Red"] = new List<GameObject>();
        _tilePools["Blue"] = new List<GameObject>();
        _tilePools["Yellow"] = new List<GameObject>();
        _tilePools["Green"] = new List<GameObject>();

        foreach (Transform child in _tiles)
        {
            if (child.tag == "Untagged")
            {
                child.gameObject.SetActive(false);
            }
            if (_tilePools.ContainsKey(child.tag))
            {
                _tilePools[child.tag].Add(child.gameObject);
            }
        }
    }

    public void DisableTiles(string tag)
    {
        // kvp = Key Value Pair
        foreach (var kvp in _tilePools)
        {
            bool shouldBeActive = kvp.Key != tag;
            foreach (var tile in kvp.Value)
            {
                tile.SetActive(shouldBeActive);
            }
        }
    }


    // void SpawnUnit(ExampleHeroType t, Vector3 pos) {
    //     var tarodevScriptable = ResourceSystem.Instance.GetExampleHero(t);

    //     var spawned = Instantiate(tarodevScriptable.Prefab, pos, Quaternion.identity,transform);

    //     // Apply possible modifications here such as potion boosts, team synergies, etc
    //     var stats = tarodevScriptable.BaseStats;
    //     stats.Health += 20;

    //     spawned.SetStats(stats);
    // }
}