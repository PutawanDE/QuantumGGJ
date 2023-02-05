using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject[] characters;
    [SerializeField] private Vector2 playerSpawnPoint;
    [SerializeField] private Vector2 enemySpawnPoint;
    [SerializeField] private CinemachineVirtualCamera cam;
    
    [SerializeField] private Scoreboard scoreboard;

    [Header("Character FX Prefab")]
    [SerializeField] private GameObject bloodSprout;
    [SerializeField] private GameObject dropSmoke;

    public GameObject player;
    public GameObject enemy;

    private void Awake()
    {
        enemy = Instantiate(RandomCharacter(), enemySpawnPoint, Quaternion.identity);
        enemy.GetComponent<Character>().Initialize("Enemy");

        player = Instantiate(RandomCharacter(), playerSpawnPoint, Quaternion.identity);
        player.GetComponent<Character>().Initialize("Player");
        cam.Follow = player.transform;

        if (bloodSprout != null)
        {
            enemy.GetComponent<Character>().bloodSprout = bloodSprout;
            player.GetComponent<Character>().bloodSprout = bloodSprout;
        }

        if (dropSmoke != null)
        {
            enemy.GetComponent<Character>().dropSmoke = dropSmoke;
            player.GetComponent<Character>().dropSmoke = dropSmoke;
        }

    }

    public void NextRound()
    {
        scoreboard.CurrentScore += 10;
        enemy = player;
        enemy.GetComponent<Character>().Initialize("Enemy");

        player = Instantiate(RandomCharacter(), playerSpawnPoint, Quaternion.identity);
        player.GetComponent<Character>().Initialize("Player");

        cam.Follow = player.transform;

        if (bloodSprout != null)
        {
            enemy.GetComponent<Character>().bloodSprout = bloodSprout;
            player.GetComponent<Character>().bloodSprout = bloodSprout;
        }

        if (dropSmoke != null)
        {
            enemy.GetComponent<Character>().dropSmoke = dropSmoke;
            player.GetComponent<Character>().dropSmoke = dropSmoke;
        }
    }

    public void GameOver()
    {
        scoreboard.findMaxScore(scoreboard.CurrentScore);
    }

    private GameObject RandomCharacter()
    {
        return characters[((int)Random.Range(0, characters.Length))];
    }
}
