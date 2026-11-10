using UnityEngine;

using Runtime.UI;

public class UIManager : MonoBehaviour
{
    #region Variables
    [Header("Panels")]
    [SerializeField] private GameObject playerPanel;
    [SerializeField] private GameObject collectionPanel;
    #endregion

    private void OnEnable()
    {
        UIEvent.PanelChanged += OnPanelChanged;
    }

    private void OnDisable()
    {
        UIEvent.PanelChanged -= OnPanelChanged;
    }

    private void Start()
    {
        Init();
    }

    #region Functions
    private void OnPanelChanged(string name, bool active)
    {
        switch(name)
        {
            case "Player":
                playerPanel.SetActive(active);
                break;
            case "Collection":
                collectionPanel.SetActive(active);
                break;
        }
    }

    private void Init()
    {
        collectionPanel.SetActive(false);
    }
    #endregion
}
