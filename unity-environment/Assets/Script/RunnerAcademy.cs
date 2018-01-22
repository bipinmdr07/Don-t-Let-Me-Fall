using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AgentSetup {
    public RunnerAgent runnerPrefab;
    public Brain defaultBrain;
    public Brain playerBrain;
    public Color color = Color.blue;
}

public class RunnerAcademy : Academy {

    [Header("Game specific")]
    [SerializeField]
    private GameObject killzone;

    [SerializeField]
    private Transform scrollRoot;

    [SerializeField]
    private float scrollSpeed;

    [Header("Agent Setup")]
    [SerializeField]
    private AgentSetup[] agentSetup;

    [Header("Runner Sequences")]
    [SerializeField]
    private RunnerSequence startSequence;

    [SerializeField]
    private RunnerSequence[] midSequences;

    [SerializeField]
    private RunnerSequence endSequence;

    [Header("UI System")]
    [SerializeField]
    private Text controlText;

    private bool isAIControlled = true;

    private List<RunnerAgent> Agents = new List<RunnerAgent>();

    public override void InitializeAcademy() {
        Physics.gravity = new Vector3(0, -18, 0);

        InstantiateObjects();
    }

    private void InstantiateObjects() {
        var zone = Instantiate(killzone, new Vector3(40, -1.75f, agentSetup.Length / 2f - 0.5f), Quaternion.identity);
        zone.transform.localScale = new Vector3(100, 0.5f, agentSetup.Length);

        for (int i = 0; i < agentSetup.Length; i++) {
            var agent = Instantiate(agentSetup[i].runnerPrefab, new Vector3(0, 1, i * 1), Quaternion.identity) as RunnerAgent;
            Brain brain = (isAIControlled) ? agentSetup[i].defaultBrain : agentSetup[i].playerBrain;
            agent.GiveBrain(brain);
            Agents.Add(agent);
        }
    }

    public override void AcademyReset()
	{
        scrollRoot.localPosition = Vector3.zero;

        foreach (Transform child in scrollRoot) {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < agentSetup.Length; i++) {
            GenerateAgentLevel(i);
        }
    }

    private void GenerateAgentLevel(int i) {
        Color sequenceColor = agentSetup[i].color;

        float posZ = i * 1;

        int posX = AddSequence(startSequence, new Vector3(0, 0, posZ), sequenceColor);

        while (posX + endSequence.Size < scrollSpeed * maxSteps) {
            posX += AddSequence(midSequences[UnityEngine.Random.Range(0, midSequences.Length)], new Vector3(posX, 0, posZ), sequenceColor);
        }

        AddSequence(endSequence, new Vector3(posX, 0, posZ), sequenceColor);
    }

    private int AddSequence(RunnerSequence sequence, Vector3 pos, Color color) {
        var newSequence = Instantiate(sequence, pos, Quaternion.identity, scrollRoot) as RunnerSequence;

        var childMaterials = newSequence.GetComponentsInChildren<MeshRenderer>();
        foreach (var m in childMaterials) {
            m.material.color = color;
        }

        return sequence.Size;

    }

    public override void AcademyStep()
	{
        controlText.text = ((isAIControlled) ? "AI" : "Player") + " Controlled";

        scrollRoot.localPosition = new Vector3(scrollRoot.localPosition.x - scrollSpeed, 0, 0);

        if (Agents.TrueForAll((a) => a.done))
            done = true;
    }

    public void ToggleAgentBrains() {
        for (int i = 0; i < Agents.Count; i++) {
            isAIControlled = ! (Agents[i].brain == agentSetup[i].defaultBrain);

            Brain brain = (isAIControlled) ? agentSetup[i].defaultBrain : agentSetup[i].playerBrain;

            Agents[i].GiveBrain(brain);
        }
    }
}
