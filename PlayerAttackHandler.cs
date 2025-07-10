using System.Collections;
using UnityEngine;

public class PlayerAttackHandler : MonoBehaviour
{
    public GameObject hitbox;

    public void PerformAttack()
    {
        StartCoroutine(EnableHitboxTemporarily());
    }

    IEnumerator EnableHitboxTemporarily()
    {
        hitbox.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        hitbox.SetActive(false);
    }
}
