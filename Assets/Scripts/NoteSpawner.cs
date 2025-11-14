using UnityEngine;
using System.Collections;

public class NoteSpawner : MonoBehaviour
{
    [Header("Tap Note Prefabs")]
    public GameObject leftTapPrefab;
    public GameObject downTapPrefab;
    public GameObject upTapPrefab;
    public GameObject rightTapPrefab;

    [Header("Hold Note Prefabs")]
    public GameObject leftHoldPrefab;
    public GameObject downHoldPrefab;
    public GameObject upHoldPrefab;
    public GameObject rightHoldPrefab;

    [Header("Spawn Points")]
    public Transform leftSpawn;
    public Transform downSpawn;
    public Transform upSpawn;
    public Transform rightSpawn;

    [Header("Timing")]
    public float spawnDelay = 1f;
    public float startDelay = 0.5f;

    [Header("Hold Settings")]
    public float minHoldDuration = 1.0f;
    public float maxHoldDuration = 2.5f;
    [Range(0f, 1f)] public float holdNoteChance = 0.3f;
    public float laneCooldown = 0.5f; // how long to wait after hold ends

    // Lane tracking
    private bool[] laneLocked = new bool[4];
    private float[] laneCooldownTimer = new float[4];

    void Start()
    {
        StartCoroutine(SpawnNotes());
    }

    void Update()
    {
        // Update cooldown timers
        for (int i = 0; i < laneCooldownTimer.Length; i++)
        {
            if (laneCooldownTimer[i] > 0)
            {
                laneCooldownTimer[i] -= Time.deltaTime;
                if (laneCooldownTimer[i] <= 0)
                    laneLocked[i] = false;
            }
        }
    }

    IEnumerator SpawnNotes()
    {
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            int lane = Random.Range(0, 4);
            bool spawnHold = Random.value < holdNoteChance;

            // Skip locked lanes
            if (laneLocked[lane])
            {
                yield return new WaitForSeconds(spawnDelay);
                continue;
            }

            GameObject prefabToSpawn = null;
            Transform spawnPoint = null;

            switch (lane)
            {
                case 0: // Left lane
                    if (spawnHold && leftHoldPrefab != null)
                    {
                        prefabToSpawn = leftHoldPrefab;
                        spawnPoint = leftSpawn;
                        laneLocked[lane] = true;
                    }
                    else
                    {
                        prefabToSpawn = leftTapPrefab;
                        spawnPoint = leftSpawn;
                    }
                    break;

                case 1: // Down lane
                    if (spawnHold && downHoldPrefab != null)
                    {
                        prefabToSpawn = downHoldPrefab;
                        spawnPoint = downSpawn;
                        laneLocked[lane] = true;
                    }
                    else
                    {
                        prefabToSpawn = downTapPrefab;
                        spawnPoint = downSpawn;
                    }
                    break;

                case 2: // Up lane
                    if (spawnHold && upHoldPrefab != null)
                    {
                        prefabToSpawn = upHoldPrefab;
                        spawnPoint = upSpawn;
                        laneLocked[lane] = true;
                    }
                    else
                    {
                        prefabToSpawn = upTapPrefab;
                        spawnPoint = upSpawn;
                    }
                    break;

                case 3: // Right lane
                    if (spawnHold && rightHoldPrefab != null)
                    {
                        prefabToSpawn = rightHoldPrefab;
                        spawnPoint = rightSpawn;
                        laneLocked[lane] = true;
                    }
                    else
                    {
                        prefabToSpawn = rightTapPrefab;
                        spawnPoint = rightSpawn;
                    }
                    break;
            }

            if (prefabToSpawn != null)
            {
                GameObject note = Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.identity);

                // If it's a hold note, randomize the duration and handle cooldown when it ends
                var holdScript = note.GetComponent<HoldNoteTrail>();
                if (holdScript != null)
                {
                    float duration = Random.Range(minHoldDuration, maxHoldDuration);
                    holdScript.holdDuration = duration;

                    // Unlock lane after hold + cooldown
                    StartCoroutine(UnlockLaneAfter(lane, duration + laneCooldown));
                }
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    IEnumerator UnlockLaneAfter(int lane, float delay)
    {
        yield return new WaitForSeconds(delay);
        laneLocked[lane] = false;
        laneCooldownTimer[lane] = laneCooldown;
    }
}