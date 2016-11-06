using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class InfantryUpgradeStats {
    public int currentHealthLevel = 1;
    public int HealthLevels = 3;
    public int currentDamageLevel = 1;
    public int DamageLevels = 3;
    public List<int> healthCosts;
    public List<int> healthValues;
    public List<int> damageCosts;
    public List<int> damageValues;
}

[System.Serializable]
public class RangedUpgradeStats {
    public int currentHealthLevel = 1;
    public int HealthLevels = 3;
    public int currentDamageLevel = 1;
    public int DamageLevels = 3;
    public List<int> healthCosts;
    public List<int> healthValues;
    public List<int> damageCosts;
    public List<int> damageValues;
}

[System.Serializable]
public class FlyingUpgradeStats {
    public int currentHealthLevel = 1;
    public int HealthLevels = 3;
    public int currentSpeedLevel = 1;
    public int SpeedLevels = 3;
    public List<int> healthCosts;
    public List<int> healthValues;
    public List<int> speedCosts;
    public List<int> speedValues;
}

[System.Serializable]
public class EngineerUpgradeStats {
    public int currentHealthLevel = 1;
    public int HealthLevels = 3;
    public List<int> healthCosts;
    public List<int> healthValues;
}

public class Upgrade : MonoBehaviour {
    public static Upgrade S;

    public InfantryUpgradeStats     infUpStats;
    public RangedUpgradeStats       rangUpStats;
    public FlyingUpgradeStats       flyUpStats;
    public EngineerUpgradeStats     engUpStats;

    [Header("Hover Text references")]
    public GameObject InfantryHealthHover;
    public GameObject InfantryDamageHover;
    public GameObject RangedHealthHover;
    public GameObject RangedDamageHover;
    public GameObject FlyingHealthHover;
    public GameObject FlyingSpeedHover;
    public GameObject EngineerHealthHover;

    [Header("Button references")]
    public GameObject InfantryHealthButton;
    public GameObject InfantryDamageButton;
    public GameObject RangedHealthButton;
    public GameObject RangedDamageButton;
    public GameObject FlyingHealthButton;
    public GameObject FlyingSpeedButton;
    public GameObject EngineerHealthButton;

    void Awake() {
        S = this;
    }

    void Start() {
        UpdateCostText(InfantryHealthButton, infUpStats.healthCosts[infUpStats.currentHealthLevel].ToString());
        UpdateCostText(InfantryDamageButton, infUpStats.damageCosts[infUpStats.currentDamageLevel].ToString());
        UpdateCostText(RangedHealthButton, rangUpStats.healthCosts[rangUpStats.currentDamageLevel].ToString());
        UpdateCostText(RangedDamageButton, rangUpStats.damageCosts[rangUpStats.currentDamageLevel].ToString());
        UpdateCostText(FlyingHealthButton, flyUpStats.healthCosts[flyUpStats.currentHealthLevel].ToString());
        UpdateCostText(FlyingSpeedButton, flyUpStats.speedCosts[flyUpStats.currentSpeedLevel].ToString());
        UpdateCostText(EngineerHealthButton, engUpStats.healthCosts[engUpStats.currentHealthLevel].ToString());
    }

    #region Health and Damage Upgrade functions
    public void InfantryUpgradeHealth() {
        if (infUpStats.currentHealthLevel >= infUpStats.HealthLevels) {
            UI.S.PlaySound("Reject");
            return;
        }
        int currentResource = ResourceManager.S.Resources;
        int cost = infUpStats.healthCosts[infUpStats.currentHealthLevel];
        if (currentResource >= cost) {
            UI.S.changeResources(-cost);
            infUpStats.currentHealthLevel++;

            if (infUpStats.currentHealthLevel == infUpStats.HealthLevels) {
                UpdateCostText(InfantryHealthButton, "n/a");
            }
            else {
                UpdateCostText(InfantryHealthButton, infUpStats.healthCosts[infUpStats.currentHealthLevel].ToString());
            }

            UI.S.PlaySound("Chaching");
            InfantryHealthHoverFunction(true);
            print("Infantry health level changed to:" + infUpStats.currentHealthLevel);
        }
        else {
            UI.S.PlaySound("Reject");
        }
    }

