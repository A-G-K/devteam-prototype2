using System;
using UnityEngine;

namespace Services
{
    public class AbilityUIController : MonoBehaviour
    {
        private AbilityUIManager _abilityUIManager;
        private UnitManager _unitManager;
        private PlayerData _selectedUnitData;

        private void Awake()
        {
            _abilityUIManager = ServiceLocator.Current.Get<AbilityUIManager>();
            _unitManager = ServiceLocator.Current.Get<UnitManager>();
            _abilityUIManager.abilityUIController = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }
        
        public void OnPlayerUnitSelect()
        {
            _selectedUnitData = _unitManager.Controller.SelectedUnit.playerData;
            if (transform.childCount > 0)
            {
                ClearAbilities();
            }
            SetAbilities();
            // gameObject.SetActive(true);
        }

        public void OnPlayerUnitDeselect()
        {
            Debug.Log("UI");
            ClearAbilities();
            // gameObject.SetActive(false);
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
                Destroy(child.gameObject);
            }
        }
    }
}
