using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
  public IEnumerator Shake (float m_duration, float m_magnitude)
    {
        Vector3 originalPos = transform.localPosition;

        float m_elapsed = 0.0f;

        while (m_elapsed < m_duration)
        {
            float x = Random.Range(-1f, 1f) * m_magnitude;
            float y = Random.Range(-1f, 1f) * m_magnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            m_elapsed += Time.deltaTime;

            yield return null;

        }

        transform.localPosition = originalPos;
    }
}
