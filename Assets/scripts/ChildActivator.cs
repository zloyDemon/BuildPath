using UnityEngine;

public class ChildActivator : MonoBehaviour
{
    public int CurrentChildIndex
    {
        get
        {
            if (transform.gameObject.activeInHierarchy)
            {
                for (int i = 0; i < transform.childCount; ++i)
                {
                    if (transform.GetChild(i).gameObject.activeSelf)
                        return i;
                }
            }

            return -1;
        }

        set
        {
            if (!enabled)
                return;
            if (value >= transform.childCount)
                return;
            for (int i = 0; i < transform.childCount; ++i)
                transform.GetChild(i).gameObject.SetActive(i == value);
        }
    }
}