    public void InfantryUpgradeDamage() {
        if (infUpStats.currentDamageLevel >= infUpStats.DamageLevels) {
            UI.S.PlaySound("Reject");
            return;
        }
        int currentResource = ResourceManager.S.Resources;
        int cost = infUpStats.damageCosts[infUpStats.currentDamageLevel];
        if (currentResource >= cost) {
            UI.S.changeResources(-cost);
            infUpStats.currentDamageLevel++;

            if (infUpStats.currentDamageLevel == infUpStats.DamageLevels) {
                UpdateCostText(InfantryDamageButton, "n/a");
            }
            else {
                UpdateCostText(InfantryDamageButton, infUpStats.damageCosts[infUpStats.currentDamageLevel].ToString());
            }

            UI.S.PlaySound("Chaching");
            InfantryDamageHoverFunction(true);
            print("Infantry damage level changed to:" + infUpStats.currentDamageLevel);
        }
        else {
            UI.S.PlaySound("Reject");
        }
    }

    public void RangedUpgradeHealth() {
        if (rangUpStats.currentHealthLevel >= rangUpStats.HealthLevels) {
            UI.S.PlaySound("Reject");
            return;
        }
        int currentResource = ResourceManager.S.Resources;
        int cost = rangUpStats.healthCosts[rangUpStats.currentHealthLevel];
        if (currentResource >= cost) {
            UI.S.changeResources(-cost);
            rangUpStats.currentHealthLevel++;

            if (rangUpStats.currentHealthLevel == rangUpStats.HealthLevels) {
                UpdateCostText(RangedHealthButton, "n/a");
            }
            else {
                UpdateCostText(RangedHealthButton, rangUpStats.healthCosts[rangUpStats.currentHealthLevel].ToString());
            }

            UI.S.PlaySound("Chaching");
            RangedHealthHoverFunction(true);
            print("Archer health level changed to:" + rangUpStats.currentHealthLevel);
        }
        else {
            UI.S.PlaySound("Reject");
        }
    }

    public void RangedUpgradeDamage() {
        if (rangUpStats.currentDamageLevel >= rangUpStats.DamageLevels) {
            UI.S.PlaySound("Reject");
            return;
        }
        int currentResource = ResourceManager.S.Resources;
        int cost = rangUpStats.damageCosts[rangUpStats.currentDamageLevel];
        if (currentResource >= cost) {
            UI.S.changeResources(-cost);
            rangUpStats.currentDamageLevel++;

            if (rangUpStats.currentDamageLevel == rangUpStats.DamageLevels) {
                UpdateCostText(RangedDamageButton, "n/a");
            }
            else {
                UpdateCostText(RangedDamageButton, rangUpStats.damageCosts[rangUpStats.currentDamageLevel].ToString());
            }

            UI.S.PlaySound("Chaching");
            RangedDamageHoverFunction(true);
            print("Infantry damage level changed to:" + rangUpStats.currentDamageLevel);
        }
        else {
            UI.S.PlaySound("Reject");
        }
    }

    public void FlyingUpgradeHealth() {
        if (flyUpStats.currentHealthLevel >= flyUpStats.HealthLevels) {
            UI.S.PlaySound("Reject");
            return;
        }
        int currentResource = ResourceManager.S.Resources;
        int cost = flyUpStats.healthCosts[flyUpStats.currentHealthLevel];
        if (currentResource >= cost) {
            UI.S.changeResources(-cost);
            flyUpStats.currentHealthLevel++;

            if (flyUpStats.currentHealthLevel == flyUpStats.HealthLevels) {
                UpdateCostText(FlyingHealthButton, "n/a");
            }
            else {
                UpdateCostText(FlyingHealthButton, flyUpStats.healthCosts[flyUpStats.currentHealthLevel].ToString());
            }

            UI.S.PlaySound("Chaching");
            FlyingHealthHoverFunction(true);
            print("Flying health level changed to:" + flyUpStats.currentHealthLevel);
        }
        else {
            UI.S.PlaySound("Reject");
        }
    }

