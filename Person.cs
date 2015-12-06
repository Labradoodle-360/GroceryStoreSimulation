using System;

namespace GroceryStoreSimulation
{
	public class Person
	{
		public int arriveMinute, shoppingTime, timeInLine;
		public double amountSpent;

		public string state;

		public Person (int tempArriveMinute, int tempShoppingTime, double tempAmountSpent)
		{
			arriveMinute = tempArriveMinute;
			shoppingTime = tempShoppingTime;
			amountSpent  = tempAmountSpent;
			timeInLine   = 0;
			state        = "outside";
		}

		public void changeState(string tempState)
		{
			state = tempState;
		}
	}
}

