using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    const int MaxResults = 20;

    [SerializeField]
    Text places, names, platforms, scores;

    DreamloManager dreamloManager;
    DreamloManager.EntryDownloadFinishAwaiter waiter;
    bool loaded = false;

    void Start()
    {
        dreamloManager = DreamloManager.Instance;
        waiter = dreamloManager.GetEntries();
    }

    private void Update()
    {
        DreamloManager.Entry[] results = null;

        if (!loaded)
        {
            if (waiter.OK)
            {
                results = waiter.Data;
                loaded = true;

                places.text = string.Empty;
                names.text = string.Empty;
                platforms.text = string.Empty;
                scores.text = string.Empty;
                
                for (int i = 0; i < results.Length; ++i)
                {
                    string platform = ((RuntimePlatform)results[i].secondValue).ToString();

                    places.text += (i + 1).ToString() + ".\n";
                    names.text += results[i].key + "\n";
                    platforms.text += platform.Substring(0, Mathf.Min(8, platform.Length)) + "\n";
                    scores.text += results[i].firstValue + "\n";
                }
            }
            else
            {
                places.text = "1.";
                names.text = "Loading...";
                platforms.text = "Loading...";
                scores.text = "Loading...";
            }
        }
    }
}
