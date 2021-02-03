using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    [SerializeField, Tooltip("If tutorial is shown")]
    private bool needTutorial;
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public bool getNeedTutorial()
    {
        return needTutorial;
    }
    
    public static GameManager getInstance()
    {
        return instance;
    }
}
