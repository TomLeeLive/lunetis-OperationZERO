using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // Center
    [Header("Common Center UI")]
    [SerializeField]
    TextMeshProUGUI speedText;
    [SerializeField]
    TextMeshProUGUI altitudeText;
    [SerializeField]
    AlertUIController alertUIController;

    [Header("1st-3rd View Control")]
    [SerializeField]
    RectTransform commonCenterUI;
    [SerializeField]
    RectTransform firstCenterViewTransform;
    [SerializeField]
    RectTransform thirdCenterViewTransform;
    
    [SerializeField]
    Canvas firstViewCanvas;
    [SerializeField]
    Vector2 firstViewAdjustAngle;

    [Header("1st View Center UI")]
    [SerializeField]
    UVController speedUV;
    [SerializeField]
    UVController altitudeUV;

    [SerializeField]
    Image throttleGauge;
    [SerializeField]
    HeadingUIController headingUIController;

    // Upper Left
    [Header("Upper Left UI")]
    [SerializeField]
    TextMeshProUGUI timeText;
    [SerializeField]
    TextMeshProUGUI scoreText;
    [SerializeField]
    TextMeshProUGUI targetText;
    
    // Lower Right
    [Header("Lower Right UI : Armament")]
    [SerializeField]
    TextMeshProUGUI gunText;
    [SerializeField]
    TextMeshProUGUI mslText;
    [SerializeField]
    TextMeshProUGUI spwText;

    [SerializeField]
    TextMeshProUGUI dmgText;

    [SerializeField]
    GameObject mslIndicator;
    [SerializeField]
    GameObject spwIndicator;

    // Status
    [Header("Lower Right UI : Aircraft/Weapon Status")]
    [SerializeField]
    Image aircraftImage;
    [SerializeField]
    CooldownImage leftMslCooldownImage;
    [SerializeField]
    CooldownImage rightMslCooldownImage;

    [Header("Minimap Misc.")]
    [SerializeField]
    MinimapCompass minimapCompass;

    [Header("Materials")]
    [SerializeField]
    Material spriteMaterial;
    [SerializeField]
    Material fontMaterial;

    [Header("Colors")]
    [SerializeField]
    Color cautionColor;

    float remainTime;
    int score = 0;

    // Center
    public void SetSpeed(int speed)
    {
        string text = string.Format("<mspace=18>{0}</mspace>", speed);
        speedText.text = text;

        speedUV.SetUV(speed);
    }

    public void SetAltitude(int altitude)
    {
        string text = string.Format("<mspace=18>{0}</mspace>", altitude);
        altitudeText.text = text;

        altitudeUV.SetUV(altitude);
    }

    public void SetThrottle(float throttle)
    {
        throttleGauge.fillAmount = (1 + throttle) * 0.5f;
    }

    public void SetHeading(float heading)
    {
        headingUIController.SetHeading(heading);
        minimapCompass.SetCompass(heading);
    }


    public void AdjustFirstViewUI(Vector3 cameraRotation)
    {
        if(cameraRotation.x > 180) cameraRotation.x -= 360;
        if(cameraRotation.y > 180) cameraRotation.y -= 360;
        
        Vector2 canvasResolution = new Vector2(firstViewCanvas.pixelRect.width, firstViewCanvas.pixelRect.height);
        Vector2 convertedRotation = new Vector2(cameraRotation.y * firstViewAdjustAngle.x, cameraRotation.x * firstViewAdjustAngle.y);

        firstCenterViewTransform.anchoredPosition = convertedRotation * canvasResolution;
    }


    // Upper Left
    public void SetRemainTime(int remainTime)
    {
        this.remainTime = (float)remainTime;
    }


    void SetTime()
    {
        if(remainTime <= 0)
        {
            remainTime = 0;
            return;
        }

        remainTime -= Time.deltaTime;
        int seconds = (int)remainTime;

        int min = seconds / 60;
        int sec = seconds % 60;
        int millisec = (int)((remainTime - seconds) * 100);
        string text = string.Format("TIME <mspace=18>{0:00}</mspace>:<mspace=18>{1:00}</mspace>:<mspace=18>{2:00}</mspace>", min, sec, millisec);
        timeText.text = text;
    }

    public void SetScoreText(int score)
    {
        this.score += score;
        string text = string.Format("SCORE <mspace=18>{0}</mspace>", this.score);
        scoreText.text = text;
    }

    public void SetTargetText(ObjectInfo objectInfo)
    {
        if(objectInfo == null || objectInfo.ObjectName == "")
        {
            targetText.text = "";
        }
        else
        {
            string objectName = objectInfo.ObjectName + " " + objectInfo.ObjectNickname;
            string text = string.Format("TARGET {0} +<mspace=18>{1}</mspace>", objectName, objectInfo.Score);
            targetText.text = text;
        }
    }


    // Lower Right
    public void SetGunText(int bullets)
    {
        string text = string.Format("<align=left>GUN<line-height=0>\n<align=right><mspace=18>{0}</mspace><line-height=0>", bullets);
        gunText.text = text;
    }

    public void SetMissileText(int missiles)
    {
        string text = string.Format("<align=left>MSL<line-height=0>\n<align=right><mspace=18>{0}</mspace><line-height=0>", missiles);
        mslText.text = text;
    }

    public void SetSpecialWeaponText(string specialWeaponName, int specialWeapons)
    {
        string text = string.Format("<align=left>{0}<line-height=0>\n<align=right><mspace=18>{1}</mspace><line-height=0>", specialWeaponName, specialWeapons);
        spwText.text = text;
    }

    public void SetDamage(int damage)
    {
        string text = string.Format("<align=left>DMG<line-height=0>\n<align=right>{0}%<line-height=0>", damage);
        dmgText.text = text;

        if(damage < 34)
        {
            aircraftImage.color = GameManager.NormalColor;
        }
        else if(damage < 67)
        {
            aircraftImage.color = cautionColor;
        }
        else
        {
            aircraftImage.color = GameManager.WarningColor;
        }
    }

    public void SwitchWeapon(WeaponSlot[] weaponSlots, bool useSpecialWeapon, Missile missile)
    {
        mslIndicator.SetActive(!useSpecialWeapon);
        spwIndicator.SetActive(useSpecialWeapon);

        // Justify that weaponSlots contains 2 slots
        leftMslCooldownImage.SetWeaponData(weaponSlots[0], missile.missileFrameSprite, missile.missileFillSprite);
        rightMslCooldownImage.SetWeaponData(weaponSlots[1], missile.missileFrameSprite, missile.missileFillSprite);
    }

    public void SwitchUI(CameraController.CameraIndex index)
    {
        bool isFirstView = (index == CameraController.CameraIndex.FirstView || 
                            index == CameraController.CameraIndex.FirstViewWithCockpit);

        firstCenterViewTransform.gameObject.SetActive(isFirstView);

        RectTransform parentTransform = (isFirstView == true) ? firstCenterViewTransform : thirdCenterViewTransform;
        commonCenterUI.SetParent(parentTransform);

        commonCenterUI.anchoredPosition = Vector2.zero;
    }

    public void SetWarningUIColor(bool isWarning)
    {
        Color color = (isWarning == true) ? GameManager.WarningColor : GameManager.NormalColor;
        spriteMaterial.color = color;
        fontMaterial.SetColor("_FaceColor", color);
    }

    public void SetLabel(AlertUIController.LabelEnum labelEnum)
    {
        alertUIController.SetLabel(labelEnum);
    }

    // Start is called before the first frame update
    void Start()
    {
        remainTime = GameManager.Instance.timeLimit;
        firstViewAdjustAngle = new Vector2(1 / firstViewAdjustAngle.x, 1 / firstViewAdjustAngle.y);

        mslIndicator.SetActive(true);
        spwIndicator.SetActive(false);

        SetScoreText(0);
        SetTargetText(null);
        SetWarningUIColor(false);
    }

    // Update is called once per frame
    void Update()
    {
        SetTime();
    }
}
