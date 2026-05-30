using UnityEngine;

public class AnimBehaviourEnter : StateMachineBehaviour
{
    [SerializeField] string[] variableNames;
    [SerializeField] bool[] boolSetValues;

    int[] hashes;

    public override void OnStateEnter(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        if (hashes == null)
        {
            hashes = new int[variableNames.Length];
            for (int i = 0; i < variableNames.Length; i++)
                hashes[i] = Animator.StringToHash(variableNames[i]);
        }

        int count = Mathf.Min(hashes.Length, boolSetValues.Length);

        for (int i = 0; i < count; i++)
            animator.SetBool(hashes[i], boolSetValues[i]);
    }
}
