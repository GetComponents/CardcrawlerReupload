using UnityEngine;
using UnityEngine.SceneManagement;

namespace Audio
{
    [DisallowMultipleComponent]
    public class ChangeBackgroundMusic : MonoBehaviour
    {
        [SerializeField]
        private AudioClip m_clipToPlay;
        [SerializeField]
        private bool m_whenSceneBecomesActive;
        [SerializeField]
        private string m_sceneName;

        // Start is called before the first frame update
        void Start()
        {
            BackgroundMusicPlayer.Instance.Play(m_clipToPlay);

            if (m_whenSceneBecomesActive)
            {
                SceneManager.activeSceneChanged += ChangeMusic;
            }
        }

        private void ChangeMusic(Scene _previous, Scene _current)
        {
            if (_current.name == m_sceneName)
            {
                BackgroundMusicPlayer.Instance.Play(m_clipToPlay);
            }
        }
    }
}