using UnityEngine;
using System.Collections;
using System;

public class AudioSpectrumDisplay : MonoBehaviour
{
    [SerializeField] AudioSource trackAudio;
    [SerializeField] GameObject[] eqCubes;
    float[] spectrum = new float[256];
    float[,] bufferSpectrum = new float[16,8]; //our buffer [size,resolution]
    private float[] splitSpectrum = new float[8];
    private int bufferCounter,bufferSize; 

    void Start()
    {
        bufferCounter = 0;
    }

    void Update()
    {
        bufferSize = 16;
        //to set current buffer index
        if (bufferCounter < (bufferSize-1)) bufferCounter++;
        else bufferCounter = 0;

        //get the spectrum data
        trackAudio.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
        
        //clear our eq
        for (int j = 0; j < splitSpectrum.Length; j++)
            splitSpectrum[j] = 0;

        int i = 1;
        int currentSplit = 0;
        while (i < spectrum.Length - 1)
        {
            currentSplit = CalculateSplitFromIndex(i); //current split we are collecting for
            splitSpectrum[currentSplit] += spectrum[i]; //add the spectrum data to the current split
            i++;
        }
        //add current result to appropriate position in buffer [Current Buffer , Buffer Index]
        for (int bufferIndex=0; bufferIndex < splitSpectrum.Length; bufferIndex++)bufferSpectrum[bufferCounter,bufferIndex] = splitSpectrum[bufferIndex];
        
        //scale the cubes
        for (int k = 0; k < splitSpectrum.Length; k++)
        {
            float thisBufferValue = (GetBufferAverage(k) * 5000f); //get the average
            thisBufferValue = Mathf.Clamp(thisBufferValue,0f,150f); //clamp it
            eqCubes[k].transform.localScale=(new Vector3 (20, thisBufferValue, 1)); //change the cube so we display it 
        }
    }

    /// <summary>
    /// iterates through  the entire buffer at a given index and returns average
    /// </summary>
    /// <param name="i">Index of buffer to get average for</param>
    /// <returns>the average as float</returns>
    private float GetBufferAverage(int i)
    {
        float thisBufferTotal=0;
        for (int j = 0; j  < bufferSize; j++)
        {
            thisBufferTotal = bufferSpectrum[j,i];
        }
       return thisBufferTotal/bufferSize;
    }

   /// <summary>
   /// This is where we can break up the splits according to frequency range
   /// we use maths to do this
   /// </summary>
   /// <param name="index"></param>
   /// <returns>index of our spectrum</returns>
    private int CalculateSplitFromIndex(int index)
    {
        float splitIndex = 1;
        splitIndex = (Mathf.Log(index+1, 2)); //Base 2 log so 256=8, 128=7 etc
        splitIndex = (Mathf.Ceil(splitIndex)); //make it whole upward
        return ((int)splitIndex-1); //we change to zero based int before we return
    }
}
