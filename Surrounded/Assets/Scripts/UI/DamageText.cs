using UnityEngine;
using TMPro;

namespace slowac_UI
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] private float destroyTime; // Damage text destruction time
        [SerializeField] private Vector3 offset; // Damage text position offset
        [SerializeField] private Vector3 randomiseOffset; // Random position offset range
        [SerializeField] private Color damageColour; // Damage text color

        private TextMeshPro damageText; // Damage text component

        private void Awake()
        {
            damageText = GetComponent<TextMeshPro>(); // Get damage text component
            transform.localPosition += offset; // Apply position offset
            transform.localPosition += new Vector3(
            Random.Range(-randomiseOffset.x, randomiseOffset.x), // Random X-axis offset
            Random.Range(-randomiseOffset.y, randomiseOffset.y), // Random Y-axis offset
            Random.Range(-randomiseOffset.z, randomiseOffset.z) // Random Z offset
            );
            Destroy(gameObject, destroyTime); // Destroy the game object after a specified time
        }

        public void Initialise(int damageValue)
        {
            damageText.text = damageValue.ToString(); // Set the value of the damage text
        }
    }
}
