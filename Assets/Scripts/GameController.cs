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

    private GameObject player;
    private GameObject enemy;

    private void Start()
    {
        enemy = Instantiate(RandomCharacter(), enemySpawnPoint, Quaternion.identity);
        enemy.GetComponent<Character>().Initialize("Enemy");

        player = Instantiate(RandomCharacter(), playerSpawnPoint, Quaternion.identity);
        player.GetComponent<Character>().Initialize("Player");
        cam.Follow = player.transform;
    }

    public void NextRound()
    {
        enemy = player;
        enemy.GetComponent<Character>().Initialize("Enemy");

        player = Instantiate(RandomCharacter(), playerSpawnPoint, Quaternion.identity);
        player.GetComponent<Character>().Initialize("Player");

        cam.Follow = player.transform;
    }

    public void GameOver()
    {

    }

    private GameObject RandomCharacter()
    {
        return characters[((int)Random.Range(0, characters.Length))];
    }
}
