using System.Collections;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [Header("Room Settings")]
    public GameObject[] enemyPrefabs;
   public Transform[] spawnPoints;
    public GameObject[] doors;
    public GameObject spawnVFX;
    public AudioClip roomEnterSound;
    public int waves = 2;
    public int enemiesPerWave = 3;

    private int currentWave = 0;
    private int enemiesAlive = 0;
    private bool roomActive = false;
    private AudioSource audioSource;

    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null )
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!roomActive && other.CompareTag("Player1"))
        {
            StartCoroutine(StartRoom());
            Debug.Log("Player Detected");
        }
    }

    private IEnumerator StartRoom()
    {
        roomActive = true;
        Debug.Log("Waves started");

        // Lock doors

        foreach (GameObject door in doors) 
        if (doors != null) door.SetActive(true);

        // VFX + Sound

        if (spawnVFX != null)
            Instantiate(spawnVFX, transform.position, Quaternion.identity);

        if (roomEnterSound != null)
            audioSource.PlayOneShot(roomEnterSound);

        yield return new WaitForSeconds(1f);

        Debug.Log("Calling SpawnWave()");
        SpawnWave();
    }

    private void SpawnWave()
    {
        Debug.Log("Waves start spawning now");
        currentWave++;
        enemiesAlive = 0;

        for (int i = 0; i < enemiesPerWave; i++)
        {

            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // VFX first
            if (spawnVFX != null)
            {
                GameObject fx = Instantiate(spawnVFX, spawnPoint.position, Quaternion.identity);
                Destroy(fx, 2f); // cleanup
            }

            StartCoroutine(SpawnEnemyWithDelay(spawnPoint, 0.5f));

           
        }
    }

    private IEnumerator SpawnEnemyWithDelay(Transform spawnPoint, float delay)
    {
        yield return new WaitForSeconds(delay);

         GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

                // Subscribe to death event
        AttributesManager am = enemy.GetComponent<AttributesManager>();
        if (am != null)
        {
            am.OnDeath += HandleEnemyDeath;
        }

        enemiesAlive++;
    }

    private void HandleEnemyDeath()
    {
        enemiesAlive--;

        if (enemiesAlive <= 0 && currentWave < waves)
        {
            SpawnWave();

        }
        else
        {
            // Room Complete -> Unlock doors
            foreach (GameObject door in doors) door.SetActive(false);


            Debug.Log("Room Cleared!");

        }
    }

}
