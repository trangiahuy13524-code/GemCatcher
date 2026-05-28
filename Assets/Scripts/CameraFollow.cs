using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform cameratf;
    [SerializeField] Vector3 offset;
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (cameratf != null)
        {
            transform.position = cameratf.position + offset;
        }
    }
}
