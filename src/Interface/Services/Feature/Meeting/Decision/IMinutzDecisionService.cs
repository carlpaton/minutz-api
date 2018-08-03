using System;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Interface.Services.Feature.Meeting.Decision
{
    public interface IMinutzDecisionService
    {
        DecisionMessage GetMeetingDecisions(Guid meetingId, AuthRestModel user);

        DecisionMessage QuickDecisionCreate(Guid meetingId, string decisionText, int order, AuthRestModel user);

        DecisionMessage UpdateDecision(Guid meetingId, MinutzDecision decision, AuthRestModel user);

        MessageBase DeleteDecision(Guid decisionId, AuthRestModel user);
    }
}