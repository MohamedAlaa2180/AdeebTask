using UnityEngine;

public abstract class BaseScreen : MonoBehaviour
{
    public BaseScreen ShowScreen()
    {
        gameObject.SetActive(true);
        return this;
    }

    public virtual void HideScreen()
    {
        gameObject.SetActive(false);
    }
}
