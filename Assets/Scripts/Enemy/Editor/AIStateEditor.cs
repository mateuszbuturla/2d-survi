using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AIState))]
public class AIStateEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AIState state = (AIState)target;

        GUILayout.Label("Actions", EditorStyles.boldLabel);
        for (int i = 0; i < state.Actions.Length; i++)
        {
            state.Actions[i] = (AIAction)EditorGUILayout.ObjectField(state.Actions[i], typeof(AIAction), false);
        }

        if (GUILayout.Button("Add Action"))
        {
            ArrayUtility.Add(ref state.Actions, null);
        }

        GUILayout.Label("Transitions", EditorStyles.boldLabel);
        foreach (var transition in state.Transitions)
        {
            transition.Decision = (AIDecision)EditorGUILayout.ObjectField("Decision", transition.Decision, typeof(AIDecision), false);
            transition.TrueStateIndex = EditorGUILayout.IntField("True State", transition.TrueStateIndex);
        }

        if (GUILayout.Button("Add Transition"))
        {
            ArrayUtility.Add(ref state.Transitions, new Transition());
        }

        EditorUtility.SetDirty(target);
    }
}