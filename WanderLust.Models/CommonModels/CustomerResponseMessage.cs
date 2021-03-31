﻿using System;
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
    }
}
