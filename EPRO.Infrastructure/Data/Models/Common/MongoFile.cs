using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPRO.Infrastructure.Data.Models.Common
{
    /// <summary>
    /// Файлово съдържание
    /// </summary>
    [Table("mongo_file")]
    public class MongoFile : UserDateWRT
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("file_id")]
        public string FileId { get; set; }
        [Column("source_type")]
        public int SourceType { get; set; }
        [Column("source_id")]
        public string SourceId { get; set; }        
        [Column("file_name")]
        public string FileName { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("file_size")]
        public int FileSize { get; set; }        
    }
}
