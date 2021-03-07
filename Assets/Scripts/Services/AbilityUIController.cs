using UnityEngine;

namespace Services
{
    public class AbilityUIController : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            ServiceLocator.Current.Get<AbilityUIManager>().abilityUIController = this;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
