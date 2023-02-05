using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using TMPro;


public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject[] characters;
    [SerializeField] private Vector2 playerSpawnPoint;
    [SerializeField] private Vector2 enemySpawnPoint;
    [SerializeField] private CinemachineVirtualCamera cam;
    
    [SerializeField] private Scoreboard scoreboard;
    [SerializeField] private string[] narrations;
    [SerializeField] private TextMeshProUGUI textUI;

    [Header("Character FX Prefab")]
    [SerializeField] private GameObject bloodSprout;
    [SerializeField] private GameObject dropSmoke;

    [Header("Enemy Color")]
    [SerializeField] private Color enemyColor;

    public GameObject player;
    public GameObject enemy;

    private bool isCutscene = false;
    private int round;

    private void Awake()
    {
        scoreboard.CurrentScore = 0;
        enemy = Instantiate(RandomCharacter(), enemySpawnPoint, Quaternion.identity);
        enemy.GetComponent<Character>().Initialize("Enemy");
        enemy.GetComponent<SpriteRenderer>().color = enemyColor;

        enemy.name = enemy.name.Replace("(Clone)", "");

        player = Instantiate(RandomCharacter(), playerSpawnPoint, Quaternion.identity);
        player.GetComponent<Character>().Initialize("Player");

        player.name = player.name.Replace("(Clone)", "");

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

        ShowNarration(narrations[0]);
    }

    public void NextRound()
    {
        scoreboard.CurrentScore += 10;
        round++;

        enemy = player;
        enemy.GetComponent<Character>().Initialize("Enemy");
        enemy.GetComponent<SpriteRenderer>().color = enemyColor;

        ShowNarration(narrations[round % narrations.Length]);

        player = Instantiate(RandomCharacter(), playerSpawnPoint, Quaternion.identity);
        player.GetComponent<Character>().Initialize("Player");

        player.name = player.name.Replace("(Clone)", "");

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

    void Update()
    {
        if (isCutscene)
        {
            if (Input.GetKey(KeyCode.F))
            {
                CloseNarration();
            }
        }
    }

    void ShowNarration(string text) 
    {
        textUI.transform.parent.gameObject.SetActive(true);
        textUI.text = text + " Press F to continue...";
        isCutscene = true;
        Time.timeScale = 0;
    }

    void CloseNarration()
    {
        textUI.transform.parent.gameObject.SetActive(false);
        textUI.text = "";
        isCutscene = false;
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        scoreboard.findMaxScore(scoreboard.CurrentScore);
        Debug.Log("IN?");
        SceneManager.LoadScene("GameOverScence");
    }

    private GameObject RandomCharacter()
    {
        return characters[((int)Random.Range(0, characters.Length))];
    }
}
