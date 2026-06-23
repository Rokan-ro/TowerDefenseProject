using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class HealthCheckTester : MonoBehaviour
{
    [Header("Backend Configuration")]
    [SerializeField]
    private string baseUrl = "https://localhost:7103";

    private void Start()
    {
        StartCoroutine(CheckBackendHealth());
    }

    private IEnumerator CheckBackendHealth()
    {
        string url = $"{baseUrl.TrimEnd('/')}/api/health";

        Debug.Log($"Sending health request to: {url}");

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.timeout = 10;
            request.SetRequestHeader("Accept", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(
                    $"Health request failed.\n" +
                    $"URL: {url}\n" +
                    $"HTTP Status: {request.responseCode}\n" +
                    $"Error: {request.error}\n" +
                    $"Response: {request.downloadHandler?.text}"
                );

                yield break;
            }

            string json = request.downloadHandler.text;

            Debug.Log($"Backend response: {json}");

            HealthCheckResponse response =
                JsonUtility.FromJson<HealthCheckResponse>(json);

            if (response == null || response.data == null)
            {
                Debug.LogError(
                    "Backend responded, but the JSON could not be parsed."
                );

                yield break;
            }

            if (!response.success)
            {
                Debug.LogError(
                    $"Backend reported an unhealthy state: {response.message}"
                );

                yield break;
            }

            Debug.Log(
                $"Connection successful.\n" +
                $"API: {response.data.apiStatus}\n" +
                $"Database: {response.data.databaseStatus}\n" +
                $"Framework: {response.data.framework}\n" +
                $"Database Server: {response.data.database}"
            );
        }
    }
}