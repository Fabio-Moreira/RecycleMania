using UnityEngine;
using UnityEngine.UI;

namespace GameScripts
{
    public class Bins : MonoBehaviour
    {
        [Header("Settings")] [SerializeField] [Tooltip("Start time between which bin switches happen")]
        private float _SWITCHINTERVAL;

        [SerializeField] [Tooltip("Step at which the time between bin switches happen decreases")]
        private float _SWITCHINTERVALSTEP;

        [Header("GameObjects")] [SerializeField] [Tooltip("Prefabs of the bins which will be instantiated at Start()")]
        private GameObject[] binsPrefabs;

        private GameObject[] bins = new GameObject[7];

        [SerializeField] [Tooltip("Positions where the bins will be placed")]
        private GameObject[] binsPositions;

        [Header("UI Text component for bins countdown")] [SerializeField, Tooltip("Text to bins switching countdown")]
        private Text[] binSwitchText;

        private float countDown;

        //countdown between switches
        private float countdownSwitch;

        private int firstSwap = -1, secondSwap = -1;

        //time between which bin switches happen
        private float switchInterval;

        //needed to award points and remove lives
        public Game game { set; private get; }

        // Start is called before the first frame update
        private void Start()
        {
            
        }

        private void Update()
        {
            Swap();
            UpdateBinSwitchText();
        }

        private void UpdateBinSwitchText()
        {
            if (firstSwap == -1)
            {
                string empty = "";
                foreach (var VARIABLE in binSwitchText)
                {
                    VARIABLE.text = empty;
                }
            }
            else
            {
                binSwitchText[firstSwap].text = countDown.ToString("n2");
                binSwitchText[secondSwap].text = countDown.ToString("n2");
            }
        }
        
        public void SetupBins()
        {
            if (bins.Length > 0)
            {
                foreach (var VARIABLE in bins)
                {
                    Destroy(VARIABLE);
                }
            }
            
            for (var index = 0; index < binsPositions.Length; index++)
            {
                bins[index] = Instantiate(binsPrefabs[index], binsPositions[index].transform.position, Quaternion.identity);
                bins[index].GetComponent<Bin>().game = game;
                
            }

            switchInterval = _SWITCHINTERVAL;
            countdownSwitch = _SWITCHINTERVAL;
        }

        private void DifficultyIncrease()
        {
            if (switchInterval - _SWITCHINTERVALSTEP > 1.5f)
                switchInterval -= _SWITCHINTERVALSTEP;
        }

        private void InitSwap()
        {
            firstSwap = Random.Range(0, bins.Length-1);
            do
            {
                secondSwap = Random.Range(0, bins.Length-1);
            } while (firstSwap == secondSwap);

            countDown = 1f;
        }

        private void Swap()
        {
            if ((countdownSwitch -= Time.deltaTime) <= 0)
            {
                if (firstSwap == -1)
                {
                    InitSwap();
                }
                else
                {
                    SwapBins();
                    //UpdateSwapText();
                }
            }
        }

        private void SwapBins()
        {
            if (firstSwap != -1)
                if ((countDown -= Time.deltaTime) <= 0)
                {
                    var temp1GO = bins[firstSwap];
                    var temp1Pos = temp1GO.transform.position;
                    var temp2GO = bins[secondSwap];
                    var temp2Pos = temp2GO.transform.position;

                    bins[firstSwap] = temp2GO;
                    temp2GO.transform.position = temp1Pos;

                    bins[secondSwap] = temp1GO;
                    temp1GO.transform.position = temp2Pos;

                    DifficultyIncrease();
                    countdownSwitch = switchInterval;

                    firstSwap = -1;
                    secondSwap = -1;
                }
        }
    }
}