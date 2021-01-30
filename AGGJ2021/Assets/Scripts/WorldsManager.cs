using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldsManager : MonoBehaviour
{
    AudioSource[] audioSources;

    AudioSource realWorld;
    AudioSource lostWorld;
    AudioClip realWorldClip;
    AudioClip lostWorldClip;

    public bool isInRealWorld = true;

    public float fadeTime = 2f;
    float fadeStartTime;


    float realWorldFade = 1f;
    float lostWorldFade = 0f;



    // Start is called before the first frame update
    void Start()
    {
        // Loading tracks
        audioSources = GetComponents<AudioSource>();
        realWorld = audioSources[0];
        lostWorld = audioSources[1];
        Debug.Log(lostWorld);
        realWorldClip = (AudioClip)Resources.Load("Sounds/gamekid_realworldnew2");
        lostWorldClip = (AudioClip)Resources.Load("Sounds/gamekid_surealworldnew1");
        realWorld.clip = realWorldClip;
        lostWorld.clip = lostWorldClip;
        realWorld.Play();
        lostWorld.Play();
        realWorld.volume = 1f;
        lostWorld.volume = 0f;

        // So that we start already fully faded
        fadeStartTime = 0 - fadeTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            fadeStartTime = Time.time;
            isInRealWorld = !isInRealWorld; //Just so we can get a state on it
        }

        float timePassed = Time.time - fadeStartTime;
        float fade = Mathf.Lerp((isInRealWorld ? 0 : 1), (isInRealWorld ? 1 : 0), timePassed / fadeTime);

        realWorld.volume = fade;
        lostWorld.volume = 1 - fade;

        realWorldFade = fade;
        lostWorldFade = 1 - fade;
        TextureFade("Real World", realWorldFade);
        TextureFade("Lost World", lostWorldFade);

        if(!realWorld.isPlaying && !lostWorld.isPlaying) //If tracks finish play them again simultaniously
        {
            realWorld.Play();
            lostWorld.Play();
        }

    }

    IEnumerator FadeTo(float aValue, float bValue, float overTime)
    {

        float startTime = Time.time;
        float targetTime = startTime + overTime;
        while(startTime < targetTime)
        {
            float timePassed = Time.time - startTime;
            float fade = timePassed / overTime;
            realWorldFade = Mathf.Lerp(aValue, bValue, fade);
            yield return null;
        }
        yield return bValue;

    }

    void TextureFade(string tag, float fade)
    {
        if (fade < 1)
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag(tag))
            {
                foreach (Material m in obj.GetComponent<Renderer>().materials)
                {
                    StandardShaderUtils.ChangeRenderMode(m, StandardShaderUtils.BlendMode.Transparent);
                    Color newColor = m.color;
                    newColor.a = fade;
                    m.color = newColor;
                }
            }
        }
        else
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag(tag))
            {
                foreach (Material m in obj.GetComponent<Renderer>().materials)
                {
                    StandardShaderUtils.ChangeRenderMode(m, StandardShaderUtils.BlendMode.Opaque);
                }
            }
        }
    }
}
