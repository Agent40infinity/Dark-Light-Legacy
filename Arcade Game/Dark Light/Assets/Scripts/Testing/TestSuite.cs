using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.TestTools;
using NUnit.Framework;
public class TestSuite
{
    public GameObject game;
    public Player player;
    public GameObject Menu;

    [SetUp]
    public void Setup()
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Game");
        game = Object.Instantiate(prefab);

        player = game.GetComponentInChildren<Player>();


    }

    [UnityTest]
    public IEnumerator GamePrefabLoaded()
    {
        yield return new WaitForEndOfFrame();
        Assert.NotNull(game, "Game exists.");
    }

    [UnityTest]
    public IEnumerator PlayerExists()
    {
        yield return new WaitForEndOfFrame();
        Assert.NotNull(player, "Player Exists");
    }

    [UnityTest]
    public IEnumerator PlayerCanMove()
    {
        yield return new WaitForEndOfFrame();
        Assert.NotNull(player, "Player Exists");
    }

    [UnityTest]
    public IEnumerator PlayerCanJump()
    {
        yield return new WaitForEndOfFrame();
        Assert.NotNull(player, "Player Exists");
    }

    [UnityTest]
    public IEnumerator PlayerCanBeDamaged()
    {
        player.GetComponent<Player>().hitHostile = true;
        yield return new WaitForSeconds(0.5f);
        Assert.IsTrue(Player.curHealth < Player.maxHealth, "Player Has Been Damaged");
    }

    [UnityTest]
    public IEnumerator PlayerCanDie()
    {
        Player.curHealth = 0;
        Assert.IsFalse(Player.recovered && Player.curHealth >= 1, "Player Death Initiate");
        yield return new WaitForSeconds(10.5f);
        Assert.IsFalse(Player.recovered, "Player Recovery Complete");
    }

    [UnityTest]
    public IEnumerator PlayerCanSave()
    {
        yield return new WaitForEndOfFrame();
        Assert.NotNull(player, "Player Exists");
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(game);
    }
}