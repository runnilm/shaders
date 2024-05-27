using UnityEngine;

namespace OccaSoftware.ToonKit2.Demo
{
    public class Pan : MonoBehaviour
    {
        [SerializeField]
        Vector3 speed;

        // Update is called once per frame
        void Update()
        {
            transform.Translate(speed * Time.deltaTime);
        }
    }
}
