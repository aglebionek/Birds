using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PopulationManager : MonoBehaviour {
    public GameObject botPrefab;
    public GameObject startingPos;
    public int populationSize = 50;
    List<GameObject> population = new List<GameObject>();
    public static float elapsed = 0;
    public float trialTime = 5;
    int generation = 1;
    public float timeScale = 1;

    GUIStyle guiStyle = new GUIStyle();
    void OnGUI() {
        guiStyle.fontSize = 25;
        guiStyle.normal.textColor = Color.white;
        GUI.BeginGroup(new Rect(10, 10, 250, 150));
        GUI.Box(new Rect(0, 0, 140, 140), "Stats", guiStyle);
        GUI.Label(new Rect(10, 25, 200, 30), "Gen: " + generation, guiStyle);
        GUI.Label(new Rect(10, 50, 200, 30), string.Format("Time: {0:0.00}", elapsed), guiStyle);
        GUI.Label(new Rect(10, 75, 200, 30), "Time scale: " + timeScale, guiStyle);
        GUI.Label(new Rect(10, 100, 200, 30), "Population: " + population.Count, guiStyle);
        GUI.EndGroup();
    }
    
    void Start() {
        for (int i = 0; i < populationSize; i++) {
            GameObject bot = Instantiate(botPrefab, startingPos.transform.position, this.transform.rotation);
            bot.GetComponent<Brain>().Init();
            population.Add(bot);
        }
        Time.timeScale = timeScale;
    }


    GameObject Breed(GameObject parent1, GameObject parent2) {
        GameObject offspring = Instantiate(botPrefab, startingPos.transform.position, this.transform.rotation);
        Brain brain = offspring.GetComponent<Brain>();
        brain.Init();
        brain.dna.Combine(parent1.GetComponent<Brain>().dna, parent2.GetComponent<Brain>().dna);
        if(Random.Range(0, 10) == 1) {
            brain.dna.Mutate();
        }
        return offspring;
    }

    void BreedNewPopulation() {
        List<GameObject> sortedList = population.OrderByDescending(o => o.GetComponent<Brain>().distanceTravelled).ToList();
        Debug.Log(sortedList[0]);
        double averageTopPhenotype = 0;
        for (int i = 0; i < 10; i++) {
            averageTopPhenotype += sortedList[i].GetComponent<Brain>().dna.Phenotype();
        }
        averageTopPhenotype /= 10;
        Debug.Log(averageTopPhenotype);
        population.Clear();
        for (int i = 0; i < (int) sortedList.Count/10f; i++) {
            population.Add(Breed(sortedList[i], sortedList[i + 1]));
            population.Add(Breed(sortedList[i+1], sortedList[i]));
            population.Add(Breed(sortedList[i], sortedList[i + 1]));
            population.Add(Breed(sortedList[i+1], sortedList[i]));
            population.Add(Breed(sortedList[i], sortedList[i + 1]));
            population.Add(Breed(sortedList[i+1], sortedList[i]));
            population.Add(Breed(sortedList[i], sortedList[i + 1]));
            population.Add(Breed(sortedList[i+1], sortedList[i]));
            population.Add(Breed(sortedList[i], sortedList[i + 1]));
            population.Add(Breed(sortedList[i+1], sortedList[i]));
        }

        for (int i = 0; i < sortedList.Count; i++) {
            Destroy(sortedList[i]);
        }
        generation++;
    }

    void Update() {
        elapsed += Time.deltaTime;
        if (elapsed >= trialTime) {
            BreedNewPopulation();
            elapsed = 0;
        }
    }
}