﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Upgrade
{
    public int upgradeCost;
    [Multiline] public string upgradeDescription;
    public int upgradeValue;
}

public class ShopMenu : MonoBehaviour
{
    public Text currency = null;
    public Text health = null;
    public Text speed = null;
    public Text damage = null;
    public Text ammo = null;

    public List<Upgrade> healthUpgrades = null;
    public List<Upgrade> speedUpgrades = null;
    public List<Upgrade> damageUpgrades = null;
    public List<Upgrade> ammoUpgrades = null;

    bool isHealthMaxed = false;
    bool isSpeedMaxed = false;
    bool isDamageMaxed = false;
    bool isAmmoMaxed = false;

    private void OnEnable()
    {
        SaveState saveState = SaveManager.Instance.state;

        isHealthMaxed = MaxUpgradeCheck(saveState.healthUpgrades, healthUpgrades, health);
        isSpeedMaxed = MaxUpgradeCheck(saveState.speedUpgrades, speedUpgrades, speed);
        isDamageMaxed = MaxUpgradeCheck(saveState.damageUpgrades, damageUpgrades, damage);
        isAmmoMaxed = MaxUpgradeCheck(saveState.capacityUpgrades, ammoUpgrades, ammo);

        currency.text = "Currency: " + saveState.currency.ToString();
    }

    public void BuyHealth()
    {
        if (isHealthMaxed) return;

        SaveState saveState = SaveManager.Instance.state;
        Upgrade healthUpgrade = healthUpgrades[saveState.healthUpgrades];

        if (healthUpgrade.upgradeCost <= saveState.currency)
        {
            saveState.currency -= healthUpgrade.upgradeCost;
            currency.text = "Currency: " + saveState.currency.ToString();

            saveState.maxHealth += healthUpgrade.upgradeValue;

            saveState.healthUpgrades++;

            isHealthMaxed = MaxUpgradeCheck(saveState.healthUpgrades, healthUpgrades, health);
        }
    }

    public void BuySpeed()
    {
        if (isSpeedMaxed) return;

        SaveState saveState = SaveManager.Instance.state;
        Upgrade speedUpgrade = speedUpgrades[saveState.speedUpgrades];

        if (speedUpgrade.upgradeCost <= saveState.currency)
        {
            saveState.currency -= speedUpgrade.upgradeCost;
            currency.text = "Currency: " + saveState.currency.ToString();

            saveState.movementSpeed += speedUpgrade.upgradeValue;

            saveState.speedUpgrades++;

            isSpeedMaxed = MaxUpgradeCheck(saveState.speedUpgrades, speedUpgrades, speed);
        }
    }

    public void BuyDamage()
    {
        if (isDamageMaxed) return;

        SaveState saveState = SaveManager.Instance.state;
        Upgrade damageUpgrade = damageUpgrades[saveState.damageUpgrades];

        if (damageUpgrade.upgradeCost <= saveState.currency)
        {
            saveState.currency -= damageUpgrade.upgradeCost;
            currency.text = "Currency: " + saveState.currency.ToString();

            saveState.bulletDamage += damageUpgrade.upgradeValue;

            saveState.damageUpgrades++;

            isDamageMaxed = MaxUpgradeCheck(saveState.damageUpgrades, damageUpgrades, damage);
        }
    }

    public void BuyAmmo()
    {
        if (isAmmoMaxed) return;

        SaveState saveState = SaveManager.Instance.state;
        Upgrade ammoUpgrade = ammoUpgrades[saveState.capacityUpgrades];

        if (ammoUpgrade.upgradeCost <= saveState.currency)
        {
            saveState.currency -= ammoUpgrade.upgradeCost;
            currency.text = "Currency: " + saveState.currency.ToString();

            saveState.ammoCapacity += ammoUpgrade.upgradeValue;
            saveState.capacityUpgrades++;

            isAmmoMaxed = MaxUpgradeCheck(saveState.capacityUpgrades, ammoUpgrades, ammo);
        }
    }

    private bool MaxUpgradeCheck(int ownedUpgrades, List<Upgrade> upgrades, Text text)
    {
        if (ownedUpgrades >= upgrades.Count)
        {
            text.text = "Maxed Out!";
            return true;
        }
        else
        {
            text.text = upgrades[ownedUpgrades].upgradeDescription;
            return false;
        }
    }
}