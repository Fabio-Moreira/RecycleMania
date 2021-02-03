using UnityEngine;
using GameScripts;

public class TutorialsManager : MonoBehaviour
{
    private bool needTutorial;
    public Game game;
 
    void Start()
    {
        needTutorial = GameManager.getInstance().getNeedTutorial();
    }
    
    void Update()
    {
        
    }
}
