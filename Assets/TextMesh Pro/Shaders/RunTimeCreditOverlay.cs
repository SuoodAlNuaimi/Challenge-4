using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class RuntimeOverlay : MonoBehaviour
{
    private TMP_Text creditText;
    private CanvasGroup canvasGroup;

    private const float fadeDuration = 0.6f;

    private float waveAmplitude = 8f;     // height of wave
    private float waveFrequency = 2f;     // spacing between letters
    private float waveSpeed = 2f;         // animation speed

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void CreateOverlay()
    {
        GameObject root = new GameObject("RuntimeOverlay");
        DontDestroyOnLoad(root);
        root.AddComponent<RuntimeOverlay>();
    }

    private void Awake()
    {
        CreateCanvas();
        StartCoroutine(FadeLoop());
    }

    private void CreateCanvas()
    {
        GameObject canvasGO = new GameObject("CreditCanvas");
        canvasGO.transform.SetParent(transform);

        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 9999;

        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>().enabled = false;

        canvasGroup = canvasGO.AddComponent<CanvasGroup>();

        GameObject textGO = new GameObject("CreditText");
        textGO.transform.SetParent(canvasGO.transform);

        creditText = textGO.AddComponent<TextMeshProUGUI>();
        creditText.text = "By HUSSAIN.";
        creditText.fontSize = 70;
        creditText.fontStyle = FontStyles.Bold;
        creditText.color = Color.red;
        creditText.alignment = TextAlignmentOptions.Center;

        RectTransform rect = creditText.rectTransform;
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = new Vector2(800, 200);
    }

    private IEnumerator FadeLoop()
    {
        while (true)
        {
            yield return Fade(0f, 1f);
            yield return Fade(1f, 0f);
        }
    }

    private IEnumerator Fade(float from, float to)
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / fadeDuration;
            canvasGroup.alpha = Mathf.Lerp(from, to, t);
            yield return null;
        }

        canvasGroup.alpha = to;
    }

    private void Update()
    {
        AnimateWave();
    }

    private void AnimateWave()
    {
        creditText.ForceMeshUpdate();

        TMP_TextInfo textInfo = creditText.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible)
                continue;

            int vertexIndex = textInfo.characterInfo[i].vertexIndex;
            int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

            Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;

            float offset = Mathf.Sin(Time.unscaledTime * waveSpeed + i * waveFrequency) * waveAmplitude;

            Vector3 waveOffset = new Vector3(0, offset, 0);

            vertices[vertexIndex + 0] += waveOffset;
            vertices[vertexIndex + 1] += waveOffset;
            vertices[vertexIndex + 2] += waveOffset;
            vertices[vertexIndex + 3] += waveOffset;
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            creditText.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }
}