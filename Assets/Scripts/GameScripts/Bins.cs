using System.Linq;
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
                foreach (var variable in binSwitchText)
                {
                    variable.text = empty;
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
                foreach (var variable in bins)
                {
                    Destroy(variable);
                }
            }
            
            for (var index = 0; index < binsPositions.Length; index++)
            {
                bins[index] = Instantiate(binsPrefabs[index], Vector3.zero, Quaternion.identity);
                bins[index].transform.parent = binsPositions[index].transform;
                bins[index].transform.localPosition = Vector3.zero;
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
                    if ((countDown -= Time.deltaTime) <= 0)
                    {
                        SwapBins();
                        DifficultyIncrease();
                        countdownSwitch = switchInterval;
                    }
                }
            }
        }

        private void SwapBins()
        {
            if (firstSwap != -1)
            {
                var binOne = bins[firstSwap];
                var binTwo = bins[secondSwap];

                binOne.transform.parent = binsPositions[secondSwap].transform;
                binOne.transform.localPosition = Vector3.zero;
                binTwo.transform.parent = binsPositions[firstSwap].transform;
                binTwo.transform.localPosition = Vector3.zero;
                
                bins[firstSwap] = binTwo;
                bins[secondSwap] = binOne;

                firstSwap = -1;
                secondSwap = -1;
            }
        }
    }
}