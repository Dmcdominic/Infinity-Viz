using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePulse : MonoBehaviour {

	public float maxScale = 2.0f;
	public float minScale = 0.5f;

	public SongController songController;


	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		int index = songController.getIndexFromTime(songController.audioSource.time);
		Debug.Log(songController.preProcessedSpectralFluxAnalyzer);
		Debug.Log(songController.preProcessedSpectralFluxAnalyzer.spectralFluxSamples[index]);
		float PSF = songController.preProcessedSpectralFluxAnalyzer.spectralFluxSamples[index].prunedSpectralFlux;

		float currentScale = transform.localScale.x;
		if (currentScale > PSF) {
			currentScale -= Time.deltaTime;
		} else {
			currentScale = PSF;
		}
		transform.localScale = new Vector3(currentScale, currentScale, currentScale);
	}
}
