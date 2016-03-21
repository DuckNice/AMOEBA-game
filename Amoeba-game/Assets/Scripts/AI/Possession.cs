using System.Collections.Generic;

public class Possession
{
	protected string name;
	public string Name{ get{ return name; } }
	public string objectName;
	public float value;
	public List<object> parameters = new List<object>();
}


public class Money : Possession
{
	//Parameters: moneyValue
	public Money(float amount = 0.0f)
	{
		name = "money";
		value = amount;
		parameters.Add (value);
	}
}


public class Axe : Possession
{
	///<summary>Parameters: 
	///  Weight
	///  Material
	///  Sharpness
	///  Durability</summary>
	public Axe(float weight, string material, float sharpness, float durability)
	{
		name = "Axe";
		parameters.Add (weight);
		parameters.Add (material);
		parameters.Add (sharpness);
		parameters.Add (durability);

	}
}