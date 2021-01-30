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

    public float fade = 1f;
    public float fadeRate = 0.01f;

    float realWorldFade = 1f;
    float lostWorldFade = 0f;



    // Start is called before the first frame update
    void Start()
    {
        // Loading tracks
        audioSources = GetComponents<AudioSource>();
        realWorld = audioSources[0];
        lostWorld = audioSources[1];
        realWorldClip = (AudioClip)Resources.Load("Sounds/gamekid_realworldnew2");
        lostWorldClip = (AudioClip)Resources.Load("Sounds/gamekid_surealworldnew1");
        realWorld.clip = realWorldClip;
        lostWorld.clip = lostWorldClip;
        realWorld.Play();
        lostWorld.Play();
        realWorld.volume = 1f;
        lostWorld.volume = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (fade > 0)
        {
            fade += - fadeRate;
        }
        fade = Mathf.Lerp(0, 1, fade);


        if (fade == 1 || fade == 0)
        {
            GameObject.Find("Player").GetComponent<PlayerMovement>().dropNow = false;
        }
        else
        {
            GameObject.Find("Player").GetComponent<PlayerMovement>().dropNow = true;
        }

        realWorld.volume = fade;
        lostWorld.volume = 1 - fade;

        realWorldFade = fade;
        lostWorldFade = 1 - fade;
        TextureFade("Real World", realWorldFade);
        TextureFade("Real World Pickup", realWorldFade);
        TextureFade("Lost World", lostWorldFade);
        TextureFade("Lost World Pickup", lostWorldFade);


        if (!realWorld.isPlaying && !lostWorld.isPlaying) //If tracks finish play them again simultaniously
        {
            realWorld.Play();
            lostWorld.Play();
        }
        isInRealWorld = false;
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
