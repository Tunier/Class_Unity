using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LoadingSceneController : MonoBehaviour
{
    [SerializeField]
    CanvasGroup canvasGroup;
    [SerializeField]
    Image progressBar;
    [SerializeField]
    Image loadingImg;
    [SerializeField]
    Text loadingTxt;

    int random;
    private string loadSceneName;

    const string loadingBG_Path = "LoadingBg/";

    private static LoadingSceneController instance;
    public static LoadingSceneController Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<LoadingSceneController>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    instance = Create(); //유일하면 로딩UI프리팹을 생성
                }
            }
            return instance;
        }
    }

    private static LoadingSceneController Create()
    {
        //로딩 ui생성
        return Instantiate(Resources.Load<LoadingSceneController>("LoadingUI"));
    }

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string _sceneName)
    {
        gameObject.SetActive(true);                 //로딩 ui 켜기             
        SceneManager.sceneLoaded += OnSceneLoaded;  //신로드될때 onsceneloaded 델리게이트 체인
        loadSceneName = _sceneName;
        StartCoroutine(LoadSceneProcess());
    }

    private IEnumerator LoadSceneProcess()
    {
        RandomLoad();
        progressBar.fillAmount = 0f;
        yield return StartCoroutine(Fade(true));                        //fade out

        AsyncOperation op = SceneManager.LoadSceneAsync(loadSceneName); //로드될 신 싱크
        op.allowSceneActivation = false;                                //로드할 씬 바로 부르지 말기

        float timer = 0f;
        while (!op.isDone)                                              //씬 씽크가 안끝났으면
        {
            yield return null;
            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = op.progress;                   //진행률이 0.9이하일 때 fillamount를 진행률과 동률시 한다.
            }
            else
            {
                timer += Time.unscaledDeltaTime;                        //진행률이 0.9이상일때 타이머에 전프레임 완료 시간을 더한다
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
                if (progressBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;                     //progressbar가득 차면 씬을 받아온다.
                    yield break;
                }
            }
        }
    }

    private void RandomLoad()
    {
        random = UnityEngine.Random.Range(0, 4);
        switch (random)
        {
            case 0:
                loadingImg.sprite = Resources.Load<Sprite>(loadingBG_Path + "Goblin");
                loadingTxt.text = "팁 : 고블린은 호전적인 종족입니다. 조심하세요~";
                break;
            case 1:
                loadingImg.sprite = Resources.Load<Sprite>(loadingBG_Path + "Human");
                loadingTxt.text = "팁 : 국왕폐하는 돼지고기를 좋아합니다.";
                break;
            case 2:
                loadingImg.sprite = Resources.Load<Sprite>(loadingBG_Path + "Undead");
                loadingTxt.text = "팁 : 언데드도 술마시는것을 좋아합니다. 근데 술은 어디로 가죠?";
                break;
            case 3:
                loadingImg.sprite = Resources.Load<Sprite>(loadingBG_Path + "Elven");
                loadingTxt.text = "팁 : 엘프는 순록을 즐겨 탑니다. 산타의 후예라는 소문도...";
                break;
        }
    }
    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name == loadSceneName)
        {
            StartCoroutine(Fade(false)); //fade in
            SceneManager.sceneLoaded -= OnSceneLoaded; //델리게이트 체인 빼기
        }
    }

    private IEnumerator Fade(bool _isFadeIn)
    {
        float timer = 0f;
        while (timer <= 1f)
        {
            yield return null;
            timer += Time.unscaledDeltaTime * 3f;
            canvasGroup.alpha = _isFadeIn ? Mathf.Lerp(0f, 1f, timer) : Mathf.Lerp(1f, 0f, timer);
        }

        if (!_isFadeIn)
        {
            gameObject.SetActive(false);
        }
    }
}
