using System;
using System.Collections.Generic;

namespace ChatRoomTest.Models
{
    /// <summary>
    /// User Entity to handling the user operations
    /// </summary>
    public class User
    {
        #region Properties

        public long Id { get; private set; }
        public string Name { get; private set; }
        public DateTimeOffset CreationDate { get; private set; }

        ////To create self referencing many-to-many UserRelations
        //public ICollection<UserRelation> UserARelations { get; private set; }
        //public ICollection<UserRelation> UserBRelations { get; private set; }


        ////To  Create self referencing many-to-many message

        //public ICollection<Message> FromMessages { get; private set; }
        //public ICollection<Message> ToMessages { get; private set; }


        #endregion


        #region UserMethods
        /// <summary>
        /// Define A User
        /// </summary>
        /// <param name="name"></param>
       
        public User(string name)
        {
            
            Name = name;
            CreationDate=DateTimeOffset.UtcNow;
        }
        /// <summary>
        /// Edit User properties that editable
        /// </summary>
        /// <param name="name"></param>
        /// <param name="lastName"></param>
        /// <param name="aboutMe"></param>
        /// <param name="profilePicture"></param>
        public void Edit(string name, string lastName, string aboutMe, string profilePicture)
        {
            Name = name;
        }



        #endregion

      


    }
}