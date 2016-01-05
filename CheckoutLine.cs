using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace GroceryStoreSimulation
{
	public class CheckoutLine
	{
		public int id;
		public Queue<int> peopleInLine = new Queue<int> ();
		public int totalPeople             = 0;
		public int totalPeopleCheckedOut   = 0;
		public double totalSales           = 0;
		public bool isOpen                 = true;
		public int numberOfPeopleInLine    = 0;
		public int totalWaitTime           = 0;
		public List<Person> People         = new List<Person>();
		public int checkoutWidth           = 75;
		public int checkoutHeight          = 200;
		public int checkoutX               = 0;
		public Rectangle checkoutRectangle;

		public CheckoutLine (int tempId, List<Person> tempPeople)
		{
			id     = tempId;
			People = tempPeople;
		}

		public Rectangle drawCheckout(PaintEventArgs e, int xOffset, int checkoutY)
		{
			Graphics g = e.Graphics;

			checkoutX = xOffset;

			SolidBrush checkoutBrush = new SolidBrush (Color.Navy);
			checkoutRectangle = new Rectangle(checkoutX, checkoutY, checkoutWidth, checkoutHeight);
			g.FillRectangle (checkoutBrush, checkoutRectangle);

			Font checkoutFont = new Font (SystemFonts.DefaultFont.FontFamily, 16, FontStyle.Regular);
			g.DrawString(id.ToString(), checkoutFont, Brushes.White, xOffset + (checkoutWidth / 2) - 8, checkoutY + 89);

			SolidBrush laneStatusBrush = new SolidBrush (isOpen == true ? Color.Green : Color.Red);
			g.FillEllipse(laneStatusBrush, xOffset + (checkoutWidth / 2) - 8, checkoutY + 150, 20, 20);

			return checkoutRectangle;
		}

		public void addPersonToLine (int newPersonId)
		{
			peopleInLine.Enqueue (newPersonId);
			++totalPeople;
			++numberOfPeopleInLine;
			Console.WriteLine("--" + People[newPersonId].getName() + " has entered checkout line " + id);
		}

		public int completeCheckout (int currentTick)
		{
			int personId = peopleInLine.Peek ();
			Person tempPerson = People[personId];
			peopleInLine.Dequeue ();

			totalSales += tempPerson.amountSpent;
			++totalPeopleCheckedOut;
			totalWaitTime += currentTick - (tempPerson.arriveMinute + tempPerson.shoppingTime);
			--numberOfPeopleInLine;
			Console.WriteLine("--" + tempPerson.getName() + " finished checking out after spending $" + tempPerson.amountSpent);
			return personId;
		}

		public void close ()
		{
			isOpen = false;
		}

		public void open ()
		{
			isOpen = true;
		}

		public double getTotalSpent()
		{
			return totalSales;
		}

		public int getTotalWaitTime()
		{
			return totalWaitTime;
		}

		public int getTotalPeopleCheckedOut()
		{
			return totalPeopleCheckedOut;
		}

		public int getAvgWaitTimeForLine()
		{
			return totalPeopleCheckedOut > 0 ? (totalWaitTime / totalPeopleCheckedOut) : 0;
		}

		public int getWaitingInLineCount()
		{
			return numberOfPeopleInLine;
		}
	}
}