using System.Collections;
using System.Collections.Generic;
using LSL;
using LSL4Unity.Utils;

using UnityEngine;

public class OpenVibeReceiver : MonoBehaviour
{
    #region
    public string StreamName;
    ContinuousResolver resolver;
    double max_chunk_duration = 2.0;
    private StreamInlet inlet;

    private float[,] data_buffer;
    private double[] timestamp_buffer;
    float EEGpow;
    [SerializeField] float contentrationValue;
    bool isSatisfied = false;
    #endregion

    private void Awake()
    {
        if(!StreamName.Equals(""))
            resolver = new ContinuousResolver("name", StreamName);
        else
        {
            Debug.LogError("specify a name");
            this.enabled = false;
            return;
        }
        StartCoroutine(ResolveExpectedStream());
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(inlet != null)
        {
            int samples_returned = inlet.pull_chunk(data_buffer, timestamp_buffer);
            if(samples_returned > 0)
            {
                float x = data_buffer[samples_returned - 1, 0];

                Debug.Log(x);
                EEGpow = x;
                if(EEGpow >= contentrationValue)
                {
                    isSatisfied = true;
                }
                else
                {
                    isSatisfied = false;
                }

                string output = EEGpow.ToString() + " " + isSatisfied;
                Debug.Log(output);
            }
        }
    }

    IEnumerator ResolveExpectedStream()
    {
        var results = resolver.results();
        while (results.Length == 0)
        {
            yield return new WaitForSeconds(.1f);
            results = resolver.results();
        }
        inlet = new StreamInlet(results[0]);
        int buf_samples = (int)Mathf.Ceil((float)(inlet.info().nominal_srate() * max_chunk_duration));
        int n_chhannels = inlet.info().channel_count();
        data_buffer = new float[buf_samples, n_chhannels];
        timestamp_buffer = new double[buf_samples];
    }

    public bool OpenVibeConnected()
    {
        return inlet != null;
    }

    public bool GetIsSatisfied()
    {
        return isSatisfied;
    }
}
