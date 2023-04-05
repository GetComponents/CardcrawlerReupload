using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    [DisallowMultipleComponent]
    public class BackgroundMusicPlayer : MonoBehaviour
    {
        public static BackgroundMusicPlayer Instance { get; private set; }

        [SerializeField]
        private float m_crossFadeDuration;

        private AudioSource m_firstAudioSource;
        private AudioSource m_secondAudioSource;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this);

            AudioSource[] sources = GetComponents<AudioSource>();
            m_firstAudioSource = sources[0];
            m_secondAudioSource = sources[1];
        }

        public void Play(AudioClip _clip)
        {
            if (m_firstAudioSource.isPlaying)
            {
                if (m_firstAudioSource.clip == _clip)
                    return;
                StartCoroutine(CrossFade(m_firstAudioSource, m_secondAudioSource, _clip));
            }
            else if (m_secondAudioSource.isPlaying)
            {
                if (m_secondAudioSource.clip == _clip)
                    return;
                StartCoroutine(CrossFade(m_secondAudioSource, m_firstAudioSource, _clip));
            }
            else
            {
                // no playing currently
                m_firstAudioSource.clip = _clip;
                m_firstAudioSource.Play();
            }
        }

        private IEnumerator CrossFade(AudioSource _start, AudioSource _goal, AudioClip _clip)
        {
            _goal.volume = 0.0f;
            _goal.clip = _clip;
            _goal.Play();

            float time = 0.0f;
            while (time < 1.0f)
            {
                time += Time.deltaTime / m_crossFadeDuration;
                _start.volume = Mathf.Lerp(1.0f, 0.0f, time);
                _goal.volume = Mathf.Lerp(0.0f, 1.0f, time);

                yield return null;
            }

            _start.Stop();
            _start.clip = null;
            _goal.volume = 1.0f;
        }
    }
}
