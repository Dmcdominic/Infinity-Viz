using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ScalarInput { amplitude, spectralFlux }

public enum ScaleMode { exact, flow }

public class ScalePulse : MonoBehaviour {

	const int NUM_SAMPLES = 1024;

	public ScalarInput scalarInput;
	public ScaleMode scaleMode;

	//public float maxScale = 2.0f;
	//public float minScale = 0.5f;
	public float decayRate = 1f;
	public float gainRate = 10.0f;

	//public FreqBand freqBandForAmplitude;
	public FreqBandToggles freqBandToggles;

	public AudioSource audioSource;
	public SongController songController;


	float[] realTimeSpectrum;
	float[] realTimeOutputData;
	int sampleRate;
	int maxFrequency;
	float freqPerBin;


	// Init
	private void Awake() {
		realTimeSpectrum = new float[NUM_SAMPLES];
		realTimeOutputData = new float[NUM_SAMPLES];
		sampleRate = audioSource.clip.frequency;
		maxFrequency = sampleRate / 2;
		freqPerBin = maxFrequency / NUM_SAMPLES;
	}


	// Update is called once per frame
	void Update() {
		float prevScale = transform.localScale.x;
		float targetScale = 0.0f;
		switch (scalarInput) {
			case ScalarInput.amplitude:
				targetScale = getTargetAmplitude();
				break;
			case ScalarInput.spectralFlux:
				targetScale = getTargetSpectralFlux();
				break;
		}

		float newScale = 0.0f;
		switch (scaleMode) {
			case ScaleMode.exact:
				newScale = targetScale;
				break;
			case ScaleMode.flow:
				if (prevScale > targetScale) {
					if (Mathf.Abs(targetScale - prevScale) < Time.deltaTime * decayRate) {
						newScale = targetScale;
					} else {
						newScale = prevScale - Time.deltaTime * decayRate;
					}
				} else {
					if (Mathf.Abs(targetScale - prevScale) < Time.deltaTime * gainRate) {
						newScale = targetScale;
					} else {
						newScale = prevScale + Time.deltaTime * gainRate;
					}
				}
				break;
		}

		transform.localScale = new Vector3(newScale, newScale, newScale);
	}


	private float getTargetAmplitude() {
		// Get realTimeOutputData
		audioSource.GetOutputData(realTimeOutputData, 0);
		float outputAmplitudeSum = 0f;
		for (int i = 0; i < realTimeOutputData.Length; i++) {
			outputAmplitudeSum += realTimeOutputData[i] * realTimeOutputData[i];
		}
		outputAmplitudeSum /= realTimeOutputData.Length;
		outputAmplitudeSum = Mathf.Sqrt(outputAmplitudeSum);

		// Get realTimeSpectrum
		audioSource.GetSpectrumData(realTimeSpectrum, 0, FFTWindow.BlackmanHarris);
		float specAmplitudeSum = 0f;
		bool[] bands = freqBandToggles.bands();
		for (int f=0; f < bands.Length; f++) {
			if (!bands[f]) continue;
			FrequencyBand frequencyBand = FrequencyBand.Bands[f];
			int min_bin = getBinFromFreq(frequencyBand.min_freq);
			int max_bin = getBinFromFreq(frequencyBand.max_freq);

			for (int i = min_bin; i < max_bin; i++) {
				specAmplitudeSum += realTimeSpectrum[i];
			}
		}

		float adjustedAmplitude = specAmplitudeSum * outputAmplitudeSum * 10.0f;

		return adjustedAmplitude;
		// If we want the average amplitude - https://answers.unity.com/questions/157940/getoutputdata-and-getspectrumdata-they-represent-t.html
	}

	private float getTargetSpectralFlux() {
		int index = songController.getIndexFromTime(songController.audioSource.time);
		Debug.Log(songController.preProcessedSpectralFluxAnalyzer);
		Debug.Log(songController.preProcessedSpectralFluxAnalyzer.spectralFluxSamples[index]);
		return songController.preProcessedSpectralFluxAnalyzer.spectralFluxSamples[index].prunedSpectralFlux;
	}


	// Returns the index of the realTimeSpectrum bin for a specific frequency
	private int getBinFromFreq(float freq) {
		return Mathf.FloorToInt(freq / freqPerBin);
	}

	// Returns the avg frequency of a particular realTimeSpectrum bin
	private float getFreqFromBin(int bin) {
		return bin * freqPerBin;
	}
}
