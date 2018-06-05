
using System.Collections.Generic;
using System.Linq;

namespace Lists
{
    /// <summary>
    /// Produces results from queries
    /// performed on a contact list.
    /// </summary>
    public class ContactQuery
    {
        #region ctores
        public ContactQuery()
        {
        }
        #endregion ctors

        #region public static methods

        /// <summary>
        /// Returns list of contacts whos last name 
        /// starts with the value in listToSearch.
        /// </summary>
        /// <param name="listToSearch">List to search.</param>
        /// <param name="beginsWith">Begins with.</param>
        public static List<Contact> SearchLastName(List<Contact> listToSearch, string beginsWith)
        {
            // Perform search regardless of case
            // (because we force everything to lower case)
            List<Contact> results = 
                (from contact in listToSearch
                    where (contact.LastName.ToLower()
                        .StartsWith(beginsWith.ToLower()))
                 select contact).ToList<Contact>();

            return results;
        } // end of public method SearchLastName

        /// <summary>
        /// Searchs the identifier range for matching contacts.
        /// </summary>
        /// <returns>The identifier range.</returns>
        /// <param name="listToSearch">List to search.</param>
        /// <param name="id1">Id1.</param>
        /// <param name="id2">Id2.</param>
        public static List<Contact> SearchIdRange(List<Contact> listToSearch, int id1, int id2)
        {
            List<Contact> results = 
                (from contact in listToSearch
                    where (contact.ID >= id1 
                        && contact.ID<= id2)
                    orderby contact.ID descending  
                    select contact).ToList<Contact>();

            return results;
        } // end of public method SearchIdRange
            
        /// <summary>
        /// Finds a contact matching the id.
        /// </summary>
        /// <param name="listToSearch">List to search.</param>
        /// <param name="id">Identifier.</param>
        public static Contact FindByID(List<Contact> listToSearch, int id)
        {
            List<Contact> results = 
                (from contact in listToSearch
                    where (contact.ID==id)
                    select contact).ToList<Contact>();

            if (results.Count >= 1)
                return results[0];
            else
                return null;
        } // end of public method FindByID



        // edited by Garrett Rathke on 10/03/17 per Assignment 4 instructions
        /// <summary>
        /// Finds a contact matching the email domain
        /// </summary>
        /// <param name="listToSearch">List to search.</param>
        public static List<Contact> FindByEmailDomain(List<Contact> listToSearch, string emailDomain)
        {
            List<Contact> results =
                (from contact in listToSearch
                 where (contact.Email.Contains(emailDomain))
                 select contact).ToList<Contact>();

                return results;
        } // end of public method FindByEmailomain

        // edited by Garrett Rathke on 10/03/17 per assignment 4 instructions
        /// <summary>
        /// Finds a contact matching the zip code
        /// </summary>
        /// <param name="listToSearch">List to search.</param>
        public static List<Contact> FindByZip(List<Contact> listToSearch, string zip)
        {
            List<Contact> results =
                (from contact in listToSearch
                where (contact.Zip == zip)
                select contact).ToList<Contact>();

            return results;
        }


        // edited by Garrett Rathke on 10/03/17 per assignment 4 instructions
        /// <summary>
        /// Finds a contact matching the state
        /// </summary>
        /// <param name="listToSearch">List to search.</param>
        public static List<Contact> FindByState(List<Contact> listToSearch, string state)
        {
            List<Contact> results =
                (from contact in listToSearch
                 where (contact.State.ToLower() == state.ToLower()) // convert everything to lowercase
                 select contact).ToList<Contact>();

            return results;
        }


        // edited by Garrett Rathke on 10/03/17 per assignment 4 instructions
        /// <summary>
        /// Finds a contact matching first and last name
        /// </summary>
        /// <param name="listToSearch">List to search.</param>
        public static List<Contact> FindByFirstAndLastName(List<Contact> listToSearch, string firstName, string lastName)
        {
            List<Contact> results =
                (from contact in listToSearch
                 where ((contact.FirstName.ToLower() == firstName.ToLower())
                            && (contact.LastName.ToLower() == lastName.ToLower())) // convert everything to lowercase
                 select contact).ToList<Contact>();

            return results;
        }

        // edited by Garrett Rathke on 10/04/17 per Assignment 4 instructions
        /// <summary>
        /// Finds a contact matching substrings in first and last names
        /// </summary>
        /// <param name="listToSearch">List to search.</param>
        public static List<Contact> FindBySubStringInFirstAndLastName(List<Contact> listToSearch, 
                                                                    string firstName, string lastName)
        {
            List<Contact> results =
                (from contact in listToSearch
                 where ((contact.FirstName.ToLower().TrimStart() == firstName.ToLower().TrimStart())
                            && (contact.LastName.ToLower().TrimStart() == lastName.ToLower().TrimStart())) // convert everything to lowercase
                 select contact).ToList<Contact>();

            return results;
        }

        // edited by Garrett Rathke on 10/04/17 _ for use in Contact.cs for Adding New Contact
        /// <summary>
        /// Finds a contact matching phone number 1 & phone number 2
        /// </summary>
        /// <param name="listToSearch">List to search.</param>
        public static List<Contact> FindByPhoneNumbers1And2(List<Contact> listToSearch, string phone1, string phone2)
        {
            List<Contact> results =
                (from contact in listToSearch
                 where ((contact.Phone1 == phone1)
                            && (contact.Phone2 == phone2))
                 select contact).ToList<Contact>();

            return results;
        }

        // edited by Garrett Rathke on 10/03/17 per assignment 4 instructions
        /// <summary>
        /// Finds a contact matching first and last names and city
        /// </summary>
        /// <param name="listToSearch">List to search.</param>
        public static List<Contact> FindByFirstLastNameAndCity(List<Contact> listToSearch, 
                                                                                    string firstName, string lastName, string city)
        {
            List<Contact> results =
                (from contact in listToSearch
                 where ((contact.FirstName.ToLower() == firstName.ToLower())
                            && (contact.LastName.ToLower() == lastName.ToLower())
                                && (contact.City.ToLower() == city.ToLower())) // convert everything to lowercase
                 select contact).ToList<Contact>();

            return results;
        }

        #endregion public static methods

    } // end of public class ContactQuery

} // end of namespace Lists