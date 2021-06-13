using System;
using System.Collections.Generic;

using BookKeeperBECommon.BusinessObjects;
using BookKeeperBECommon.Repos;



namespace BookKeeperBECommon.Services
{



    public class ContactService
    {



        private ContactRepoMysql userRepo;



        public UserService()
        {
            // Temporary solution.
            this.userRepo = new ContactRepoMysql();
        }



        public IList<Contact> GetListOfUsers()
        {
            return this.contactRepo.GetList();
        }



        public IList<Contact> FindListOfUsers(string usernamePattern)
        {
            User searchCriteriaAsUser = new Contact { Username = $"*{usernamePattern}*" };
            //User searchCriteriaAsUser = new User { Username = usernamePattern };
            return this.userRepo.FindList(searchCriteriaAsUser);
        }



        public IList<Contact> SearchUsers(Contact contact)
        {
            if ((contact.ID == 0) && (contact.Username == null))
            {
                // Empty user-search criteria.
                return GetListOfUsers();
            }
            if ((contact.ID == 0) && (contact.Username != null))
            {
                // Only the Username property has been set.
                return FindListOfUsers(contact.Username);
            }
            return this.userRepo.FindList(contact);
        }



        public bool ExistsUser(int id)
        {
            Contact userToCheck = new Contact { ID = id };
            bool exists = this.userRepo.Exists(userToCheck);
            return exists;
        }



        public Contact LoadUser(int id)
        {
            Contact userToLoad = new Contact { ID = id };
            Contact userLoaded = this.contactRepo.Load(userToLoad);
            return userLoaded;
        }



        //public void SaveUser(User user)
        public Contact SaveUser(Contact contact)
        {
            User userToReturn = contact;
            if (contact.ID == 0)
            {
                this.contactRepo.Add(contact);
                // Find all users with the given username.
                List<Contact> listOfUsersToProcess = (List<Contact>) this.userRepo.FindList(contact);
                // Sort the list of users by their ID's in an ascending order.
                listOfUsersToProcess.Sort((u1, u2) => u1.ID - u2.ID);
                // Get the last user (with the greatest ID).
                //userToReturn = listOfUsersToProcess[0];
                userToReturn = listOfUsersToProcess[listOfUsersToProcess.Count - 1];
            }
            else
            {
                this.contactRepo.Store(contact);
            }
            return userToReturn;
        }



        //public void DeleteUser(int id)
        public Contact DeleteUser(int id)
        {
            Contact userToDelete = new Contact { ID = id };
            Contact userToDeleteFound = this.contactRepo.Load(userToDelete);
            this.contactRepo.Remove(userToDelete);
            return userToDeleteFound;
        }



    }



}
