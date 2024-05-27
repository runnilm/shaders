using UnityEngine;

namespace OccaSoftware.ToonKit2.Demo
{
    public class Oscillate : MonoBehaviour
    {
        Vector3 startPos;

        [SerializeField]
        float distance = .5f;

        [SerializeField]
        float speed = 0.5f;
        float randomOffset;

        // Start is called before the first frame update
        void Start()
        {
            startPos = transform.position;
            randomOffset = Random.Range(0f, 999f);
        }

        // Update is called once per frame
        void Update()
        {
            transform.position =
                startPos + Vector3.up * Mathf.Sin(speed * Time.time + randomOffset) * distance;
        }
    }
}
