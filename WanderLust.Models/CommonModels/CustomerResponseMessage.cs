using System;
using System.Collections.Generic;
using System.Text;

namespace WanderLust.Models.CommonModels
{
    public class CustomResponseMessage
    {
        public static string InternalServerError { get { return "Internal server error occured"; } }
        public static string UnableToFindInformation { get { return "Unable to find inforamtion"; } }
        public static string InvalidLoginAttempt { get { return "INVALID LOGIN ATTEMPT"; } }
        public static string UserCreated { get { return "User successfully created"; } }
        public static string RoleCreated { get { return "Role successfully created"; } }
        public static string UserCouldNotBeCreated { get { return "Could not create user"; } }
        public static string PasswordValidationFailed { get { return "Password validation Failed"; } }
        public static string DuplicateUser { get { return "User with same email already exist. Please use different email"; } }
        public static string DuplicateRole { get { return "Role with same name already exist. Please use different name"; } }
        public static string RoleNotFound { get { return "Role you are trying to use does not exits"; } }
        public static string RoleDeleted { get { return "Role successfully deleted"; } }
        public static string SectionAdded { get { return "Section successfully added"; } }
        public static string SectionUpdated { get { return "Section successfully updated"; } }
        public static string SectionDeleted { get { return "Section successfully deleted"; } }
        public static string SectionDeletionError { get { return "The data you are trying to delete has foreign key reference with Content table."; } }
        public static string ContentAdded { get { return "Content successfully added"; } }
        public static string ContentUpdated { get { return "Content successfully updated"; } }
        public static string ContentDeleted { get { return "Content successfully deleted"; } }
        public static string ContentDeletionError { get { return "The data you are trying to delete has dependency with other table."; } }
    }
}
