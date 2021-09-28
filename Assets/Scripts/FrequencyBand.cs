using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Source - https://www.teachmeaudio.com/mixing/techniques/audio-spectrum
public enum FreqBand { SubBass, Bass, LowMidrange, Midrange, UpperMidrange, Presence, Brilliance, COUNT }

public class FrequencyBand {
	public FreqBand freqBand;
	public string name; // The name of the frequency band
	public int min_freq; // The minimum frequency in this band
	public int max_freq; // The maximum frequency in this band

	public FrequencyBand(FreqBand _freqBand, string _name, int _min_freq, int _max_freq) {
		freqBand = _freqBand;
		name = _name;
		min_freq = _min_freq;
		max_freq = _max_freq;
	}

	public static FrequencyBand[] Bands = {
		new FrequencyBand(FreqBand.SubBass, "Sub-bass", 20, 60),
		new FrequencyBand(FreqBand.Bass, "Bass", 60, 250),
		new FrequencyBand(FreqBand.LowMidrange, "Low midrange", 250, 500),
		new FrequencyBand(FreqBand.Midrange, "Midrange", 500, 2000),
		new FrequencyBand(FreqBand.UpperMidrange, "Upper midrange", 2000, 4000),
		new FrequencyBand(FreqBand.Presence, "Presence", 4000, 6000),
		new FrequencyBand(FreqBand.Brilliance, "Brilliance", 6000, 20000)
	};
}

[System.Serializable]
public struct FreqBandToggles {
	public bool SubBass;
	public bool Bass;
	public bool LowMidrange;
	public bool Midrange;
	public bool UpperMidrange;
	public bool Presence;
	public bool Brilliance;

	public bool[] bands() {
		return new bool[] { SubBass, Bass, LowMidrange, Midrange, UpperMidrange, Presence, Brilliance };
	}
}
