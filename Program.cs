using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace GroceryStoreSimulation
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Run (new MyForm ());
		}
	}

	class Store : Form
	{
		public TextBox t;
		public MyForm()
		{

			t          = new TextBox ();
			t.Width    = 300;
			t.Location = new Point (10, 10);
			t.Height   = 40;
			t.Enabled  = false;
			t.ReadOnly = true;

			Controls.Add (t);

			MenuItem open = new MenuItem ("Start Simulation");
			open.Click += Open_Click;

			MainMenu bar = new MainMenu ();
			bar.MenuItems.Add (file);

			Menu = bar;
		}
	}
}
