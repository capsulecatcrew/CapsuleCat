using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public virtual void StartAttack(){}

    public delegate void FinishDelegate();

    public event FinishDelegate OnFinish;

    protected void DeclareAttackDone()
    {
        OnFinish?.Invoke();
    }
}
