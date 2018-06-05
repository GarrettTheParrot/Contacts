using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Lists
{

    /// <Deep Copy> 
    /// Public Method for Deep Copies
    /// </Deep Copy>>
    // edited by Garrett Rathke on 10/04/17 per Assignment 4 instructions
    public static class ExtendedObject
    {
        public static T DeepCopy<T>(this T objectToCopy)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(memStream, objectToCopy);

            memStream.Position = 0;
            T newObject = (T)formatter.Deserialize(memStream);

            memStream.Close();
            memStream.Dispose();

            return newObject;
        }
    }


    /// <summary>
    /// Class to hold individual contact information.
    /// </summary>
    [Serializable]
    public class Contact
    {
        #region ctors
        public Contact()
        {
            
        }
        #endregion ctors

        #region public properties
        //contact properties
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone1 {get;set;}
        public string Phone2 { get; set; }
        public string Email {get;set;}
        public string Web { get; set; }
        public int ID { get; set; }
        #endregion public properties

     
        #region public static methods

        /// <summary>
        /// Saves list of contacts contacts.
        /// </summary>
        /// <param name="contacts">Contacts.</param>
        public static void SaveContacts (List<Contact> contacts, string fileName = "contacts.dat")
        {
            try
            {
                using (Stream stream = File.Open(fileName, FileMode.Create))
                {
                    BinaryFormatter bin = new BinaryFormatter();

                    // Write the entire list of contacts to the file.
                    bin.Serialize(stream, contacts);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("SaveContacts: " + ex.Message); 
                throw;
            }
        }

        /// <summary>
        /// Loads list of contacts.
        /// </summary>
        /// <returns>The contacts.</returns>
        /// <param name="filename">Filename.</param>
        public static List<Contact> LoadContacts(string filename = "contacts.dat")
        {
            List<Contact> contacts = new List<Contact>();
            try
            {
                using (Stream stream = File.Open(filename, FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();

                    // Load file and create a new instance 
                    // of contact list from a previously serialized list.
                    contacts = (List<Contact>)bin.Deserialize(stream);
 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("LoadContacts: " + ex.Message); 
                throw;
            }
            return contacts;
        }

        /// <summary>
        /// Users the edit contacts.
        /// </summary>
        /// <param name="contacts">Contacts.</param>
        public static void UserEditContacts (List<Contact> contacts)
        {
            // Continue to allow user to edit contacts 
            // until user enters nothing for an ID

            while (true)
            {               
                Console.Clear();
                Contact contact = null;

                //let user type ID of contact to edit
                Console.WriteLine("Edit contacts.  Leave ID blank to stop editing...");
                string val = Display.GetUserInput("Enter ID to edit: ").Trim();
                string doAgain = "";

                if (val.Length > 0)
                {
                    int id = 0;

                    // search for and display results
                    if (Validation.IsNumeric(val, ref id))
                    {
                        contact = ContactQuery.FindByID(contacts,id);

                        if (contact != null) // we have found the contact
                            Display.DisplayContact(contact);
                        else
                            Display.Pause("Contact with that ID was not found.");   
                    
                    }
                    else
                        Display.Pause("Please enter a valid ID."); 
                }
                else
                    return;

                // Now, allow user to edit the contact
                if (contact != null)
                {
                    /// <COPY>
                    /// create deep copy of contact in case of bad input
                    /// </COPY>     
                    Contact tempCopy = contact.DeepCopy();

                    Console.WriteLine("Edit fields.  Leave a blank in any field that you do not wish to change.");
                    Console.WriteLine("");

                    // Ask user for new values for each field.
                    // Assign the new value ONLY if one was provided.
                    // Otherwise, leave the old value in its place.
                    string input = Display.GetUserInput("First Name: ",false);
                    if (input.Trim().Length > 0)
                        tempCopy.FirstName = input;

                    input = Display.GetUserInput("Last Name: ",false);
                    if (input.Trim().Length > 0)
                        tempCopy.LastName = input;

                    input = Display.GetUserInput("Street Address: ",false);
                    if (input.Trim().Length > 0)
                        tempCopy.Address = input;                    

                    input = Display.GetUserInput("City: ",false);
                    if (input.Trim().Length > 0)
                        tempCopy.City = input; 

                    input = Display.GetUserInput("State: ",false);
                    if (input.Trim().Length > 0)
                        tempCopy.State = input;                     

                    input = Display.GetUserInput("Zip: ",false);
                    if (input.Trim().Length > 0)
                        tempCopy.Zip = input; 

                    input = Display.GetUserInput("County: ",false);
                    if (input.Trim().Length > 0)
                        tempCopy.County = input; 
                    
                    input = Display.GetUserInput("Company: ",false);
                    if (input.Trim().Length > 0)
                        tempCopy.CompanyName  = input;

                    do {
                        // edited by Garrett Rathke on 10/03/17 per Assignment 4 instructions
                        input = Display.GetUserInput("Phone1: ", false);
                        if (input.Trim().Length > 0)
                        {
                            if (Validation.IsValidUSPhoneNumber(input))
                            {
                                tempCopy.phone1IsValid = true;
                                tempCopy.Phone1 = input;
                                doAgain = "N"; // just in case
                            }
                            else
                            {
                                tempCopy.phone1IsValid = false;
                                tempCopy.dataIsValid = false;
                                Console.WriteLine("Invalid Phone Number Format...Contact Cannot be Entered");
                                doAgain = Display.GetUserInput("Do you want to enter phone number 1 again? Y/N \n");
                            }
                        }
                        else
                        {
                            tempCopy.phone1IsValid = true; // user doesn't want contact to have a phone #
                            doAgain = "N"; // just in case
                        }
                    } while (doAgain.Contains("Y") || doAgain.Contains("y")); // check phone 1 valid format...if not do loop
                    // if user entered incorrect data but does not want to retry
                    // then break out of data entry and return to menu
                    if ((tempCopy.phone1IsValid == false) && (!(doAgain.Contains("y") || doAgain.Contains("Y"))))
                    {
                        contacts.RemoveAt(tempCopy.ID); // remove object that contained user's bad data
                        break;
                    }

                    do
                    {
                    // edited by Garrett Rathke on 10/03/17 per Assignment 4 instructions
                        input = Display.GetUserInput("Phone2: ", false);
                        if (input.Trim().Length > 0)
                        {
                            if (Validation.IsValidUSPhoneNumber(input))
                            {
                                tempCopy.phone2IsValid = true;
                                tempCopy.Phone2 = input;
                                doAgain = "N"; // just in case
                            }
                            else
                            {
                                tempCopy.phone2IsValid = false;
                                tempCopy.dataIsValid = false;
                                Console.WriteLine("Invalid Phone Number Format...Contact Cannot be Entered");
                                doAgain = Display.GetUserInput("Do you want to enter phone number 2 again? Y/N \n");
                            }
                        }
                        else
                        {
                            tempCopy.phone2IsValid = true; // user doesn't want contact to have a phone #
                            doAgain = "N"; // just in case
                        }
                    } while (doAgain.Contains("Y") || doAgain.Contains("y")); // check phone 2 valid format...if not do loop
                    // if user entered incorrect data but does not want to retry
                    // then break out of data entry and return to menu
                    if ((tempCopy.phone2IsValid == false) && (!(doAgain.Contains("y") || doAgain.Contains("Y"))))
                    {
                        contacts.RemoveAt(tempCopy.ID); // remove object that contained user's bad data
                        break;
                    }

                    do
                    {
                        // edited by Garrett Rathke on 10/03/17 per Assignment 4 instructions
                        input = Display.GetUserInput("Email: ", false);
                        if (input.Trim().Length > 0)
                        {
                            if (Validation.IsValidEmail(input))
                            {
                                tempCopy.emailIsValid = true;
                                tempCopy.Email = input;
                                doAgain = "N"; // just in case
                            }
                            else
                            {
                                tempCopy.emailIsValid = false;
                                tempCopy.dataIsValid = false;
                                Console.WriteLine("Invalid Email Format...Contact Cannot be Entered");
                                doAgain = Display.GetUserInput("Do you want to enter email again? Y/N \n");
                            }
                        }
                        else
                        {
                            tempCopy.emailIsValid = true; // user doesn't want contact to have a phone #
                            doAgain = "N"; // just in case
                        }
                    } while (doAgain.Contains("Y") || doAgain.Contains("y")) ; // check email valid format...if not do loop
                    // if user entered incorrect data but does not want to retry
                    // then break out of data entry and return to menu
                    if ((tempCopy.emailIsValid == false) && (!(doAgain.Contains("y") || doAgain.Contains("Y"))))
                    {
                        contacts.RemoveAt(tempCopy.ID); // remove object that contained user's bad data
                        break;
                    }

                    do
                    {
                        // edited by Garrett Rathke on 10/03/17 per Assignment 4 instructions
                        input = Display.GetUserInput("Web Page: ", false);
                        if (input.Trim().Length > 0)
                        {
                            if (Validation.IsValidUrl(input))
                            {
                                tempCopy.urlIsValid = true;
                                tempCopy.Web = input;
                                doAgain = "N"; // just in case
                            }
                            else
                            {
                                tempCopy.urlIsValid = false;
                                tempCopy.dataIsValid = false;
                                Console.WriteLine("Invalid URL Format...Contact Cannot be Entered");
                                doAgain = Display.GetUserInput("Do you want to enter URL again? Y/N \n");
                            }
                        }
                        else if (input.Trim().Length <= 0)
                        {
                            tempCopy.urlIsValid = true; // user doesn't want contact to have a website
                            doAgain = "N"; // just in case
                        }
                    } while (doAgain.Contains("Y") || doAgain.Contains("y")) ; // check URL valid format...if not do loop
                    // if user entered incorrect data but does not want to retry
                    // then break out of data entry and return to menu
                    if ((tempCopy.urlIsValid == false) && (!(doAgain.Contains("y") || doAgain.Contains("Y"))))
                    {
                        contacts.RemoveAt(tempCopy.ID); // remove object that contained user's bad data
                        break;
                    }

                    // check for all data valid
                    if (tempCopy.phone1IsValid == true 
                     && tempCopy.Phone2IsValid == true
                     && tempCopy.emailIsValid == true
                     && tempCopy.urlIsValid == true)
                    {
                        tempCopy.dataIsValid = true;
                        // save edits to original
                        contact.ID = tempCopy.ID;
                        contact.FirstName = tempCopy.FirstName;
                        contact.LastName = tempCopy.LastName;
                        contact.Address = tempCopy.Address;
                        contact.City = tempCopy.City;
                        contact.State = tempCopy.State;
                        contact.Zip = tempCopy.Zip;
                        contact.County = tempCopy.County;
                        contact.CompanyName = tempCopy.CompanyName;
                        contact.Phone1 = tempCopy.Phone1;
                        contact.Phone2 = tempCopy.Phone2;
                        contact.Email = tempCopy.Email;
                        contact.Web = tempCopy.Web;
                        contacts.RemoveAt(tempCopy.ID); // delete copy
                        Console.Clear();
                        Console.WriteLine("Review your changes: "); // display final changes to the user
                        Display.DisplayContact(contact);
                        Display.Pause();
                    }
                }
            }
        }


        /// <summary>
        /// Allows the user to enter new contacts.
        /// </summary>
        /// <returns>The enter contacts.</returns>
        public static void UserEnterContacts(List<Contact> contacts)
        {
            // continue entering new contacts as long as user wishes
            // prompt if user wants to enter another contact

            string anotherUser = ""; // used for entering another conatct after successful entry
            do
            {
                Console.Clear();

                Contact newContact = new Contact();
                /// <COPY>
                /// create deep copy of contact in case of bad input
                /// </COPY>     
                Contact tempCopy = newContact.DeepCopy();

                /// <SEARCH ID>
                /// find available ID
                /// </SEARCH ID>
                // edited by Garrett Rathke on 10/05/17 per Assignment 4 instructions
                // automatically calculate ID
                // here I assume that the contact list should be able to grow beyond 50,000
                // I also assume that the contact list will not need to be reorganized if ever a contact
                //              is deleted, freeing up space in the list
                // In the case where there is an available ID somewhere in the list that is not at the end of the list
                //              the new contact will just be assigned that available slot
                int startingID = 1;
                tempCopy.ID = startingID;
                bool IDisAvailable = false;
                // reassure user the program has not crashed
                Console.WriteLine("Working on Finding Available ID");
                Console.WriteLine("This may take a few minuties...Please Wait...");
                while (IDisAvailable == false)
                {
                    // loops through contact list & checks if any ID is available
                    // if the loop reaches the end of the contact list & no ID is available
                    // then the new contact is put at the end of the list...now the list has increased in size by 1
                    if (ContactQuery.FindByID(contacts, startingID) != null)
                    {
                        startingID++;
                    }
                    else
                    {
                        IDisAvailable = true;
                    }
                }
                // now we have an available ID
                tempCopy.ID = startingID;


                Console.WriteLine("Enter contacts.  Leave name blank to stop entering...");
                Console.WriteLine("You have {0} contacts in your list."
                    , contacts.Count.ToString(Display.INT_DISPLAY_FORMAT));


                // don't allow user to enter new contact info with duplicate names & phone #s
                // don't allow user to enter new contact info with bad phone, email, or url data
                string doAgain = ""; // if bad data, and user wants to reeneter data
                do
                {
                    string input = "";
                    Console.Write("First Name: ");
                    tempCopy.FirstName = Console.ReadLine();

                    if (tempCopy.FirstName.Trim().Length == 0)
                    {
                        //stop entering contacts
                        break;
                    }

                    // continue entering rest of fields...
                    Console.Write("Last Name: ");
                    tempCopy.LastName = Console.ReadLine();

                    Console.Write("Street Address: ");
                    tempCopy.Address = Console.ReadLine();

                    Console.Write("City: ");
                    tempCopy.City = Console.ReadLine();

                    Console.Write("State: ");
                    tempCopy.State = Console.ReadLine();

                    Console.Write("Zip: ");
                    tempCopy.Zip = Console.ReadLine();

                    Console.Write("County: ");
                    tempCopy.County = Console.ReadLine();

                    Console.Write("Company: ");
                    tempCopy.CompanyName = Console.ReadLine();


                    do
                    {
                        // edited by Garrett Rathke on 10/03/17 per Assignment 4 instructions
                        input = Display.GetUserInput("Phone1: ", false);
                        if (input.Trim().Length > 0)
                        {
                            if (Validation.IsValidUSPhoneNumber(input))
                            {
                                tempCopy.phone1IsValid = true;
                                tempCopy.Phone1 = input;
                                doAgain = "N"; // just in case
                            }
                            else
                            {
                                tempCopy.phone1IsValid = false;
                                tempCopy.dataIsValid = false;
                                Console.WriteLine("Invalid Phone Number Format...Contact Cannot be Entered");
                                doAgain = Display.GetUserInput("Do you want to enter phone number 1 again? Y/N \n");
                            }
                        }
                        else
                        {
                            tempCopy.phone1IsValid = true; // user doesn't want contact to have a phone #
                            doAgain = "N"; // just in case
                        }
                    } while (doAgain.Contains("Y") || doAgain.Contains("y")); // check phone 1 valid format...if not do loop
                    // if user entered incorrect data but does not want to retry
                    // then break out of data entry and return to menu
                    if ((tempCopy.phone1IsValid == false) && (!(doAgain.Contains("y") || doAgain.Contains("Y"))))
                    {
                        contacts.RemoveAt(tempCopy.ID); // remove object that contained user's bad data
                        break;
                    }
                    
                    do
                    {
                        // edited by Garrett Rathke on 10/03/17 per Assignment 4 instructions
                        input = Display.GetUserInput("Phone2: ", false);
                        if (input.Trim().Length > 0)
                        {
                            if (Validation.IsValidUSPhoneNumber(input))
                            {
                                tempCopy.phone2IsValid = true;
                                tempCopy.Phone2 = input;
                                doAgain = "N"; // just in case
                            }
                            else
                            {
                                tempCopy.phone2IsValid = false;
                                tempCopy.dataIsValid = false;
                                Console.WriteLine("Invalid Phone Number Format...Contact Cannot be Entered");
                                doAgain = Display.GetUserInput("Do you want to enter phone number 2 again? Y/N \n");
                            }
                        }
                        else
                        {
                            tempCopy.phone2IsValid = true; // user doesn't want contact to have a phone #
                            doAgain = "N"; // just in case
                        }
                    } while (doAgain.Contains("Y") || doAgain.Contains("y")); // check phone 2 valid format...if not do loop
                    // if user entered incorrect data but does not want to retry
                    // then break out of data entry and return to menu
                    if ((tempCopy.phone2IsValid == false) && (!(doAgain.Contains("y") || doAgain.Contains("Y"))))
                    {
                        contacts.RemoveAt(tempCopy.ID); // remove object that contained user's bad data
                        break;
                    }

                    do
                    {
                        // edited by Garrett Rathke on 10/03/17 per Assignment 4 instructions
                        input = Display.GetUserInput("Email: ", false);
                        if (input.Trim().Length > 0)
                        {
                            if (Validation.IsValidEmail(input))
                            {
                                tempCopy.emailIsValid = true;
                                tempCopy.Email = input;
                                doAgain = "N"; // just in case
                            }
                            else
                            {
                                tempCopy.emailIsValid = false;
                                tempCopy.dataIsValid = false;
                                Console.WriteLine("Invalid Email Format...Contact Cannot be Entered");
                                doAgain = Display.GetUserInput("Do you want to enter email again? Y/N \n");
                            }
                        }
                        else
                        {
                            tempCopy.emailIsValid = true; // user doesn't want contact to have a phone #
                            doAgain = "N"; // just in case
                        }
                    } while (doAgain.Contains("Y") || doAgain.Contains("y")); // check email valid format...if not do loop
                    // if user entered incorrect data but does not want to retry
                    // then break out of data entry and return to menu
                    if ((tempCopy.emailIsValid == false) && (!(doAgain.Contains("y") || doAgain.Contains("Y"))))
                    {
                        contacts.RemoveAt(tempCopy.ID); // remove object that contained user's bad data
                        break;
                    }
                    
                    do
                    {
                        // edited by Garrett Rathke on 10/03/17 per Assignment 4 instructions
                        input = Display.GetUserInput("Web Page: ", false);
                        if (input.Trim().Length > 0)
                        {
                            if (Validation.IsValidUrl(input))
                            {
                                tempCopy.urlIsValid = true;
                                tempCopy.Web = input;
                                doAgain = "N"; // just in case
                            }
                            else
                            {
                                tempCopy.urlIsValid = false;
                                tempCopy.dataIsValid = false;
                                Console.WriteLine("Invalid URL Format...Contact Cannot be Entered");
                                doAgain = Display.GetUserInput("Do you want to enter URL again? Y/N \n");
                            }
                        }
                        else if (input.Trim().Length <= 0)
                        {
                            tempCopy.urlIsValid = true; // user doesn't want contact to have a website
                            doAgain = "N"; // just in case
                        }
                    } while (doAgain.Contains("Y") || doAgain.Contains("y")); // check URL valid format...if not do loop
                    // if user entered incorrect data but does not want to retry
                    // then break out of data entry and return to menu
                    if ((tempCopy.urlIsValid == false) && (!(doAgain.Contains("y") || doAgain.Contains("Y"))))
                    {
                        contacts.RemoveAt(tempCopy.ID); // remove object that contained user's bad data
                        break;
                    }


                    // check if user already exists with same first & last names, & same phone1 & phone 2
                    List<Contact> duplicateNames =
                                        ContactQuery.FindByFirstAndLastName(contacts, tempCopy.FirstName, tempCopy.LastName);
                    List<Contact> duplicatePhones =
                                        ContactQuery.FindByPhoneNumbers1And2(contacts, tempCopy.Phone1, tempCopy.Phone2);

                    // queries found duplicate names and phone numbers
                    if (duplicateNames.Count > 0 && duplicatePhones.Count > 0)
                    {
                        Console.WriteLine("Duplicate Names and Phone Numbers NOT ALLOWED...Cannot Create Contact");
                        doAgain = Display.GetUserInput("Would you like to reenter the names and phone numbers?");
                    }
                    else
                    {
                        break; // no duplicates
                    }
                } while (doAgain.Contains("Y") || doAgain.Contains("y"));


                // no duplicates & correct data format
                // save edits to original
                if ((tempCopy.dataIsValid == true) && (!(doAgain.Contains("Y")) || (doAgain.Contains("y"))))
                {                                                       
                    newContact.ID = tempCopy.ID;                        
                    newContact.FirstName = tempCopy.FirstName;
                    newContact.LastName = tempCopy.LastName;
                    newContact.Address = tempCopy.Address;
                    newContact.City = tempCopy.City;
                    newContact.State = tempCopy.State;
                    newContact.Zip = tempCopy.Zip;
                    newContact.County = tempCopy.County;
                    newContact.CompanyName = tempCopy.CompanyName;
                    newContact.Phone1 = tempCopy.Phone1;
                    newContact.Phone2 = tempCopy.Phone2;
                    newContact.Email = tempCopy.Email;
                    newContact.Web = tempCopy.Web;
                    contacts.RemoveAt(tempCopy.ID); // delete copy
                    Console.Clear();
                    Console.WriteLine("New Contact Info: "); // display new contact to the user
                    Display.DisplayContact(newContact);
                    Display.Pause();
                    contacts.Add(newContact);
                }
                else
                {
                    contacts.RemoveAt(tempCopy.ID); // delete copy
                }

                anotherUser = Display.Pause("Do you want to enter another new contact? Y/N \n");
            } while (anotherUser.Contains("Y") || anotherUser.Contains("y"));

        } // end of public static void UserEnterContacts method
        

        public bool Phone1IsValid
        {
            get { return phone1IsValid; }
        }
        public bool Phone2IsValid
        {
            get { return phone2IsValid; }
        }
        public bool EmailIsValid
        {
            get { return emailIsValid; }
        }
        public bool UrlIsValid
        {
            get { return urlIsValid; }
        }
        public bool DataIsValid
        {
            get { return dataIsValid; }
        }

        #endregion public static methods


        #region private properties
        // edited by Garrett Rathke on 10/03/17 per assignment 4 instructions
        private bool dataIsValid = false;
        private bool phone1IsValid = false;
        private bool phone2IsValid = false;
        private bool emailIsValid = false;
        private bool urlIsValid = false;
        #endregion private properties


    } // end of public class Contact
} // end of namespace Lists

