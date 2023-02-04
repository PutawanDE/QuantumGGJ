using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject[] characters;
    [SerializeField] private Vector2 playerSpawnPoint;
    [SerializeField] private Vector2 enemySpawnPoint;
    
    private GameObject player;
    private GameObject enemy;

    private void Start()
    {
        SpawnCharacters(RandomCharacter(), RandomCharacter());
    }

    public void NextRound(GameObject nextChar)
    {
        playerSpawnPoint = nextChar.transform.position;
        SpawnCharacters(nextChar, RandomCharacter());
    }

    private void SpawnCharacters(GameObject player, GameObject enemy)
    {
        player = Instantiate(player, playerSpawnPoint, Quaternion.identity);
        player.GetComponent<Character>().Initialize("Player");

        enemy = Instantiate(enemy, enemySpawnPoint, Quaternion.identity);
        enemy.GetComponent<Character>().Initialize("Enemy");
    }

    public void GameOver()
    {

    }

    private GameObject RandomCharacter()
    {
        return characters[Mathf.FloorToInt(Random.Range(0, characters.Length))];
    }
}
