namespace BusinessLogicLayer.Helpers
{
    public static class FriendlyErrorMessages
    {
        public static readonly string ErrorOnInsertOpeation = "Ops!, we were unable to register the new data. Please try again later";
        public static readonly string ErrorOnUpdateOpeation = "Ops!, we were unable to update the current data. Please try again later";
        public static readonly string ErrorOnReadOpeation = "Ops!, we were unable to bring you back the requested information. Please try again later";
        public static readonly string ErrorOnDeleteOperation = "Ops!, we were unable to delete that. Please try again later";
        public static readonly string ErrorGeneric = "Ops!. Something went wrong please try again later.";
    }

    public static class ValidationAccountErrorMessages
    {
        public static readonly string OnInsertEmailAlreadyInUse = "There's already an user with that email. Please use another email";
        public static readonly string OnInsertUserNameAlreadyInUse = "There's already an user with that user name. Please use another user name";
        public static readonly string OnLoginIncorrectUserNameOrPassword = "Incorrect username or password.";
        public static readonly string OnUpdatePasswordWrongPassword = "Incorrect password.";
        public static readonly string OnUpdatePasswordIncorretConfirmPassword = "Your new password is different than confirm new password. Please verify";


    }


    public static class ValidationErrorMessages
    {
        public static readonly string OnInsertAnItemAlreadyExists = "An item with the same description already exists, please type another";
        public static readonly string OnDeleteNoRecordWasFound = "No record was found or was previously deleted";
        public static readonly string OnUpdateNoRecordWasFound = "No record was found or was previously deleted";
    }

    public static class ValidationPostErrorMessages
    {
        public static readonly string OnTryPublishAlreadyPublished = "This post was already published";
        public static readonly string ErrorChangingVoteForThisPost = "Somethig went wrong, when we find the developer who make this we will punish him";
        public static readonly string AlreadyVotedForThisPost = "You already voted for this post";

    }

}
