using Shortener.Data;
using System.Diagnostics.CodeAnalysis;

namespace Shortener.Models
{
    public class ShortenedLink : IEntity
    {
        public int Id { get; set; }
        public string ShortLink { get; set; }
        public string OriginalLink { get; set; }
        [NotNull]
        public int Views { get; set; }

    }
}