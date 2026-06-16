/*****************************************************************************
// File Name : AudioRelay.cs
// Author : Arcadia Koederitz
// Creation Date : 6/15/2026
// Last Modified : 6/15/2026
//
// Brief Description : Plays a sound effect via the AudioManager.
*****************************************************************************/
using UnityEngine;

namespace FoolsBrand.Audio
{
    public class AudioRelay : MonoBehaviour
    {
        public void PlaySound(string soundName)
        {
            AudioManager.Instance.PlayOneShot(soundName);
        }

        public void PlaySoundAtLocation(string soundName)
        {
            AudioManager.Instance.PlayOneShot(soundName, transform.position);
        }

        public void SetMusic(int musicType)
        {
            AudioManager.Instance.SetMusic((MusicType)musicType);
        }

        public void StopMusic()
        {
            AudioManager.Instance.StopMusic();
        }

        public void StartSound(string soundName)
        {
            AudioManager.Instance.StartSound(soundName);
        }

        public void StopSound(string soundName)
        {
            AudioManager.Instance.StopSound(soundName);
        }
    }
}
