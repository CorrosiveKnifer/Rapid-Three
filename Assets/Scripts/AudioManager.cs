using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager
{
    #region Singleton

    private static AudioManager instance = null;

    public static AudioManager GetInstance()
    {
        if (instance == null)
        {
            instance = new AudioManager();
        }

        return instance;
    }
    public static void DestroyInstance()
    {
        instance = null;
    }

    private AudioManager()
    {
        agents = new List<AudioAgent>();
    }
    #endregion

    private List<AudioAgent> agents;

    public void AddAgent(AudioAgent agent)
    {
        agents.Add(agent);
    }

    public void RemoveAgent(AudioAgent agent)
    {
        agents.Remove(agent);
    }

    public void MakeSolo(AudioAgent _agent)
    {
        foreach (var agent in agents)
        {
            agent.Mute();
        }

        _agent.UnMute();
    }

    public void UnMuteAll()
    {
        foreach (var agent in agents)
        {
            agent.UnMute();
        }
    }
}
