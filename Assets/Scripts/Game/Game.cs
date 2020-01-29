using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    //Game start settings / to be set in the editor
    public float speed;
    //at which rate the speed augments  
    public int speedTick;
    //between which amount of ticks the trash spawns
    public int minSpawnTick;
    public int maxSpawnTick;
    //between which amount of ticks the bins swap
    public int minSwapTick;
    public int maxSwapTick;

    private bool newRecord = false;

    //Objects
    public GameObject Spawn;
    public GameObject[] TrashObjects = new GameObject[7];
    public GameObject[] BinsPositions = new GameObject[6];
    public GameObject[] Bins = new GameObject[6];

    public Text highScoreText,fastestText,livesHighscoreText;

    // Attributes needed for Swapping Bins
    private int firstSwap;
    private int secondSwap;
    public Text[] swapText;
    private float countDown;

    public Button restartButton,menuButton;

    public GameObject pauseMenu;
    
    //Points and lives
    private int Points = 0;
    private int lives  = 3;
    public Text textPoints;
    public Text textLives;

    //Trash List
    public List<Rigidbody2D> trash;
    private Rigidbody2D selected;
    //Touch values
    private Vector2 touchStart;
    private Vector2 touchEnd;

    public static Game instance;

    //currentTicks used for dificulty increase
    private int currentSwapTick = 30;
    private int currentSpawnTick = 0;
    private int currentSpeedTick = 0;
    private int currentTickDifficulty = 0;
    private int tickDifficultyIncrease = 60;
    private float currentSpeed;

    //achievementTracker
    private int pointsWOMistakes = 0;
    private float bestSpeed = 0;
    private int highScore = 0;

    private bool paused = false;
    public Button PauseButton;
    public Button ContinueButton;

    public Text PointsText;


    // Start is called before the first frame update
    void Start()
    {
        pointsWOMistakes = GameInstance.getInstance().achievements.woMistake;
        bestSpeed = GameInstance.getInstance().achievements.speed;
        highScore = GameInstance.getInstance().achievements.highscore;
        //set the button functions
        restartButton.onClick.AddListener(restartGame);
        menuButton.onClick.AddListener(GoToMenu);
        PauseButton.onClick.AddListener(PauseGame);
        ContinueButton.onClick.AddListener(ContinueGame);
        
        restartGame();

        //avoid phone from locking
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        //singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        //set text
        textPoints.text = ("Points: " + Points);
        

        for(int index = 0; index < BinsPositions.Length; index++){
            GameObject tmpObject;
            
            tmpObject = Instantiate(Bins[index],BinsPositions[index].transform.position,Quaternion.identity);
            Bins[index] = tmpObject;
        }

        currentSpeed = speed;
    }

    private void FixedUpdate()
    {
        //Tick countdown

        //Others
        TouchInput();
        Spawner();
        Swap();
        IncreaseSpeed();
        CheckNotMoving();
        DifficultyIncrease();
    }

    private void DifficultyIncrease(){
        if(currentTickDifficulty < 80 && --tickDifficultyIncrease==0){
            currentTickDifficulty++;
            tickDifficultyIncrease = 60;
        }

    }

    private void Spawner()
    {
        if(currentSpawnTick == 0)
        {
            Vector3 pos = Spawn.transform.position;
            pos.x += Random.Range(-1.6f, 1.6f);
            pos.y += 2f;

            currentSpawnTick = Random.Range(minSpawnTick-currentTickDifficulty, maxSpawnTick-currentTickDifficulty);
            int type = Random.Range(0, 7);

            trash.Add(Instantiate(TrashObjects[type], pos, Quaternion.identity).GetComponent<Rigidbody2D>());
        }
        currentSpawnTick--;
    }

    private void IncreaseSpeed(){
        if(currentSpeed > -7f && --currentSpeedTick == 0){
            currentSpeed -= 0.1f;
            currentSpeedTick = speedTick - currentTickDifficulty;
            if(currentSpeed<bestSpeed){
                bestSpeed = currentSpeed;
                GameInstance.getInstance().achievements.speed = bestSpeed;
                GameInstance.getInstance().SaveOffline();
                newRecord = true;
            }
        }
    }

    private void CheckNotMoving(){
        for(int index = 0; index < trash.Count; index++)
        {
            if (trash[index] == null)
            {
                trash.RemoveAt(index);
            }
            else
            {
                if(!trash[index].Equals(selected) || trash[index].gameObject.GetComponent<Trash>().destroy!=false){
                    trash[index].velocity = new Vector2(0, currentSpeed);
                }
            }
        }
    }

    private void TouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
            {
                
                Vector2 v2 = Camera.main.ScreenToWorldPoint(touch.position);
                touchStart = v2;
                RaycastHit2D hit = Physics2D.CircleCast(v2,0.3f,Vector2.zero);
                if (hit.collider != null && hit.collider.gameObject.layer == 8)
                {
                    selected = hit.collider.gameObject.GetComponent<Rigidbody2D>();
                    selected.velocity = Vector2.zero;
                }
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if (selected != null)
                {
                    Vector2 v2 = Camera.main.ScreenToWorldPoint(touch.position);
                    selected.gameObject.transform.position = new Vector3(v2.x, v2.y, 0);
                    touchStart = v2;
                }else{
                    Vector2 v2 = Camera.main.ScreenToWorldPoint(touch.position);
                    touchStart = v2;
                    RaycastHit2D hit = Physics2D.CircleCast(v2,0.3f,Vector2.zero);
                    if (hit.collider != null && hit.collider.gameObject.layer == 8)
                    {
                        selected = hit.collider.gameObject.GetComponent<Rigidbody2D>();
                        selected.velocity = Vector2.zero;
                    }
                }
            }
            else
            {
                if (touch.phase == TouchPhase.Ended)
                {
                    if (selected)
                    {
                        Vector2 v2 = Camera.main.ScreenToWorldPoint(touch.position);
                        touchEnd = v2;
                        Vector2 VandD = touchEnd - touchStart;
                        VandD = VandD.normalized * 5f;
                        selected.velocity = VandD;
                        selected = null;
                    }
                }
            }
        }
        
    }

    public void addPoints(int amount)
    {
        Points += amount;
        textPoints.text = ("Points: " + Points);
        if(Points>highScore){
            highScore = Points;
            GameInstance.getInstance().achievements.highscore = highScore;
            GameInstance.getInstance().SaveOffline();
            newRecord = true;

        }
        if(lives == 3 && Points>pointsWOMistakes){
            pointsWOMistakes = Points;
            GameInstance.getInstance().achievements.woMistake = pointsWOMistakes;
            GameInstance.getInstance().SaveOffline();
            newRecord = true;
        }
    }

    private void initSwap(){
        firstSwap = Random.Range(0, 6);
        do{
            secondSwap = Random.Range(0, 6);
        }while(firstSwap==secondSwap);
        countDown = 1f;
    }

    private void UpdateSwapText(){
        if(firstSwap==-1){
            foreach(Text text in swapText){
                text.text = "";
            }
        }else{
            for(int index = 0; index < swapText.Length; index++){
                if(firstSwap == index){
                    swapText[firstSwap].text = countDown.ToString("0.00");
                }else if(secondSwap == index){
                    swapText[secondSwap].text = countDown.ToString("0.00");
                }else{
                    swapText[index].text = "";
                }
            }
        }
    }

    private void SwapBins(){
        if(firstSwap != -1){
            if(countDown>0){
                countDown -=Time.deltaTime;
            }else{
                GameObject temp1GO= Bins[firstSwap];
                Vector3 temp1Pos = temp1GO.transform.position;
                GameObject temp2GO= Bins[secondSwap];
                Vector3 temp2Pos = temp2GO.transform.position;

                Bins[firstSwap] = temp2GO;
                temp2GO.transform.position = temp1Pos;

                Bins[secondSwap] = temp1GO;
                temp1GO.transform.position = temp2Pos;

                firstSwap = -1;
                secondSwap = -1;
            }
        }
    }

    private void Swap(){
        if(firstSwap == -1){
            if(--currentSwapTick==0){
                initSwap();
                currentSwapTick = 30;
            }
        }else{
            SwapBins();
            UpdateSwapText();
        }
    }

    public void loseLive(){
        //reduce the amount of lives and update the string
        --lives;
        textLives.text = lives.ToString();
        //check if the player has no lives left if so the trash will be deleted and the pause/restart menu will be shown
        if(lives<=0){
            for(int i=trash.Count-1; i>0; i--){
                if(trash[i]!=null){
                    Destroy(trash[i].gameObject);
                }
            }
            Time.timeScale = 0;
            highScoreText.text = "Highscore: " + highScore.ToString();
            fastestText.text = "Speed: " + bestSpeed.ToString();
            livesHighscoreText.text = "Highscore w/o mistakes: " + pointsWOMistakes.ToString();
            PointsText.text = "Points this try: " + Points.ToString();

            pauseMenu.SetActive(true);
            ContinueButton.interactable = false;
            PauseButton.interactable = false;
            if (newRecord)
            {
                Debug.Log("SaveSaveSave");
                newRecord = false;
                saveServer();
            }
        }
    }

    public void saveServer()
    {
        if (GameInstance.getInstance().loggedIn)
        {
            StartCoroutine(GameInstance.getInstance().SyncSave());
        }
    }

    public void restartGame(){
        //resets the game to the default starting param and start the game
        currentSpeed = speed;
        Points = 0;
        textPoints.text = Points.ToString();
        lives = 3;
        textLives.text = lives.ToString();
        currentSpeedTick = speedTick;
        currentTickDifficulty = 0;
        currentSwapTick = 30;
        currentSpawnTick = 0;
        tickDifficultyIncrease = 60;

        pauseMenu.SetActive(false);
        Time.timeScale = 1;

        ContinueButton.interactable = true;
        PauseButton.interactable = true;
    }

    void GoToMenu(){
        SceneManager.LoadScene("GameMenu",LoadSceneMode.Single);
    }

    public void PauseGame(){
        ContinueButton.interactable = true;
        restartButton.interactable = false;
        highScoreText.text = "Highscore: " + highScore.ToString();
        fastestText.text = "Speed: " + bestSpeed.ToString();
        livesHighscoreText.text = "Highscore w/o mistakes: " + pointsWOMistakes.ToString();
        PointsText.text = "Points this try: " + Points.ToString();
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void ContinueGame(){
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
}
