namespace RegMan.Backend.BusinessLayer.Helpers
{
    public enum TranscriptGpaPolicy
    {
        AllAttempts = 0,
        LatestAttemptPerCourse = 1
    }

    public class InstitutionSettings
    {
        public string UniversityName { get; set; } = string.Empty;
        public string RegistrarOfficeName { get; set; } = string.Empty;
        public string InstitutionName { get; set; } = string.Empty;
        public IEnumerable<string> RegistrarAddressLines { get; set; } = new List<string>();

        public TranscriptGpaPolicy TranscriptGpaPolicy { get; set; } = TranscriptGpaPolicy.LatestAttemptPerCourse;
    }
}
