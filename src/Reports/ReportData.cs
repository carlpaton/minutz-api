using System.Collections.Generic;

namespace Reports
{
    public class ReportData
    {
        public string name { get; set; }
        public string location { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string purpose { get; set; }
        public string outcome { get; set; }
        public List<agendaModel> agenda { get; set; }
        public List<attendeesModel> attendees { get; set; }
        public List<notesModel> notes { get; set; }

        public class agendaModel
        {
            public string agendaHeading { get; set; }
            public string agendaText { get; set; }
        }

        public class attendeesModel
        {
            public string name { get; set; }
            public string role { get; set; }
        }

        public class notesModel
        {
            public string noteText { get; set; }
        }
    }
}
