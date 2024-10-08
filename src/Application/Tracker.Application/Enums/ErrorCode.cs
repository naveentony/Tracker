﻿namespace Tracker.Application.Enums
{
    public enum ErrorCode
    {
        NotFound = 404,
        ServerError = 500,

        //Validation errors should be in the range 100 - 199
        ValidationError = 101,
        FriendRequestValidationError = 102,

        //Infrastructure errors should be in the range 200-299
        IdentityCreationFailed = 202,
        DatabaseOperationException = 203,

        //Application errors should be in the range 300 - 399
        PostUpdateNotPossible = 300,
        PostDeleteNotPossible = 301,
        InteractionRemovalNotAuthorized = 302,
        IdentityUserAlreadyExists = 303,
        IdentityUserDoesNotExist = 304,
        IncorrectPassword = 305,
        UnauthorizedAccountRemoval = 306,
        CommentRemovalNotAuthorized = 307,
        FriendRequestAcceptNotPossible = 308,
        FriendRequestRejectNotPossible = 309,


        UnknownError = 999


    }
    public enum AmountStatus
    {
        Paid = 0,
        Pending= 1,
        Processing= 2
    }
    public enum VehicleStatus
    {
        Moving,
        Stop,
        Idle,
        Unreachable,
        Towed,
        Disabled
    }


}
