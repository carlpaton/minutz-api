using System;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Interface.Repositories.Feature.Meeting.Decision
{
    public interface IMinutzDecisionRepository
    {
        DecisionMessage GetDecisionCollection(Guid meetingId, string schema, string connectionString);

        DecisionMessage QuickCreateDecision(Guid meetingId, string decisionText, int order, string schema, string connectionString);

        DecisionMessage UpdateDecision(Guid meetingId, MinutzDecision decision, string schema, string connectionString);

        MessageBase DeleteDecision(Guid decisionId, string schema, string connectionString);
    }
}