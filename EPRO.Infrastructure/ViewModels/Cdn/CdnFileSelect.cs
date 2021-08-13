namespace EPRO.Infrastructure.ViewModels.Cdn
{
    public class CdnFileSelect
    {
        public int SourceType { get; set; }
        public string SourceId { get; set; }
        public string FileId { get; set; }

        public enum PostProcess { None, Flatten };
    }
}
