using UnityEngine;

namespace GameScripts
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        [SerializeField, Tooltip("If tutorial is shown")]
        private bool needTutorial;

        private void Start()
        {
            if (_instance == null)
                _instance = this;
            else
                Destroy(this);
        }

        public bool getNeedTutorial()
        {
            return needTutorial;
        }
    
        public static GameManager getInstance()
        {
            return _instance;
        }
    }
}
