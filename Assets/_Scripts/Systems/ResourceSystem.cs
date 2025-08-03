using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// One repository for all scriptable objects. Create your query methods here to keep your business logic clean.
/// I make this a MonoBehaviour as sometimes I add some debug/development references in the editor.
/// If you don't feel free to make this a standard class
/// </summary>
public class ResourceSystem : StaticInstance<ResourceSystem> {
    public List<ScriptableCycles> ExampleHeroes { get; private set; }
    // private Dictionary<ExampleHeroType, ScriptableCycles> _ExampleHeroesDict;

    protected override void Awake() {
        base.Awake();
        // AssembleResources();
    }

    // private void AssembleResources() {
    //     ExampleHeroes = Resources.LoadAll<ScriptableCycles>("ExampleHeroes").ToList();
    //     _ExampleHeroesDict = ExampleHeroes.ToDictionary(r => r.HeroType, r => r);
    // }

    // public ScriptableCycles GetExampleHero(ExampleHeroType t) => _ExampleHeroesDict[t];
    // public ScriptableCycles GetRandomHero() => ExampleHeroes[Random.Range(0, ExampleHeroes.Count)];
}   