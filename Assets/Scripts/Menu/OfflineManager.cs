public class OfflineManager
{
    private GameInstance gameInstance;
    public bool firstStart;
    public string username;

    public OfflineManager(){
        gameInstance = GameInstance.getInstance();
    }

    public void loadPlayer(){
        username = gameInstance.getPlayer();
        firstStart = (username==null)?true:false;
    }

    public void loadOffline(){
        gameInstance.LoadOffline();
    }

    public void createOffline(string username){
        gameInstance.CreatePlayer(username,null);
    }
}
