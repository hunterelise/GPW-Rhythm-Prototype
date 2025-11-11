using UnityEngine;

public class NoteScroller : MonoBehaviour
{
    [Header("BPM Settings")]
    public float bpm = 120f;
    public float beatsAhead = 4f;  // how many beats before the target to spawn

    // Hard-coded target position 
    private Vector3 targetPosition = new Vector3(0f, 2f, 0f);

    private float noteSpeed;

    void Start()
    {
        // Distance between spawn point and target Y
        float distance = transform.position.y - targetPosition.y;

        // Time per beat (seconds)
        float secondsPerBeat = 60f / bpm;

        // Total travel time based on beatsAhead
        float travelTime = beatsAhead * secondsPerBeat;

        // Speed = distance / time
        noteSpeed = distance / travelTime;
    }

    void Update()
    {
        transform.position -= new Vector3(0f, noteSpeed * Time.deltaTime, 0f);
    }
}