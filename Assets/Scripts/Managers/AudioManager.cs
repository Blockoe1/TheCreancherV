/*****************************************************************************
// File Name : AudioManager.cs
// Author : Arcadia Koederitz
// Creation Date : 6/15/2026
// Last Modified : 6/15/2026
//
// Brief Description : Responsible for storing FMOD events and playing sounds.
*****************************************************************************/
using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

namespace FoolsBrand.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private EventKey[] events;
        [SerializeField] private EventReference musicEvent;

        private EventInstance musicInstance;

        private readonly Dictionary<string, EventReference> soundDict = new Dictionary<string, EventReference>();

        private readonly Dictionary<string, EventInstance> runningInstances = new Dictionary<string, EventInstance>();

        private static AudioManager instance;
        public static AudioManager Instance
        {
            get { return instance; }
        }

        [System.Serializable]
        private struct EventKey
        {
            [SerializeField] internal string key;
            [SerializeField] internal EventReference eventRef;
        }

        private void Awake()
        {
            // Assign the singleton instance.
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning("Duplicate AudioManager found.  Deleting.");
                Destroy(gameObject);
                return;
            }
            else
            {
                instance = this;
                if (gameObject.transform.root == gameObject.transform)
                {
                    DontDestroyOnLoad(gameObject);
                }
            }

            foreach (EventKey e in events)
            {
                soundDict.Add(e.key, e.eventRef);
            }
            if (!musicEvent.IsNull)
            {
                musicInstance = RuntimeManager.CreateInstance(musicEvent);
            }
            
        }

        public EventReference GetEvent(string key)
        {
            if (key == null) { return default; }
            if (soundDict.ContainsKey(key))
            {
                return soundDict[key];
            }
            return default;
        }

        #region Playing Sounds
        public void PlayOneShot(string soundName)
        {
            if (soundName == null || soundName == "") { return; }
            RuntimeManager.PlayOneShot(GetEvent(soundName));
        }

        public void PlayOneShot(string soundName, Vector3 worldPos)
        {
            if (soundName == null || soundName == "") { return; }
            RuntimeManager.PlayOneShot(GetEvent(soundName), worldPos);
        }

        public void StartSound(string soundName)
        {
            if (soundName == null || soundName == "") { return; }
            try
            {
                EventInstance inst = RuntimeManager.CreateInstance(GetEvent(soundName));
                runningInstances.Add(soundName, inst);
                inst.start();
            }
            catch (EventNotFoundException)
            {
                Debug.LogWarning($"No FMOD Event with the name {soundName} was found.");
            }

        }

        public void StopSound(string soundName)
        {
            if (soundName == null || soundName == "") { return; }
            if (runningInstances.ContainsKey(soundName))
            {
                EventInstance inst = runningInstances[soundName];
                inst.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                runningInstances.Remove(soundName);
                inst.release();
            }
        }
        #endregion

        #region Music
        public void SetMusic(MusicType music)
        {

        }

        public void StopMusic(bool allowFadeout = false)
        {
            musicInstance.stop(allowFadeout ? FMOD.Studio.STOP_MODE.ALLOWFADEOUT : FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
        #endregion
    }
}
