using System;
using System.Collections.Generic;

namespace Lists
{
    /// <summary>
    /// Methods for displaying information 
    /// in the user console window.
    /// </summary>
    public class Display
    {
        public const string INT_DISPLAY_FORMAT="###,##0";

        #region ctors
        public Display()
        {
        }
        #endregion ctors

        #region public static methods

        /// <summary>
        /// Pause the program and wait for when user is ready...
        /// </summary>
        public static string Pause(
            string message="Press any key to continue...",
            bool forceLf = true)
        {
            Console.WriteLine();

            if (message != null)
                Console.Write(message);
            else
                Console.Write("Press any key to continue...");

            // Return the value of user input
            if (!forceLf)
                return Console.ReadKey().KeyChar.ToString();
            else
            {
                string val = Console.ReadKey().KeyChar.ToString();
                Console.WriteLine();
                return val;
            }
        }

        /// <summary>
        /// Gets user input.
        /// </summary>
        /// <returns>The user input.</returns>
        /// <param name="message">Message.</param>
        /// <param name="forceLf">If set to <c>true</c> force lf.</param>
        public static string GetUserInput(
            string message=null,
            bool forceLf = true)
        {
            Console.WriteLine();

            if (message != null)
                Console.Write(message);

            // Return the value of user input
            if (!forceLf)
                return Console.ReadLine();
            else
            {
                string val = Console.ReadLine();
                Console.WriteLine();
                return val;
            }
        }

        /// <summary>
        /// Displays the main program's main menu.
        /// </summary>
        /// <returns>The contact menu.</returns>
        public static string DisplayMainMenu(int recordCount = 0)
        {            

            List<string> menu = new List<string>();

            menu.Add(@"(1) Save Contacts");
            menu.Add(@"(2) Modify Contact List");
            menu.Add(@"(3) View Contacts");
            menu.Add(@"(4) Find Contact by ID");
            menu.Add(@"(5) Query Contacts");
            menu.Add(@"(6) Sort Contacts");
            menu.Add("");
            menu.Add(@"(Q) Quit Program");

            return DisplayMenu("Contacts: Main Menu", menu,
                "("+recordCount.ToString() + " contacts in list)");
        }

        /// <summary>
        /// Displays the sort menu.
        /// </summary>
        /// <returns>The sort menu.</returns>
        /// <param name="recordCount">Record count.</param>
        public static string DisplaySortMenu(int recordCount = 0)
        {            

            List<string> menu = new List<string>();

            menu.Add(@"(1) Sort by Last Name");
            menu.Add(@"(2) Sort by First Name");
            menu.Add(@"(3) Sort by Email");
            menu.Add(@"(4) Sort by State");
            menu.Add(@"(5) Sort by State and City");
            menu.Add(@"(6) Sort by ID");
            menu.Add(@"(7) Sort by State, City, and Zip");
            menu.Add(@"(8) Sort by substring within First and Last Names");
            menu.Add("");
            menu.Add(@"(R) Return to Main");

            return DisplayMenu("Contacts: Sort Contact List", menu,
                "("+recordCount.ToString() + " contacts in list)");
        }

        /// <summary>
        /// Displays the program's query menu.
        /// </summary>
        /// <returns>The contact menu.</returns>
        public static string DisplayQueryMenu(int recordCount = 0)
        {            

            List<string> menu = new List<string>();

            menu.Add(@"(1) Find Contacts with Last Name...");
            menu.Add(@"(2) Find Contacts with ID Between...");
            menu.Add(@"(3) Find Contacts with Common Email Domain...");
            menu.Add(@"(4) Find Contacts within Zip...");
            menu.Add(@"(5) Find Contacts within State...");
            menu.Add(@"(6) Find Contacts with First and Last Name");
            menu.Add(@"(7) Find Contacts With First Name, Last Name, and City");
            menu.Add("");
            menu.Add(@"(R) Return to Main");

            return DisplayMenu("Contacts: Query Contacts", menu,
                "("+recordCount.ToString() + " contacts in list)");
        }

        /// <summary>
        /// Displays the main program's main menu.
        /// </summary>
        /// <returns>The contact menu.</returns>
        public static string DisplayContactMenu(int recordCount = 0)
        {            

            List<string> menu = new List<string>();

            menu.Add(@"(A) Add New Contacts");
            menu.Add(@"(D) Delete Contacts");
            menu.Add(@"(E) Edit Contacts");
            menu.Add("");
            menu.Add(@"(R) Return to Main");

            return DisplayMenu("Contacts: Modify Contact List", menu,
                "("+recordCount.ToString() + " contacts in list)");
        }

