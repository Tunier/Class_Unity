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
                    instance = Create(); //�����ϸ� �ε�UI�������� ����
                }
            }
            return instance;
        }
    }

    private static LoadingSceneController Create()
    {
        //�ε� ui����
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
        gameObject.SetActive(true);                 //�ε� ui �ѱ�             
        SceneManager.sceneLoaded += OnSceneLoaded;  //�ŷε�ɶ� onsceneloaded ��������Ʈ ü��
        loadSceneName = _sceneName;
        StartCoroutine(LoadSceneProcess());
    }

    private IEnumerator LoadSceneProcess()
    {
        RandomLoad();
        progressBar.fillAmount = 0f;
        yield return StartCoroutine(Fade(true));                        //fade out

        AsyncOperation op = SceneManager.LoadSceneAsync(loadSceneName); //�ε�� �� ��ũ
        op.allowSceneActivation = false;                                //�ε��� �� �ٷ� �θ��� ����

        float timer = 0f;
        while (!op.isDone)                                              //�� ��ũ�� �ȳ�������
        {
            yield return null;
            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = op.progress;                   //������� 0.9������ �� fillamount�� ������� ������ �Ѵ�.
            }
            else
            {
                timer += Time.unscaledDeltaTime;                        //������� 0.9�̻��϶� Ÿ�̸ӿ� �������� �Ϸ� �ð��� ���Ѵ�
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
                if (progressBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;                     //progressbar���� ���� ���� �޾ƿ´�.
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
                loadingTxt.text = "�� : ����� ȣ������ �����Դϴ�. �����ϼ���~";
                break;
            case 1:
                loadingImg.sprite = Resources.Load<Sprite>(loadingBG_Path + "Human");
                loadingTxt.text = "�� : �������ϴ� ������⸦ �����մϴ�.";
                break;
            case 2:
                loadingImg.sprite = Resources.Load<Sprite>(loadingBG_Path + "Undead");
                loadingTxt.text = "�� : �𵥵嵵 �����ô°��� �����մϴ�. �ٵ� ���� ���� ����?";
                break;
            case 3:
                loadingImg.sprite = Resources.Load<Sprite>(loadingBG_Path + "Elven");
                loadingTxt.text = "�� : ������ ������ ��� ž�ϴ�. ��Ÿ�� �Ŀ���� �ҹ���...";
                break;
        }
    }
    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name == loadSceneName)
        {
            StartCoroutine(Fade(false)); //fade in
            SceneManager.sceneLoaded -= OnSceneLoaded; //��������Ʈ ü�� ����
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
