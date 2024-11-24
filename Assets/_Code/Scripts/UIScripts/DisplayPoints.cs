using TMPro;
using UnityEngine;

namespace _Code.Scripts.UIScripts
{
    public class DisplayPoints : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI pointsPlace;

        public void UpdatePoints(float updatedPoints)
        {
            pointsPlace.text = $"Points: {updatedPoints:n2}";
        }
    }
}