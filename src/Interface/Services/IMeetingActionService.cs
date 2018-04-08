using System.Collections.Generic;
using Minutz.Models.Entities;

namespace Interface.Services
{
    public interface IMeetingActionService
    {
        (bool condition, string message, IEnumerable<MinutzAction> data) GetMinutzActions
            (string referenceId, AuthRestModel user);

        (bool condition, string message, MinutzAction value) CreateMinutzAction
            (string referenceId, MinutzAction action, AuthRestModel user);

        (bool condition, string message, MinutzAction value) UpdateMinutzAction
            (string referenceId, MinutzAction action, AuthRestModel user);

        (bool condition, string message) DeleteMinutzAction
            (string referenceId, string actionId, AuthRestModel user);
    }
}