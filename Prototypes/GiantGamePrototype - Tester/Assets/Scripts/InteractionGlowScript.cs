using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionGlowScript : MonoBehaviour {

    public bool glow;

    public Color glowColour;

    public SpriteRenderer[] objectsToEffect;

    public float glowSpeed;

    public float maxGlow;
    public float minGlow;

    float currentGlowAmt = 0;

    float timeSinceGlow = 0;
	
    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (glow)
        {
            float glowParam = maxGlow - minGlow;

            currentGlowAmt = glowParam * (Mathf.Sin(timeSinceGlow * glowSpeed) + 1) / 2;

            currentGlowAmt += minGlow;


            foreach (SpriteRenderer sprite in objectsToEffect)
            {
                sprite.material.SetColor("_GlowColor", glowColour);
                sprite.material.SetFloat("_GlowAmt", currentGlowAmt);
            }

            timeSinceGlow += Time.deltaTime;
        }

        if (!glow && timeSinceGlow != 0) 
        {
            timeSinceGlow = 0;
            foreach (SpriteRenderer sprite in objectsToEffect)
            {
                sprite.material.SetFloat("_GlowAmt", 0);
            }
        }
    }
}
