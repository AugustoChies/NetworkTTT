using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;

public class MenuUIController : MonoBehaviour
{    
    #region DificultySelection
    public Canvas dificultyCanvas;
    public string suicidal,random,mixed,impossible;
    public TextMeshProUGUI ttText;
    protected Button disabledButton;
    public Button startButton;
    #endregion

    
    public Canvas AISelectCanvas;

    #region Audio
    public AudioMixer mixer;
    public Slider masterSlider, musicSlider, sfxSlider;
    public float volumeMaster, volumeMusic, volumeSFX;
    #endregion



    void Start()
    {
        AISelectCanvas.enabled = true;
        dificultyCanvas.enabled = false;
        GlobalInfo.aiSettings = AISettings.impossible;
        startButton.interactable = false;
        mixer.GetFloat("volumeMaster",out volumeMaster);
        mixer.GetFloat("volumeMusic", out volumeMusic);
        mixer.GetFloat("volumeSFX", out volumeSFX);
        masterSlider.value = volumeMaster + 60;
        musicSlider.value = volumeMusic + 60;
        sfxSlider.value = volumeSFX + 60;
    }

    private void Update()
    {
        mixer.SetFloat("volumeMaster", masterSlider.value -60);
        mixer.SetFloat("volumeMusic",musicSlider.value -60);
        mixer.SetFloat("volumeSFX", sfxSlider.value - 60);
    }

    public void ButtonSuicide(Button clicked)
    {
        this.GetComponent<AudioSource>().Play();
        if (disabledButton != null)
        {
            disabledButton.interactable = true;
        }
        disabledButton = clicked;
        disabledButton.interactable = false;
        GlobalInfo.aiSettings = AISettings.suicide;
        ttText.text = suicidal;
        startButton.interactable = true;
    }

    public void ButtonRand(Button clicked)
    {
        this.GetComponent<AudioSource>().Play();
        if (disabledButton != null)
        {
            disabledButton.interactable = true;
        }
        disabledButton = clicked;
        disabledButton.interactable = false;
        GlobalInfo.aiSettings = AISettings.random;
        ttText.text = random;
        startButton.interactable = true;

    }

    public void ButtonMix(Button clicked)
    {
        this.GetComponent<AudioSource>().Play();
        if (disabledButton != null)
        {
            disabledButton.interactable = true;
        }
        disabledButton = clicked;
        disabledButton.interactable = false;
        GlobalInfo.aiSettings = AISettings.semi;
        ttText.text = mixed;
        startButton.interactable = true;
    }

    public void ButtonMinMax(Button clicked)
    {
        this.GetComponent<AudioSource>().Play();
        if (disabledButton != null)
        {
            disabledButton.interactable = true;
        }
        disabledButton = clicked;
        disabledButton.interactable = false;
        GlobalInfo.aiSettings = AISettings.impossible;
        ttText.text = impossible;
        startButton.interactable = true;
    }

    public void StartButton()
    {
        this.GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("GameScene");
    }


    public void SelectPvP()
    {
        this.GetComponent<AudioSource>().Play();
        GlobalInfo.isAi[0] = false;
        GlobalInfo.isAi[1] = false;
        AISelectCanvas.enabled = false;
        StartButton();
    }

    public void SelectPvBot()
    {
        this.GetComponent<AudioSource>().Play();
        GlobalInfo.isAi[0] = false;
        GlobalInfo.isAi[1] = true;
        AISelectCanvas.enabled = false;
        dificultyCanvas.enabled = true;
    }

    public void SelectBotvP()
    {
        this.GetComponent<AudioSource>().Play();
        GlobalInfo.isAi[0] = true;
        GlobalInfo.isAi[1] = false;
        AISelectCanvas.enabled = false;
        dificultyCanvas.enabled = true;
    }

    public void SelectBotvBot()
    {
        this.GetComponent<AudioSource>().Play();
        GlobalInfo.isAi[0] = true;
        GlobalInfo.isAi[1] = true;
        AISelectCanvas.enabled = false;
        dificultyCanvas.enabled = true;
    }
}
