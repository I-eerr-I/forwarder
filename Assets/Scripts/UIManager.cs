using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Slider[] xpSliders;
    public Text levelText;

    private float known_xp;
    private float known_level;

    private GameObject player;
    private PlayerController pc;

    public void SetPlayer(ref GameObject player, ref PlayerController pc)
    {
        this.player = player;
        this.pc = pc;
    }

    void Start()
    {
        known_xp = pc.xp;
        known_level = pc.level;
        InitStartUI();
    }

    void Update()
    {
        if(known_xp != pc.xp)
        {
            SetSlidersValue(pc.xp/pc.GetMaxLevelXP());
            known_xp = pc.xp;
        }
        if(known_level != pc.level)
        {
            levelText.text = pc.level.ToString();
            known_level = pc.level;
        }
    }

    void InitStartUI()
    {
        levelText.text = pc.level.ToString();
        SetSlidersValue(0);
    }

    void SetSlidersValue(float value)
    {
        foreach(Slider slider in xpSliders)
        {
            slider.value = value;
        }
    }
}
