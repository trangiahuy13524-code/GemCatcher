using UnityEngine;

public class GroundTrigger : MonoBehaviour
{
    [SerializeField] PlayerController controller;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            controller.InGround();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            controller.OffGround();
        }
    }
}
