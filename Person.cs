using System;

namespace GroceryStoreSimulation
{
	public class Person
	{
		public int arriveMinute, shoppingTime, timeInLine;
		public double amountSpent;

		public string state;

		public Person (int tempArriveMinute, int tempShoppingTime, int tempAmountSpent)
		{
			arriveMinute = tempArriveMinute;
			shoppingTime = tempShoppingTime;
			amountSpent  = tempAmountSpent;
			timeInLine   = 0;
			state        = "outside";
		}
	}
}