    public void FlyingUpgradeSpeed() {
        if (flyUpStats.currentSpeedLevel >= flyUpStats.SpeedLevels) {
            UI.S.PlaySound("Reject");
            return;
        }
        int currentResource = ResourceManager.S.Resources;
        int cost = flyUpStats.speedCosts[flyUpStats.currentSpeedLevel];
        if (currentResource >= cost) {
            UI.S.changeResources(-cost);
            flyUpStats.currentSpeedLevel++;

            if (flyUpStats.currentSpeedLevel == flyUpStats.SpeedLevels) {
                UpdateCostText(FlyingSpeedButton, "n/a");
            }
            else {
                UpdateCostText(FlyingSpeedButton, flyUpStats.speedCosts[flyUpStats.currentSpeedLevel].ToString());
            }

            UI.S.PlaySound("Chaching");
            FlyingSpeedHoverFunction(true);
            print("Flying health level changed to:" + flyUpStats.currentSpeedLevel);
        }
        else {
            UI.S.PlaySound("Reject");
        }
    }

    public void EngineerUpgradeHealth() {
        if (engUpStats.currentHealthLevel >= engUpStats.HealthLevels) {
            UI.S.PlaySound("Reject");
            return;
        }
        int currentResource = ResourceManager.S.Resources;
        int cost = engUpStats.healthCosts[engUpStats.currentHealthLevel];
        if (currentResource >= cost) {
            UI.S.changeResources(-cost);
            engUpStats.currentHealthLevel++;

            if (engUpStats.currentHealthLevel == engUpStats.HealthLevels) {
                UpdateCostText(EngineerHealthButton, "n/a");
            }
            else {
                UpdateCostText(EngineerHealthButton, engUpStats.healthCosts[engUpStats.currentHealthLevel].ToString());
            }

            UI.S.PlaySound("Chaching");
            EngineerHealthHoverFunction(true);
            print("Engineer health level changed to:" + engUpStats.currentHealthLevel);
        }
        else {
            UI.S.PlaySound("Reject");
        }
    }

    #endregion

    #region HoverButtonFunctions

    public void InfantryHealthHoverFunction(bool active) {
        if (active) {
            InfantryHealthHover.SetActive(true);
            Text txt = getTextChild(InfantryHealthHover);
            txt.text = "That Guy\n" +
                       "<color=red>Health Upgrade</color>\n";
            if (infUpStats.currentHealthLevel >= infUpStats.HealthLevels) {
                txt.text += "<size=25>Max Level Reached\n</size>" +
                            "<i>Current Health: " + infUpStats.healthValues[infUpStats.currentHealthLevel - 1] + "\n</i>";
            }
            else {
                txt.text += "<size=25>Cost: " + infUpStats.healthCosts[infUpStats.currentHealthLevel] + "\n</size>" +
                            "<i>Current Health: " + infUpStats.healthValues[infUpStats.currentHealthLevel - 1] + "\n</i>" +
                            "<i>Next Level Health: " + infUpStats.healthValues[infUpStats.currentHealthLevel] + "\n</i>";
            }
        }
        else {
            InfantryHealthHover.SetActive(false);
        }
    }

