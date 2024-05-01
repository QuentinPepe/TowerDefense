using DG.Tweening;
using UnityEngine;
public class OpenCloseAnimationPanel : MonoBehaviour
{
    public void Show()
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }

    public void Hide()
    {
        transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() => gameObject.SetActive(false));
    }

    public bool IsOpen => gameObject.activeSelf;
}