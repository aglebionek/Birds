using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA {

    List<byte> genes = new List<byte>();
    int dnaLength = 0;
    byte maxValues = 0;

    public DNA(int length, byte max) {
        dnaLength = length;
        maxValues = max;
        SetRandom();
    }

    public void SetRandom() {
        genes.Clear();
        for (int i = 0; i < dnaLength; i++) {
            genes.Add((byte)Random.Range(-maxValues, maxValues));
        }
    }

    public void SetInt(int pos, byte value) {
        genes[pos] = value;
    }

    public void Combine(DNA parent1, DNA parent2) {
        for (int i = 0; i < dnaLength; i++) {
            genes[i] = Random.Range(0, 2) < 1 ? parent1.genes[i] : parent2.genes[i];
        }
    }

    public void Mutate() {
        genes[Random.Range(0, dnaLength)] = (byte)Random.Range(-maxValues, maxValues);
    }

    public byte GetGene(int pos) {
        return genes[pos];
    }

    public List<byte> GetGenes() {
        return genes;
    }

    public double Phenotype() {
        double phenotype = 0f;
        double phenotypeBase = 1.1f;
        double currentBase = 1;
        for (int i = 0; i < genes.Count; i++) {
            byte gene = genes[i];
            for (int j = 0; j < 8; j++) {
                phenotype += (gene & 1) * currentBase;
                gene = (byte)(gene >> 1);
                currentBase *= phenotypeBase;
            }
        }
        return phenotype;
    }
}