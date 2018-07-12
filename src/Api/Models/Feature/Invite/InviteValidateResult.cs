namespace Api.Models.Feature.Invite
{
    /// <summary>
    /// The object model that is used to handle validation of the invite request
    /// </summary>
    public class InviteValidateResult
    {
        /// <summary>
        /// The Boolean result condition
        /// </summary>
        public bool Condition { get; set; }

        /// <summary>
        /// The logical message that is returned
        /// </summary>
        public string Message { get; set; }
    }
}