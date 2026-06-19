using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ShowFPS : MonoBehaviour
{
    public bool showFPS = true;
    public TextMeshProUGUI fpsText;

    float timer;
    float totalTime;
    int frameCount;

    Queue<float> frameTimes = new Queue<float>();
    const int sampleSize = 60;

    void Start()
    {
        if (fpsText == null)
        {
            Debug.LogError("FPS Text is not assigned.");
            enabled = false;
            return;
        }
        fpsText.gameObject.SetActive(showFPS);
    }

    void Update()
    {
        if (!showFPS) return;
        float dt = Time.unscaledDeltaTime;

        timer += dt;
        totalTime += dt;
        frameCount++;

        frameTimes.Enqueue(dt);
        if (frameTimes.Count > sampleSize)
            frameTimes.Dequeue();

        if (timer >= 0.2f)
        {
            float currentFPS = 1f / dt;
            float avgFPS = frameCount / totalTime;

            float maxFrameTime = 0f;
            foreach (float t in frameTimes)
            {
                if (t > maxFrameTime)
                    maxFrameTime = t;
            }

            float worstFPS = 1f / maxFrameTime;

            fpsText.text =
                $"FPS: {currentFPS:F0}\n" +
                $"AVG: {avgFPS:F0}\n" +
                $"1% Low: {worstFPS:F0}\n" +
                $"Frame: {(dt * 1000f):F1} ms";

            timer = 0f;
        }
    }
}