using System;
using UnityEngine;

namespace Services
{
    public class AbilityUIController : MonoBehaviour
    {
        private AbilityUIManager _abilityUIManager;
        //[SerializeField] private GameObject abilityCardsHolder;
        private PlayerData _selectedUnitData;

        private void Awake()
        {
            _abilityUIManager = ServiceLocator.Current.Get<AbilityUIManager>();
            ServiceLocator.Current.Get<AbilityUIManager>().abilityUIController = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void OnPlayerUnitSelect()
        {
            _selectedUnitData = _abilityUIManager.SelectedUnit.playerData;
            if (transform.childCount > 0)
            {
                ClearAbilities();
            }
            SetAbilities();
            gameObject.SetActive(true);
        }

        public void OnPlayerUnitDeselect()
        {
            Debug.Log("UI");
            ClearAbilities();
            gameObject.SetActive(false);
        }

        private void SetAbilities()
        {
            foreach (var ability in _selectedUnitData.abilitiesList)
            {
                Instantiate(ability.abilityPrefab, transform);
            }
        }
        
        private void ClearAbilities()
        {
            foreach (Transform child in transform)
            {
                Debug.Log("test");
                Destroy(child.gameObject);
            }
        }
    }
}
