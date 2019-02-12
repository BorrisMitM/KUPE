using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Death : MonoBehaviour {
    [SerializeField]
    private bool restartWholeLevel = false;
    private float respawnCoolDown = 1f;
    private float deathanimationDuration = .5f;
    private bool dead = false;
    public Animator animator;
    public static int deathCount;
    public static Dictionary<PlayerType, int> playerDeaths;
    PlayerType playerType;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if(SceneManager.GetActiveScene().name != "CharacterSelection")
        {
            deathCount = 0;
            playerType = GetComponent<CharacterInfo>().playerType;
            if(playerDeaths == null || playerDeaths.ContainsKey(playerType))
                playerDeaths = new Dictionary<PlayerType, int>();
            playerDeaths.Add(playerType, 0);
        }
    }
    public void Do()
    {
        if(restartWholeLevel)
            SceneLoader.LoadScene(SceneManager.GetActiveScene().name);
        else if(!dead)
        {
            dead = true;
            deathCount++;
            playerDeaths[playerType]++;
            GetComponent<InputManager>().Deactivate();
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<PlayerSounds>().Death();
            StartCoroutine(RespawnCooldown());
        }
    }

    IEnumerator RespawnCooldown()
    {
        yield return new WaitForSeconds(deathanimationDuration);
        transform.position = FindObjectOfType<PlayerSpawner>().GetFreeSpawnLocation(gameObject.name);
        dead = false;
        yield return new WaitForSeconds(respawnCoolDown);
        GetComponent<InputManager>().active = true;
    }
}
