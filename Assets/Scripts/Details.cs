
using UnityEngine;

public class Details : MonoBehaviour
{
    public float distance = 50f;
    private Transform _player;

    void Start()
    {
        Invoke(nameof(UpdateDetail), Random.Range(0, 0.5f));
        _player = FindObjectOfType<DetailsFollowPoint>().transform;
        FindObjectOfType<GameManager>().details.Add(this);
    }

    public void ReturnToStart(float distance)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - distance);
    }

    private void Update()
    {
       // UpdateDetail();
    }

    void UpdateDetail()
    {
        Invoke(nameof(UpdateDetail), 0.5f);


        if (transform.position.z - _player.position.z > distance)
        {
            transform.position += Vector3.forward * distance * -2f;
            return;
        }
        if (_player.position.z - transform.position.z > distance)
        {
            transform.position += Vector3.forward * distance * 2f;
        }
    }
}
