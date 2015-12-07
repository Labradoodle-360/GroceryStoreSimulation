using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Collections.Generic;

namespace GroceryStoreSimulation
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Run (new Store ());
		}
	}

	class Store : Form
	{
		public bool simulationDataAdded;
		public const int totalCheckoutLines = 4;
		public string title = "Grocery Store Simulation";

		public List<Person> People = new List<Person>();

		public List<CheckoutLine> CheckoutLines = new List<CheckoutLine>();

		public Dictionary<int, int> PeopleInStore = new Dictionary<int, int>();

		public Dictionary<int, List<int>> arrivalTimeToPerson = new Dictionary<int, List<int>>();
		public Dictionary<int, List<int>> enterLineTimeToPerson = new Dictionary<int, List<int>>();

		public System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer ();
		public int currentTicks = 0;
		public int totalTicks   = 60;

		public Random randomGenerator = new Random();

		public Store()
		{
			simulationDataAdded = false;

			timer.Interval = 1 * 1000;
			timer.Tick += new EventHandler (TimerTick);

			MenuItem importSimulationData = new MenuItem ("Import Simulation Data");
			importSimulationData.Click += importSimulationDataClick;

			MenuItem startSimulator = new MenuItem ("Start Simulation");
			startSimulator.Click += StartSimulatorClick;

			MenuItem file = new MenuItem ("File");
			file.MenuItems.Add (importSimulationData);
			file.MenuItems.Add (startSimulator);

			MainMenu bar = new MainMenu ();
			bar.MenuItems.Add (file);

			Menu = bar;

			//-- Create our checkout lines.
			for (int iteration = 0; iteration < Store.totalCheckoutLines; ++iteration)
			{
				CheckoutLines.Add (new CheckoutLine ());
			}
		}

		void TimerTick(object sender, EventArgs e)
		{
			Console.WriteLine ("Tick: " + currentTicks);
			if (currentTicks == (totalTicks -1))
			{
				timer.Stop ();
				Console.WriteLine ("Simulation Complete!");
			}

			if (arrivalTimeToPerson.ContainsKey(currentTicks))
			{
				arrivalTimeToPerson [currentTicks].ForEach (x => Console.WriteLine ("--Person Number " + (x + 1) + " has entered the store."));
			}
			if (enterLineTimeToPerson.ContainsKey(currentTicks))
			{
				enterLineTimeToPerson [currentTicks].ForEach (x => Console.WriteLine ("--Person Number " + (x + 1) + " has entered a shopping line."));
				int tempRandomCheckoutLine = randomGenerator.Next(Store.totalCheckoutLines - 1);
				Console.WriteLine ("--Person was put in Checkout Line number: " + (tempRandomCheckoutLine + 1));

			}
			++currentTicks;
		}

		void StartSimulatorClick(object sender, EventArgs e)
		{
			if (simulationDataAdded == false)
			{
				MessageBox.Show ("You must import data before starting the simulation.", title, 
					MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return;
			}
			Console.WriteLine ("Simulation Started");
			timer.Start ();
		}

		void importSimulationDataClick(object sender, EventArgs e)
		{
			OpenFileDialog fileDialog = new OpenFileDialog ();
			if (fileDialog.ShowDialog() == DialogResult.OK)
			{
				StreamReader reader = File.OpenText (fileDialog.FileName);
				string line;
				int row = 0;
				while ((line = reader.ReadLine()) != null)
				{
					++row;
					string[] customerData = line.Split('|');
					if (customerData.Length != 3)
					{
						MessageBox.Show (
							"Row " + row + " is not formatted properly. Row has been skipped.\n\nCorrect Format: ArriveMinute(Int)|ShoppingTime(Int)|AmountSpent(Double).",
							title,
							MessageBoxButtons.OK,
							MessageBoxIcon.Asterisk
						);
						continue;
					}

					int arriveMinute, shoppingTime;
					double amountSpent;

					bool arriveMinuteConverted, shoppingTimeConverted, amountSpentConverted;

					arriveMinuteConverted = int.TryParse(customerData [0], out arriveMinute);
					shoppingTimeConverted = int.TryParse(customerData [1], out shoppingTime);
					amountSpentConverted  = double.TryParse(customerData [2], out amountSpent);

					if (arriveMinuteConverted == false || shoppingTimeConverted == false || amountSpentConverted == false)
					{
						MessageBox.Show (
							"Row " + row + " does not have the proper data types.\n\nCorrect Format: ArriveMinute(Int)|ShoppingTime(Int)|AmountSpent(Double).",
							title,
							MessageBoxButtons.OK,
							MessageBoxIcon.Asterisk
						);
						continue;
					}
					Person newPerson = new Person (arriveMinute, shoppingTime, amountSpent);
					People.Add (newPerson);
				}
				simulationDataAdded = true;
				Console.WriteLine ("Data Imported:");
				for (int iteration = 0; iteration < People.Count; ++iteration)
				{
					Person thisPerson = People [iteration];

					if (!arrivalTimeToPerson.ContainsKey(thisPerson.arriveMinute))
					{
						List<int> thisArrivalTimeList = new List<int>();
						arrivalTimeToPerson.Add (thisPerson.arriveMinute, thisArrivalTimeList);
					}
					arrivalTimeToPerson [thisPerson.arriveMinute].Add(iteration);

					int tempAdditiveTime = thisPerson.arriveMinute + thisPerson.shoppingTime;
					if (!enterLineTimeToPerson.ContainsKey(tempAdditiveTime))
					{
						List<int> thisEnterLineTimeList = new List<int> ();
						enterLineTimeToPerson.Add (tempAdditiveTime, thisEnterLineTimeList);
					}
					enterLineTimeToPerson [tempAdditiveTime].Add(iteration);

					Console.WriteLine ("Person:");
					Console.WriteLine ("Arrive Minute: " + thisPerson.arriveMinute);
					Console.WriteLine ("Shopping Time: " + thisPerson.shoppingTime);
					Console.WriteLine ("Amount Spent: " + thisPerson.amountSpent);
					Console.WriteLine ("Enter Line Time: " + tempAdditiveTime);
					Console.WriteLine ("------------------------");
				}

				// TEST: arrivalTimeToPerson [12].ForEach (x => Console.WriteLine (People[x].amountSpent));
				// TEST: enterLineTimeToPerson[21].ForEach(x => Console.WriteLine(x));

				/*foreach (var value in arrivalTimeToPerson.Values)
				{
					value.ForEach (x => Console.WriteLine ("Customer " + x + " enters the store at..."));
					//Console.WriteLine("Value of the Dictionary Item is: {0}", value);
				}*/
				/*for(int iteration = 0; iteration < arrivalTimeToPerson.Count; ++iteration)
				{
					Console.WriteLine (iteration.GetType ());
					if (arrivalTimeToPerson.ContainsKey (iteration))
						arrivalTimeToPerson [iteration].ForEach (x => Console.WriteLine ("Customer " + x + " enters the store at click: " + iteration));
					else
						continue;
					//Console.WriteLine (arrivalTimeToPerson [iteration]);
					//for (int sub_iteration = 0; sub_iteration < arrivalTimeToPerson[iteration].Count; ++sub_iteration)
					//{
					//	Console.WriteLine (arrivalTimeToPerson [iteration]);
					//}
				}*/
			}
		}
	}
}
