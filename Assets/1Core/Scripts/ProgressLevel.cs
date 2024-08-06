using TMPro;
using UnityEngine;

namespace _1Core.Scripts
{
    public class ProgressLevel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _levelText;

        public int level = 1;
        public int indexLevel = 1;

        private void Awake()
        {
            if (ES3.KeyExists(Str.Level))
            {
                level = ES3.Load<int>(Str.Level);
                indexLevel = level;
                if (level > 10) indexLevel = 10;
            }

            SetTextLevel();
        }

        public void AddLevel()
        {
            level++;
            indexLevel = level;
            if (level > 10) indexLevel = 10;
            ES3.Save(Str.Level, level);
        }

        public void SetTextLevel() => _levelText.text = $"Level {level}";
    }
}