using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;
using UnityEngine.UI;

public class TokensUI : MonoBehaviour
{
    private UnitManager _unitManager;
    [SerializeField] private Image tokensPrefab;

    // Start is called before the first frame update
    void Awake()
    {
        _unitManager = ServiceLocator.Current.Get<UnitManager>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnPlayerUnitSelect()
    {
        if (transform.childCount > 0)
        {
            ClearTokens();
        }

        ShowTokens();
        gameObject.SetActive(true);
    }

    public void OnPlayerUnitDeselect()
    {
        gameObject.SetActive(false);
        ClearTokens();
    }

    public void UpdateTokens()
    {
        // HELP MEEEEEE || Idk make it so we just remove used tokens instead
        if (gameObject.activeSelf)
        {
            ClearTokens();
            ShowTokens();
        }
    }

    private void ShowTokens()
    {
        var tokens = _unitManager.Controller.SelectedUnit.playerData.currentTokens;
        foreach (var token in tokens)
        {
            var go = Instantiate(tokensPrefab, transform);
            go.GetComponent<Image>().sprite = token.icon;
        }
    }

    private void ClearTokens()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}