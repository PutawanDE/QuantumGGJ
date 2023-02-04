using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject[] characters;
    [SerializeField] private Vector2 enemySpawnPoint;

    public void NextRound(GameObject nextChar)
    {
        GameObject player = Instantiate(nextChar, nextChar.GetComponent<Transform>().position, Quaternion.identity);
        player.GetComponent<Character>().Initialize("Player");

        GameObject enemyToSpawn = characters[Mathf.FloorToInt(Random.Range(0, characters.Length))];
        Instantiate(enemyToSpawn, enemySpawnPoint, Quaternion.identity);
    }

    public void GameOver()
    {
        
    }
}
