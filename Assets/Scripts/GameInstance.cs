using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine.SceneManagement;
public class GameInstance:MonoBehaviour
{
    private static GameInstance instance;
    public string serverIP;

    private int CheckInternetTick = 300;

    //Data to be saved localy and if possible on the server
    public User user = null;
    public Achievements achievements;

    public bool loggedIn = false;

    //Debug only \/
    public bool resetPlayerPrefs;
    //Debug only /\

    void Awake(){
        if(instance == null){
            instance = this;
        }else{
            Destroy(this);
        }

        //Debug only \/
        if(resetPlayerPrefs){
            PlayerPrefs.DeleteAll();
            achievements.highscore = 0;
            achievements.speed = -2;
            achievements.woMistake = 0;
            SaveOffline();
        }
        //Debug only /\
        
        instance = this;

        DontDestroyOnLoad(this);
    }

    // if player save already exists returns a name else returns null
    public string getPlayer(){
        //checks if there is a username already saved else it will return null
        return PlayerPrefs.HasKey("username")? PlayerPrefs.GetString("username"):null;
    }

    public void LoadOffline(){
        LoadUserInformation();
        achievements = new Achievements();

        if(File.Exists(Application.persistentDataPath+"/gamesave.save")){
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            achievements = save.achievements;
        }else{
        }
    }

    public void SaveOffline(){
        Save save = new Save();
        save.achievements = achievements;
        
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath+"/gamesave.save");
        bf.Serialize(file,save);
        file.Close();

        LoadOffline();
    }

    //Check if the game has already been started once
    bool CheckFirstStart(){
        if(PlayerPrefs.HasKey("username")){
            return false;
        }else{
            return true;
        }
    }

    //Check if there is a GameData saved
    public void LoadUserInformation(){
        user = new User();
        user.username = PlayerPrefs.GetString("username","");
        user.token = PlayerPrefs.GetString("token","");
    }

    public void CreatePlayer(string username, string token){
        //Save Username and Token inside Player Prefs
        user = new User();
        PlayerPrefs.SetString("username",user.username = username);
        PlayerPrefs.SetString("token",user.token = token);
        PlayerPrefs.Save();

        //set achievements all to 0 when creating new player
        initAchievements();

        //create save file of new player
        SaveOffline();
    }

    void initAchievements(){
        achievements = new Achievements();
        achievements.woMistake = 0;
        achievements.highscore = 0;
        achievements.speed = -2f;
    }

    void FixedUpdate(){
        //check if internet is connected if so it will save every 5 min
        if(Application.internetReachability != NetworkReachability.NotReachable)
        {
            if (--CheckInternetTick == 0)
            {
                CheckInternetTick = 18000;
            }
        }
        
    }



    public static GameInstance getInstance(){
        return instance;
    }

    public IEnumerator Register(string username, string password)
    {
        WWWForm form = new WWWForm();
        LoadOffline();
        form.AddField("name", username);
        form.AddField("password", password);
        form.AddField("speed", achievements.speed.ToString());
        form.AddField("woMistake", achievements.woMistake.ToString());
        form.AddField("highscore", achievements.highscore.ToString());


        string ip = serverIP + "/register";
        UnityWebRequest request = UnityWebRequest.Post(ip,form);

        yield return request.SendWebRequest();

        if (request.isNetworkError)
        {
            throw new Exception("Username already exists");
        }
    }

    public IEnumerator Login(string username, string password){
        WWWForm form = new WWWForm();
        form.AddField("name", username);
        form.AddField("password", password);

        string ip = serverIP + "/login";
        UnityWebRequest request = UnityWebRequest.Post(ip,form);

        yield return request.SendWebRequest();

        if (request.isNetworkError)
        {
            throw new Exception("Could not connect to server");
        }
        else if(request.responseCode == 400){
            throw new Exception("Username or Password are wrong");
        }else{
            try{
                JsonToken token = JsonUtility.FromJson<JsonToken>(request.downloadHandler.text);
                user.username = username;
                user.token = token.token;
                PlayerPrefs.SetString("username",user.username);
                PlayerPrefs.SetString("token",user.token);
                PlayerPrefs.Save();
                loggedIn = true;

                StartCoroutine(SyncLoad());

            }catch(ArgumentException ex){
                if(request.responseCode==400){
                    throw new Exception("Username or password are wrong");
                }else{
                    throw new Exception(ex.Message.ToString());
                }
            }
        }
    }

    public IEnumerator Login()
    {
        if(user.token!="" && user.token != null)
        {
            string ip = serverIP + "/syncLoad";
            UnityWebRequest request = UnityWebRequest.Get(ip);

            request.SetRequestHeader("Authorization", user.token);
            yield return request.SendWebRequest();
            if (request.isNetworkError)
            {
                throw new Exception("Could not connect to server");
            }
            else
            {
                Achievements loaded = JsonUtility.FromJson<Achievements>(request.downloadHandler.text);
                if (achievements.highscore < loaded.highscore)
                {
                    achievements.highscore = loaded.highscore;
                }
                if (achievements.speed > loaded.speed)
                {
                    achievements.speed = loaded.speed;
                }
                if (achievements.woMistake < loaded.woMistake)
                {
                    achievements.woMistake = loaded.woMistake;
                }
                SaveOffline();
                SceneManager.LoadScene("GameMenu", LoadSceneMode.Single);
            }
        }
    }

    public IEnumerator SyncLoad(){
        string ip = serverIP + "/syncLoad";
        UnityWebRequest request = UnityWebRequest.Get(ip);
        
        request.SetRequestHeader("Authorization",user.token);
        yield return request.SendWebRequest();
        if(request.isNetworkError){
            Debug.Log(request.error);
        }else{
            Achievements loaded = JsonUtility.FromJson<Achievements>(request.downloadHandler.text);
            if (achievements.highscore < loaded.highscore)
            {
                achievements.highscore = loaded.highscore;
            }
            if(achievements.speed > loaded.speed)
            {
                achievements.speed = loaded.speed;
            }
            if (achievements.woMistake < loaded.woMistake)
            {
                achievements.woMistake = loaded.woMistake;
            }
            SaveOffline();
            SceneManager.LoadScene("GameMenu", LoadSceneMode.Single);
        }
    }

    public IEnumerator SyncSave()
    {
        string ip = serverIP + "/syncSave";
        LoadOffline();

        string achievementsToJson = JsonUtility.ToJson(achievements);
        Debug.Log(achievementsToJson);
        UnityWebRequest request = UnityWebRequest.Put(ip,achievementsToJson);

        request.SetRequestHeader("Authorization", user.token);
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        if (request.isNetworkError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log(request.responseCode);
            Debug.Log(request.downloadHandler.text);
        }
    }
}