    public void InfantryDamageHoverFunction(bool active) {
        if (active) {
            InfantryDamageHover.SetActive(true);
            Text txt = getTextChild(InfantryDamageHover);
            txt.text = "That Guy\n" +
                       "<color=green>Damage Upgrade</color>\n";

            if (infUpStats.currentDamageLevel >= infUpStats.DamageLevels) {
                txt.text += "<size=25>Max Level Reached\n</size>" +
                            "<i>Current Health: " + infUpStats.damageValues[infUpStats.currentDamageLevel - 1] + "\n</i>";
            }
            else {
                txt.text += "<size=25>Cost: " + infUpStats.damageCosts[infUpStats.currentDamageLevel] + "\n</size>" +
                            "<i>Current Damage: " + infUpStats.damageValues[infUpStats.currentDamageLevel - 1] + "\n</i>" +
                            "<i>Next Level Damage: " + infUpStats.damageValues[infUpStats.currentDamageLevel] + "\n</i>";
            }
        }
        else {
            InfantryDamageHover.SetActive(false);
        }
    }

    public void RangedHealthHoverFunction(bool active) {
        if (active) {
            RangedHealthHover.SetActive(true);
            Text txt = getTextChild(RangedHealthHover);
            txt.text = "Spitballer\n" +
                       "<color=red>Health Upgrade</color>\n";
            if (rangUpStats.currentHealthLevel >= rangUpStats.HealthLevels) {
                txt.text += "<size=25>Max Level Reached\n</size>" +
                            "<i>Current Health: " + rangUpStats.healthValues[rangUpStats.currentHealthLevel - 1] + "\n</i>";
            }
            else {
                txt.text += "<size=25>Cost: " + rangUpStats.healthCosts[rangUpStats.currentHealthLevel] + "\n</size>" +
                            "<i>Current Health: " + rangUpStats.healthValues[rangUpStats.currentHealthLevel - 1] + "\n</i>" +
                            "<i>Next Level Health: " + rangUpStats.healthValues[rangUpStats.currentHealthLevel] + "\n</i>";
            }
        }
        else {
            RangedHealthHover.SetActive(false);
        }
    }

    public void RangedDamageHoverFunction(bool active) {
        if (active) {
            RangedDamageHover.SetActive(true);
            Text txt = getTextChild(RangedDamageHover);
            txt.text = "Spitballer\n" +
                       "<color=green>Damage Upgrade</color>\n";

            if (rangUpStats.currentDamageLevel >= rangUpStats.DamageLevels) {
                txt.text += "<size=25>Max Level Reached\n</size>" +
                            "<i>Current Health: " + rangUpStats.damageValues[rangUpStats.currentDamageLevel - 1] + "\n</i>";
            }
            else {
                txt.text += "<size=25>Cost: " + rangUpStats.damageCosts[rangUpStats.currentDamageLevel] + "\n</size>" +
                            "<i>Current Damage: " + rangUpStats.damageValues[rangUpStats.currentDamageLevel - 1] + "\n</i>" +
                            "<i>Next Level Damage: " + rangUpStats.damageValues[rangUpStats.currentDamageLevel] + "\n</i>";
            }
        }
        else {
            RangedDamageHover.SetActive(false);
        }
    }

    public void FlyingHealthHoverFunction(bool active) {
        if (active) {
            FlyingHealthHover.SetActive(true);
            Text txt = getTextChild(FlyingHealthHover);
            txt.text = "Class Clown\n" +
                       "<color=red>Health Upgrade</color>\n";
            if (flyUpStats.currentHealthLevel >= flyUpStats.HealthLevels) {
                txt.text += "<size=25>Max Level Reached\n</size>" +
                            "<i>Current Health: " + flyUpStats.healthValues[flyUpStats.currentHealthLevel - 1] + "\n</i>";
            }
            else {
                txt.text += "<size=25>Cost: " + flyUpStats.healthCosts[flyUpStats.currentHealthLevel] + "\n</size>" +
                            "<i>Current Health: " + flyUpStats.healthValues[flyUpStats.currentHealthLevel - 1] + "\n</i>" +
                            "<i>Next Level Health: " + flyUpStats.healthValues[flyUpStats.currentHealthLevel] + "\n</i>";
            }
        }
        else {
            FlyingHealthHover.SetActive(false);
        }
    }

