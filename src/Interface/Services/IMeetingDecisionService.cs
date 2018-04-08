using System.Collections.Generic;
using Minutz.Models.Entities;

namespace Interface.Services
{
    public interface IMeetingDecisionService
    {
        (bool condition, string message, IEnumerable<MinutzDecision> data) GetMinutzDecisions
            (string referenceId, AuthRestModel user);

        (bool condition, string message, MinutzDecision value) CreateMinutzDecision
            (string referenceId, MinutzDecision decision, AuthRestModel user);

        (bool condition, string message, MinutzDecision value) UpdateMinutzDecision
            (string referenceId, MinutzDecision decision, AuthRestModel user);

        (bool condition, string message) DeleteMinutzDecision
            (string referenceId, string decisionId, AuthRestModel user);
    }
}