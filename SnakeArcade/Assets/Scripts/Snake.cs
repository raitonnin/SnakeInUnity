using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2 _direction = Vector2.right;
    private Rigidbody2D rb;
    private List<Transform> _segments = new List<Transform>();
    public Transform segmentPrefab;

    public int initialSize = 4;

    private KeyCode latestKey;

    private void Start()
    {
        ResetState();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && latestKey != KeyCode.S){
            _direction = Vector2.up;
            latestKey = KeyCode.W;
        } else if (Input.GetKeyDown(KeyCode.S) && latestKey != KeyCode.W) {
            _direction = Vector2.down;
            latestKey = KeyCode.S;
        } else if (Input.GetKeyDown(KeyCode.D) && latestKey != KeyCode.A) {
            _direction = Vector2.right;
            latestKey = KeyCode.D;
        } else if (Input.GetKeyDown(KeyCode.A) && latestKey != KeyCode.D) {
            _direction = Vector2.left;
            latestKey = KeyCode.A;
        }
    }
    private void FixedUpdate()
    {
        //account for movement in reverse of which part in the segment furthest back moves forward first, then each in front of it moves one in order of back to front
        for (int i = _segments.Count -1; i> 0; i--){
            _segments[i].position = _segments[i - 1].position;
        }
        
        this.transform.position = new Vector3(
            Mathf.Round(this.transform.position.x) + _direction.x,
            Mathf.Round(this.transform.position.y) + _direction.y,
            0.0f
        );
    }
    private void Grow()
    {
        //create new transform, call it segment
        Transform segment = Instantiate(this.segmentPrefab);
        //decide where it goes (same place as the farthest back segment)
        segment.position =_segments[_segments.Count - 1].position;
        //add to list
        _segments.Add(segment);
    }
    private void ResetState()
    {
        for (int i = 1; i < _segments.Count; i++){
            Destroy(_segments[i].gameObject);
        }

        _segments.Clear();
        _segments.Add(this.transform);

        for (int i = 1; i < this.initialSize; i++){
            _segments.Add(Instantiate(this.segmentPrefab));
        }

        this.transform.position = Vector3.zero;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Apple"){
            Grow();
        }
        if (other.tag == "Obstacle"){
            ResetState();
        }
    }
}
