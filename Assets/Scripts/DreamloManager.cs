using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class DreamloManager : MonoBehaviour
{
    [SerializeField]
    string publicKey, privateKey;

    static public DreamloManager Instance { get; private set; } = null;
    const string server = "http://dreamlo.com/lb/";

    private void Start()
    {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;
    }

    public class Entry
    {
        public int firstValue, secondValue;
        public string text;
        public DateTime uploadTime;

        public string key;
        public int index;
    }

    public void AddEntry(Entry entry)
    {
        UnityWebRequest request = UnityWebRequest.Get(
            server + privateKey + "/add/"
            + UnityWebRequest.EscapeURL(entry.key)
            + "/" + entry.firstValue
            + "/" + entry.secondValue
            + "/" + (string.IsNullOrWhiteSpace(entry.text) ? "null" : entry.text));

        new EntryDownloadFinishAwaiter(request, false);
    }
    public void DeleteEntry(string key)
    {
        UnityWebRequest request = UnityWebRequest.Get(
            server + privateKey + "/delete/"
            + UnityWebRequest.EscapeURL(Clear(key)));

        new EntryDownloadFinishAwaiter(request, false);
    }
    public void ClearAllEntries()
    {
        UnityWebRequest request = UnityWebRequest.Get(
            server + privateKey + "/clear");

        new EntryDownloadFinishAwaiter(request, false);
    }
    public EntryDownloadFinishAwaiter GetEntries(uint startIndex, uint endIndex)
    {
        UnityWebRequest request = UnityWebRequest.Get(
            server + publicKey + "/pipe/"
            + startIndex
            + "/" + endIndex);

        return new EntryDownloadFinishAwaiter(request, true);
    }
    public EntryDownloadFinishAwaiter GetEntries()
    {
        UnityWebRequest request = UnityWebRequest.Get(
            server + publicKey + "/pipe");

        return new EntryDownloadFinishAwaiter(request, true);
    }
    public EntryDownloadFinishAwaiter GetEntry(string key)
    {
        UnityWebRequest request = UnityWebRequest.Get(
            server + publicKey + "/pipe-get/"
            + UnityWebRequest.EscapeURL(Clear(key)));

        return new EntryDownloadFinishAwaiter(request, true);
    }

    string Clear(string name)
    {
        return name.Replace('|', ' ').Replace('/', ' ').Replace('\\', ' ').Replace('*', ' ');
    }

    public class EntryDownloadFinishAwaiter
    {
        IEnumerator AwaitWebRequestRoutine(UnityWebRequest request)
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Error = true;
                Debug.LogWarning(request.error);
            }
            if (request.isHttpError)
            {
                Error = true;
                Debug.LogWarning("HTTP error " + request.responseCode);
            }

            if (!string.IsNullOrWhiteSpace(request.downloadHandler.text) && !Error && recieveData)
            {
                string[] strings = request.downloadHandler.text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                Entry[] entries = new Entry[strings.Length];
                for (int i = 0; i < strings.Length; ++i)
                {
                    string[] infos = strings[i].Split(new char[] { '|' });
                    entries[i] = new Entry
                    {
                        key = infos[0],
                        firstValue = Convert.ToInt32(infos[1]),
                        secondValue = Convert.ToInt32(infos[2]),
                        text = infos[3],
                        uploadTime = Convert.ToDateTime(infos[4]),
                        index = Convert.ToInt32(infos[5]),
                    };
                }

                Data = entries;
            }
        }

        public EntryDownloadFinishAwaiter(UnityWebRequest request, bool recieveData)
        {
            this.recieveData = recieveData;
            Data = null;
            task = new Task(AwaitWebRequestRoutine(request), true);
            task.Finished += Task_Finished;
        }

        private void Task_Finished(bool manual)
        {
            task.Finished -= Task_Finished;
            OnFinish?.Invoke(manual);
        }

        public event Task.FinishedHandler OnFinish;
        public bool Finished => !task.Running;
        public bool OK => !Error && Finished && (recieveData ? Data != null : true);
        public bool Error { get; private set; }
        public Entry[] Data { get; private set; }

        readonly Task task;
        readonly bool recieveData;
    }
}
