using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lists
{
    /// <summary>
    /// Main class.
    /// </summary>
    class MainClass
    {
        static List<Contact> contacts = new List<Contact>();

        #region Main
        public static void Main(string[] args)
        {

            //Start out by loading up contacts into memory...
            Console.Write("Loading contacts.  Please wait...");
            if (File.Exists("MyContacts.dat"))
            {                
                // load existing contact data file...
                contacts = Contact.LoadContacts("MyContacts.dat");
                Display.Pause("There are "
                    + contacts.Count.ToString(Display.INT_DISPLAY_FORMAT)
                    + " contacts in your list.");
            }
            else
            {
                //No .dat file exists yet.
                //Build it from csv
                contacts = BuildInitialContacList("us-50000.csv");

                Display.Pause("Built contact list from csv."
                    + "\nBe sure to SAVE YOUR NEW LIST before ending the program!");

                Display.Pause("There are "
                    + contacts.Count.ToString(Display.INT_DISPLAY_FORMAT)
                    + " contacts in your list.");
            }

            string selection = "";

            // Display menu and keep program "alive" 
            // until user presses Q for quit.
            while (selection != "q")
            {

                try
                {
                    selection = Display.DisplayMainMenu
                        (contacts.Count).Trim().ToLower();
                    
                    switch (selection)
                    {

                        case "1": // save current contact list.
                            Contact.SaveContacts(contacts, "MyContacts.dat");

                            Display.Pause("Successfully saved "
                                + contacts.Count.ToString(Display.INT_DISPLAY_FORMAT)
                                + " contacts."); 

                            break;

                        case "2": // modify contact list
                            SwitchToUserContactMenu();
                            break;

                        case "3": // view contacts
                            Display.DisplayAllContacts(contacts);
                            break;

                        case "4":// view specific contact
                            {
                                string id = Display.GetUserInput("Enter contact ID: ");
                                int val=0;
                                if (!Validation.IsNumeric(id,ref val))
                                    Display.Pause("Invalid ID; must be numeric.");
                                else
                                {
                                    Contact contact = ContactQuery.FindByID(contacts,val);
                                    if (contact==null)
                                        Display.Pause("Contact with that ID not found.");
                                    else
                                    {
                                        Console.Clear();
                                        Display.DisplayContact(contact);
                                        Display.Pause();
                                    }
                                }

                            }
                            break;
                     
                        case "5":// query contacts
                            SwitchToUserQueryMenu();
                            break;

                        case "6": // sort contacts
                            SwitchToSortMenu();
                            break;

                    }
                }
                catch (Exception e)
                {
                    Display.Pause("Error: " + e.Message);
                }
            } 

            // save contact list before exiting...
            if (contacts.Count > 0)
            {
                Contact.SaveContacts(contacts, "MyContacts.dat");

                Display.Pause("Successfully saved "
                    + contacts.Count.ToString(Display.INT_DISPLAY_FORMAT)
                    + " contacts.\nPress any key to exit program."); 
            }
        }

        #endregion Main    

        #region user menu methods

        /// <summary>
        /// Switches to user contact menu.
        /// </summary>
        public static void SwitchToUserContactMenu()
        {
            string selection = "";

            try
            {
                // Stay in this menu until user wants 
                // to go back to main menu
                while (selection != "r")
                {
                    selection = Display.DisplayContactMenu(contacts.Count).Trim().ToLower();

                    switch (selection)
                    {
                        case "a": // load contacts

                            Contact.UserEnterContacts(contacts);
                            break;


            // edited by Garrett Rathke on 10/03/17 per Assignment 3 instructions
                        case "d": // delete existing contact from the list
                            Console.Clear();
                            Contact contact = null;
                            Display.Pause("Delete Contact...");
                            string val = Display.GetUserInput("Enter ID to delete: ").Trim();
                            string confirm = "";

                            // check if id match is found
                            if (val.Length > 0)
                            {
                                int id = 0;

                                // search for and display results
                                if (Validation.IsNumeric(val, ref id))
                                {
                                    contact = ContactQuery.FindByID(contacts, id);

                                    if (contact != null) // we have found the contact
                                    {
                                        Display.DisplayContact(contact);
                                    }
                                    else
                                        Display.Pause("Contact with that ID was not found.");

                                }
                                else
                                    Display.Pause("Please enter a valid ID.");
                            }
                            else
                                return;

                            // Now, allow user to delete the contact
                            if (contact != null)
                            {
                                confirm = Display.GetUserInput("Confirm Delete: Y/N\n");
                                if(confirm.Contains("y") || confirm.Contains("Y"))
                                {
                                    contacts.Remove(contact);
                                    Display.Pause("Contact Was Successfully Deleted");
                                }
                                else
                                {
                                    Display.Pause("Deletion Canceled");
                                }
                            }
                            break;

                        case "e": // edit an existing contact in the list
                            Contact.UserEditContacts(contacts);
                            break;


                    }
                }
            }
            catch (Exception e)
            {
                Display.Pause("Error: " + e.Message);
            }

        }

        /// <summary>
        /// Switches to user query menu.
        /// </summary>
        public static void SwitchToUserQueryMenu()
        {
            string selection = "";

            try
            {
                // Stay in this menu until user wants 
                // to go back to main menu
                while (selection != "r")
                {
                    // display the query menu options to the user
                    selection = Display.DisplayQueryMenu(contacts.Count).Trim().ToLower();

                    switch (selection)
                    {
                        case "1": // find by last name

                            //let user type name to search for
                            string val = Display.GetUserInput("Enter last name to search: ").Trim();

                            if (val.Length > 0)
                            {
                                // search for and display results
                                List<Contact> results = ContactQuery.SearchLastName(contacts, val);
                                DisplayQueryResults(results);
                            }
                                                            
                            break;

                        case "2": //Find contacts with ID between (starting and ending number)
                            //let user type name to search for
                            string idVal1 = " ";
                            string idVal2 = " ";
                            int id1 = 0;
                            int id2 = 0;
                            int tries = 0;

                            //get starting id
                            while (tries <= 5 && (!Validation.IsNumeric(idVal1, ref id1) || idVal1 == ""))
                            {
                                tries++;
                                idVal1 = Display.GetUserInput("Enter STARTING ID: ").Trim();
                            }
                                                                                     
                            if (tries <= 5)
                            {
                                tries = 0;
                                // get ending id (only if starting id is good)
                                while (tries <= 5 && (!Validation.IsNumeric(idVal2, ref id2) || idVal2 == ""))
                                {
                                    idVal2 = Display.GetUserInput("Enter ENDING ID: ").Trim();
                                    tries++;
                                }
                            }

                            if (id2 <= id1)
                            {
                                Display.Pause("The first ID value must be less than the second ID value.");
                                break;
                            }
                            else
                            {
                                // search for and display results
                                List<Contact> results = ContactQuery.SearchIdRange(contacts, id1, id2);
                                DisplayQueryResults(results);
                            }
                            break;


         // edited by Garrett Rathke on 10/03/17 per assignment 4 instructions
                        case "3": //Find contacts with common email domain (like: .aol, .gmail, .whatever)
                                                                                    // trim removes leading &/or trailing whitespace
                            string emailDomain = Display.GetUserInput("Enter email domain to search: ").Trim();
                            if (!Validation.IsValidUrl(emailDomain))
                            {
                                Console.WriteLine("Invalid URL");
                            }
                            else
                            {
                                List<Contact> results = ContactQuery.FindByEmailDomain(contacts, emailDomain);
                                DisplayQueryResults(results);
                            }
                            break;


         // edited by Garrett Rathke on 10/03/17 per assignment 4 instructions
                        case "4": // Find contacts within zip
                            string zip = Display.GetUserInput("Enter zip code to search: ").Trim();
                            if(zip.Length > 5 || zip.Length < 0)
                            {
                                Console.WriteLine("Invalid Zip Code");
                            }
                            else
                            {
                                List<Contact> results = ContactQuery.FindByZip(contacts, zip);
                                DisplayQueryResults(results);
                            }
                            break;

          // edited by Garrett Rathke on 10/03/17 per assignment 4 instructions
                        case "5": // Find contacts within state
                            string state = Display.GetUserInput("Enter state to search: ").Trim();
                            if (state.Length > 2 || state.Length < 0)
                            {
                                Console.WriteLine("Invalid State Format");
                            }
                            else
                            {
                                List<Contact> results = ContactQuery.FindByState(contacts, state);
                                DisplayQueryResults(results);
                            }
                            break;

           // edited by Garrett Rathke on 10/03/17 per assignment 4 instructions
                        case "6": //Find contacts with first and last name
                            string firstName = Display.GetUserInput("Enter first name to search: ").Trim();
                            string lastName = Display.GetUserInput("Enter last name to search: ").Trim();
                            if (!(firstName.Length > 0 || lastName.Length > 0))
                            {
                                Console.WriteLine("Invalid Name Format");
                            }
                            else
                            {
                                List<Contact> results = ContactQuery.FindByFirstAndLastName(contacts, firstName, lastName);
                                DisplayQueryResults(results);
                            }
                            break;

           // edited by Garrett Rathke on 10/03/17 per assignment 4 instructions
                        case "7": //Find contacts with (first name, last name, and city)
                            Console.WriteLine("Search for substring within first & last names...");
                            firstName = Display.GetUserInput("Enter first name to search: ").Trim();
                            lastName = Display.GetUserInput("Enter last name to search: ").Trim();
                            string city = Display.GetUserInput("Enter city to search: ").Trim();
                            if (!(firstName.Length > 0 || lastName.Length > 0 || city.Length > 0))
                            {
                                Console.WriteLine("Invalid Name Format");
                            }
                            else
                            {
                                List<Contact> results = ContactQuery.FindByFirstLastNameAndCity(contacts, firstName, lastName, city);
                                DisplayQueryResults(results);
                            }
                            break;

                        // edited by Garrett Rathke on 10/03/17 per assignment 4 instructions
                        case "8": //Find contacts with substrings in (first name, last name)
                            firstName = Display.GetUserInput("Enter first name to search: ").Trim();
                            lastName = Display.GetUserInput("Enter last name to search: ").Trim();
                            if (!(firstName.Length > 0 || lastName.Length > 0))
                            {
                                Console.WriteLine("Invalid Name Format");
                            }
                            else
                            {
                                List<Contact> results = ContactQuery.FindBySubStringInFirstAndLastName(contacts, firstName, lastName);
                                DisplayQueryResults(results);
                            }
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Display.Pause("Error: " + e.Message);
            }

            

        }

        /// <summary>
        /// Switches to the sort menu.
        /// </summary>
        public static void SwitchToSortMenu()
        {

            string selection = "";

            try
            {
                // Stay in this menu until user wants 
                // to go back to main menu
                while (selection != "r")
                {
                    selection = Display.DisplaySortMenu(contacts.Count).Trim().ToLower();

                    switch (selection)
                    {
                // edited by Garrett Rathke on 10/03/17 per Assignment 4 instructions
                        case "1": // sort by last name
                            contacts = contacts.OrderBy(contact => contact.LastName).ToList();
                            Display.Pause("Contacts are now sorted by last name. \nGo back to main menu to view sorted list.");
                            break;

                // edited by Garrett Rathke on 10/03/17 per Assignment 4 instructions
                        case "2": // sort by first name
                            contacts = contacts.OrderBy(contact => contact.FirstName).ToList();
                            Display.Pause("Contacts are now sorted by first name. \nGo back to main menu to view sorted list."); break;
                        
                // edited by Garrett Rathke on 10/03/17 per Assignment 4 instructions
                        case "3": // sort by state and email
                            contacts = contacts.OrderBy(contact => contact.State).
                                                 ThenBy(contact => contact.Email).ToList();
                            Display.Pause("Contacts are now sorted by state and email. \nGo back to main menu to view sorted list.");
                            break;

                // edited by Garrett Rathke on 10/03/17 per Assignment 4 instructions
                        case "4": // sort by state
                            contacts = contacts.OrderBy(contact => contact.State).ToList();
                            Display.Pause("Contacts are now sorted by state. \nGo back to main menu to view sorted list.");
                            break;

                        case "5": // sort by state and city
                            contacts = contacts.OrderBy(contact => contact.State).
                                                 ThenBy(contact => contact.City).ToList();
                            Display.Pause("Contacts are now sorted by state and city.\nGo back to main menu to view sorted list.");
                            break;

                // edited by Garrett Rathke on 10/03/17 per Assignment 4 instructions
                        case "6": // sort by ID
                            contacts = contacts.OrderBy(contact => contact.ID).ToList();
                            Display.Pause("Contacts are now sorted by ID. \nGo back to main menu to view sorted list.");
                            break;

                        // edited by Garrett Rathke on 10/03/17 per Assignment 4 instructions
                        case "7": // sort by state, city, and zip
                            contacts = contacts.OrderBy(contact => contact.State).
                                                 ThenBy(contact => contact.City).
                                                 ThenBy(contact => contact.Zip).ToList();
                            Display.Pause("Contacts are now sorted by state, city, and zip. \nGo back to main menu to view sorted list.");
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Display.Pause("Error: " + e.Message);
            }

        }

        #endregion user menu methods

        #region public static methods


        /// <summary>
        /// Displays query results so user can view them
        /// </summary>
        /// <param name="results">Results.</param>
        public static void DisplayQueryResults(List<Contact> results)
        {

            Console.Clear();
            Display.Pause("***** SEARCH RESULTS *****");            
            if (results.Count == 0)
                Display.Pause("No contacts found.");
            else
            {
                Display.Pause("Found "
                    + results.Count.ToString(Display.INT_DISPLAY_FORMAT)
                    + " contacts during our search...");
                Display.DisplayAllContacts(results,1,0,true);
            }
        }


        /// <summary>
        /// Splits a line with csv values 
        /// with ',' between double-quotes.
        /// </summary>
        /// <returns>The splitter.</returns>
        /// <param name="line">Line.</param>
        public static IEnumerable<string> LineSplitter(string line)
        { 
            int fieldStart = 0; 
            for (int i = 0; i < line.Length; i++)
            { 
                if (line[i] == ',')
                {
                    yield return line.Substring(fieldStart, i - fieldStart);
                    fieldStart = i + 1;
                }
                if (line[i] == '"')
                    for (i++; line[i] != '"'; i++)
                    {
                    }
            }

            yield return line.Substring(fieldStart, line.Length - fieldStart);
        }


        /// <summary>
        /// Builds the initial contact list from csv file of 
        /// 50,000 contact records.
        /// </summary>
        /// <returns>The initial contac list.</returns>
        /// <param name="usingFile">Using file.</param>
        public static List<Contact> BuildInitialContacList 
        (string usingFile, int maxRecords = 100000)
        {
            List<Contact> contacts = new List<Contact>();
            int id=1;

            try
            {                
                using (TextReader reader = File.OpenText(usingFile))
                {
                    //skip first line (column names)
                    string line = reader.ReadLine();

                    line = reader.ReadLine();

                    // Throw exception if file is empty!
                    if (line==null)
                        throw new FileLoadException ("File is empty!");

                    // while:
                    // (1) we have not reached the end of the file, and
                    // (2) we have not loaded the max number of contacts into memory
                    // continue to load contacts...
                    while (line!=null 
                           && line.Trim().Length>0 
                           && id<=maxRecords)
                    {                        
                        //split values into fields
                        var fields = LineSplitter(line).ToList();

                        //now, create contact and assign field values.
                        // (we strip out double-quotes from the value)
                        Contact newContact= new Contact();
                        newContact.FirstName = fields[0].Replace("\"", "");
                        newContact.LastName = fields[1].Replace("\"", "");
                        newContact.CompanyName = fields[2].Replace("\"", "");
                        newContact.Address = fields[3].Replace("\"", "");
                        newContact.City = fields[4].Replace("\"", "");
                        newContact.County = fields[5].Replace("\"", "");
                        newContact.State = fields[6].Replace("\"", "");
                        newContact.Zip = fields[7].Replace("\"", "");
                        newContact.Phone1 = fields[8].Replace("\"", "");
                        newContact.Phone2 = fields[9].Replace("\"", "");
                        newContact.Email = fields[10].Replace("\"", "");
                        newContact.Web = fields[11].Replace("\"", "");
                        newContact.ID=id++;
                        contacts.Add(newContact);

                        //read next line of text from the file
                        line = reader.ReadLine();

                    }
                    reader.Close();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(""); 
                Console.WriteLine(ex.Message); 
                return contacts;
            }

            Console.WriteLine(""); 
            Console.WriteLine("Successfully built {0} contacts."
                , contacts.Count.ToString(Display.INT_DISPLAY_FORMAT)); 
            return contacts;

        }
        #endregion public static methods
    }
}
