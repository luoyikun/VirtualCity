using UnityEngine;
using System.Collections;

public class RandomGenerator
{
	const uint B = 1842502087;
	const uint C = 1357980759;
	const uint D = 273326509;
	
	static uint counter = 0;

	uint a, b, c, d;

	public RandomGenerator (uint val)
	{
		SetSeed(val);
	}

	public RandomGenerator()
	{
		SetSeed(counter++);
	}

	/// <summary>
	/// Random integer ranging from 0 to 0xFFFFFFFF.
	/// </summary>

	public uint GenerateUint()
	{
		uint t = (a ^ (a << 11));
		a = b;
		b = c;
		c = d;
		return d = (d ^ (d >> 19)) ^ (t ^ (t >> 8));
	}

	/// <summary>
	/// Return a random number up to but not including 'max'.
	/// </summary>

	public int Range (int max) { return (int)(GenerateUint() % max); }

	/// <summary>
	/// Return a random number between 'min' and up to but not including 'max'.
	/// </summary>

	public int Range (int min, int max) { return min + (int)(GenerateUint() % (max - min)); }
	
	/// <summary>
	/// Random single precision floating point value ranging from 0 to 1.
	/// </summary>
	
	public float GenerateFloat()
	{
		return 0.00000000023283064370807974f * GenerateUint();
	}

	/// <summary>
	/// Random single precision floating point value ranging from -1 to 1.
	/// </summary>

	public float GenerateRangeFloat()
	{
		uint val = GenerateUint();
		return 0.00000000046566128741615948f * (int)val;
	}

	/// <summary>
	/// Random double precision floating point value ranging from 0 to 1.
	/// </summary>

	public double GenerateDouble()
	{
		return 0.00000000023283064370807974 * GenerateUint();
	}

	/// <summary>
	/// Random double precision floating point value ranging from -1 to 1.
	/// </summary>

	public double GenerateRangeDouble()
	{
		uint val = GenerateUint();
		return 0.00000000046566128741615948 * (int)val;
	}

	/// <summary>
	/// Set the seed used for random number generation.
	/// </summary>

	public void SetSeed (uint val)
	{
		a = val;
		b = val ^ B;
		c = (val >> 5) ^ C;
		d = (val >> 7) ^ D;
	
		// Fully seed the generator
		for (uint i = 0; i < 4; ++i) a = GenerateUint();
	}
};