        /// <summary>
        /// Displays a menu, and waits for user input.
        /// </summary>
        /// <returns>The menu.</returns>
        /// <param name="title">Title.</param>
        /// <param name="menuOptions">Menu options.</param>
        /// <param name="instruction">Instruction.</param>
        public static string DisplayMenu(string title, 
                                         List<string> menuOptions,
                                         string subTitle="",
                                         string instruction = "Please make a selection: ", 
                                         bool forceLf = true,
                                         bool doNotClearConsole=false)
        {
            //clear the window before displaying menu?
            if (!doNotClearConsole)
                Console.Clear();

            title = title+"".Trim();
            subTitle = subTitle+"".Trim();

            int uRow = 1;
            int lineLength = 0;

            if (subTitle.Length >
                title.Length)
                lineLength = subTitle.Length;
            else
                lineLength = title.Length;

            //Center the title at the top of the console window
            Console.SetCursorPosition((Console.WindowWidth - title.Length) / 2, 0);
            Console.WriteLine(title);

            if (subTitle.Length > 0)
            {
                uRow++;
                //center sub title
                Console.SetCursorPosition((Console.WindowWidth - subTitle.Length) / 2, 1);
                Console.WriteLine(subTitle);
            }

            //Now, underline the menu title, and center
            Console.SetCursorPosition((Console.WindowWidth - lineLength) / 2, uRow);
            Console.WriteLine(new String('-', lineLength));

            Console.WriteLine();

            // Display each menu option in the console window
            foreach (string option in menuOptions)
            {
                Console.WriteLine(option);
            }

            //wait for user selection...
            return Pause(instruction,forceLf);
        }

        /// <summary>
        /// Displays all contacts on the screen.
        /// </summary>
        /// <param name="contacts">Contacts.</param>
        public static void DisplayAllContacts(
            List<Contact> contacts, 
            int recordsPerPage = 10,
            int startingIndex=0, 
            bool doNotClear=false)
        {
            if (!doNotClear)
                Console.Clear();

            int index = startingIndex;
            bool continueList = true;
            int i = index;

            // While we have not reached the end 
            // of the contact list, and also we 
            // wish to continue displaying the list...
            while (index < contacts.Count
                && continueList)
            {   

                // Display the amount of records 
                // as indicated per page
                for (;i<(index+recordsPerPage);i++)
                {
                    if (i < contacts.Count)
                        DisplayContact(contacts[i]);
                    else
                    {
                        // Break if we have reached 
                        // the end of the list.
                        continueList = false;
                        break;
                    }
                }

                // Pause to allow user to view records 
                // before listing more records.
                if (Display.Pause("Press 'R' to return, or any key to view next " 
                    + recordsPerPage.ToString() +" records...")
                    .ToLower().Trim() == "r")
                    // Quit displaying contacts,
                    // if user typed 'q'.
                    continueList = false;

                //Set index to start where we left off
                index = i;
            }
        }

        /// <summary>
        /// Displays a contact on the screen.
        /// </summary>
        /// <param name="contact">Contact.</param>
        public static void DisplayContact (Contact contact)
        {
            Console.WriteLine("Contact: {0}, {1}, {2}"
                , contact.LastName,contact.FirstName
                , contact.ID.ToString("000000"));
            Console.WriteLine ("______________________________________________________");
            Console.WriteLine("Address: {0}", contact.Address);
            Console.WriteLine("City: {0}", contact.City);
            Console.WriteLine ("State: {0}", contact.State);
            Console.WriteLine("Zip: {0}", contact.Zip);
            Console.WriteLine ("County: {0}", contact.County);
            Console.WriteLine("Company: {0}", contact.CompanyName);
            Console.WriteLine("Phone1: {0}", contact.Phone1);
            Console.WriteLine("Phone2: {0}", contact.Phone2 );
            Console.WriteLine("Email: {0}", contact.Email);
            Console.WriteLine("Web: {0}", contact.Web);
            Console.WriteLine ("______________________________________________________");
            Console.WriteLine ("");
            Console.WriteLine ("");
        }

        #endregion public static methods
    }
}