    public void FlyingSpeedHoverFunction(bool active) {
        if (active) {
            FlyingSpeedHover.SetActive(true);
            Text txt = getTextChild(FlyingSpeedHover);
            txt.text = "Class Clown\n" +
                       "<color=blue>Speed Upgrade</color>\n";
            if (flyUpStats.currentSpeedLevel >= flyUpStats.HealthLevels) {
                txt.text += "<size=25>Max Level Reached\n</size>" +
                            "<i>Current Speed: " + flyUpStats.speedValues[flyUpStats.currentSpeedLevel - 1] + "\n</i>";
            }
            else {
                txt.text += "<size=25>Cost: " + flyUpStats.speedCosts[flyUpStats.currentSpeedLevel] + "\n</size>" +
                            "<i>Current Speed: " + flyUpStats.speedValues[flyUpStats.currentSpeedLevel - 1] + "\n</i>" +
                            "<i>Next Level Speed: " + flyUpStats.speedValues[flyUpStats.currentSpeedLevel] + "\n</i>";
            }
        }
        else {
            FlyingSpeedHover.SetActive(false);
        }
    }

    public void EngineerHealthHoverFunction(bool active) {
        if (active) {
            EngineerHealthHover.SetActive(true);
            Text txt = getTextChild(EngineerHealthHover);
            txt.text = "Nerd\n" +
                       "<color=red>Health Upgrade</color>\n";
            if (engUpStats.currentHealthLevel >= engUpStats.HealthLevels) {
                txt.text += "<size=25>Max Level Reached\n</size>" +
                            "<i>Current Health: " + engUpStats.healthValues[engUpStats.currentHealthLevel - 1] + "\n</i>";
            }
            else {
                txt.text += "<size=25>Cost: " + engUpStats.healthCosts[engUpStats.currentHealthLevel] + "\n</size>" +
                            "<i>Current Health: " + engUpStats.healthValues[engUpStats.currentHealthLevel - 1] + "\n</i>" +
                            "<i>Next Level Health: " + engUpStats.healthValues[engUpStats.currentHealthLevel] + "\n</i>";
            }
        }
        else {
            EngineerHealthHover.SetActive(false);
        }
    }

    #endregion

    public int getUpgradedHealth(UnitType type) {
        if (type == UnitType.UnitBasic) {
            return infUpStats.healthValues[infUpStats.currentHealthLevel - 1];
        }
        else if (type == UnitType.Archer) {
            return rangUpStats.healthValues[rangUpStats.currentHealthLevel - 1];
        }
        else if (type == UnitType.Balloon) {
            return flyUpStats.healthValues[flyUpStats.currentHealthLevel - 1];
        }
        else if (type == UnitType.Engineer) {
            return engUpStats.healthValues[engUpStats.currentHealthLevel - 1];
        }
        else {
            return 0;
        }
    }

    public int getUpgradedDamage(UnitType type) {
        if (type == UnitType.UnitBasic) {
            return infUpStats.damageValues[infUpStats.currentDamageLevel - 1];
        }
        else if (type == UnitType.Archer) {
            return rangUpStats.damageValues[rangUpStats.currentDamageLevel - 1];
        }
        else {
            return 0;
        }
    }

    public int getUpgradedSpeed(UnitType type) {
        if (type == UnitType.Balloon) {
            return flyUpStats.speedValues[flyUpStats.currentSpeedLevel - 1];
        }
        else {
            return 0;
        }
    }

    // Works only if the parent gameObject has one Text object as child, Used only for hover buttons really
    public Text getTextChild(GameObject GO) {
        //GameObject temp = GO.transform.GetChild(0).gameObject;
        return (GO.transform.GetChild(0).gameObject.GetComponent<Text>());
    }

    private void UpdateCostText(GameObject GO, string value) {
        Text txt = getTextChild(GO);
        txt.text = value;
    }

}
