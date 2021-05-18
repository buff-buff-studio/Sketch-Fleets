using UnityEngine;

namespace ManyTools.Variables
{
    [System.Serializable]
    public class AudioClipReference : Reference<AudioClip, AudioClipVariable>
    {
        public AudioClipReference(AudioClip value) : base(value)
        {
        }
    }
}