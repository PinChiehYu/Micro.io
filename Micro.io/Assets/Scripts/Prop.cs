using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class Prop : MonoBehaviour
{
    private PlayableGraph playableGraph;
    private Effect effect;

    private bool isPrepared = false;

    public void Awake()
    {
        isPrepared = false;
    }

    public void BuildProp(Effect effect, AnimationClip clip)
    {
        this.effect = effect;
        playableGraph = PlayableGraph.Create();
        var playableOutput = AnimationPlayableOutput.Create(playableGraph, "Animation", GetComponent<Animator>());
        var clipPlayable = AnimationClipPlayable.Create(playableGraph, clip);
        playableOutput.SetSourcePlayable(clipPlayable);
        playableGraph.Play();

        isPrepared = true;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (isPrepared && col.gameObject.CompareTag("Microbe"))
        {
            if (!effect.IsPositive && (col.name[1] - '0') == 0) return;

            col.gameObject.GetComponent<Microbe>().ReceiveEffects(effect);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (playableGraph.IsValid())
        {
            playableGraph.Destroy();
        }
    }
}
