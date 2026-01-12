using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class HttpUtils : MonoBehaviour
{
    private static HttpUtils _Instance;

    private static HttpUtils Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new GameObject("HttpUtils").AddComponent(typeof(HttpUtils)) as HttpUtils;
                DontDestroyOnLoad(_Instance);
            }

            return _Instance;
        }
    }
    
    public static void SendRequest(string method, string url,
        Action<string> onSuccess, Action<string> onFailure = null, string jsonData = "")
    {
        Instance.StartCoroutine(Request(method, url, onSuccess, onFailure, jsonData));
    }

    private static IEnumerator Request(string method, string url,
        Action<string> onSuccess, Action<string> onFailure, string jsonData)
    {

        using (UnityWebRequest request = new UnityWebRequest(url, method))
        {
            var bodyRaw = (jsonData != null && jsonData.Length > 0) ? Encoding.UTF8.GetBytes(jsonData) : null;
            request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
            if (!string.IsNullOrEmpty(jsonData)) request.SetRequestHeader("content-type", "application/json");
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                onFailure?.Invoke(request.error);
            }
            else
            {
                var data = request.downloadHandler.text;
                onSuccess(data);
            }
        }
    }
    
    public static void SendPostRequest(string url, byte[] bytes, string filename,
        Action<string> onSuccess, Action<string> onFailure = null)
    {
        Instance.StartCoroutine(RequestFromData(url, onSuccess, onFailure, bytes, filename));
    }
    private static IEnumerator RequestFromData(string url, Action<string> onSuccess, Action<string> onFailure, byte[] dataBytes, string filename)
    {
        var boundary = "----" + DateTime.Now.Ticks.ToString("x");
        var boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

        var headerTemplate =
            "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
        var header = string.Format(headerTemplate, "picture", filename, "image/jpeg");
        var headerBytes = Encoding.UTF8.GetBytes(header);
        var trailer = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");

        var bd = new byte[boundaryBytes.Length + headerBytes.Length + dataBytes.Length + trailer.Length];
        Buffer.BlockCopy(boundaryBytes, 0, bd, 0, boundaryBytes.Length);
        Buffer.BlockCopy(headerBytes, 0, bd, boundaryBytes.Length, headerBytes.Length);
        Buffer.BlockCopy(dataBytes, 0, bd, boundaryBytes.Length + headerBytes.Length, dataBytes.Length);
        Buffer.BlockCopy(trailer, 0, bd, boundaryBytes.Length + headerBytes.Length + dataBytes.Length,
            trailer.Length);

        var request = new UnityWebRequest(url) {method = "POST"};
        var uploader = new UploadHandlerRaw(bd);
        var downloader = new DownloadHandlerBuffer();
        uploader.contentType = "multipart/form-data; boundary=" + boundary;
        request.SetRequestHeader("content-type", "multipart/form-data; boundary=" + boundary);

        request.uploadHandler = uploader;
        request.downloadHandler = downloader;

        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            onFailure.Invoke(request.error);
        }
        else
        {
            var response = request.downloadHandler.text;
            Debug.Log(response);
            onSuccess.Invoke(response);
        }
    }
}