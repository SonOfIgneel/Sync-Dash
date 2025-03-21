using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Reference to Player
    private Vector3 offset;
    public float duration;
    public float magnitude;

    #region Camera Follow Player Functions
    public void Start()
    {
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, player.position.z + offset.z);
        }
    }
    #endregion

    #region Shake Functions
    public void Shake()
    {
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        Vector3 originalPosition = transform.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPosition + new Vector3(x, y, 0);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
    }
    #endregion

    public void SetPlayerReference(Transform playerRef)
    {
        player = playerRef;
    }
}
