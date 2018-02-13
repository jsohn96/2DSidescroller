using UnityEngine;

public static class MathHelpers {
	public static int Mod (int n, int m) {
		return ((n % m) + m) % m;
	}

	public static float LinMap (float inputStart, float inputEnd, float outputStart, float outputEnd, float inputValue) {
		float domain = inputEnd - inputStart;
		float range = outputEnd - outputStart;
		return (inputValue - inputStart) / domain * range + outputStart;
	}

	public static float LinMapFrom01 (float outputStart, float outputEnd, float inputValue01) {
		float range = outputEnd - outputStart;
		return inputValue01 * range + outputStart;
	}

	public static float LinMapTo01 (float inputStart, float inputEnd, float inputValue) {
		float domain = inputEnd - inputStart;
		return (inputValue - inputStart) / domain;
	}
}