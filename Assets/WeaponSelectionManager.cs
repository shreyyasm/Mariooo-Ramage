using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using InfimaGames.LowPolyShooterPack;
using static WeaponSelectionManager;

public class WeaponSelectionManager : MonoBehaviour
{
    public static WeaponSelectionManager instance;
    [System.Serializable]
    public class Weapon
    {
        public GameObject weaponPrefab; // The weapon's 3D model prefab
        public string weaponName;
        public int requiredLevel; // The level required to unlock the weapon
        public int weaponID;
        public bool isUnlocked;

        // Weapon stats
        public int damage;
        public float fireRate;
        public int magazineSize;
    }

    public List<Weapon> weapons;
    public TextMeshProUGUI weaponNameText;
    public Button selectButton;
    public TextMeshProUGUI selectedButtontext;
    public GameObject lockedButton; // New button to display when locked
    public GameObject SelectedTick;
    public Button leftArrowButton, rightArrowButton;
    public TextMeshProUGUI levelText; // Displays the player's current level

    // Sliders for weapon stats
    public Slider damageSlider;
    public Slider fireRateSlider;
    public Slider magazineSizeSlider;

    public Transform unlockedParent; // Parent for unlocked weapons
    public Transform lockedParent;   // Parent for locked weapons
    // UI to display the unlock level of the weapon
    public TextMeshProUGUI unlockLevelText;

    public int currentIndex = 0;
    public int currentSelectedWeaponIndex = 0;

    private const string SelectedWeaponKey = "SelectedWeapon";
    private const string WeaponUnlockedKeyPrefix = "WeaponUnlocked_";

    public bool MainMenu;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    void Start()
    {
        LoadWeaponData();
        UpdateUI();

        if (weaponNameText == null)
            return;
        leftArrowButton.onClick.AddListener(SelectPreviousWeapon);
        rightArrowButton.onClick.AddListener(SelectNextWeapon);
        selectButton.onClick.AddListener(SelectWeapon);

        // Ensure only the current weapon's prefab is active at the start
        UpdateWeaponDisplay();
    }

    void SelectPreviousWeapon()
    {
        currentIndex = (currentIndex - 1 + weapons.Count) % weapons.Count;
        UpdateUI();
    }

    void SelectNextWeapon()
    {
        currentIndex = (currentIndex + 1) % weapons.Count;
        UpdateUI();
    }

    void SelectWeapon()
    {
        Weapon currentWeapon = weapons[currentIndex];

        if (currentWeapon.isUnlocked)
        {
            SelectedTick.SetActive(true);
            selectedButtontext.text = "Selected";
            currentSelectedWeaponIndex = currentWeapon.weaponID;
            PlayerPrefs.SetInt(SelectedWeaponKey, currentWeapon.weaponID);
            Debug.Log($"{currentWeapon.weaponName} selected!");
        }
    }
    public Inventory inventory;
    public List<GameObject> weaponsPrefabs;
    public void CheckWeaponUnlocks()
    {
        foreach (var weapon in weapons)
        {
            if (LevelManager.Instance.playerLevel >= weapon.requiredLevel)
            {
                // Mark weapon as unlocked
                weapon.isUnlocked = true;

                // Move the weapon object to the unlocked parent
                if (MainMenu)
                    return;
                weaponsPrefabs[weapon.weaponID].transform.SetParent(unlockedParent);
                //weapon.weaponPrefab.transform.SetParent(unlockedParent);               
                //inventory.Init();
                Debug.Log("worked");
            }
            else if (!weapon.isUnlocked)
            {
                // Ensure locked weapons are under the locked parent
                if (MainMenu)
                    return;
                weapon.weaponPrefab.transform.SetParent(lockedParent);
            }
        }

        // Update the UI after processing all weapons
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (weaponNameText != null)
        {
            Weapon currentWeapon = weapons[currentIndex];

            // Update weapon name
            weaponNameText.text = currentWeapon.weaponName;

            // Update sliders
            damageSlider.value = currentWeapon.damage;
            fireRateSlider.value = currentWeapon.fireRate;
            magazineSizeSlider.value = currentWeapon.magazineSize;

            // Update player's level display
            //levelText.text = $"Level: {playerLevel}";

            // Only show the unlock level text if the weapon is locked
            if (currentWeapon.isUnlocked)
            {
                unlockLevelText.gameObject.SetActive(false);
            }
            else
            {
                unlockLevelText.gameObject.SetActive(true);
                unlockLevelText.text = $"Unlocks at level {currentWeapon.requiredLevel}";
            }

            // Check if the weapon should be unlocked based on player level
            UnlockWeaponBasedOnLevel(currentWeapon);

            // Update button visibility
            if (currentWeapon.isUnlocked)
            {
                selectButton.gameObject.SetActive(true);
                lockedButton.gameObject.SetActive(false);
                if(currentWeapon.weaponID == currentSelectedWeaponIndex)
                {
                    SelectedTick.SetActive(true);
                    selectedButtontext.text = "Selected";
                }
                else
                {
                    SelectedTick.SetActive(false);
                    selectedButtontext.text = "Select";
                }
            }
            else
            {
                selectButton.gameObject.SetActive(false);
                lockedButton.gameObject.SetActive(true);
                SelectedTick.SetActive(false);
            }

            // Update weapon prefab display
            UpdateWeaponDisplay();
        }

    }

    void UnlockWeaponBasedOnLevel(Weapon weapon)
    {
        if (!weapon.isUnlocked && LevelManager.Instance.playerLevel >= weapon.requiredLevel)
        {
            weapon.isUnlocked = true;
            PlayerPrefs.SetInt(WeaponUnlockedKeyPrefix + weapon.weaponID, 1);
            PlayerPrefs.Save();
            Debug.Log($"{weapon.weaponName} unlocked at level {LevelManager.Instance.playerLevel}!");
        }
    }

    void UpdateWeaponDisplay()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].weaponPrefab != null)
            {
                // Only activate the prefab of the current weapon
                weapons[i].weaponPrefab.SetActive(i == currentIndex);
            }
        }
    }

    void LoadWeaponData()
    {
        foreach (var weapon in weapons)
        {
            weapon.isUnlocked = PlayerPrefs.GetInt(WeaponUnlockedKeyPrefix + weapon.weaponID, 0) == 1;
            currentSelectedWeaponIndex = PlayerPrefs.GetInt(SelectedWeaponKey);
        }
    }
}
